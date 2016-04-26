using System;
using UnityEngine;
using System.Collections;

public class MenuEffectGestureControl : MonoBehaviour
{
    private float _current;
    private bool _isActivated = true;
    private MenuManager _menuManager;
	private GameObject _playerLeftHand;
	private GameObject _playerRightHand;
	public GameObject theMenu;
	public GameObject thePlayer;
	public GameObject currentEffectOnDisplay;
	public float xScaleFactor = 0.5f;

	private Transform playerTransform;
	private Transform effectTransform;
    // Use this for initialization
    void Start()
    {
        //_menuManager = transform.parent.gameObject.GetComponent<MenuManager>();
		theMenu = GameObject.FindWithTag("Menu");
		thePlayer = GameObject.FindWithTag("Player");
		//_menuManager = theMenu.transform.parent.gameObject.GetComponent<MenuManager>();
		_menuManager = theMenu.GetComponent<MenuManager>();
		_playerLeftHand = GameObject.Find("LeftHand");//thePlayer.GetComponent<LeftHand>();
		_playerRightHand = GameObject.Find("RightHand");

    }

    // Update is called once per frame
    void Update()
    {
		if (_menuManager.CurrentDisplayEffect != null) 
		{
			currentEffectOnDisplay = _menuManager.CurrentDisplayEffect;
			playerTransform = thePlayer.transform;
			effectTransform = currentEffectOnDisplay.transform;
			//currentEffectOnDisplay.transform.position = new Vector3 (_playerLeftHand.transform.position.x, effectPosition.y, effectPosition.z);


			currentEffectOnDisplay.transform.localScale = new Vector3 (Mathf.Abs (_playerLeftHand.transform.position.x - _playerRightHand.transform.position.x) * xScaleFactor, Mathf.Abs (_playerLeftHand.transform.position.y - _playerRightHand.transform.position.y) * xScaleFactor, effectTransform.localScale.z);
		}
		//Debug.Log ("Menu gestures");

    }

  


}
