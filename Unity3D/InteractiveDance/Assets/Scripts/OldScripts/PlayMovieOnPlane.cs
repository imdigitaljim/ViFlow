using UnityEngine;
using System.Collections;

public class PlayMovieOnPlane : MonoBehaviour {

    public MovieTexture movText;

	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().material.mainTexture = movText;
        movText.Play();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
