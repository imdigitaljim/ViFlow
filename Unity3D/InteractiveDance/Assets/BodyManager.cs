using UnityEngine;
using System.Collections;

public class BodyManager : MonoBehaviour
{

    public ulong Id;
	// Use this for initialization
	void Start ()
    {
	    for (var i = 0; i < transform.childCount; ++i)
	    {
	        transform.GetChild(i).gameObject.GetComponent<JointInfo>().Id = i;
	    }
	}
	
	// Update is called once per frame
	void Update ()
    {

	}
}
