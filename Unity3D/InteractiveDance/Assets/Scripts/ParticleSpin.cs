using UnityEngine;
using System.Collections;

public class ParticleSpin : MonoBehaviour
{

    public float Speed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, (transform.localEulerAngles.y + Speed) % 360, transform.localEulerAngles.z);
	}
}
