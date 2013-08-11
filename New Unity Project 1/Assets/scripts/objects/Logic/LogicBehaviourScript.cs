using UnityEngine;

public class LogicBehaviourScript : InteractiveObject
{

    public ObjectState[] requiredObjects;
    public InteractiveObject[] targetObjects;
    //  Once locked, any changes to the required objects will have no affect. 
    //  Can be forced with ActivateObject on this object
    public bool lockAfterUsed;

    private bool _requiresResync;
    private bool _locked;

    protected override void Start()
    {
        for ( var i = 0; i < requiredObjects.Length; i++ )
        {
            var t = requiredObjects[i];
            if (!t)
            {
                Debug.LogWarning("WARNING: " + name + " has an empty required object! Either it's been removed or needs deleting from the array");
                continue;
            }
            t.targetObject.OnActivate += RequireResync;
            t.targetObject.OnDeactivate += RequireResync;
        }

        if (targetObjects.Length > 0)
        {
            //  Doing this just allows it to be used as a way of triggering multiple things simultaniously
            OnActivate += CheckRequirements;
            CheckRequirements(this);
        }
    }

    private void Update()
    {
        if (_locked == false)
            if (_requiresResync)
                CheckRequirements(this);
    }

    private void RequireResync(object sender)
    {
        _requiresResync = true;
    }

    private void CheckRequirements(object sender)
    {
        var allCorrect = true;
        _requiresResync = false;

        for (var i = 0; i < requiredObjects.Length; i++)
        {
            if (requiredObjects[i].targetObject.objectActive == 
				requiredObjects[i].requiredState) 
					continue;

            allCorrect = false;
            i = requiredObjects.Length;
        }

        if (allCorrect != objectActive)
        {
            if (objectActive)
                DeactivateObject(null);
            else
                ActivateObject(null);
        }
    }

    private void TriggerObjects()
    {
        for (var i = 0; i < targetObjects.Length; i++)
        {
            var t = targetObjects[i];
            t.ToggleObject(this);
        }

        if (lockAfterUsed && objectActive)
            _locked = true;
    }

    private void OnDrawGizmosSelected()
    {
        int i;
        if (requiredObjects != null)
            for (i = 0; i < requiredObjects.Length; i++)
            {
                if (!requiredObjects[i].targetObject) continue;

                Gizmos.color = requiredObjects[i].requiredState == requiredObjects[i].targetObject.objectActive
                                   ? Color.green
                                   : Color.red;
                Gizmos.DrawLine(transform.position, requiredObjects[i].targetObject.transform.position);
            }

        Gizmos.color = Color.blue;

        if (targetObjects.Length > 0)
            for (i = 0; i < targetObjects.Length; i++)
                if (targetObjects[i])
                    Gizmos.DrawLine(transform.position, targetObjects[i].transform.position);
    }
	
	public override void ActivateObject(object sender)
	{
		base.ActivateObject(sender);

        TriggerObjects();
	}
	
	public override void DeactivateObject(object sender)
	{
		base.DeactivateObject(sender);	
		
		TriggerObjects();
	}
	
    private void OnDrawGizmos()
    {
        Gizmos.color = objectActive ? Color.green : Color.red;
        Gizmos.DrawCube(transform.position, Vector3.one);
    }
}
