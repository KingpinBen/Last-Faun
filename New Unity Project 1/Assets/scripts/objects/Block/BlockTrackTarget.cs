using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BlockTrackTarget : MonoBehaviour
{

    public BlockScript occupyingBlock;

    void OnDrawGizmos()
    {
        var col = collider as BoxCollider;

        Gizmos.color = Color.green * 0.75f;
        Gizmos.DrawCube(transform.position + col.center, col.size);
    }
}
