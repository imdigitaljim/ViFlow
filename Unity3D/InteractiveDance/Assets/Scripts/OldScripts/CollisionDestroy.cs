using UnityEngine;
using System.Collections;

public class CollisionDestroy : MonoBehaviour {



    private bool _isDestroyed;
    public float Hangtime;
    public float Speed;
    public float ShrinkRate;
    private float _currentTime;
	// Use this for initialization
	void Start () {
 
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (_isDestroyed)
        {
            if (_currentTime >= Hangtime)
            {
                Destroy(gameObject);
            }
            transform.localScale = new Vector3(transform.localScale.x - ShrinkRate, transform.localScale.y - ShrinkRate, transform.localScale.z - ShrinkRate);
            transform.position = new Vector3(transform.position.x, transform.position.y + Speed, transform.position.z);
            _currentTime += Time.deltaTime;
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Player")
        {
            _isDestroyed = true;
        }
    }

}
