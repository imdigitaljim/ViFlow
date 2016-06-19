using UnityEngine;
using System.Collections;

public class CrackEffect : MonoBehaviour
{

    private GameObject cracking;
    private GameObject hourglass;
    public bool IsCracking;
    public bool Reset;
    private int currentCrack;
    public float delayBetween = .5f;
    private float current;
	// Use this for initialization
	void Start ()
	{

	    cracking = GameObject.Find("Cracking");
        hourglass = GameObject.Find("HourGlass");
	}
	
	// Update is called once per frame
	void Update () {

	    if (IsCracking)
	    {
	        current += Time.deltaTime;
	        if (current > delayBetween)
	        {
	            if (currentCrack < cracking.transform.childCount)
	            {
	                cracking.transform.GetChild(currentCrack++).gameObject.SetActive(true);
	            }
	            else
	            {
	                for (var i = 0; i < cracking.transform.childCount; i++)
	                {
                        cracking.transform.GetChild(i).gameObject.SetActive(false);
                    }
                    hourglass.SetActive(false);
                }
	            current = 0;
	        }
	    }
	    if (Reset)
	    {
	        IsCracking = false;
	        Reset = false;
	        currentCrack = 0;
            hourglass.SetActive(true);
	    }

	}
}
