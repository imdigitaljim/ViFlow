using UnityEngine;
using System.Collections;

public class PianoCollisionController : MonoBehaviour {

	// Use this for initialization
    private GameObject _colorControl;
	void Start ()
	{
	    _colorControl = transform.parent.GetChild(1).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetColors()
    {
        _colorControl.GetComponent<ColorController>().SetColors();
    }
}
