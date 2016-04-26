using UnityEngine;
using System.Collections;


public class MenuManager : MonoBehaviour
{
    //singleton data

    public static readonly int TextureCount = 9;
    public static readonly int OptionMax = 3;
    public readonly int TimeToActivate = 1;
    public int CurrentRange = 0;

	public GameObject CurrentDisplayEffect;
    private GameObject Effect, AttachedEffect;
    public Texture[] TextureList = new Texture[TextureCount];
    public GameObject[] PrefabToActivate = new GameObject[TextureCount];
    public bool[] IsAttached = new bool[TextureCount];
    private GameObject[] _options = new GameObject[OptionMax];
    

    private int _newRange = 0;
    private bool _hasChanged = true;

    // Use this for initialization
    void Start ()
	{
	    Effect = GameObject.Find("ActiveEffect");
        AttachedEffect = gameObject.transform.GetChild(gameObject.transform.childCount - 1).gameObject;
        for (var i = 0; i < OptionMax; i++)
	    {
            _options[i] = transform.GetChild(i).gameObject;
	        _options[i].GetComponent<OptionSelector>().id = i;
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
	    if (_newRange != CurrentRange)
	    {
	        _hasChanged = true;
            _newRange = CurrentRange;
	    }

		//Debug.Log("effect Gestures");

	
	}

    public void ActivateElement(int x)
    {
		try
		{
        	Debug.Log((x + CurrentRange) + " is activated");
        	foreach (Transform child in Effect.transform)
        	{
				//if(child != null)
					{Destroy(child.gameObject);}
        	}
        	foreach (Transform child in AttachedEffect.transform)
    	    {
				//if(child != null)
					{Destroy(child.gameObject);}
        	}
		}
		catch (System.Exception e)
		{
			print(e.ToString());
		}


		Destroy(CurrentDisplayEffect);

        if (IsAttached[x + CurrentRange])
        {
            var p = transform.parent.transform.position;
            //var m = transform.position;
            var e = Effect.transform.position;
            var objVector = PrefabToActivate[(x + CurrentRange)].transform.position;
            var obj = (GameObject)Instantiate(PrefabToActivate[(x + CurrentRange)], objVector + p + e, Quaternion.identity);
			CurrentDisplayEffect = obj;
            obj.transform.parent = AttachedEffect.transform;
        }
        else
        {
            var obj = Instantiate(PrefabToActivate[(x + CurrentRange)]);
			CurrentDisplayEffect = obj;
            obj.transform.parent = Effect.transform;
        }

    }

}
