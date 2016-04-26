//SocketClient Version 0.2
//Takes data via UDP and displays it acordingly.

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class FormControllerZ : MonoBehaviour
{

    //object info
    public Vector3 offset; //-27.2, 6.2, 30
    public float handOffset;
    public float div = 10.0f;
    public static Form[] bodies;
    private int bodycount;

    public GameObject dancePrefab;
    //connection info
    private Thread receiveThread;
    private UdpClient client;
    public int port = 5005;

    //debug info
    private string lastReceivedUDPPacket;

    private IPEndPoint anyIP;
    private float currentNoUpdateTime = 0;
    public float maxTimeWithoutUpdate = 3;
    private EffectGenerator _effectGen;

    void Start()
    {
        Debug.Log(string.Format(@"Sending to 127.0.0.1 : {0}", port));
        client = new UdpClient(port);
        anyIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);

        _effectGen = GameObject.Find("MasterControl").GetComponent<EffectGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFormData();
    }

    private void UpdateFormData()
    {
        //rewrite to serialize and deserialize packets.
        try
        {
            if (client.Available > 0)
            {
                currentNoUpdateTime = 0;
                var msg = Encoding.UTF8.GetString(client.Receive(ref anyIP));
                lastReceivedUDPPacket = msg;
                Debug.Log(lastReceivedUDPPacket);
                var words = msg.Split(',');
                switch (words[0])
                {
                    case "SET":
                        bodycount = int.Parse(words[2]);
                        Debug.Log("New Set Called");
                        SetNewForms();
                        break;
                    case "DATA":
                        UpdateForms(int.Parse(words[1]), words);
                        break;
                    default:
                        Debug.Log("UNKNOWN UDP MESSAGE");
                        break;
                }
                var front = 9999f;
                var second = 9999f;
                var frontIndex = -1;
                var secondIndex = -1;
                for (var i = 0; i < bodies.Length; i++)
                {
                    bodies[i].UpdatePositions();
                    bodies[i].IsFrontMagnitude = bodies[i].RootVector.z;
                    if (frontIndex == -1)
                    {
                        front = bodies[i].IsFrontMagnitude;
                        frontIndex = i;
                    }
                    else if (front > bodies[i].IsFrontMagnitude)
                    {
                        if (secondIndex == -1)
                        {
                            second = front;
                            secondIndex = frontIndex;
                        }
                        front = bodies[i].IsFrontMagnitude;
                        frontIndex = i;
                    }
                    else if (secondIndex == -1 || second > bodies[i].IsFrontMagnitude)
                    {
                        second = bodies[i].IsFrontMagnitude;
                        secondIndex = i;
                    }
                }
                for (var i = 0; i < bodies.Length; i++)
                {
                    if (i == frontIndex)
                    {
                        bodies[i].IsFront = true;
                        bodies[i].IsFrontMagnitude = second - front;
                    }
                    else
                    {
                        bodies[i].IsFront = false;
                        bodies[i].IsFrontMagnitude = 0;
                    }

                }
            }
            else if (bodies != null && currentNoUpdateTime < maxTimeWithoutUpdate)
            {
                currentNoUpdateTime += Time.deltaTime;
                var front = 9999f;
                var second = 9999f;
                var frontIndex = -1;
                var secondIndex = -1;
                for (var i = 0; i < bodies.Length; i++)
                {
                    bodies[i].IsFrontMagnitude = bodies[i].Root.transform.position.z;
                    if (frontIndex == -1)
                    {
                        front = bodies[i].IsFrontMagnitude;
                        frontIndex = i;
                    }
                    else if (front > bodies[i].IsFrontMagnitude)
                    {
                        if (secondIndex == -1)
                        {
                            second = front;
                            secondIndex = frontIndex;
                        }
                        front = bodies[i].IsFrontMagnitude;
                        frontIndex = i;
                    }
                    else if (secondIndex == -1 || second > bodies[i].IsFrontMagnitude)
                    {
                        second = bodies[i].IsFrontMagnitude;
                        secondIndex = i;
                    }
                }
                for (var i = 0; i < bodies.Length; i++)
                {
                    if (i == frontIndex)
                    {
                        bodies[i].IsFront = true;
                        bodies[i].IsFrontMagnitude = second - front;
                    }
                    else
                    {
                        bodies[i].IsFront = false;
                        bodies[i].IsFrontMagnitude = 0;
                    }
                }
            }
            else
            {
                bodycount = 0;
                for (var i = 0; i < transform.childCount; i++)
                {
                    Destroy(transform.GetChild(i).gameObject);
                }
            }

        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    private void SetNewForms()
    {
        Debug.Log(bodycount);
        for (var i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        bodies = new Form[bodycount];
        for (var i = 0; i < bodies.Length; i++)
        {
            bodies[i] = new Form();
            bodies[i].Root = (GameObject)Instantiate(dancePrefab, Vector3.zero, Quaternion.identity);
            bodies[i].RightHand = bodies[i].Root.transform.GetChild(0).gameObject;
            bodies[i].LeftHand = bodies[i].Root.transform.GetChild(1).gameObject;
            bodies[i].Root.transform.parent = transform;
            _effectGen.SetCurrentEffect(bodies[i]);
        }
    }
    private void UpdateForms(int id, IList<string> msg)
    {
        if (id + 1 > bodycount)
        {
            bodycount = id + 1;
            SetNewForms();
        }
        bodies[id].id = id;
        // 0 = Message Type; 1 = id; 2,3 = xy root; 4,5 = xy leftmost; 6,7 = xy rightmost; 8 = self.radius; 9 = self.velocity
        bodies[id].RootVector = new Vector3(float.Parse(msg[2]) / div, (-float.Parse(msg[3])) / (div * 2),0 ) + offset;
        bodies[id].LeftHandVector = new Vector3(float.Parse(msg[4]) / div + handOffset, (-float.Parse(msg[5])) / (div * 2), 0 ) + offset - bodies[id].RootVector;
        bodies[id].RightHandVector = new Vector3(float.Parse(msg[6]) / div - handOffset, (-float.Parse(msg[7])) / (div * 2), 0) + offset - bodies[id].RootVector;
        bodies[id].Radius = float.Parse(msg[8]);
        bodies[id].Velocity = float.Parse(msg[9]);
    }


}
