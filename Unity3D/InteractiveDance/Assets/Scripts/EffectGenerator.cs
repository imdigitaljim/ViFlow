using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions.Must;

public class EffectGenerator : MonoBehaviour
{
    public List<EffectBlock> EffectList;

    private float _fpTolerance = .5f;
    private float _currentTime = 0;
    private ParticleEffectController _pec;
    // Use this for initialization
    void Start()
    {
        _pec = GetComponent<ParticleEffectController>();
        EffectList = new List<EffectBlock>();
        var list = transform.GetChild(0).gameObject.transform;
        var production = new List<GameObject>();
        for(var i = 0; i < list.childCount; i++)
        {
            production.Add(list.GetChild(i).gameObject);
        }
        var id = 0;
        foreach (var effect in production)
        {
            var ttl = effect.gameObject.GetComponent<TTL>();
            EffectList.Add(new EffectBlock
            {
                GObject = effect,
                Position = effect.transform.position,
                Rotation = effect.transform.rotation,
                Scale = effect.transform.localScale,
                StartTime = ttl.start,
                EndTime = ttl.end,
                IsAttached =  ttl.attached,
                Id = id++
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalTimer.RunningTime > _currentTime)
        {
            UpdateEffects();
            _currentTime++;
        }
    }

    void UpdateEffects()
    {
        for (var i = 0; i < EffectList.Count; i++)
        {
            if (EffectList[i].EndTime < GlobalTimer.RunningTime)
            {
                Destroy(EffectList[i].GObject);
                EffectList.RemoveAt(i--);
            }
            else if (!EffectList[i].GObject.activeSelf && EffectList[i].StartTime < GlobalTimer.RunningTime)
            {
                var e = EffectList[i];
                EffectList[i].GObject.SetActive(true);
                if (e.IsAttached)
                {
                    if (FormController.bodies == null) break;
                    foreach (var dancer in FormController.bodies)
                    { 

                        SetCurrentEffect(dancer);
                    }
                    _pec.CheckEffects();
                    _pec.UpdateSpeed();
                }

            }
        }

    }



    public void SetCurrentEffect(Form o)
    {
        foreach (var e in EffectList)
        {
            if (e.IsAttached && e.StartTime < GlobalTimer.RunningTime)
            {
                Debug.Log("Activating attached");
                var activeEffects = o.Root.transform.GetChild(2).gameObject.transform;
                var effCount = activeEffects.childCount;
                for (var i = 0; i < effCount; i++)
                {
                    var eff = activeEffects.GetChild(i).gameObject;
                    if (eff.GetComponent<Identifier>().Id == e.Id && !eff.activeSelf)
                    {
                        eff.SetActive(true);
                        break;
                    }
                }
            }
        }
    }
}
