using UnityEngine;

public class AiCharacter : Character
{
    public AINode Target
    {
        get { return _target; }
        set
        {
            if (_target)
                _target.DeactivateObject(this);

            _target = value;
        }
    }
    public bool waitingAtTarget { get; private set; }

    protected AINode _target;

    private Vector3 _oldTargetPosition;

    protected override void Start()
    {
        base.Start();

        if (!_target) 
            return;
        if (_target.objectActive) 
            return;

        _target.ActivateObject(this);
    }

    protected override void Update()
    {
        if (_navAgent.pathStatus == NavMeshPathStatus.PathComplete &&
            _navAgent.remainingDistance <= _navAgent.stoppingDistance)
            SetWaiting(true);
        else
            SetWaiting(false);

        _animator.SetFloat("Speed", (_navAgent.velocity.magnitude / 3.3f));

        if (_navAgent.enabled && _target)
        {
            if (_target.transform.position != _oldTargetPosition)
            {
                _oldTargetPosition = _target.transform.position;
                _navAgent.SetDestination(_oldTargetPosition);
            }
        }
    }

    private void SetWaiting(bool toSet)
    {
        waitingAtTarget = toSet;
    }

    public override void RegisterSlowZone(SlowZoneScript script)
    {
        if (_slowZones.Count == 0)
            _navAgent.speed *= .5f;

        base.RegisterSlowZone(script);
    }

    public override void RemoveSlowZone(SlowZoneScript script)
    {
        base.RemoveSlowZone(script);

        if (_slowZones.Count == 0)
            _navAgent.speed *= 2f;
    }

    public bool CheckAtTargetByDistance()
    {
        if (_navAgent.remainingDistance <= _navAgent.stoppingDistance)
            return true;

        return false;
    }
}