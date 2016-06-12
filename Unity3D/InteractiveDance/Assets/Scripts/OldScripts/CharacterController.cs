using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

    Transform obj;
    [Range(0,1)]public float Speed = .5f;
    public bool ZisOn = false;
    public bool XisOn = true;
	// Use this for initialization
	void Start () {
        obj = gameObject.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        
	    if (Input.GetKey("a") && XisOn)
        {
            obj.position = new Vector3(obj.position.x - Speed, obj.position.y, obj.position.z);
        }
        if (Input.GetKey("d") && XisOn)
        {
            obj.position = new Vector3(obj.position.x + Speed, obj.position.y, obj.position.z);
        }
        if (Input.GetKey("w") && ZisOn)
        {
            obj.position = new Vector3(obj.position.x, obj.position.y , obj.position.z + Speed);
        }
        if (Input.GetKey("s") && ZisOn)
        {
            obj.position = new Vector3(obj.position.x, obj.position.y , obj.position.z - Speed);
        }
    }


}
