using UnityEngine;
using System.Collections;

public class TTL : MonoBehaviour
{

    public float start;
    public float end;
    public bool attached;
	// Use this for initialization
	void Start ()
	{
	    var ttl = transform.parent.gameObject.GetComponent<TTL>();

        if (ttl != null)
        {
            start = ttl.start;
            end = ttl.end;
        }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
