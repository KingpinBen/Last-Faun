using UnityEngine;
using System.Collections;

[RequireComponent(typeof (BoxCollider))]
public class SlowZoneScript : MonoBehaviour
{
    void OnTriggerEnter(Collider body)
    {
        var character = body.GetComponent<Character>();

        if (character)
            character.RegisterSlowZone(this);
    }

    void OnTriggerExit(Collider body)
    {
        var character = body.GetComponent<Character>();

        if (character)
            character.RemoveSlowZone(this);
    }

    void OnDrawGizmos()
    {
        var col = collider as BoxCollider;
        var size = new Vector3(
            col.size.x * transform.localScale.x,
            col.size.y * transform.localScale.y,
            col.size.z * transform.localScale.z);

        Gizmos.color = Color.yellow * .8f;
        Gizmos.matrix = Matrix4x4.TRS(transform.position + col.center, transform.rotation, size);


        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
}
