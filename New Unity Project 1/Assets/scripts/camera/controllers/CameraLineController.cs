using UnityEngine;

public class CameraLineController : CameraBaseController
{
    public Transform[] cameraNodes = new Transform[2];

    public TriggerObjectScript startTrigger;
    public TriggerObjectScript endTrigger;

    private float _maxDistance;
    private GameObject _playerTracker;
    private Transform _player;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();

        _player = GameObject.FindWithTag("Player").transform;

        cameraTransformNode.transform.position = cameraNodes[0].position;
        _maxDistance = (startTrigger.transform.position - endTrigger.transform.position).magnitude;

        _playerTracker = new GameObject();
        _playerTracker.transform.parent = startTrigger.transform;
        _playerTracker.transform.localPosition = Vector3.zero;
    }

    protected override void Update()
    {
        base.Update();

        if (!objectActive) return;

        FixTrackerPosition();

        var dist = (_playerTracker.transform.position - _player.position).magnitude;
        var percent = Mathf.Clamp(dist / _maxDistance, 0, 1);

        if (percent >= 0.0f && percent <= 1.0f)
            cameraTransformNode.transform.localPosition =
                (percent * cameraNodes[1].localPosition) +
                ((1 - percent) * cameraNodes[0].localPosition);
        else
        {
            //	we give it a .3 addition in hopes that it soon grabs a new camera. 
            if (percent >= 1.3f)
                DeactivateObject(this);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(cameraNodes[0].position, cameraNodes[1].position);
    }

    void FixTrackerPosition()
    {
        float width = transform.parent.localScale.z * 0.5f;

        _playerTracker.transform.position = _player.position;

        _playerTracker.transform.localPosition =
            new Vector3(0, (-startTrigger.transform.localScale.y * 0.5f) * _playerTracker.transform.localScale.y, Mathf.Clamp(_playerTracker.transform.localPosition.z, -width, width));
    }
}
