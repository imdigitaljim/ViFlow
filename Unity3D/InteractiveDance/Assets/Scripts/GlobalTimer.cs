using UnityEngine;
using System.Collections;

public class GlobalTimer : MonoBehaviour {

    public static float RunningTime = -1;
    public bool IsStarted = false;
    // Use this for initialization
    void Start()
    {

    }
    void OnGUI()
    {
        GUI.Box(
            new Rect(40, 10, 200, 400),
            string.Format(@"<color={0}>Current Running Time: {1}</color>", "#FFFFFF", RunningTime),
            new GUIStyle() { alignment = TextAnchor.UpperLeft });
    }
    // Update is called once per frame
    void Update()
    {
        if (IsStarted)
        {
            RunningTime += Time.deltaTime;
        }
    }
}
