using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.CoreScripts;

public class SandWaterfall : MonoBehaviour, IGesturable
{

    private ParticleSystem _ps;

    // Use this for initialization
    void Start ()
    {
        GestureActivation.CurrentGesture = this;
	    _ps = gameObject.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (!GestureActivation.IsGesturing) return;

        _ps.startSize = GestureManager.SandFall.StartSize;
        transform.position = new Vector3(GestureManager.SandFall.StartLocation, transform.position.y, transform.position.z);
        
	}

    public void OnStart()
    {
        GestureManager.SandFall.Initialize();
    }

    public void OnNext(float leftHandX, float leftHandY, float rightHandX, float rightHandY)
    {
        GestureManager.SandFall.SetSize(leftHandY);
        GestureManager.SandFall.SetLocation(leftHandX);
    }


    public void OnCompleted()
    {

    }


}
