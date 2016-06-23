using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.CoreScripts;

public class PixelBlocks : MonoBehaviour, IGesturable
{
    // Use this for initialization
    void Start () {
        GestureActivation.CurrentGesture = this;


    }
	
	// Update is called once per frame
	void Update () {
        if (!GestureActivation.IsGesturing) return;
    }

    public void OnStart()
    {
        GestureManager.BlockWall.Initialize();
    }

    public void OnNext(float leftHandX, float leftHandY, float rightHandX, float rightHandY)
    {
  
    }

    public void OnCompleted()
    {

    }


}
