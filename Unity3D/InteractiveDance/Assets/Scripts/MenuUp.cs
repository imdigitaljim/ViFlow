using System;
using UnityEngine;
using System.Collections;

public class MenuUp : MonoBehaviour
{
    private float _current;
    private bool _isActivated = false;
    private MenuManager _menuManager;
    // Use this for initialization
    void Start()
    {
        _menuManager = transform.parent.gameObject.GetComponent<MenuManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider c)
    {
        if (c.gameObject.tag == "Player")
        {
            if (!_isActivated)
            {
                _current += Time.deltaTime;
                if (_current > _menuManager.TimeToActivate)
                {
                    var temp = (_menuManager.CurrentRange - 3);
                    if (temp < 0)
                    {
                        temp += MenuManager.TextureCount;
                    }
                    _menuManager.CurrentRange = temp % MenuManager.TextureCount;

                    _isActivated = true;
                }
            }

        }

    }

    void OnTriggerExit(Collider c)
    {
        if (c.gameObject.tag == "Player")
        {
            _current = 0;
            _isActivated = false;
        }
    }
}
