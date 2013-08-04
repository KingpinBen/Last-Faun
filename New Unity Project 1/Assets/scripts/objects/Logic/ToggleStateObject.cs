using UnityEngine;
using System.Collections;

public class ToggleStateObject : InteractiveObject {

    void OnDrawGizmos()
    {
        Gizmos.color = objectActive ? Color.green : Color.red;
        Gizmos.DrawCube(transform.position, Vector3.one * .25f);
    }
}
