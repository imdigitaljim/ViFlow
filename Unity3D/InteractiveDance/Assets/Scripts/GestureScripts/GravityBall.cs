using UnityEngine;
using System;
using Assets.Scripts.CoreScripts;
using System.Collections;

public class GravityBall : MonoBehaviour, IGesturable
{
    private Rigidbody ball;

    // Use this for initialization
    void Start ()
    {
        GestureActivation.CurrentGesture = this;
        ball = gameObject.transform.GetChild(0).gameObject.GetComponent<Rigidbody>();
    }

    public void OnStart()
    {
        GestureManager.BounceBall.Initialize();
    }


    // Update is called once per frame
    // Update with new data from the Gesture
    void Update ()
    {
        if (!GestureActivation.IsGesturing)
            { return; }
        else
        {
            //get current ball position
            var x = ball.transform.position.x;
            var y = ball.transform.position.y;
            ball.AddForce(GestureManager.BounceBall.forceVector);

            //_ps.startSize = GestureManager.SandFall.StartSize;
            //transform.position = new Vector3(GestureManager.SandFall.StartLocation, transform.position.y, transform.position.z);

            //var x = c.gameObject.transform.position.x < transform.position.x ? 1 : -1;
            //var y = c.gameObject.transform.position.y < transform.position.y ? 1 : -1;
            //rb.AddForce(new Vector3(x * 300, y * 300, 0));
        }
    }

    public void OnNext(float leftHandX, float leftHandY, float rightHandX, float rightHandY)
    {
        GestureManager.BounceBall.SetSpeedUpFactor(leftHandX, leftHandY, rightHandX, rightHandY);
    }

    public void OnCompleted()
    {

    }
}
