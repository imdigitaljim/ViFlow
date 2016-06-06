﻿using UnityEngine;
using System.Collections;

public class OptionSelector : MonoBehaviour {

    private float _current;
    private bool _isActivated = false;
    public int id;
    private MenuManager _menuManager;
    // Use this for initialization
    void Start()
    {
        _menuManager = transform.parent.parent.gameObject.GetComponent<MenuManager>();
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
                    Debug.Log("activating");
                    _menuManager.ActivateElement(id);
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
