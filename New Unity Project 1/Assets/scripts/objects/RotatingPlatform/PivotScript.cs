using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// Handles rotation of an object around a point.
/// Pivot should always be made to be in its 
/// default position (usually closed-> 0, 0, 0 rotation)
/// </summary>
public class PivotScript : MonoBehaviour
{
    public Vector3 rotation;
    public float movementSpeed = 1.0f;

    private Transform _transform;
    private bool _open;
    private bool _finishedMoving;
    private bool _xFinished;
    private bool _yFinished;
    private bool _zFinished;
	
	void Update ()
	{
        if (_finishedMoving) return;

        if (_open)
        {
            if (_zFinished == false)
            {
                transform.rotation *= 
                    Quaternion.AngleAxis(rotation.z * movementSpeed * Time.deltaTime, Vector3.forward);  //  z
                
                var euler = transform.rotation.eulerAngles;

                if (rotation.z < 0)
                {
                    if (euler.z - 360 < rotation.z)
                    {
                        euler.z = rotation.z;
                        _zFinished = true;
                    }
                }
                else
                {
                    if (euler.z > rotation.z)
                    {
                        euler.z = rotation.z;
                        _zFinished = true;
                    }
                }
            }

            if (_yFinished == false)
            {
                transform.rotation *=
                    Quaternion.AngleAxis(rotation.y * movementSpeed * Time.deltaTime, Vector3.up);  //  y
                
                var euler = transform.rotation.eulerAngles;

                if (rotation.y < 0)
                {
                    if (euler.y - 360 < rotation.y)
                    {
                        euler.y = rotation.y;
                        _yFinished = true;
                    }
                }
                else
                {
                    if (euler.y > rotation.y)
                    {
                        euler.y = rotation.y;
                        _yFinished = true;
                    }
                }
            }

            if (_xFinished == false)
            {
                transform.rotation *=
                    Quaternion.AngleAxis(rotation.x * movementSpeed * Time.deltaTime, Vector3.right);  //  x

                var euler = transform.rotation.eulerAngles;

                if (rotation.x < 0)
                {
                    if (euler.x - 360 < rotation.x)
                    {
                        euler.x = rotation.x;
                        _xFinished = true;
                    }
                }
                else
                {
                    if (euler.x > rotation.x)
                    {
                        euler.x = rotation.x;
                        _xFinished = true;
                    }
                }
            }

            if (_xFinished && _yFinished && _zFinished)
                _finishedMoving = true;
        }
        else
        {
            if (_zFinished == false)
            {
                transform.rotation *=
                    Quaternion.AngleAxis(rotation.z * movementSpeed * Time.deltaTime, Vector3.back);  //  z

                var euler = transform.rotation.eulerAngles;

                if (Math.Abs(euler.z) <= 5)
                {
                    _zFinished = true;
                    euler.z = 0.0f;
                    transform.rotation = Quaternion.Euler(euler);
                }
            }

            if (_yFinished == false)
            {
                transform.rotation *=
                    Quaternion.AngleAxis(rotation.y * movementSpeed * Time.deltaTime, Vector3.down);  //  y

                var euler = transform.rotation.eulerAngles;

                if (Math.Abs(euler.y) <= 5)
                {
                    _yFinished = true;
                    euler.y = 0.0f;
                    transform.rotation = Quaternion.Euler(euler);
                }
            }

            if (_xFinished == false)
            {
                transform.rotation *=
                    Quaternion.AngleAxis(rotation.x*movementSpeed*Time.deltaTime, Vector3.left); //  x

                var euler = transform.rotation.eulerAngles;

                if (Math.Abs(euler.x) <= 5)
                {
                    _xFinished = true;
                    euler.x = 0.0f;
                    transform.rotation = Quaternion.Euler(euler);
                }
            }

            if (_xFinished && _yFinished && _zFinished)
                _finishedMoving = true;
        }
	}

    public void Open()
    {
        _open = true;
        _finishedMoving = false;
        _xFinished = false; 
        _yFinished = false;
        _zFinished = false;
    }

    public void Close()
    {
        _open = false;
        _finishedMoving = false;
        _xFinished = false;
        _yFinished = false;
        _zFinished = false;
    }
}
