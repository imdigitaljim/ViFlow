using UnityEngine;
using System.Collections;

public class FireworkLauncher : MonoBehaviour {

    public GameObject fx;
    private Transform xform;
    // Use this for initialization
    public float reloadtime = 5f;
    private float currenttime = 0;
    private bool isActivated = false;
	void Start ()
    {
        xform = gameObject.transform;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (isActivated)
        {
            currenttime += Time.deltaTime;
            if (currenttime >= reloadtime)
            {
                isActivated = false;
            }
        }
        
	}
    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player")
        {
            if (!isActivated) 
            {
                currenttime = 0;
                isActivated = true;
                Instantiate(fx, new Vector3(xform.position.x, xform.position.y, xform.position.z), xform.rotation);
            }
        }
    }
}
