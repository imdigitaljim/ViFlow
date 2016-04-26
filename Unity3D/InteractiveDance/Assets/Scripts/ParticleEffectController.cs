using UnityEngine;
using System.Collections;

public class ParticleEffectController : MonoBehaviour
{


    public bool ReverseGravity;
    public bool Looping;
    public bool StopTime;
    public bool ConfirmSet;
    private bool isLooping;

    private int _lastSpeedUpdate;
    private float _frequency = 1;
    private float _freqUpdate;
    [Range(0, 20)] public int Speed = 10;
    public int CustomId;
    public float CurrentTime;
    // Use this for initialization
    void Start ()
	{
        //_lastSpeedUpdate = Speed;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentTime = GlobalTimer.RunningTime;
        if (ConfirmSet)
        {
            CheckEffects();
            ConfirmSet = false;
        }
        if (_lastSpeedUpdate != Speed)
        {
            UpdateSpeed();
            _lastSpeedUpdate = Speed;
        }
        if (isLooping)
        {  
            _freqUpdate += Time.deltaTime;
            if (_freqUpdate > _frequency)
            {
                UpdateLoop();
                _freqUpdate = 0;
            }
        }
    }

    public void UpdateLoop()
    {
        if (FormController.bodies == null) return;
        foreach (var dancer in FormController.bodies)
        {
            var activeEffects = dancer.Root.transform.GetChild(2).gameObject.transform;
            var effect = activeEffects.GetChild(0).gameObject;
            if (effect != null && effect.GetComponent<Identifier>().Id == CustomId)
            {
                var effectPs = effect.GetComponent<ParticleSystem>();
                if (effectPs.gravityModifier < 0)
                {
                    effect.transform.localPosition = new Vector3(0, 36.95f, 0);
                    effect.transform.localEulerAngles = new Vector3(0, 180, 180);
                    effectPs.gravityModifier = .5f;
                }
                else
                {
                    effect.transform.localPosition = new Vector3(0, 2, 0);
                    effect.transform.localEulerAngles = Vector3.zero;
                    effectPs.gravityModifier = -.5f;
                }                  
            }
            
        }

    }

    public void UpdateSpeed()
    {
        if (FormController.bodies == null) return;
        foreach (var dancer in FormController.bodies)
        {
            var activeEffects = dancer.Root.transform.GetChild(2).transform;
            for (var i = 0; i < activeEffects.childCount; i++)
            {
                var effect = activeEffects.GetChild(i).gameObject;
                if (effect != null && effect.GetComponent<Identifier>().Id == CustomId)
                {
                    effect.GetComponent<ParticleSystem>().playbackSpeed = Speed * .1f;
                    break;
                }
            }
        }
    }

    public void CheckEffects()
    {
        if (FormController.bodies == null) return;
        foreach (var dancer in FormController.bodies)
        {
            var activeEffects = dancer.Root.transform.GetChild(2).gameObject.transform;
            for (var i = 0; i < activeEffects.childCount; i++)
            {
                var effect = activeEffects.GetChild(i).gameObject;
                if (effect.GetComponent<Identifier>().Id == CustomId)
                {
                    var effectPs = effect.GetComponent<ParticleSystem>();
                    if (ReverseGravity && effectPs.gravityModifier > 0)
                    {
                        effect.transform.localPosition = new Vector3(0,2,0);
                        effect.transform.localEulerAngles = Vector3.zero;
                        effectPs.gravityModifier = -.5f;
                    }
                    else if (!ReverseGravity && effectPs.gravityModifier < 0)
                    {
                        effect.transform.localPosition = new Vector3(0, 36.95f, 0);
                        effect.transform.localEulerAngles = new Vector3(0, 180, 180);
                        effectPs.gravityModifier = .5f;
                    }
                    if (effectPs.isPlaying && StopTime)
                    {
                        effectPs.Pause();
                    }
                    else if (effectPs.isPaused && !StopTime)
                    {
                        effectPs.Play();
                    }
                    isLooping = Looping;
                    break;
                }
            }
        }
    }


}
