using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Assets.Scripts.CoreScripts;

public class ColorPiano : MonoBehaviour, IGesturable
{

    private List<Renderer> _tiles = new List<Renderer>();
    public void OnCompleted()
    {

    }

    public void OnNext(float leftHandX, float leftHandY, float rightHandX, float rightHandY)
    {
        GestureManager.Piano.SetX(leftHandX);
        GestureManager.Piano.SetY(leftHandY);
    }

    public void OnStart()
    {
        GestureManager.Piano.Initialize();
    }

    // Use this for initialization
    void Start()
    {
        foreach (Transform tile in transform.GetChild(1).gameObject.transform)
        {
            _tiles.Add(tile.gameObject.GetComponent<Renderer>());
        }
        GestureActivation.CurrentGesture = this;
        //get components
    }

    // Update is called once per frame
    void Update()
    {
        if (!GestureActivation.IsGesturing) return;      
        var color = EffectUtility.FloatToColor(GestureManager.Piano.X, GestureManager.Piano.Y);
        foreach (var tile in _tiles)
        {
            tile.material.color = color;
        }
        

        //set components
    }


}
