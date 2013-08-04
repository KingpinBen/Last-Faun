using System;
using UnityEngine;

public class PlayerJumpScript : MonoBehaviour
{
    private PlayerScript _playerScript;
    private Transform _targetBody;
    private bool _waitingToLand;

    public Transform TargetBody
    {
        get { return _targetBody; }
        set
        {
            _targetBody = value;
            _playerScript.jumpTarget = value;
        }
    }

    private void Awake()
    {
        _playerScript = transform.parent.GetComponent<PlayerScript>();
    }

    private void Update()
    {
        if (!_playerScript.IsIdle()) return;

        if (_targetBody)
        {
            var sqrDistanceFromTarget = (_targetBody.position - transform.position).sqrMagnitude;
            if (sqrDistanceFromTarget < 2 || sqrDistanceFromTarget > 9)
                TargetBody = null;
        }

        var grounded = Physics.Raycast(transform.position + (transform.forward + Vector3.up) * .25f, Vector3.down, 1f, ~(1 << 2));

        if (_waitingToLand)
        {
            if (grounded) 
                _waitingToLand = false;
        }
        else
        {
            if (!grounded)
            {
                TargetBody = null;
                _playerScript.Jump();

                //  Force the player to have to land before allowing another jump.
                _waitingToLand = true;  
            }
        }
    }

    private void OnTriggerStay(Collider body)
    {
        if (body.tag == "JumpTarget")
            TargetBody = body.transform;
    }
}
