using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using MsgPack.Serialization;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

public class BodyController : MonoBehaviour {

    //object info
    public GameObject BodyPrefab;

    //connection info 
    private static readonly IPEndPoint IpEndPoint = new IPEndPoint(IPAddress.Any, 9050); // Ports have to line up in python code
    private static IPEndPoint _sender = new IPEndPoint(IPAddress.Any, 0); // Possibly take out IPAd...Any and put in local host (127.0.0.1)
    private static readonly UdpClient Server = new UdpClient(IpEndPoint); 
    private byte[] _data;

    public static SimpleFrame Bodies = new SimpleFrame();
    public List<int> theData = new List<int>();
    public float Modifier;
    public Vector2 minThreshold;
    
    void Start()
    {
        _data = new byte[65535];
    //    _data = Server.Receive(ref _sender); //Commented to keep from freezing at Play
        Debug.Log(Encoding.ASCII.GetString(_data, 0, _data.Length));
    }

    // Update is called once per frame
    void Update()
    {
        ReceiveUpdates();
        UpdatePositions();
    }

    void ReceiveUpdates()
    {
        if (Server.Available > 0)
        {
            _data = Server.Receive(ref _sender);

        // MAYBE JUST SEND THE LEFT FOOT SINCE THE RIGHT FOOT IS WONKY FROM PYTHON

            var serializer = MessagePackSerializer.Get<List<int>>(); //IR code
        //    var serializer = MessagePackSerializer.Get<SimpleFrame>(); //Actual points #Kinect, WORKS!

        //    var serializer = MessagePackSerializer.Get<String>(); //Debug, swap comment with above

            using (var stream = new MemoryStream(_data))
            {
        //       Bodies = serializer.Unpack(stream); //Actual Points ERROR, WHY?!?!?!?!
                theData = serializer.Unpack(stream);

                SimpleJoint Joint = new SimpleJoint();

                //Check to see if dancer is already in Bodies dict
                //If not init and add dancer and joints
                if(!Bodies.Data.ContainsKey((ulong)theData[0])){
                    //create temp joint and body variables to store new data
                    SimpleBody Joints = new SimpleBody();
                    //skip dancer id 
                    int j = 1;
                    //Getting data from stream for each joint and putting 
                    //joint class into simple body array at enum index
                    while(j < theData.Count){
                        Joint.Point.x = theData[j++];
                        Joint.Point.y = theData[j++];
                        Joint.Type = (JointType)theData[j++];
                        //Joint.Type = JointType.Head;
                        Debug.Log("LLegX: " + Joint.Point.x + ", LLegY: " + Joint.Point.y);
                        Joints.Joints.Add(Joint);
                    }
                    //Add new SimpleBody list to SimpleFrame dict
                    Bodies.Data.Add((ulong)theData[0], Joints);
                } 
                else 
                {
                    foreach(var body in Bodies.Data){
                        int j = 1;
                        body.Value.Joints.Clear();

                        while(j < theData.Count){
                            Joint.Point.x = theData[j++];
                            Joint.Point.y = theData[j++];
                            Joint.Type = (JointType)theData[j++];
                            //Joint.Type = JointType.Head; // <- Fix!!!
                            //Debug.Log(body.GetType());
                            Debug.Log("LLegX: " + Joint.Point.x + ", LLegY: " + Joint.Point.y);
                            body.Value.Joints.Add(Joint);
                        }
                    }
                }

                

        //       Debug.Log(serializer.Unpack(stream)); //Debug, swap comment with above

        /*       foreach(var body in Bodies.Data)
                {
                    Debug.Log(body.Key);
                    Debug.Log(body.Value.Joints[0].Point);
                }
        */        
                foreach(var datum in theData)
                {
                    Debug.Log(datum);
                }


            //    Debug.Log("done printing");
            //    Debug.Log(Bodies.Data.Count); // -> "1" = WORKING!!! (more or less)
            }
        }
    }

    void UpdatePositions()
    {
        foreach (var body in Bodies.Data)
        {          
            var isFound = false;
            foreach (Transform child in transform)
            {
                var bodyManager = child.gameObject.GetComponent<BodyManager>();
                if (bodyManager.Id == body.Key)
                {                   
                    isFound = true;
                    SetBody(child.gameObject, body.Key);
                }
            }
            if (!isFound)
            {
                var obj = Instantiate(BodyPrefab);
                obj.transform.parent = gameObject.transform;
                obj.GetComponent<BodyManager>().Id = body.Key;
                SetBody(obj, body.Key);
            }

        }
    }

    void SetBody(GameObject body, ulong id)
    {
        foreach (Transform child in body.transform)
        {
            foreach (var joint in Bodies.Data[id].Joints)
            {
                body.transform.GetChild((int)joint.Type).position = joint.Point;

                //var test = joint.Point - minThreshold;
                //if (test.x > 0 && test.y > 0)
                //{
                //    body.SetActive(true);
                    
                //    //Debug.Log(joint.Point);
                //}
                //else
                //{
                //    body.SetActive(false);
                //}


             
            }
        }
    }
}
