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
            private static float _initialSize, _initialLocationX;
            private const float StartSizeMin = 0.2f, StartSizeMax = 2.5f;
            private static readonly GestureParameter StartSizeGesture = new GestureParameter(StartSizeMin, StartSizeMax);
            public static float StartSize = .3f;


            private const float StartLocationMin = 30, StartLocationMax = 100;
            private static readonly GestureParameter StartLocationGesture = new GestureParameter(StartLocationMin, StartLocationMax);
            public static float StartLocation = 62;

            public static void Initialize()
            {
                _initialSize = StartSize;
                _initialLocationX = StartLocation;
            }

            public static void SetSize(float value)
            {
                StartSize = StartSizeGesture.Clamp(_initialSize + StartSizeGesture.GetUnit() * value);
            }

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


    }
}
