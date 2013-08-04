using UnityEngine;
using System.Collections;

public class PlayerAINode : AINode
{

    public float minimumDistanceFromPlayer = 2;

    private Transform _transform;

    protected override void Start()
    {
        base.Start();

        _transform = transform;
    }

	protected override void Update ()
	{
        var directionToCompanion = _aiCharacter.transform.position - _transform.parent.position;
	    directionToCompanion.y = 0;
        directionToCompanion.Normalize();

        _transform.position = _transform.parent.position + directionToCompanion * minimumDistanceFromPlayer;

	    base.Update();
	}
}
