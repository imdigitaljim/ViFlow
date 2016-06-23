using UnityEngine;
using System.Collections;
using System.Globalization;

public class GestureActivation : MonoBehaviour
{

    public static IGesturable CurrentGesture;
    private static IGesturable LastUpdate;
    public static string GUIMessage;
    private float _leftHandX, _leftHandY, _rightHandX, _rightHandY;
    public static bool IsReading, IsGesturing;
    private float _current, _lastUpdate;
    private int _nextDisplay = 1;
    public float WaitTime, XModifier, YModifier, UpdateTime;
    private GameObject _leftHand, _rightHand;
    private Vector3 _initleftVector, _initrightVector;
    private Renderer _quad;
    // Use this for initialization
    void Start()
    {
        _quad = transform.parent.gameObject.GetComponent<Renderer>();
        _quad.material.color = Color.green;
        GUIMessage = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        if (LastUpdate != CurrentGesture) EndGesture();
        LastUpdate = CurrentGesture;
        if (CurrentGesture == null || !IsReading) return;//if not set or activated from touching
        if (!IsGesturing) //activated but not beginning to detect gestures
        {
            if (_current > WaitTime) 
            {
                BeginGesture();//begin gestures, initialize points, begin updating UI info
            }
            else 
            {
                BeginPreGestureSetup(); //wait period to let user return hands to normal position
            }
        }
        else
        {             
            ReadGesture();//activated and reading gestures     
        }
    }

    void BeginPreGestureSetup()
    {
        _current += Time.deltaTime;
        if ((int)_current != _nextDisplay) return;
        GUIMessage = (WaitTime - _nextDisplay).ToString();
        _nextDisplay++;
    }
    void BeginGesture()
    {
        _leftHand = GameObject.Find("LeftHand");
        _rightHand = GameObject.Find("RightHand");
        _initleftVector = _leftHand.transform.position;
        _initrightVector = _rightHand.transform.position;
        ResetReadClock();
        IsGesturing = true;
        _quad.material.color = Color.red;
        CurrentGesture.OnStart();
    }
    void EndGesture()
    {
        IsGesturing = false;
        IsReading = false;
        ResetReadClock();
        _quad.material.color = Color.green;
    }

    void ReadGesture()
    {
        var newLh = _leftHand.transform.position;
        var newRh = _rightHand.transform.position;
        _leftHandX = Mathf.Clamp((_initleftVector.x - newLh.x) * 100 / XModifier, -100, 100);
        _leftHandY = Mathf.Clamp((_initleftVector.y - newLh.y) * 100 / YModifier * -1, -100, 100); //- 1 reverse coordinate system
        _rightHandX = Mathf.Clamp((_initrightVector.x - newRh.x) * 100 / XModifier, -100, 100);
        _rightHandY = Mathf.Clamp((_initrightVector.y - newRh.y) * 100 / YModifier * -1, -100, 100);//- 1 reverse coordinate system
        CurrentGesture.OnNext(_leftHandX, _leftHandY, _rightHandX, _rightHandY);
    }

    void ResetReadClock()
    {
        _current = _lastUpdate = 0;
        _nextDisplay = 1;
        GUIMessage = string.Empty;
        _leftHandX = _leftHandY = _rightHandX = _rightHandY = 0;
    }


    void OnTriggerEnter(Collider c)
    {
        if (CurrentGesture == null || c.gameObject.tag != "Player") return; //no gesture set, not the player
        var joint = c.gameObject.GetComponent<JointInfo>();
        if (joint == null || joint.Id != (int)JointType.HandRight) return; //another player tag but not the right joint
        if (!IsReading)
        {
            IsReading = true;
            _quad.material.color = Color.yellow;
            GUIMessage = WaitTime.ToString();
        }
        else if (IsGesturing)
        {
            EndGesture();
            CurrentGesture.OnCompleted();
        }
    }

    void OnGUI()
    {
        if (GUIMessage != string.Empty)
        {
            GUI.Label(new Rect(1000, 100, 150, 100), string.Format("Countdown: {0}", GUIMessage));
        }
        //if (!IsGesturing) return;     
        //GUI.Label(new Rect(1000, 150, 150, 100), string.Format("Left X:{0}", _leftHandX.ToString()));
        //GUI.Label(new Rect(1000, 200, 150, 100), string.Format("Left Y:{0}", _leftHandY.ToString()));
        //GUI.Label(new Rect(1200, 150, 150, 100), string.Format("Right X:{0}", _rightHandX.ToString()));
        //GUI.Label(new Rect(1200, 200, 150, 100), string.Format("Right Y:{0}", _rightHandY.ToString()));
        

    }
}
