using UnityEngine;
using System.Collections;

public class ObjectKiller : MonoBehaviour {

    public float hangtime = 3f;
    private float current = 0; 
	// Use this for initialization
	void Start () {
        transform.rotation = new Quaternion(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z, transform.localRotation.w);
	
	}
	
	// Update is called once per frame
	void Update () {
        current += Time.deltaTime;
        if (current >= hangtime)
        {
            Destroy(this.gameObject);
        }
	}
}
