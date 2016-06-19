using UnityEngine;
using System.Collections;

public class MenuTransitionGesture : MonoBehaviour {

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
                var movingUp = exitY - activatedY > 0;
                //Debug.Log("Moving!");
                if (movingUp)
                {
                    //Debug.Log("Up!");
                    var temp = (_menuManager.CurrentRange - 3);
                    if (temp < 0)
                    {
                        temp += MenuManager.TextureCount;
                    }
                    _menuManager.CurrentRange = temp % MenuManager.TextureCount;
                }
                else
                {
                    //Debug.Log("Down!");
                    _menuManager.CurrentRange = (_menuManager.CurrentRange + 3) % MenuManager.TextureCount;
                }
            }
            _current = 0;
            _isActivated = false;
        }
    }
}
