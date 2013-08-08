using System;
using UnityEngine;

public class DoorScript : InteractiveObject
{
    public PivotScript pivot;
    public Transform door;
    public bool rotates;
    public float openingSpeed = 1.0f;
    public float closingSpeed = 1.0f;
    public float distanceToMove = 4.0f;
    public NavMeshObstacle navObstacle;

    private bool _isMoving;

    private void Update()
    {
        if ( !_isMoving )
            return;

        if ( rotates == false )
        {
            var targetPosition = Vector3.zero;

            if ( objectActive )
            {
                targetPosition = new Vector3( 0, distanceToMove, 0 );

                door.transform.localPosition =
                    Vector3.Slerp( door.transform.localPosition,
                                   targetPosition, Time.deltaTime * openingSpeed );
            }
            else
            {
                door.transform.localPosition =
                    Vector3.Slerp( door.transform.localPosition,
                                   targetPosition, Time.deltaTime * closingSpeed );
            }


            if ( door.transform.localPosition == targetPosition )
            {
                _isMoving = false;
            }
        }
    }

    /// <summary>
    /// Should be the state at which the player can pass the object.
    /// E.g. bridge down, door open,
    /// </summary>
    /// <param name="sender"></param>
    public override void ActivateObject( object sender )
    {
        if ( navObstacle )
        {
            navObstacle.enabled = false;
        }

        if ( rotates && pivot )
        {
            pivot.Close();
        }


        base.ActivateObject( sender );
        _isMoving = true;
    }

    /// <summary>
    /// Should be the state at which the player cannot pass the object.
    /// E.g. bridge up, door closed,
    /// </summary>
    /// <param name="sender"></param>
    public override void DeactivateObject( object sender )
    {
        if ( navObstacle )
        {
            navObstacle.enabled = true;
        }

        if ( rotates && pivot )
        {
            pivot.Open();
        }

        base.DeactivateObject( sender );
        _isMoving = true;
    }
}
