using UnityEngine;
using System.Collections;
using System.Linq;

public class TimeSand : MonoBehaviour
{

    public static int Hits = 0;
    public int ShrinkIncrement = 5;
    public float ShrinkSize = .03f;
    public int AbsorbTime = 200;
    public GameObject[] DancerSandWaterFalls;
    private ParticleSystem.ShapeModule _width;
    private GameObject _parent;
    public int CurrentShrink;
	public bool SingleDancer = true;
    [Range(-10,10)] public int LastAbsorbTime = 0;
    // Use this for initialization
    void Start ()
	{
        DancerSandWaterFalls = GameObject.FindGameObjectsWithTag("Dancer").ToArray();
        Debug.Log(string.Format(@"Found {0} dancers", DancerSandWaterFalls.Length));
	    _parent = transform.parent.gameObject;
        CurrentShrink = 0;
        _width = gameObject.GetComponent<ParticleSystem>().shape;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (DancerSandWaterFalls.Length > 0 && SingleDancer)
	    {
            transform.position = new Vector3(transform.position.x, transform.position.y, DancerSandWaterFalls[0].transform.position.z);
        }
	    if (Hits > AbsorbTime && CurrentShrink < ShrinkIncrement)
	    {
            _width.radius -= ShrinkSize;
            CurrentShrink += 1;
	        Hits = 0;
	        AbsorbTime = (int)(AbsorbTime * 1.5f);
	    }
	    if (CurrentShrink == ShrinkIncrement && Hits == AbsorbTime + (LastAbsorbTime * 100))
	    {
            _parent.SetActive(false);
	        foreach (var dancer in DancerSandWaterFalls)
	        {
                dancer.transform.GetChild(0).gameObject.SetActive(true);
                dancer.transform.GetChild(1).gameObject.SetActive(true);
            }
            Destroy(gameObject.transform.parent.gameObject);
	    }
	}
}
