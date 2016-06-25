using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.CoreScripts;

public class WhiteWaterFall : MonoBehaviour, IGesturable
{

    private ParticleAnimator _pa;
	// Use this for initialization
	void Start () {
        GestureActivation.CurrentGesture = this;
	    _pa = GetComponent<ParticleAnimator>();
        SetGesture();
    }

    void SetGesture()
    {
        _pa.sizeGrow = GestureManager.Waterfall.Size;
    }

	// Update is called once per frame
	void Update () {
        if (!GestureActivation.IsGesturing) return;
        SetGesture();
    }

    public void OnStart()
    {
        GestureManager.Waterfall.Initialize();
    }

    public void OnNext(float leftHandX, float leftHandY, float rightHandX, float rightHandY)
    {
        GestureManager.Waterfall.SetSize(leftHandY);
    }

    public void OnCompleted()
    {
     
    }


}
