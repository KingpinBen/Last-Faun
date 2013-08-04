using UnityEngine;

[RequireComponent(typeof (DynamicObjectScript))]
public class MovingPlatformScript : InteractiveObject
{
    public Vector3 moveToPosition;
    public float movementSpeed;

    private Vector3 _targetPosition;

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + moveToPosition);
    }
}
