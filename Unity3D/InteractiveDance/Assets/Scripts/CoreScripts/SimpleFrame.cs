using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum JointType
{
    SpineBase, SpineMid, Neck, Head, ShoulderLeft, ElbowLeft, WristLeft, HandLeft, ShoulderRight, ElbowRight, WristRight, HandRight, HipLeft, KneeLeft, AnkleLeft, FootLeft, HipRight, KneeRight, AnkleRight, FootRight, SpineShoulder, HandTipLeft, ThumbLeft, HandTipRight, ThumbRight
}


public class SimpleFrame
{
    public Dictionary<ulong, SimpleBody> Data = new Dictionary<ulong, SimpleBody>();
}

public class SimpleJoint
{
    public UnityEngine.Vector2 Point = new Vector2();
    public JointType Type;
}

public class SimpleBody
{
    public List<SimpleJoint> Joints = new List<SimpleJoint>();
}

