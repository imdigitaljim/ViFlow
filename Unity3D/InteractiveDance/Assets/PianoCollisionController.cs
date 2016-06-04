using UnityEngine;
using System.Collections;

public class PianoCollisionController : MonoBehaviour {

	// Use this for initialization
    private GameObject _colorControl;
	void Start ()
	{
	    _colorControl = transform.parent.GetChild(1).gameObject;
	    var count = 0;
	    foreach (Transform child in transform)
	    {
	        child.gameObject.GetComponent<PianoTrigger>().Id = count++;
	    }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetColors(int childId)
    {
        _colorControl.GetComponent<ColorController>().SetColors(childId);
    }
}
