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
        base.Update();

        var directionToCompanion = (_aiCharacter.transform.position - _transform.parent.position).normalized;
	    directionToCompanion.y = 0;

        _transform.position = Vector3.Slerp(_transform.position, _transform.parent.position + directionToCompanion * minimumDistanceFromPlayer, Time.deltaTime * 3);
	}
}
