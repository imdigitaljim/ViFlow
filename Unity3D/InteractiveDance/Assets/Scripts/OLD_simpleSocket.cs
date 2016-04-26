//SocketClient Version 0.2
//Takes data via UDP and displays it acordingly.

using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class simpleSocket : MonoBehaviour {
	
	// Use this for initialization
	
	//private const int listenPort = 5005;
	public GameObject d0_center;
	public GameObject d1_center;
	public GameObject d2_center;
	public GameObject d3_center;
	
	public struct form {
		public int id;
		public GameObject root;
		public float xPosRoot;
		public float yPosRoot;
		public float radius;
	} 
	
	
	form[] bodies;
	private int[] initial; // number of forms to track// maximum limb form
	
	Thread receiveThread;
	UdpClient client;
	public int port;
	public float yoffset = 0;
	public float xoffset = 0;
	public float div = 10.0f;
	public int bodycount = 1;
	//info
	
	public string lastReceivedUDPPacket = "";
	public string allReceivedUDPPackets = "";
	
	void Start () {
		init();
		
		bodies = new form[bodycount];
		Debug.Log("Staring Up");
		for(int i = 0; i < bodies.Length; i++){
			if (i == 0){
				bodies[i].root = d0_center;
			}
			if (i == 1){
				bodies[i].root = d1_center;
			}	
			if (i == 2){
				bodies[i].root = d2_center;
			}
			if (i == 3){
				bodies[i].root = d3_center;
			}						
		}
		
	}
	
	void printForm(form f,string mess=""){
		Debug.Log (mess + "\n" +
		           "Center " + f.root.transform.position.x + " " + f.root.transform.position.y 
		            );		
	}
	
	void OnGUI(){
		Rect  rectObj=new Rect (40,10,200,400);
		
		GUIStyle  style  = new GUIStyle ();
		
		style .alignment  = TextAnchor.UpperLeft;
		
		GUI .Box (rectObj,"# UDPReceive\n127.0.0.1 "+port +" #\n"
		          
		          //+ "shell> nc -u 127.0.0.1 : "+port +" \n"
		          
		          + "\nLast Packet: \n"+ lastReceivedUDPPacket
		          
		          //+ "\n\nAll Messages: \n"+allReceivedUDPPackets
		          
		          ,style );
		
	}
	
	private void init(){
		print ("UPDSend.init()");
		port = 5005;//All ports start here				
		print ("Sending to 127.0.0.1 : " + port);
		receiveThread = new Thread (new ThreadStart(ReceiveData));
		receiveThread.IsBackground = true;
		receiveThread.Start ();
	}
	
	private void ReceiveData(){
		client = new UdpClient (port);
		while (true) {
			try{
				IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
				byte[] data = client.Receive(ref anyIP);				
				string text = Encoding.UTF8.GetString(data);
				lastReceivedUDPPacket=text;
				allReceivedUDPPackets=allReceivedUDPPackets+text;
				
				string[] words = text.Split(',');
				
				
				
				if (words[0] == "INITIAL DATA"){
					Debug.Log("Initial Data " + text);
				}
				
				if (words[0] == "DATA"){
					Debug.Log("Data Recieved "+text);
					int id = int.Parse(words[1]);
					bodies[id].xPosRoot = (float.Parse(words[2]))/div;
					bodies[id].yPosRoot = (-float.Parse(words[3]) + yoffset)/div;
					bodies[id].radius = (float.Parse(words[4]))/div;
				}
									
			}catch(Exception e){
				//print (e.ToString());
			}
		}
	}
	
	public string getLatestUDPPacket(){
		allReceivedUDPPackets = "";
		return lastReceivedUDPPacket;
	}
	
	// Update is called once per frame
	void Update () {
		if (bodies.Length > 0){
			for(int i = 0 ; i < bodies.Length; i++){
				
				bodies[i].root.transform.position = new Vector3(bodies[i].xPosRoot+xoffset,bodies[i].yPosRoot+yoffset,0);
			}
		}
	}
	
	void OnApplicationQuit(){
		if (receiveThread != null) {
			receiveThread.Abort();
			Debug.Log(receiveThread.IsAlive); //must be false
		}
	}
}
