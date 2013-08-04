using UnityEngine;

/// <summary>
/// This can be removed once we have a working, animating bridge. 
/// 
/// This is simply an pre-alpha development script.
/// </summary>
public class BridgeScript : InteractiveObject
{
    public DynamicObjectScript bridge;
    public float distanceToMove = 5;

    private bool _completedMovement = true;
    private float _length;

    private void Awake()
    {
        _length = distanceToMove;

        _completedMovement = true;

        if (!objectActive)
            bridge.transform.localPosition = new Vector3(0, 0, -_length);
    }

    private void Update()
    {
        if (_completedMovement) return;
        var target = Vector3.zero;

        //  Closed is 0,0,0 so if it's supposed to be open, move it.
        if (!objectActive)
            target.z = -_length;

        bridge.transform.localPosition = Vector3.Slerp(bridge.transform.localPosition, target, Time.deltaTime*5.0f);

        if (objectActive)
        {
            if (bridge.transform.localPosition.z >= -0.03f)
                CompleteTransition();
        }
        else
        {
            if (bridge.transform.localPosition.z <= -_length*0.97f)
                CompleteTransition();
        }
    }

    private void CompleteTransition()
    {
        _completedMovement = true;
    }

    public override void ActivateObject(object sender)
    {
        if (objectActive) return;
        base.ActivateObject(sender);
        _completedMovement = false;
    }

    public override void DeactivateObject(object sender)
    {
        if (objectActive)
        {
            base.DeactivateObject(sender);
            _completedMovement = false;
        }
    }

    public override void ToggleObject(object sender)
    {
        if (objectActive)
            DeactivateObject(sender);
        else
            ActivateObject(sender);
    }
}
