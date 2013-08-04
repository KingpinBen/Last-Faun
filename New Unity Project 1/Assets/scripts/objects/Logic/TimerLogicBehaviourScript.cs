using UnityEngine;

public class TimerLogicBehaviourScript : InteractiveObject
{
    public float countdownToActivate = 3.0f; //  Seconds obv.
    public InteractiveObject[] targetObjects;

    private float _timeRemaining;

    private void Start()
    {
        _timeRemaining = countdownToActivate;
    }

    private void Update()
    {
        if (!objectActive) return;
        _timeRemaining -= Time.deltaTime;

        if (_timeRemaining <= 0.0f)
            TriggerTargetObjects();
    }

    public override void ActivateObject(object sender)
    {
        if (objectActive) return;
        //  Reset the time on activate rather than only 
        //  on deactivate incase we want to just trigger it again.
        _timeRemaining = countdownToActivate;
        base.ActivateObject(sender);
    }

    private void TriggerTargetObjects()
    {
        foreach (var t in targetObjects)
            t.ToggleObject(null);

        DeactivateObject(null);
    }

    private void OnDrawGizmosSelected()
    {
        if (targetObjects == null) return;
        foreach (InteractiveObject t in targetObjects)
        {
            if (!t.gameObject) continue;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, t.transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = objectActive ? Color.green : Color.red;
        Gizmos.DrawCube(transform.position, Vector3.one);
    }
}