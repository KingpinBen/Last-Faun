using UnityEngine;

public class CameraManagerScript : MonoBehaviour
{

    public CameraBaseController firstCameraController;

    private CameraNodeScript _currentCameraNode;
    private CameraScript _cameraScript;

    private void Awake()
    {
    }

    private void Start()
    {
        _cameraScript = Camera.main.gameObject.GetComponent<CameraScript>();

        _cameraScript.transform.position = firstCameraController.transform.position;
        _cameraScript.transform.rotation = firstCameraController.cameraTransformNode.transform.rotation;

        firstCameraController.ActivateObject(this);
    }

    public bool ChangeActiveCamera(CameraNodeScript cameraNode)
    {
        if (!cameraNode)
        {
            Debug.Log("Passed camera node is null");
            return false;
        }

        SetNewCameraNode(cameraNode);
        return true;
    }

    private void SetNewCameraNode(CameraNodeScript script)
    {
        if (script == _cameraScript) return;

        if (_currentCameraNode)
            _currentCameraNode.GetController().DeactivateObject(this);

        _currentCameraNode = script;
        _cameraScript.SetNewNode(script);
    }
}
