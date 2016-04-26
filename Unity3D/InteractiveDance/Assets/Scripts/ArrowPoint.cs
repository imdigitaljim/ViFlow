using UnityEngine;
using System.Collections;

public class ArrowPoint : MonoBehaviour {

    private GameObject o;
	// Use this for initialization
	void Start () {
        o = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(o.transform);
    }
}
