using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Assets.Scripts.CoreScripts;

public class ColorPiano : MonoBehaviour, IGesturable
{

    private readonly List<Renderer> _tiles = new List<Renderer>();
    public void OnCompleted()
    {

    }

    public void OnNext(float leftHandX, float leftHandY, float rightHandX, float rightHandY)
    {
        GestureManager.Piano.SetGestureValues(leftHandX, leftHandY);
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
        SetGesture();
    }


    void SetGesture()
    {
        var color = EffectUtility.Vector2ToColor(GestureManager.Piano.Value);
        foreach (var tile in _tiles)
        {
            tile.material.color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GestureActivation.IsGesturing) return;
        SetGesture();
    }


}
