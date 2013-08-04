using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class BlockTargetBlocker : MonoBehaviour {

    void OnDrawGizmos()
    {
        var box = collider as BoxCollider;
        var size = new Vector3(box.size.x * transform.localScale.x, box.size.y * transform.localScale.y, box.size.z * transform.localScale.z);

        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(transform.position + box.center, transform.localRotation, size);
        
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
}
