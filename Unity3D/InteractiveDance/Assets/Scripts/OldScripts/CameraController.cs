using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public bool is2D = true;
    private bool was2D = false;
    public Vector3 Camera3D; // = new Vector3(0, 21.2f, -38.4f);
    public Vector3 Camera3DRotation; // = new Vector3(18.3f, 0, 0);
    public Vector3[] TransitionPoints = new Vector3[5];
    public Vector3[] TransitionRotation = new Vector3[5];
    public Vector3 camera2D = new Vector2(0, 20.4f);
    private GameObject _environment;
    private float _current;
    private int _currentPoint;
    public float TransitionTime;
    public float TransitionSpeed;
    private bool _isTransitioning = false;
    // Use this for initialization

    void Start () {
       _environment = GameObject.Find("Environment");
    }

    void Update()
    {
        if (!_isTransitioning)
        {
            AwaitEvent();
        }
        if (_isTransitioning)
        {
            Transition();
        }
    }

    void AwaitEvent()
    {
        if (is2D && !was2D)
        {
            _currentPoint = 0;
            _isTransitioning = true;
        }
        else if (!is2D && was2D)
        {
            _currentPoint = TransitionPoints.Length - 1;
            _isTransitioning = true;
            GetComponent<Camera>().orthographic = false;
        }
        was2D = is2D;
    }
	// Update is called once per frame
    void Transition()
    {
        _current += Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, TransitionPoints[_currentPoint], TransitionSpeed);
        transform.localEulerAngles = Vector3.MoveTowards(transform.localEulerAngles, TransitionRotation[_currentPoint], TransitionSpeed);
        if (_current > TransitionTime)
        {
            if (is2D)
            {
                _currentPoint++;
            }
            else
            {
                _currentPoint--;
            }
            if (is2D && _currentPoint > TransitionPoints.Length - 1)
            {
                _isTransitioning = false;
                gameObject.GetComponent<Camera>().orthographic = true;
                gameObject.transform.position = camera2D;
            }
            if (!is2D && _currentPoint < 0)
            {
                _isTransitioning = false;
                transform.position = Camera3D;
                transform.localEulerAngles = Camera3DRotation;
            }
            _current = 0;
        }
    }

}
