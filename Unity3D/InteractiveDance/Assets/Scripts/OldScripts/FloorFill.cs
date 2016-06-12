using UnityEngine;
using System.Collections;

public class FloorFill : MonoBehaviour
{

    private GameObject floor;
    private float _runningTime;
    public float FloorMaxY;
	// Use this for initialization
	void Start () {
        floor = GameObject.Find("SandFloor");
	    _runningTime = gameObject.GetComponent<TTL>().start + 5;

	}
	
	// Update is called once per frame
	void Update () {
	    if (GlobalTimer.RunningTime > gameObject.GetComponent<TTL>().start)
        {

            if (_runningTime < GlobalTimer.RunningTime && floor.transform.position.y < FloorMaxY)
            {
                _runningTime += 1;
                floor.transform.position += new Vector3(0,.1f,0);
            }
	    }

    }
}
