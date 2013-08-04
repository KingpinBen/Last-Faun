using UnityEngine;

/// <summary>
/// Used by the Main Game Camera as a way for the camera manager
/// to give it properties to follow such as correcting it's position
/// or rotation if it needs to move.
/// </summary>
public class CameraScript : MonoBehaviour
{
    public float movementSpeed = 2.0f;

    private CameraNodeScript _targetNode;
    private GameObject _voidObject;

    void Start()
    {
        _voidObject = new GameObject();
    }

    void Update()
    {
        if (_targetNode)
        {
            transform.position = Vector3.Slerp(transform.position, _targetNode.transform.position, movementSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetNode.GetCameraTransform().rotation, Time.deltaTime * 2.0f);
        }
    }

    public void SetNewNode(CameraNodeScript script)
    {
        if (script)
        {
            if (script.GetController().autoSnapOnTouch)
            {
                transform.position = script.transform.position;
                transform.rotation = script.transform.rotation;
            }

            _targetNode = script;
        }
        else
            _targetNode = null;
    }

    public Transform GetCameraTransform(bool set)
    {
        if (set)
        {
            _voidObject.transform.position = transform.position;
            _voidObject.transform.rotation = transform.rotation;
            return _voidObject.transform;
        }

        return transform;
    }
}
