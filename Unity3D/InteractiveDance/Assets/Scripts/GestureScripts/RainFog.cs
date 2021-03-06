﻿using UnityEngine;
using System.Collections;
using Assets.Scripts.CoreScripts;

public class RainFog : MonoBehaviour, IGesturable
{
    private ParticleSystem _ps;
	// Use this for initialization
	void Start () {
        GestureActivation.CurrentGesture = this;
	    _ps = gameObject.GetComponent<ParticleSystem>();
	    SetGesture();

	}

    void SetGesture()
    {
        _ps.startSize = GestureManager.Fog.Size;
    }
        

	// Update is called once per frame
	void Update () {
        if (!GestureActivation.IsGesturing) return;
	    SetGesture();
	}

    public void OnStart()
    {
        GestureManager.Fog.Initialize();
    }

    public void OnNext(float leftHandX, float leftHandY, float rightHandX, float rightHandY)
    {
        GestureManager.Fog.SetSize(leftHandY);
    }

    public void OnCompleted()
    {

    }
}
