using UnityEngine;

public class CompanionClimbScript : InteractiveObject
{
    public Transform endPiece;

    private const float CLIMB_SPEED = 0.2f;

    private CompanionScript _companionScript;
    private OffMeshLink _meshLink;
    private NavMeshAgent _companionAgent;
    private float _companionBaseSpeed;
    private bool _beingUsed;

    private void Awake()
    {
        var companion = GameObject.FindWithTag("Companion");

        _companionScript = companion.GetComponent<CompanionScript>();
        _meshLink = GetComponent<OffMeshLink>();
    }

    protected override void Start()
    {
        _companionAgent = _companionScript.GetAgent();
        _companionBaseSpeed = _companionAgent.speed;

        base.Start();
    }

    private void Update()
    {
        if (_beingUsed)
        {
            if (_meshLink.occupied == false)
            {
                _beingUsed = false;
                _companionAgent.speed = _companionBaseSpeed;
                _companionScript.SetCurrentAction(Character.CharacterAction.None);
                _companionAgent.angularSpeed = 360;
            }
        }
        else
        {
            if (_meshLink.activated)
            {
                if (!_companionScript.IsIdle())
                {
                    _meshLink.activated = false;
                    return;
                }
            }
            else
            {
                if (_companionScript.IsIdle())
                    _meshLink.activated = true;
            }


            if (_meshLink.occupied)
            {
                _beingUsed = true;
                _companionAgent.speed = CLIMB_SPEED;
                _companionScript.SetCurrentAction(Character.CharacterAction.Climbing);

                var lookAt = endPiece.position - transform.position;
                lookAt.y = 0;
                _companionScript.transform.rotation = Quaternion.LookRotation(lookAt);
                _companionAgent.angularSpeed = 0;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.25f);
    }

    public OffMeshLink GetMeshLink()
    {
        return _meshLink;
    }

    public override void ActivateObject(object sender)
    {
        base.ActivateObject(sender);

        _meshLink.activated = true;
        transform.parent.gameObject.SetActive(true);
    }

    public override void DeactivateObject(object sender)
    {
        base.DeactivateObject(sender);

        _meshLink.activated = false;
        transform.parent.gameObject.SetActive(false);
    }
}
