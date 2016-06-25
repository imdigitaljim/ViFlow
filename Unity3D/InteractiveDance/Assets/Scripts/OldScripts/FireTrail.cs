using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.CoreScripts;

public class FireTrail : MonoBehaviour //, IGesturable
{

 //   private ParticleSystem _fireTrail;

 //   // Use this for initialization
 //   void Start ()
 //   {
 //       GestureActivation.CurrentGesture = this;
 //       _fireTrail = gameObject.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
	//}
	
	//// Update is called once per frame
	//void Update ()
	//{
	//    //if (!GestureActivation.IsGesturing) return;

 //       //If we are gesturing, the fire trail size and lifetime values will be updated.
 //       //If we are not gesturing, we will be picking up the values that were set by the gesture system

 //       _fireTrail.startLifetime = GestureManager.FireTrailPath.StartLifeTime;
 //       _fireTrail.startSize = GestureManager.FireTrailPath.StartSize;

 //   }

 //   public void OnNext(float leftHandX, float leftHandY, float rightHandX, float rightHandY)
 //   {
 //       GestureManager.FireTrailPath.SetSize(leftHandY, rightHandY);
 //       GestureManager.FireTrailPath.SetLifetime(leftHandX, rightHandX);
 //   }

 //   public void OnStart()
 //   {
 //       GestureManager.FireTrailPath.Initialize();
 //   }
 //   public void OnCompleted()
 //   {

 //   }


}
