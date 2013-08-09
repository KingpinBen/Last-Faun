using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BlockTrackTarget : MonoBehaviour
{

    public BlockScript occupyingBlock;

    void OnDrawGizmos()
    {
        var col = collider as BoxCollider;
        Gizmos.matrix = Matrix4x4.TRS( transform.position, transform.rotation, transform.lossyScale );

        Gizmos.color = Color.green * 0.75f;
        Gizmos.DrawCube( col.center, col.size);
    }
}
