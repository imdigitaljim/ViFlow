using UnityEngine;
using System.Collections;
using Assets.Scripts.CoreScripts;

public class RainFog : MonoBehaviour, IGesturable
{
    private ParticleSystem _ps;
	// Use this for initialization
	void Start () {
        GestureActivation.CurrentGesture = this;
	    _ps = gameObject.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!GestureActivation.IsGesturing) return;
	    _ps.startSize = GestureManager.Fog.Size;

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
