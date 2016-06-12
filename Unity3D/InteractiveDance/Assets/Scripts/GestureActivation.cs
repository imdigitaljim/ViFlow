using UnityEngine;
using System.Collections;
using System.Globalization;

public class GestureActivation : MonoBehaviour
{

    public static string GUIMessage;
    public static float GestureValue1, GestureValue2, GestureValue3, GestureValue4;
    private bool _isReading, _isGesturing;
    private float _current = 0f;
    private int _nextDisplay = 1;
    public float WaitTime = 5f;
    private GameObject _leftHand, _rightHand;
    private Vector3 _initleftVector, _initrightVector;
    // Use this for initialization
    void Start()
    {
        GUIMessage = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isReading) return;
        if (!_isGesturing)
        {
            if (_current > WaitTime)
            {
                Debug.Log("Gesturing");
                _leftHand = GameObject.Find("LeftHand");
                _rightHand = GameObject.Find("RightHand");
                _initleftVector = _leftHand.transform.position;
                _initrightVector = _rightHand.transform.position;
                ResetReadClock();
                _isGesturing = true;
            }
            else
            {
                _current += Time.deltaTime;
                if ((int)_current != _nextDisplay) return;

                GUIMessage = (WaitTime - _nextDisplay).ToString();
                _nextDisplay++;
            }
        }
        else
        {
            if (_current > WaitTime)
            {
                _isGesturing = false;
                _isReading = false;
                GUIMessage = string.Empty;
            }
            else
            {
                ReadGesture();
                _current += Time.deltaTime;
                if ((int)_current != _nextDisplay) return;

                GUIMessage = (WaitTime - _nextDisplay).ToString();
                _nextDisplay++;
            }

        }
    
  
    }

    void ReadGesture()
    {
        var newLh = _leftHand.transform.position;
        var newRh = _rightHand.transform.position;
        GestureValue1 = (_initleftVector.x - newLh.x);
        GestureValue2 = (_initleftVector.y - newLh.y);
        GestureValue3 = (_initrightVector.y - newRh.y);
        GestureValue4 = (_initrightVector.y - newRh.y);
    }

    void ResetReadClock()
    {
        _current = 0;
        _nextDisplay = 1;
        GUIMessage = string.Empty;
        GestureValue1 = GestureValue2 = GestureValue3 = GestureValue4 = 0;
    }


    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Player")
        {
            _isReading = true;
            GUIMessage = WaitTime.ToString();
        }

    }

    void OnGUI()
    {
        if (GUIMessage != string.Empty)
        {
            GUI.Label(new Rect(1000, 100, 150, 100), string.Format("Countdown: {0}", GUIMessage));
        }
        if (!_isGesturing) return;     
        GUI.Label(new Rect(1000, 150, 150, 100), string.Format("Left X:{0}", GestureValue1.ToString()));
        GUI.Label(new Rect(1000, 200, 150, 100), string.Format("Left Y:{0}", GestureValue2.ToString()));
        GUI.Label(new Rect(1200, 150, 150, 100), string.Format("Right X:{0}", GestureValue3.ToString()));
        GUI.Label(new Rect(1200, 200, 150, 100), string.Format("Right Y:{0}", GestureValue4.ToString()));
        

    }
}
