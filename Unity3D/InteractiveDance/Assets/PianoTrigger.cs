using UnityEngine;
using System.Collections;

public class PianoTrigger : MonoBehaviour {

	// Use this for initialization
    private GameObject _parent;
	void Start ()
	{
	    _parent = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider c)
    {
        _parent.GetComponent<PianoCollisionController>().SetColors();
    }
}
