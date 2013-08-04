using System;
using UnityEngine;
using System.Collections.Generic;

public enum GestureType {
    None = -1,
    Up, Down, Left, Right,
    CWFullCircle, CCWFullCircle,
    CWSemiCircle, CCWSemiCircle
}

public class GradientBasedGestureScript : MonoBehaviour
{
    private GestureObject _listenerObject;
    private readonly List<int> _gradientIndices = new List<int>();
    private Vector2 _currentGradientStartPosition;
    private bool _capturingGestures;

    private float _distance = 40;

    private void Awake()
    {
        _distance *= _distance; // square it.
    }

    private void Update()
    {
        if (_capturingGestures)
        {
            if (InGamePause.instance.GetIsPaused())
            {
                CancelCapture();
                return;
            }

            if (Input.GetMouseButtonUp(0))
                EndCapture();
            else
                UpdateCurrentGesture();
        }
            
    }

    private void UpdateCurrentGesture()
    {
        //  Altering the mouse position by 1% so we don't get a rounded value for the 
        //  later analysis. It shouldn't affect the actual workings though.
        Vector2 currentMousePosition = Input.mousePosition*1.01f;
        var sampleVector = (currentMousePosition - _currentGradientStartPosition)*1.01f;

        if (sampleVector.sqrMagnitude > _distance)
        {
            if (Math.Abs(sampleVector.sqrMagnitude) > Mathf.Epsilon)
            {
                var newIndex = AnalyzeSampleVector(sampleVector);

                if (_gradientIndices.Count == 0 || newIndex != _gradientIndices[_gradientIndices.Count - 1])
                    _gradientIndices.Add(newIndex);

                _currentGradientStartPosition = currentMousePosition;
            }
        }
    }

    private int AnalyzeSampleVector(Vector2 samplesVector)
    {
        //  Gradient = deltaY / deltaX
        //  3 was an artbitrary choice. Most values above it are useless anyway
        var gradient = Mathf.Clamp((samplesVector.y/samplesVector.x), -3, 3);

        //  Top right
        if (samplesVector.x > 0 && samplesVector.y > 0)
        {
            if (gradient > 2.0f)
                return 0;

            return gradient > 0.5f ? 1 : 2;
        }

        //  Bottom right
        if (samplesVector.x > 0 && samplesVector.y < 0)
        {
            if (gradient > -0.5f)
                return 2;

            return gradient < -2 ? 4 : 3;
        }

        //  Bottom left
        if (samplesVector.x < 0 && samplesVector.y < 0)
        {
            if (gradient > 2.0f)
                return 4;

            return gradient > 0.5f ? 5 : 6;
        }

        //  Top left 
        return gradient < -2.0f ? 0 : (gradient < -0.5f ? 7 : 6);
    }

    private GestureType AnalyzeGesture()
    {
        if (_gradientIndices.Count > 0)
        {
            if (_gradientIndices.Count <= 2)
            {
                if (_gradientIndices[0] == 0)
                    return GestureType.Up;

                if (_gradientIndices[0] == 4)
                    return GestureType.Down;

                if (_gradientIndices[0] == 2)
                    return GestureType.Right;

                if (_gradientIndices[0] == 6)
                    return GestureType.Left;
            }
            else
            {
                var lastDigit = _gradientIndices[_gradientIndices.Count - 1];

                if ((_gradientIndices[0] + 1)%8 == _gradientIndices[1])
                {
                    //  Clockwise
                    if (lastDigit > (_gradientIndices[0] - 1%8) &&
                        lastDigit < (_gradientIndices[0] + 1)%8)
                        return GestureType.CWFullCircle;

                    return GestureType.CWSemiCircle;
                }

                //  Sum is done on the right side as to avoid problems with 0 being 
                //  the first value in gradientindices.
                if (_gradientIndices[0] == (_gradientIndices[1] + 1)%8)
                {
                    //  Counter clockwise

                    if (lastDigit > (_gradientIndices[0] - 1%8) &&
                        lastDigit < (_gradientIndices[0] + 1)%8)
                        return GestureType.CCWFullCircle;

                    return GestureType.CCWSemiCircle;
                }
            }
        }

        //  Gesture wasn't acceptable.
        return GestureType.None;
    }

    public void StartCapture(GestureObject listener)
    {
        if (InGamePause.instance.GetIsPaused()) return;

        _capturingGestures = true;
        _listenerObject = listener;
        _gradientIndices.Clear();
        _currentGradientStartPosition = Input.mousePosition;
        CursorManager.instance.ChangeCursorStatus(CursorManager.CursorGestureStatus.Capturing);
    }

    private void EndCapture()
    {
        _capturingGestures = false;
        _listenerObject.ReceiveGesture(AnalyzeGesture());
        _listenerObject = null;
    }

    private void CancelCapture()
    {
        _listenerObject = null;
        _capturingGestures = false;
    }
}