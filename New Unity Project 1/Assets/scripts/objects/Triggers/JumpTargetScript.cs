using UnityEngine;

public class JumpTargetScript : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue*0.5f;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
