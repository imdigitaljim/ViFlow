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
        var color = FloatToColor(GestureManager.Piano.X, GestureManager.Piano.Y);
        foreach (var tile in _tiles)
        {
            tile.material.color = color;
        }
        

        //set components
    }

    Color FloatToColor(float x, float y)
    {
        var z = Mathf.Clamp((1 - x + y), 0, 1);
        var r = Mathf.Clamp((2.5623f * x + (-1.1661f) * y + (-.3962f) * z), 0, 1);
        var g = Mathf.Clamp(((-1.0215f) * x + 1.9778f * y + 0.0437f * z), 0, 1);
        var b = Mathf.Clamp((0.0752f * x + (-0.2562f) * y + 1.1810f * z), 0, 1);
        return new Color(r, g, b);
    }
}
