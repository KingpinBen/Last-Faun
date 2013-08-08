using System;
using UnityEngine;
using System.Collections.Generic;

public class PressurePadScript : InteractiveObject
{

    public GameObject padObject;
    public Transform[] requiredObjects;
    public InteractiveObject[] targetObjects;
    public bool holdSwitch;
    public GameObject particlePrefab;
    public float heightDepression = 0.285f;
    public AINode aiTargetNode;

    private bool _companionWaiting;
    private bool _mouseOvered;
    private bool _moving;
    private GameObject _instantiatedParticles;
    private readonly List< Collider > _collidingObjects = new List< Collider >();
    private AINode _oldTarget;

    protected override void Start()
    {
        foreach ( var t in targetObjects )
        {
            OnActivate += t.ToggleObject;
            OnDeactivate += t.ToggleObject;
        }
    }

    private void Update()
    {
        if ( aiTargetNode.objectActive )
        {
            if ( _companionWaiting == false &&
                 aiTargetNode.GetAffectingCharacter().waitingAtTarget )
                _companionWaiting = true;

            //  Clicked again, so cancel waiting
            if ( Input.GetMouseButtonDown( 0 ) )
            {
                _oldTarget.ActivateObject( this );
                CursorManager.instance.ChangeCursorStatus( CursorManager.CursorGestureStatus.Normal );
                _companionWaiting = false;
            }
        }
        else
        {
            if ( _mouseOvered )
            {
                var action = aiTargetNode.GetAffectingCharacter().currentAction;

                if ( Input.GetMouseButtonDown( 0 ) &&
                     ( action == Character.CharacterAction.None ||
                       action == Character.CharacterAction.Waiting ) )
                {
                    _oldTarget = aiTargetNode.GetAffectingCharacter().Target;

                    aiTargetNode.ActivateObject( this );
                    CursorManager.instance.ChangeCursor( CursorType.Wait );
                    CursorManager.instance.ChangeCursorStatus( CursorManager.CursorGestureStatus.Capturing );
                }
            }
        }

        if ( _moving )
        {
            if ( _collidingObjects.Count > 0 )
            {
                padObject.transform.localPosition = Vector3.Slerp( padObject.transform.localPosition,
                                                                   new Vector3( 0, -heightDepression, 0 ),
                                                                   Time.deltaTime * 4.0f );
                if ( Math.Abs( padObject.transform.localPosition.y - -heightDepression ) < Mathf.Epsilon )
                    _moving = false;
            }
            else
            {
                padObject.transform.localPosition = Vector3.Slerp( padObject.transform.localPosition, Vector3.zero,
                                                                   Time.deltaTime * 4.0f );
                if ( Math.Abs( padObject.transform.localPosition.y - 0 ) < Mathf.Epsilon )
                    _moving = false;
            }
        }
    }

    private void OnTriggerEnter( Collider body )
    {
        if ( body.isTrigger )
            return;

        if ( requiredObjects.Length > 0 )
        {
            for ( var i = 0; i < requiredObjects.Length; i++ )
            {
                var t = requiredObjects[i];
                if ( body.transform != t )
                    continue;

                _collidingObjects.Add( body );
            }
        }
        else
		{
            _collidingObjects.Add( body );
		}
			
        if ( _collidingObjects.Count == 1 )
            ToggleObject( this );
    }

    private void OnTriggerExit( Collider body )
    {
        if ( body.isTrigger )
            return;

        for ( var i = 0; i < _collidingObjects.Count; i++ )
        {
            if ( _collidingObjects[i] != body )
                continue;
			
            _collidingObjects.RemoveAt( i );
			
			if ( _collidingObjects.Count == 0 && holdSwitch )
            	ToggleObject( this );
			
            break;
        }
    }

    private void OnMouseEnter()
    {
        //  These objects aren't gesture objects so need to do the gesture mouseover 
        _mouseOvered = true;
        CursorManager.instance.ChangeCursor( CursorType.Wait );
    }

    private void OnMouseExit()
    {
        //  These objects aren't gesture objects so need to do the gesture mouseover 
        _mouseOvered = false;
        CursorManager.instance.ChangeCursor( CursorType.Normal );
    }

    public override void ActivateObject( object sender )
    {
        if ( objectActive )
            return;

        ToggleObject( sender );
        _moving = true;
        CreateParticlePrefab();
    }

    public override void DeactivateObject( object sender )
    {
        if ( !objectActive )
            return;

        if ( holdSwitch )
            ToggleObject( sender );
        else
            objectActive = false;
    }

    public override void ToggleObject( object sender )
    {
        if ( objectActive )
            base.DeactivateObject( sender );
        else
            base.ActivateObject( sender );

        _moving = true;
    }

    private void CreateParticlePrefab()
    {
        if ( !particlePrefab )
            return;

        _instantiatedParticles = Instantiate( particlePrefab ) as GameObject;
        _instantiatedParticles.transform.parent = transform;
        _instantiatedParticles.transform.localPosition = Vector3.zero;
        _instantiatedParticles.transform.localEulerAngles = new Vector3( 270, 0, 0 );

        Invoke( "KillParticlePrefab", 2 );
    }

    private void KillParticlePrefab()
    {
        Destroy( _instantiatedParticles );
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = objectActive ? Color.green : Color.blue;
        foreach ( InteractiveObject t in targetObjects )
        {
            Gizmos.DrawLine( transform.position, t.transform.position );
        }
    }
}