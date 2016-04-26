using UnityEngine;
using System.Collections;

public class SandFollow : MonoBehaviour
{
    public int WaitTime = 5;
    private ParticleSystem.EmissionModule _emit;
    private float _currentTime = 0;
    private bool _isInit;
	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
	    //if (!_isInit)
	    //{
	    //    var child = transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;

     //       if (child.activeSelf)
	    //    {
     //           transform.GetChild(2).gameObject.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
     //           _emit = child.GetComponent<ParticleSystem>().emission;
     //           _emit.enabled = false;

     //           _currentTime += Time.deltaTime;
     //           if (_currentTime >= WaitTime)
     //           {
     //               _emit.enabled = true;
     //               _isInit = true;
     //           }
     //       }
     //   }

	}
}
