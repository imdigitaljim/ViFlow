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

public class FormController : MonoBehaviour {

    //object info
    public GameObject BodyPrefab;

    //connection info 
    private static readonly IPEndPoint IpEndPoint = new IPEndPoint(IPAddress.Any, 9050);
    private static IPEndPoint _sender = new IPEndPoint(IPAddress.Any, 0);
    private static readonly UdpClient Server = new UdpClient(IpEndPoint);
    private byte[] _data;

    public static SimpleFrame Bodies = new SimpleFrame();
    public float Modifier;
    public Vector2 minThreshold;
    void Start()
    {
        _data = new byte[65535];
        _data = Server.Receive(ref _sender);
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

            var serializer = MessagePackSerializer.Get<SimpleFrame>();
            using (var stream = new MemoryStream(_data))
            {
                Bodies = serializer.Unpack(stream);
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
