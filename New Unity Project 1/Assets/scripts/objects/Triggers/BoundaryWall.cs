using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class BoundaryWall : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.color = new Color(1, .5f, 0f)*0.75f;
        Gizmos.DrawCube(Vector3.zero, transform.localScale);
    }
}
