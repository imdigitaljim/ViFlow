using UnityEngine;
using System.Collections;

public class WaterFollow : MonoBehaviour {

    private int x;
    private int z;
    private bool isActive = false;
    private float hangTime = 5f;
    private float current = 0;
    Transform o;
    // Use this for initialization
    void Start() {
        o = gameObject.GetComponentInParent<Transform>();
        x = (int)o.position.x;
        z = (int)o.position.z;
        ToggleVisible();
    }
    void ToggleVisible()
    {
        gameObject.GetComponent<ParticleRenderer>().enabled = isActive;
    }
    // Update is called once per frame
    void Update()
    {
        if ((int)o.position.x != x)
        {
            Debug.Log("changing!");
            ToggleVisible();
            isActive = true;
            current = 0;
        }
        else if (isActive)
        {
            current += Time.deltaTime;
            if (current >= hangTime)
            {
                isActive = false;
                ToggleVisible();
            }
            
        }
        x = (int)o.position.x;


    }
}
