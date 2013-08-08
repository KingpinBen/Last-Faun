using UnityEngine;

public abstract class CameraBaseController : InteractiveObject
{
    public CameraNodeScript cameraTransformNode;
    public Transform cameraTarget;
    public bool autoSnapOnTouch = false;
    public bool lockXRotation = false;
    public float rotationSpeed = 1.0f;

    protected CameraManagerScript _cameraManager;

    protected virtual void Awake()
    {
        _cameraManager = GameObject.FindWithTag("CameraManager").GetComponent<CameraManagerScript>();

        if (!autoSnapOnTouch) return;
        if (cameraTarget)
        {
            var lookAt = Quaternion.LookRotation(cameraTarget.position - cameraTransformNode.transform.position);
            cameraTransformNode.transform.rotation = lookAt;
        }
    }

    protected virtual void Update()
    {
        HandleRotation();
    }

    public override void ActivateObject(object sender)
    {
        var old = objectActive;
        base.ActivateObject(sender);

        if (!old && objectActive)
            _cameraManager.ChangeActiveCamera(cameraTransformNode);
    }

    public virtual void SetTarget(Transform target)
    {
        cameraTarget = target;
    }

    protected void HandleRotation()
    {
        var targetRotation = Quaternion.identity;

        if (!cameraTarget) return;
        if (lockXRotation)
        {
            targetRotation = Quaternion.LookRotation(
                new Vector3(cameraTarget.position.x,
                            cameraTransformNode.transform.position.y,
                            cameraTarget.position.z) -
                cameraTransformNode.transform.position, Vector3.up);

            targetRotation.x = cameraTransformNode.transform.rotation.x;
        }
        else
            targetRotation =
                Quaternion.LookRotation(
                    cameraTarget.position - cameraTransformNode.transform.position);

        cameraTransformNode.transform.rotation =
            Quaternion.Slerp(cameraTransformNode.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
