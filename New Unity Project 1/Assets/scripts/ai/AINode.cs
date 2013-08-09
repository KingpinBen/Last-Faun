using UnityEngine;

/// <summary>
/// Makes the Agent move towards the node, and if theres a neighbour to move to next,
/// makes that the next target when this one has been reached.
/// </summary>
public class AINode : InteractiveObject
{
    public float stoppingDistance = 1.0f;
    public float speed = 4.0f;
    public AINode nextObject;
    public float timeBeforeNext = 0.1f;
    public NavMeshAgent agent;

    private float _elapsed;
    protected AiCharacter _aiCharacter;

    protected virtual void Awake()
    {
        if (agent == false)
        {
            var companion = GameObject.FindWithTag("Companion");
            if (companion) 
                agent = companion.GetComponent<NavMeshAgent>();

        }

        if (agent)
            _aiCharacter = agent.GetComponent<AiCharacter>();
        else
        {
            gameObject.SetActive(false);    //  No faun exists, so turn this off
            return;
        }
            
 
        if (objectActive) ActivateObject(null);
    }

    protected override void Start()
    {
        if (nextObject)
        {
            var aiNode = nextObject.gameObject.GetComponent<AINode>();
            OnDeactivate += aiNode.ActivateObject;
        }
    }

    protected virtual void Update()
    {
        if (objectActive)
            if (nextObject)
                if (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance <= 0.1f)
                {
                    _elapsed += Time.deltaTime;

                    if (_elapsed > timeBeforeNext)
                        DeactivateObject(this);
                }
    }

    public override void ActivateObject(object sender)
    {
        _aiCharacter.Target = this;

        agent.stoppingDistance = stoppingDistance;
        agent.speed = speed;

        base.ActivateObject(sender);
    }
    public override void DeactivateObject(object sender)
    {
        _elapsed = 0.0f;
        base.DeactivateObject(sender);
    }
    public override void ToggleObject(object sender)
    {
        if (!objectActive) ActivateObject(sender);
    }
    public override void SetObject(object sender, bool toSet) { }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue * 0.75f;
        Gizmos.DrawCube(transform.position, Vector3.one * 0.2f);

        if (!nextObject) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, nextObject.transform.position);
    }

    public AiCharacter GetAffectingCharacter()
    {
        return _aiCharacter;
    }
}
