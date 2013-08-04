using UnityEngine;

public class CameraNodeScript : MonoBehaviour {

    private CameraBaseController _controller;

    void Awake() {
        _controller = transform.parent.GetComponent<CameraBaseController>();
    }

    void OnDrawGizmos () {
        DrawViewFrustum(transform);
	}

    public Transform GetCameraTransform()
    {
        return _controller ? _controller.cameraTransformNode.transform : transform;
    }

    public CameraBaseController GetController() {
        return _controller;
    }

    public static void DrawViewFrustum(Transform transform)
    {
        var mat = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
		Gizmos.matrix = mat;

        Gizmos.DrawFrustum(transform.position, 45, 1, .5f, 1.35f);
    }
}
