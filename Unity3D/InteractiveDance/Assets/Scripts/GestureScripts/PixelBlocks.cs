using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.CoreScripts;

public class PixelBlocks : MonoBehaviour, IGesturable
{
    // Use this for initialization
    void Start () {
        GestureActivation.CurrentGesture = this;
        SetGesture();
    }

    void SetGesture()
    {
        var newPosition = (Vector3)GestureManager.BlockWall.Value;
        newPosition.z = transform.position.z;
        transform.position = newPosition;
    }

	// Update is called once per frame
	void Update () {
        if (!GestureActivation.IsGesturing) return;
	    SetGesture();
	}

    public void OnStart()
    {
        GestureManager.BlockWall.Initialize();
    }

    public void OnNext(float leftHandX, float leftHandY, float rightHandX, float rightHandY)
    {
        GestureManager.BlockWall.SetGestureValues(leftHandX, leftHandY);
    }

    public void OnCompleted()
    {

    }


}
