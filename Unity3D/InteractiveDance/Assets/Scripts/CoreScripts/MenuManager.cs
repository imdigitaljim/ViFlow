using System;
using UnityEngine;
using System.Collections;


public class MenuManager : MonoBehaviour
{
    //singleton data

    public static readonly int TextureCount = 9;
    public static readonly int OptionMax = 3;
    public readonly int TimeToActivate = 1;
    public int CurrentRange = 0;

	public static GameObject CurrentDisplayEffect;
    public static int CurrentDisplayIndex;
    private GameObject Effect, _attachedEffectLeft, _attachedEffectRight, _attachedEffectBody;
    public Texture[] TextureList = new Texture[TextureCount];
    public GameObject[] PrefabToActivate = new GameObject[TextureCount];
    public bool[] IsAttached = new bool[TextureCount];
    public bool[] HandEffect = new bool[TextureCount];
    private readonly GameObject[] _options = new GameObject[OptionMax]; //this is the list of objections
    

    private int _newRange = 0;
    private bool _hasChanged = true;

    // Use this for initialization
    void Start ()
	{
	    Effect = GameObject.Find("ActiveEffect");
        for (var i = 0; i < OptionMax; i++)
        {
            _options[i] = transform.GetChild(i).gameObject;
	        _options[i].transform.GetChild(0).gameObject.GetComponent<OptionSelector>().id = i;
	    }
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (_hasChanged)
	    {
	        for (var i = 0; i < OptionMax; i++)
	        {
                _options[i].GetComponent<Renderer>().material.mainTexture = TextureList[CurrentRange + i];
            }
            _hasChanged = false;
	    }
	    if (_newRange == CurrentRange)
	    
	    _hasChanged = true;
        _newRange = CurrentRange;
	    

		//Debug.Log("effect Gestures");

	
	}

    public void ActivateElement(int x)
    {
        _attachedEffectLeft = GameObject.Find("AttachedEffectLeft");
        _attachedEffectRight = GameObject.Find("AttachedEffectRight");
        _attachedEffectBody = GameObject.Find("AttachedEffectBody");
        try
		{
        	foreach (Transform child in Effect.transform)
        	{
        	    Destroy(child.gameObject);
        	}
        	foreach (Transform child in _attachedEffectLeft.transform)
    	    {
			    Destroy(child.gameObject);
        	}
		    foreach (Transform child in _attachedEffectRight.transform)
		    {
		        Destroy(child.gameObject);
		    }
            foreach (Transform child in _attachedEffectBody.transform)
            {
                Destroy(child.gameObject);
            }
        }
		catch (System.Exception e)
		{
			Debug.Log(e.ToString());
		}


		Destroy(CurrentDisplayEffect);

        if (IsAttached[x + CurrentRange])
        {
            try
            {
                var objVector = PrefabToActivate[(x + CurrentRange)].transform.position;
                if (HandEffect[x + CurrentRange])
                {
                    _attachedEffectLeft = GameObject.Find("AttachedEffectLeft");
                    _attachedEffectRight = GameObject.Find("AttachedEffectRight");
                    var leftPos = _attachedEffectLeft.transform.parent.transform.position;
                    var rightPos = _attachedEffectRight.transform.parent.transform.position;
                    var obj = (GameObject)Instantiate(PrefabToActivate[(x + CurrentRange)], objVector + leftPos + Vector3.right, Quaternion.identity);
                    var obj2 = (GameObject)Instantiate(PrefabToActivate[(x + CurrentRange)], objVector + rightPos + Vector3.right, Quaternion.identity);

                    obj.transform.parent = _attachedEffectLeft.transform;
                    obj2.transform.parent = _attachedEffectRight.transform;
                }
                else
                {

                    _attachedEffectBody = GameObject.Find("AttachedEffectBody");
                    var p = _attachedEffectBody.transform.parent.transform.position;
                    var obj = (GameObject)Instantiate(PrefabToActivate[(x + CurrentRange)], objVector + p + Vector3.right, Quaternion.identity);
                    CurrentDisplayEffect = obj;
                    CurrentDisplayIndex = x + CurrentRange;
                    obj.transform.parent = _attachedEffectBody.transform;
                }

            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }
        else
        {
            var obj = Instantiate(PrefabToActivate[(x + CurrentRange)]);
			CurrentDisplayEffect = obj;
            CurrentDisplayIndex = x + CurrentRange;
            obj.transform.parent = Effect.transform;
        }

    }

}
