using UnityEngine;

[RequireComponent(typeof(NavMeshAgent))]
public class AiCharacter : Character
{
    public float maximumDistanceFromTarget = 30;
    public bool waitingAtTarget { get; private set; }

    protected AINode _target;
    protected NavMeshAgent _navAgent;

    private Vector3 _lastGoodPosition;

    public AINode Target
    {
        get
        {
            return _target;
        }
        set
        {
            if ( _target )
                _target.DeactivateObject( this );

            _target = value;
			CheckIfWaiting();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _lastGoodPosition = transform.position;
        _navAgent = GetComponent< NavMeshAgent >();
    }

    protected override void Start()
    {
        base.Start();

        if ( !_target )
            return;

        if ( !_target.objectActive )
            _target.ActivateObject( this );
    }

    protected override void Update()
    {
        Navigate();
		CheckIfWaiting();
		
		_animator.SetFloat( _hashes.speed, ( _navAgent.velocity.magnitude / 3.3f ) );
    }
	
	void CheckIfWaiting() 
	{
        if (!_navAgent.enabled)
            return;

        //  Check if the destination is the targets current position.
        //  They can be different as navagent uses last known good position
		if (Mathf.Abs((_navAgent.destination - _target.transform.position).magnitude) < .5f)
		{
			waitingAtTarget = _navAgent.remainingDistance <= _navAgent.stoppingDistance;
		}
	}
	
	private float CalculatePathDistance(Vector3 targetPosition)
	{
		var path = new NavMeshPath();
                var i = 0;
                _navAgent.CalculatePath( targetPosition, path );

                var allWayPoints = new Vector3[path.corners.Length + 2];
                allWayPoints[0] = transform.position;
                allWayPoints[allWayPoints.Length - 1] = targetPosition;

                for ( ; i < path.corners.Length; i++ )
                {
                    allWayPoints[i + 1] = path.corners[i];
                }

                var pathDistance = 0.0f;

                for ( i = 0; i < allWayPoints.Length - 1; i++ )
                {
                    pathDistance += Vector3.Distance( allWayPoints[i], allWayPoints[i + 1] );
                }
		
		return pathDistance;
	}

    private void Navigate()
    {
        if ( _navAgent.enabled )
        {
            var targetIsPlayer = ( _target as PlayerAINode ) != null;
            var targetTransform = _target.transform;

            if ( targetIsPlayer )
            {
                if ( targetTransform.position == _lastGoodPosition )
                {
                    return;
                }
				
				var distance = CalculatePathDistance(targetTransform.position);

                if (distance <= maximumDistanceFromTarget)
                {
                    _navAgent.SetDestination( targetTransform.position );
                    _lastGoodPosition = targetTransform.position;
                }
            }
            else
            {
                if ( _target.transform.position != _lastGoodPosition )
                {
                    _navAgent.SetDestination( targetTransform.position );
                    _lastGoodPosition = targetTransform.position;
                }
            }
        }
    }

    private void SetWaiting( bool toSet )
    {
        waitingAtTarget = toSet;
    }

    public override void RegisterSlowZone( SlowZoneScript script )
    {
        if ( _slowZones.Count == 0 )
            _navAgent.speed *= .5f;

        base.RegisterSlowZone( script );
    }

    public override void RemoveSlowZone( SlowZoneScript script )
    {
        base.RemoveSlowZone( script );

        if ( _slowZones.Count == 0 )
            _navAgent.speed *= 2f;
    }

    public bool CheckAtTargetByDistance()
    {
        if ( _navAgent.remainingDistance <= _navAgent.stoppingDistance )
            return true;

        return false;
    }

    public NavMeshAgent GetAgent()
    {
        return _navAgent;
    }
}