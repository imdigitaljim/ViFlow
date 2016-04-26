using UnityEngine;
using System.Collections;

public class ParticleDiminish : MonoBehaviour
{

    public float DiminishStartPercent;
    public float DiminishFrequencyInSeconds;
    public float DiminishSpeed;

    private ParticleSystem _emitter;
    private TTL _ttl;
    private float _current;
    private float _diminishSize;
    private float _diminishSpeed;
    private float _startTime;


    // Use this for initialization
    void Start ()
	{
	    _emitter = GetComponent<ParticleSystem>();
        _ttl = GetComponent<TTL>();
        DiminishStartPercent /= 100.0f;
	    var duration = (_ttl.end - _ttl.start) * (1 - DiminishStartPercent);
	    _startTime = (_ttl.end - _ttl.start) - duration;
        _diminishSize = _emitter.startSize / (duration / DiminishFrequencyInSeconds);
        _diminishSpeed = _emitter.startSpeed / (duration / DiminishFrequencyInSeconds) + DiminishSpeed;
    }
	
	// Update is called once per frame
	void Update () {

	    if (_startTime < GlobalTimer.RunningTime)
	    {
            if (_current > DiminishFrequencyInSeconds)
	        {
                if (_emitter.startSpeed > 0)
                {
                    _emitter.startSpeed -= _diminishSpeed;
                }
	            if (_emitter.startSize > 0)
	            {
                    _emitter.startSize -= _diminishSize;
                }
                _current = 0;
            }
	        _current += Time.deltaTime;
	    }
	}


}
