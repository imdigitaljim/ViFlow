using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.CoreScripts
{
    public static class GestureManager
    {
        public static class SandFall
        {
            private static float _initialSize;
            private const float StartSizeMin = 0.2f, StartSizeMax = 2.5f;
            private static readonly GestureParameter StartSizeGesture = new GestureParameter(StartSizeMin, StartSizeMax);
            public static float StartSize = .3f;


            private static float _initialLocationX;
            private const float StartLocationMin = 30, StartLocationMax = 100;
            private static readonly GestureParameter StartLocationGesture = new GestureParameter(StartLocationMin, StartLocationMax);
            public static float StartLocation = 62;

            public static void Initialize()
            {
                _initialSize = StartSize;
                _initialLocationX = StartLocation;
            }

            //value here comes in from left hand y coordinate
            public static void SetSize(float value)
            {
                StartSize = StartSizeGesture.Clamp(_initialSize + StartSizeGesture.GetUnit() * value);
            }

            //value here comes in from left hand x coordinate
            public static void SetLocation(float value)
            {
                StartLocation = StartLocationGesture.Clamp(_initialLocationX - StartLocationGesture.GetUnit() * value);
            }
        }

        public static class Piano
        {
            private static float _initialX = 1, _initialY = 1;
            private const float StartXMin = 0, StartXMax = 1;
            private static readonly GestureParameter XGesture = new GestureParameter(StartXMin, StartXMax);
            public static float X = 1;


            private const float StartYMin = 0, StartYMax = 1;
            private static readonly GestureParameter YGesture = new GestureParameter(StartYMin, StartYMax);
            public static float Y = 1;

            public static void Initialize()
            {
                _initialX = X;
                _initialY = Y;
            }

            public static void SetX(float value)
            {
                X = XGesture.Clamp(_initialX + XGesture.GetUnit() * value);
            }

            public static void SetY(float value)
            {
                Y = YGesture.Clamp(_initialY - YGesture.GetUnit() * value);
            }
        }

        public static class BounceBall
        {
            private static Vector3 previousRightHandPostion;
            private static Vector3 previousLeftHandPostion;

            public static Vector3 forceVector;

            private static float xSpeedFactor;
            private const float xSpeedMin = -500, xSpeedMax = 500;
            private static readonly GestureParameter SpeedGesture = new GestureParameter(xSpeedMin, xSpeedMax);

            public static void Initialize()
            {
                forceVector = new Vector3(0, 0, 0);
                previousRightHandPostion = new Vector3(0, 0, 0);
                previousLeftHandPostion = new Vector3(0, 0, 0);
                xSpeedFactor = 0;
            }

            public static void SetSpeedUpFactor(float leftHandX, float leftHandY, float rightHandX, float rightHandY)
            {
                //We will set a speed up factor by comarping the current hand positions to the previous hand positions
                //This gives a rough estimate of the speed of movement of the player
                float xdistance = Vector3.Distance((new Vector3(leftHandX, leftHandY, 0)), previousLeftHandPostion);
                xdistance = SpeedGesture.Clamp(xSpeedFactor + SpeedGesture.GetUnit() * xdistance);

                float ydistance = Vector3.Distance((new Vector3(rightHandX, rightHandY, 0)), previousRightHandPostion);
                ydistance = SpeedGesture.Clamp(xSpeedFactor + SpeedGesture.GetUnit() * ydistance);

                //Update the hand positions for the next iteration
                previousRightHandPostion.x = rightHandX;
                previousRightHandPostion.y = rightHandY;
                previousLeftHandPostion.x = leftHandX;
                previousLeftHandPostion.y = leftHandY;

                //Set the force vector for the ball
                forceVector.x = xdistance;
                forceVector.y = ydistance;

            }


        }


    }
}
