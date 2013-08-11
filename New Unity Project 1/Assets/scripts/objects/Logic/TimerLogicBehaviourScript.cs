using UnityEngine;
using System.Collections;

public class TimerLogicBehaviourScript : InteractiveObject
{
    public float countdownToActivate = 3.0f; //  Seconds obv.
    public InteractiveObject[] targetObjects;

    protected override void Start()
    {
        if (objectActive)
            ActivateObject(null);
    }

    public override void ActivateObject(object sender)
    {
        if (objectActive) 
            StopAllCoroutines();

        base.ActivateObject(sender);
        StartCoroutine( Countdown() );
    }

    public override void DeactivateObject(object sender)
    {
        base.DeactivateObject(sender);
        StopAllCoroutines();
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds( countdownToActivate );
        TriggerTargetObjects();
    }

    private void TriggerTargetObjects()
    {
        for ( int i = 0; i < targetObjects.Length; i++ )
            targetObjects[i].ToggleObject( this );

        DeactivateObject(null);
    }

    private void OnDrawGizmosSelected()
    {
        if (targetObjects == null) return;
        for ( int i = 0; i < targetObjects.Length; i++ )
        {
            InteractiveObject t = targetObjects[i];
            if ( !t )
                continue;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine( transform.position, t.transform.position );
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = objectActive ? Color.green : Color.red;
        Gizmos.DrawCube(transform.position, Vector3.one);
    }
}