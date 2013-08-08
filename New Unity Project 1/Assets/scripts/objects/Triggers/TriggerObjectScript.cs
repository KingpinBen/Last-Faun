using UnityEngine;

public class TriggerObjectScript : InteractiveObject
{
    public InteractiveObject targetObject;

    private void OnTriggerEnter(Collider body)
    {
        if (body.tag == "Player" && objectActive)
            targetObject.ActivateObject(this);
    }

    private void OnDrawGizmos()
    {
        var mat = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.matrix = mat;

        Gizmos.color = new Color(1, 0.1f, 0.25f, 0.5f);

        Gizmos.DrawCube(Vector3.zero, transform.localScale);
    }

    void OnDrawGizmosSelected()
    {
        if (targetObject)
        {
            var isCamera = targetObject as CameraBaseController;

            if (isCamera)
            {
                if (isCamera.cameraTransformNode)
                    Gizmos.DrawLine(transform.position, isCamera.cameraTransformNode.transform.position);
            }
            else
                Gizmos.DrawLine(transform.position, targetObject.transform.position);
        }
            
    }
}