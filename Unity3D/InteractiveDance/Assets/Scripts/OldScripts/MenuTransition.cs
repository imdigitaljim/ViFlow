using System;
using UnityEngine;
using System.Collections;

public class MenuTransition : MonoBehaviour
{
    private float _current;
    private bool _isActivated = false;
    private MenuManager _menuManager;
    private float activatedY, exitY, thresholdY;
    // Use this for initialization
    void Start()
    {
        thresholdY = GetComponent<BoxCollider>().size.y;
        _menuManager = transform.parent.gameObject.GetComponent<MenuManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((int) activatedY != 0)
        {
            
        }
    }

    void OnTriggerStay(Collider c)
    {
        if (c.gameObject.tag == "Player")
        {

            if (!_isActivated)
            {
                _isActivated = true;
                activatedY = c.gameObject.transform.position.y;
            }
        }

    }

    void OnTriggerExit(Collider c)
    {
        if (c.gameObject.tag == "Player")
        {
            exitY = c.gameObject.transform.position.y;
            if (Mathf.Abs(exitY - activatedY) > thresholdY - 1)
            {
                Debug.Log("Move Up!");
                var temp = (_menuManager.CurrentRange - 3);
                if (temp < 0)
                {
                    temp += MenuManager.TextureCount;
                }
                _menuManager.CurrentRange = temp % MenuManager.TextureCount;
            }
            _current = 0;
            _isActivated = false;
        }
    }
}
