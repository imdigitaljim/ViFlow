using UnityEngine;
using System;
using Assets.Scripts.CoreScripts;
using System.Collections;

public class GravityBall : MonoBehaviour, IGesturable
{
    // Use this for initialization
    void Start ()
    {
        GestureActivation.CurrentGesture = this;
        SetGesture();
    }

    void SetGesture()
    {
        transform.localScale = new Vector3(GestureManager.BounceBall.Magnitude, GestureManager.BounceBall.Magnitude, GestureManager.BounceBall.Magnitude);
    }

    // Update is called once per frame
    void Update ()
    {
        if (!GestureActivation.IsGesturing) return;
        SetGesture();

    }

    public void OnStart()
    {
        GestureManager.BounceBall.Initialize();
    }

    public void OnNext(float leftHandX, float leftHandY, float rightHandX, float rightHandY)
    {
        GestureManager.BounceBall.SetMagnitude(leftHandY);
    }

    public void OnCompleted()
    {

    }
}
