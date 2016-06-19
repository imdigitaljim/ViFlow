using UnityEngine;
using System.Collections;
using JetBrains.Annotations;

public interface IGesturable
{
    void OnStart();
    void OnNext(float leftHandX, float leftHandY, float rightHandX, float rightHandY);
    void OnCompleted();
}
