using UnityEngine;

public class EditorObjectGizmo : MonoBehaviour {

    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, Vector3.one * 0.25f);
    }
}
