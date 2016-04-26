using UnityEngine;
using System.Collections;

public class Form {
    public int id;
    public GameObject Root, RightHand, LeftHand;
    public Vector3 RootVector, LeftHandVector, RightHandVector;
    public float Radius, Velocity;
    public bool IsFront = false;
    public float IsFrontMagnitude;

    public void UpdatePositions()
    {
        Root.transform.position = RootVector;
        RightHand.transform.position = RootVector - RightHandVector;
        LeftHand.transform.position = RootVector - LeftHandVector;
    }
}
