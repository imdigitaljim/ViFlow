using UnityEngine;
using System.Collections;

public class CameraEffectController : MonoBehaviour
{

    private CameraController _camera;
    public bool Is2D = true;
    private bool _lastUpdate = true;
    public float[] TransitionTime = new float[5];
    private int _currentTransition;
    private float _current;
    private float _update = 1;
    private TTL _ttl;
    public float ThresholdDistance;
	// Use this for initialization
	void Start () {
	    _camera = GameObject.Find("Main Camera").GetComponent<CameraController>();
	    _ttl = GameObject.Find("EffectList").transform.GetChild(2).GetComponent<TTL>();
	}

    // Update is called once per frame
    void Update()
    {
        if (_lastUpdate != Is2D)
        {
            _camera.is2D = !_camera.is2D;
        }
        _lastUpdate = Is2D;

        if (_currentTransition < TransitionTime.Length && TransitionTime[_currentTransition] < GlobalTimer.RunningTime)
        {
            _currentTransition++;
            _camera.is2D = !_camera.is2D;
        }
 
        if (_ttl.start < GlobalTimer.RunningTime && FormController.bodies != null)
        {
            _current += Time.deltaTime;
            if (_current > _update)
            {
                foreach (var form in FormController.bodies)
                {
                    if (form.IsFront && form.IsFrontMagnitude > ThresholdDistance + 2 && _camera.is2D)
                    {
                        _camera.is2D = false;
                        form.Root.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    }
                    else if (form.IsFront && form.IsFrontMagnitude < ThresholdDistance - 2)
                    {
                        _camera.is2D = true;
                        form.Root.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.SetActive(false);
                    }
                }
                _current = 0;
            }
        }
    }

}
