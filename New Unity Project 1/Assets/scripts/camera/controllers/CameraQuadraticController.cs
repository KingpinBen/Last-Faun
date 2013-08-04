using UnityEngine;
using System.Collections.Generic;

public class CameraQuadraticController : CameraBaseController
{
    public Transform[] cameraNodes = new Transform[3];
    public TriggerObjectScript startTrigger;
    public TriggerObjectScript endTrigger;

    private float _maxDistance;
    private Transform _player;
    private GameObject _playerTracker;

    protected override void Awake()
    {
        base.Awake();
        _player = GameObject.FindWithTag("Player").transform;

        cameraTransformNode.transform.localPosition = cameraNodes[0].localPosition;

        _maxDistance = (endTrigger.transform.position - startTrigger.transform.position).magnitude;
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
        var percent = Mathf.Clamp(dist/_maxDistance, 0.0f, 1.0f);

        if (percent >= 0.0f && percent <= 1.3f)
        {
            var e = ((percent)*cameraNodes[2].transform.localPosition) +
                        ((1 - percent)*cameraNodes[1].transform.localPosition);
            var f = ((percent)*cameraNodes[1].transform.localPosition) +
                        ((1 - percent)*cameraNodes[0].transform.localPosition);

            var g = ((percent)*e) + ((1 - percent)*f);

            cameraTransformNode.transform.localPosition = g;
        }
    }

    private void OnDrawGizmos()
    {
        //  It ain't pretty, but it works.
        int i;
        const int resolution = 8; //  Amount of divisions
        const float divisionPercent = (1.0f/resolution);
        var vectors = new List<Vector3>();

        for (i = 0; i < resolution + 1; i++)
        {
            var percent = divisionPercent*i;
            var e = ((percent)*cameraNodes[2].transform.position) +
                        ((1 - percent)*cameraNodes[1].transform.position);

            var f = ((percent)*cameraNodes[1].transform.position) +
                        ((1 - percent)*cameraNodes[0].transform.position);

            var g = ((percent)*e) + ((1 - percent)*f);
            vectors.Add(g);
        }

        for (i = 0; i < vectors.Count - 1; i++)
            Gizmos.DrawLine(vectors[i], vectors[i + 1]);
    }

    private void FixTrackerPosition()
    {
        var width = transform.parent.localScale.z*0.5f;

        _playerTracker.transform.position = _player.position;

        _playerTracker.transform.localPosition =
            new Vector3(0, (-startTrigger.transform.localScale.y*0.5f)*_playerTracker.transform.localScale.y,
                        Mathf.Clamp(_playerTracker.transform.localPosition.z, -width, width));
    }
}
