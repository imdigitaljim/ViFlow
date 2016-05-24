using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.IO.Pipes;
using MsgPack.Serialization;

namespace KinectCoordinateMapping
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private KinectSensor _sensor;
        private MultiSourceFrameReader _reader;
        private IList<Body> _bodies;
        private SimpleFrame _ipcMsg = new SimpleFrame();
        public static readonly uint WM_SETTEXT = 0x000C;  
        private string _path;
        private byte[] _data;
        private static readonly UdpClient Client = new UdpClient("127.0.0.1", 9050);
        private const string Success = "Connected!";
     
        public MainWindow()
        {
            InitializeComponent();
            _data = new byte[65535];
            _data = Encoding.ASCII.GetBytes(Success);
            Client.Send(_data, _data.Length);
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var path = $@"C:\Users\Public\ViFlowIPC";
                if (!Directory.Exists(path))
                {
                    Console.WriteLine($"creating {path}");
                    Directory.CreateDirectory(path);
                }
                _path = System.IO.Path.Combine(path, "DATA");
                File.CreateText(_path);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            _sensor = KinectSensor.GetDefault();

            if (_sensor != null)
            {
                _sensor.Open();

                _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Body);
                _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_reader != null)
            {
                _reader.Dispose();
            }

            if (_sensor != null)
            {
                _sensor.Close();
            }
            Client.Close();
        }


        public class Vector2
        {
            public float x;
            public float y;

            public Vector2()
            {
            }
            public Vector2(float xval, float yval)
            {
                x = xval;
                y = yval;
            }
        }


        public class SimpleJoint
        {
            public Vector2 Point = new Vector2();
            public JointType Type;
        }

        public class SimpleBody
        {
            public List<SimpleJoint> Joints = new List<SimpleJoint>();
        }

        public class SimpleFrame
        {
            public Dictionary<ulong, SimpleBody> Data = new Dictionary<ulong, SimpleBody>();
        }
    

        void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var reference = e.FrameReference.AcquireFrame();


            using (var frame = reference.ColorFrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    camera.Source = frame.ToBitmap();
                }
            }

            // Body
            using (var frame = reference.BodyFrameReference.AcquireFrame())
            {

                if (frame != null)
                {
                    canvas.Children.Clear();

                    _bodies = new Body[frame.BodyFrameSource.BodyCount];
                    frame.GetAndRefreshBodyData(_bodies);

                    _ipcMsg.Data.Clear();


                    foreach (var body in _bodies)
                    {

                        if (body.IsTracked)
                        {
                            var jointList = new SimpleBody();
                            // COORDINATE MAPPING
                            foreach (var joint in body.Joints.Values)
                            {

                                if (joint.TrackingState == TrackingState.Tracked)
                                {

                                    // 3D space point
                                    var jointPosition = joint.Position;

                                    // 2D space point
                                    var point = new Point();
                                    var colorPoint = _sensor.CoordinateMapper.MapCameraPointToColorSpace(jointPosition);

                                    point.X = float.IsInfinity(colorPoint.X) ? 0 : colorPoint.X;
                                    point.Y = float.IsInfinity(colorPoint.Y) ? 0 : colorPoint.Y;

                                    var simJoint = new SimpleJoint()
                                    {
                                        Point = new Vector2((float) point.X / 16, (float) ((-1.0 * point.Y) / 9)),
                                        Type = joint.JointType
                                    };

                                    // Draw
                                    var ellipse = new Ellipse
                                    {
                                        Fill = Brushes.Blue,
                                        Width = 30,
                                        Height = 30
                                    };

                                    Canvas.SetLeft(ellipse, point.X - ellipse.Width/2);
                                    Canvas.SetTop(ellipse, point.Y - ellipse.Height/2);

                                    canvas.Children.Add(ellipse);
                                    jointList.Joints.Add(simJoint);

                                }
                            }

                            _ipcMsg.Data[body.TrackingId] = jointList;
                        }
                    }
                    if (_ipcMsg.Data.Count > 0)
                    {
                        var sendMsg = MessagePackSerializer.Get<SimpleFrame>();
                        using (var stream = new MemoryStream())
                        {
                            sendMsg.Pack(stream, _ipcMsg);
                            _data = stream.ToArray();
                        }
                        Client.Send(_data, _data.Length);
                    }
                }
            }
        }

    }
}
