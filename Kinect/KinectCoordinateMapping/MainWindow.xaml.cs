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
using KinectCoordinateMapping.Properties;
using KinectCoordinateMapping.Utilities;
using MsgPack.Serialization;

/*
Extended and modified from:
https://github.com/Vangos/kinect-2-coordinate-mapping 
under the MIT License
 */
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
        private byte[] _data;
        private static readonly UdpClient Client = new UdpClient(Settings.Default.LocalIp, Settings.Default.LocalPort);
        private const string SuccessMessage = "Connected!";
     
        public MainWindow()
        {
            InitializeComponent();
            _data = new byte[65535];
            _data = Encoding.ASCII.GetBytes(SuccessMessage);
            Client.Send(_data, _data.Length);
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _sensor = KinectSensor.GetDefault();

            if (_sensor == null) return;
            
            _sensor.Open();
            _reader = _sensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Body);
            _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _reader?.Dispose();
            _sensor?.Close();
            Client.Close();
        }
        
        private void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
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

                if (frame == null) return;
                
                canvas.Children.Clear();

                _bodies = new Body[frame.BodyFrameSource.BodyCount];
                frame.GetAndRefreshBodyData(_bodies);

                _ipcMsg.Data.Clear();

                foreach (var body in _bodies)
                {
                    //if body isnt tracked ignore
                    if (!body.IsTracked) continue;     
                    var jointList = UpdateJoints(body);
                    _ipcMsg.Data[body.TrackingId] = jointList; 
                }       
            }

            //IPC to Server
            if (_ipcMsg.Data.Count > 0) SendDataToServer(); 
            
        }//end method


        private SimpleBody UpdateJoints(Body b) // COORDINATE MAPPING
        {
            var jointList = new SimpleBody();
            foreach (var joint in b.Joints.Values)
            {

                //if joint isnt tracked ignore
                if (joint.TrackingState != TrackingState.Tracked) continue;

                // 3D space point
                var jointPosition = joint.Position;

                // 2D space point
                var point = new Point();
                var colorPoint = _sensor.CoordinateMapper.MapCameraPointToColorSpace(jointPosition);

                point.X = float.IsInfinity(colorPoint.X) ? 0 : colorPoint.X;
                point.Y = float.IsInfinity(colorPoint.Y) ? 0 : colorPoint.Y;

                var simJoint = new SimpleJoint()
                {
                    Point = new Vector2((float)point.X / 16, (float)((-1.0 * point.Y) / 9)), //conversion to 16:9 ratio
                    Type = joint.JointType
                };

                // Draw
                var ellipse = new Ellipse
                {
                    Fill = Brushes.Blue,
                    Width = Settings.Default.JointSize,
                    Height = Settings.Default.JointSize
                };

                Canvas.SetLeft(ellipse, point.X - ellipse.Width / 2);
                Canvas.SetTop(ellipse, point.Y - ellipse.Height / 2);

                canvas.Children.Add(ellipse);
                jointList.Joints.Add(simJoint);
            }
            return jointList;
        }


        private void SendDataToServer()
        {
            var sendMsg = MessagePackSerializer.Get<SimpleFrame>();
            using (var stream = new MemoryStream())
            {
                sendMsg.Pack(stream, _ipcMsg);
                _data = stream.ToArray();
            }
            Client.Send(_data, _data.Length);
        }


    }//end class
} //end namespace
