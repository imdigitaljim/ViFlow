using UnityEngine;
using System.Collections;

public class BouncyBall : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Player")
        {
            var rb = gameObject.GetComponent<Rigidbody>();
            var x = c.gameObject.transform.position.x < transform.position.x ? 1 : -1;
            var y = c.gameObject.transform.position.y < transform.position.y ? 1 : -1;
            rb.AddForce(new Vector3(x * 300, y * 300,0));
        }
    }
}
