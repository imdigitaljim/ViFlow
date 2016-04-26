using UnityEngine;
using System.Collections;

public class RandomRoam : MonoBehaviour
{

    private Vector3 _initialPosition;
    private Vector3 _driftLocation;
    public float LockedOnTime = 5f;
    private float _currentLockTime;
    public float RoamSpeed;
	// Use this for initialization
	void Start ()
	{
	    _initialPosition = transform.position;


    }
	
	// Update is called once per frame
	void Update ()
	{

        _currentLockTime += Time.deltaTime;
	    if (_currentLockTime > LockedOnTime || transform.position == _driftLocation)
	    {
            _currentLockTime = 0;
            _driftLocation = _initialPosition + new Vector3(Random.Range(-30, 30), 0, Random.Range(-5, 30));
        }       
        transform.position = Vector3.MoveTowards(transform.position, _driftLocation, RoamSpeed);

    }
}
