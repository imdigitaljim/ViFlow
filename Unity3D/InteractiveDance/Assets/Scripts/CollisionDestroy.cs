using UnityEngine;
using System.Collections;

public class CollisionDestroy : MonoBehaviour {


    private Transform xform;
    private bool isDestroyed = false;
    public float hangtime;
    public float speed;
    public float shrink;
    private float currentTime = 0;
	// Use this for initialization
	void Start () {
        xform = gameObject.transform;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isDestroyed)
        {
            if (currentTime >= hangtime)
            {
                Destroy(gameObject);
            }
            xform.localScale = new Vector3(xform.localScale.x - shrink, xform.localScale.y - shrink, xform.localScale.z - shrink);
            xform.position = new Vector3(xform.position.x, xform.position.y + speed, xform.position.z);
            currentTime += Time.deltaTime;
        }
        xform.position = new Vector3(xform.position.x, xform.position.y, xform.position.z);

    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Player")
        {
            isDestroyed = true;
        }
    }

}
