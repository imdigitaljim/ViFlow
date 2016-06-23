using UnityEngine;
using System;
using Assets.Scripts.CoreScripts;
using System.Collections;

public class WaterFallGstr : MonoBehaviour, IGesturable
{
    private Transform waterfall;

    // Use this for initialization
    void Start ()
    {
        GestureActivation.CurrentGesture = this;
        //waterfall = gameObject.transform.GetChild(0).gameObject.transform;
        waterfall = gameObject.transform;
    }

    public void OnStart()
    {
        GestureManager.Waterfall.Initialize();
    }


    // Update is called once per frame
    // Update with new data from the Gesture
    void Update ()
    {
        if (!GestureActivation.IsGesturing)
            { return; }
        else
        {
            waterfall.localScale = GestureManager.Waterfall.scaleVector;
        }
    }

    public void OnNext(float leftHandX, float leftHandY, float rightHandX, float rightHandY)
    {
        GestureManager.Waterfall.SetWaterFallScale(leftHandX, leftHandY, rightHandX, rightHandY);
    }

    public void OnCompleted()
    {

    }
}
