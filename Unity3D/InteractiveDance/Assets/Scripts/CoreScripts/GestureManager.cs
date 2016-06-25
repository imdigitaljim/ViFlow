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
            private static Vector2 _initial = new Vector2(1, 1);
            public static Vector2 Value = new Vector2(1, 1);
            private const float StartXMin = 0, StartXMax = 1;
            private const float StartYMin = 0, StartYMax = 1;
            private static readonly GestureParameter XGesture = new GestureParameter(StartXMin, StartXMax);
            private static readonly GestureParameter YGesture = new GestureParameter(StartYMin, StartYMax);

            public static void Initialize()
            {
                _initial = Value;
            }

            public static void SetGestureValues(float x, float y)
            {
                var xValue = XGesture.Clamp(_initial.x + XGesture.GetUnit() * x);
                var yValue = YGesture.Clamp(_initial.y - YGesture.GetUnit() * y);
                Value = new Vector2(xValue, yValue);
            }

        }

        public static class BounceBall
        {
            private static float _initialMagnitude = 1;
            private const float MinMagnitude = 1, MaxMagnitude = 10;
            private static readonly GestureParameter YGesture = new GestureParameter(MinMagnitude, MaxMagnitude);
            public static float Magnitude = 2;

            public static void Initialize()
            {
                _initialMagnitude = Magnitude;
            }

            public static void SetMagnitude(float value)
            {
                Magnitude = YGesture.Clamp(_initialMagnitude + YGesture.GetUnit() * value);
            }
        }

        public static class HandFire
        {
            private static float _initialSize, _initialDecay;
            private const float MinSize = 1.3f, MaxSize = 4.7f;
            private const float MinDecay = .3f, MaxDecay = 3f;
            private static readonly GestureParameter YGesture = new GestureParameter(MinSize, MaxSize);
            private static readonly GestureParameter XGesture = new GestureParameter(MinDecay, MaxDecay);
            public static float Size = 1.3f, Decay = .4f;

            public static void SetSize(float value)
            {
                Size = YGesture.Clamp(_initialSize + YGesture.GetUnit() * value);
            }

            public static void SetDecay(float value)
            {
                Decay = XGesture.Clamp(_initialDecay + XGesture.GetUnit() * value);
            }

            public static void Initialize()
            {
                _initialSize = Size;
                _initialDecay = Decay;
            }
        }

        public static class BlockWall
        {
            private static Vector2 _initial = new Vector2(79, -75);
            public static Vector2 Value = new Vector2(79, -75);
            private const float XMin = 79, XMax = 116;
            private const float YMin = -94, YMax = -62;
            private static readonly GestureParameter XGesture = new GestureParameter(XMin, XMax);
            private static readonly GestureParameter YGesture = new GestureParameter(YMin, YMax);

            public static void Initialize()
            {
                _initial = Value;
            }

            public static void SetGestureValues(float x, float y)
            {
                var xValue = XGesture.Clamp(_initial.x - XGesture.GetUnit() * x);
                var yValue = YGesture.Clamp(_initial.y + YGesture.GetUnit() * y);
                Value = new Vector2(xValue, yValue);
            }
        }

        public static class Fog
        {
            private static float _initialSize;
            private const float MinSize = 5, MaxSize = 30;
            private static readonly GestureParameter YGesture = new GestureParameter(MinSize, MaxSize);
            public static float Size = 15;

            public static void Initialize()
            {
                _initialSize = Size;
            }

            public static void SetSize(float value)
            {
                Size = YGesture.Clamp(_initialSize + YGesture.GetUnit() * value);
            }
        }

        public static class Explode
        {
            private static Vector2 _initial = new Vector2(79, -75);
            public static Vector2 Value = new Vector2(79, -75);
            private const float XMin = 15.5f, XMax = 116;
            private const float YMin = -67, YMax = -22;
            private static readonly GestureParameter XGesture = new GestureParameter(XMin, XMax);
            private static readonly GestureParameter YGesture = new GestureParameter(YMin, YMax);

            public static void Initialize()
            {
                _initial = Value;
            }

            public static void SetGestureValues(float x, float y)
            {
                var xValue = XGesture.Clamp(_initial.x - XGesture.GetUnit() * x);
                var yValue = YGesture.Clamp(_initial.y + YGesture.GetUnit() * y);
                Value = new Vector2(xValue, yValue);
            }
        }

        public static class Aura
        {
            private static float _initialSize;
            private const float MinSize = 100f, MaxSize = 5000f;
            private static readonly GestureParameter YGesture = new GestureParameter(MinSize, MaxSize);
            public static float Size = 100f;

            public static void SetSize(float value)
            {
                Size = YGesture.Clamp(_initialSize + YGesture.GetUnit() * value);
            }

            public static void Initialize()
            {
                _initialSize = Size;
            }
        }

        public static class Waterfall
        {
            private static float _initialSize;
            private const float MinSize = .1f, MaxSize = .38f;
            private static readonly GestureParameter YGesture = new GestureParameter(MinSize, MaxSize);
            public static float Size = .25f;

            public static void SetSize(float value)
            {
                Size = YGesture.Clamp(_initialSize + YGesture.GetUnit() * value);
            }

            public static void Initialize()
            {
                _initialSize = Size;
            }
        }

    }



}
