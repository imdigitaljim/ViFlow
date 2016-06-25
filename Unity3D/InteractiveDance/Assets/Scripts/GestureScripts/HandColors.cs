using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.CoreScripts;

public class HandColors : MonoBehaviour, IGesturable {


    private ParticleSystem _leftHandColor;
    private ParticleSystem _rightHandColor;

    public void OnCompleted()
    {
     
    }

    public void OnNext(float leftHandX, float leftHandY, float rightHandX, float rightHandY)
    {
        GestureManager.Aura.SetSize(leftHandY);
    }

    public void OnStart()
    {
        GestureManager.Aura.Initialize();
    }

    void SetGesture()
    {
        var emLeft = _leftHandColor.emission;
        var emRight = _rightHandColor.emission;
        var rate = emLeft.rate;
        rate.constantMin = GestureManager.Aura.Size;
        rate.constantMax = GestureManager.Aura.Size;
        emLeft.rate = rate;
        emRight.rate = rate;
    }

    void SetHandObjects()
    {
        _leftHandColor = GameObject.Find("LeftHand")
        .transform
        .GetChild(0) //ActiveEffects
        .GetChild(0) //PrefabFire
        .GetChild(0) //Containing GameObject
        .gameObject.GetComponent<ParticleSystem>();
        _rightHandColor = GameObject.Find("RightHand")
        .transform
        .GetChild(0)
        .GetChild(0)
        .GetChild(0)
        .gameObject.GetComponent<ParticleSystem>();
    }
    // Use this for initialization
    void Start () {
        GestureActivation.CurrentGesture = this;
        SetHandObjects();
        SetGesture();
    }
	
	// Update is called once per frame
	void Update () {
        if (!GestureActivation.IsGesturing) return;
        SetGesture();
    }
}
