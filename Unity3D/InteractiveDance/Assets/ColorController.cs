using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorController : MonoBehaviour {

	// Use this for initialization
    private List<GameObject> _colorQuads;
	void Start ()
    {
        _colorQuads = new List<GameObject>();
	    for (var i = 0; i < transform.childCount; i++)
	    {
	        _colorQuads.Add(transform.GetChild(i).gameObject);
	    }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetColors()
    {
        var r = new System.Random();
        var howMany = r.Next(0, _colorQuads.Count);
        var whichOnes = new List<int>();
        for (var i = 0; i < howMany; i++)
        {
            while (true)
            {
                var thisOne = r.Next(0, _colorQuads.Count);
                if (!whichOnes.Contains(thisOne))
                {
                    whichOnes.Add(thisOne);
                    break;
                }
            }
        }
        foreach (var obj in whichOnes)
        {
            var newColor = new Color(Random.value, Random.value, Random.value, 1.0f);
            _colorQuads[obj].GetComponent<Renderer>().material.color = newColor;
        }
    }
}
