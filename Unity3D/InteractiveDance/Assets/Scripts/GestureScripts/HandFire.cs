using UnityEngine;
using System.Collections;
using Assets.Scripts.CoreScripts;

public class HandFire : MonoBehaviour, IGesturable
{

    private ParticleSystem _leftHandFire;
    private ParticleSystem _rightHandFire;
    // Use this for initialization
    void Start()
    {
        GestureActivation.CurrentGesture = this;
        SetHandObjects();
        SetGesture();
    }

    void SetGesture()
    {
        _leftHandFire.startSize = GestureManager.HandFire.Size;
        _rightHandFire.startSize = GestureManager.HandFire.Size;
        _leftHandFire.startLifetime = GestureManager.HandFire.Decay;
        _rightHandFire.startLifetime = GestureManager.HandFire.Decay;
    }

    void SetHandObjects()
    {
        _leftHandFire = GameObject.Find("LeftHand")
        .transform
        .GetChild(0) //ActiveEffects
        .GetChild(0) //PrefabFire
        .GetChild(0) //Containing GameObject
        .gameObject.GetComponent<ParticleSystem>();
        _rightHandFire = GameObject.Find("RightHand")
        .transform
        .GetChild(0)
        .GetChild(0)
        .GetChild(0)
        .gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GestureActivation.IsGesturing) return;
        SetGesture();
    }

    public void OnStart()
    {
        GestureManager.HandFire.Initialize();
    }

    public void OnNext(float leftHandX, float leftHandY, float rightHandX, float rightHandY)
    {
        GestureManager.HandFire.SetSize(leftHandY);
        GestureManager.HandFire.SetDecay(leftHandX);
    }

    public void OnCompleted()
    {

    }
}
