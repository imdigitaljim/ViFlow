using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.CoreScripts;

public class Exploding : MonoBehaviour, IGesturable
{
    public void OnCompleted()
    {
      
    }

    public void OnNext(float leftHandX, float leftHandY, float rightHandX, float rightHandY)
    {
        GestureManager.Explode.SetGestureValues(leftHandX, leftHandY);
    }

    public void OnStart()
    {
        GestureManager.Explode.Initialize();
        SetGesture();
    }

    // Use this for initialization
    void Start () {
        GestureActivation.CurrentGesture = this;
        SetGesture();
    }

    void SetGesture()
    {
        var newPosition = (Vector3)GestureManager.Explode.Value;
        newPosition.z = transform.position.z;
        Debug.Log(transform.position);
        transform.position = newPosition;
        Debug.Log(transform.position);
    }
    // Update is called once per frame
    void Update () {
        if (!GestureActivation.IsGesturing) return;
        SetGesture();
    }
}
