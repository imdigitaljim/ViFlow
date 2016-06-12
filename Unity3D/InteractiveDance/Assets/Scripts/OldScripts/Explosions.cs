using UnityEngine;
using System.Collections;

public class Explosions : MonoBehaviour
{
    public GameObject prefab;
    private float _current;
    private float _frequency = 3;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    _current += Time.deltaTime;
	    if (_current > _frequency)
	    {
	        Instantiate(prefab, new Vector3(Random.Range(-10, 10), Random.Range(0, 20), Random.Range(-20, 20)),
	            Quaternion.identity);
	        _current = 0;
	    }
	}
}
