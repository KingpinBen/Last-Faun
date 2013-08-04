using UnityEngine;
using System;

public class InteractiveObject : MonoBehaviour
{
    public delegate void OnActivationEventHandler(object sender);

    public delegate void OnDeactivationEventHandler(object sender);

    private OnActivationEventHandler _onActivateDelegate;
    private OnDeactivationEventHandler _onDeactivateDelegate;

    public event OnActivationEventHandler OnActivate
    {
        add { _onActivateDelegate = (OnActivationEventHandler) Delegate.Combine(_onActivateDelegate, value); }
        remove { _onActivateDelegate = (OnActivationEventHandler) Delegate.Remove(_onActivateDelegate, value); }
    }

    public event OnDeactivationEventHandler OnDeactivate
    {
        add { _onDeactivateDelegate = (OnDeactivationEventHandler) Delegate.Combine(_onDeactivateDelegate, value); }
        remove { _onDeactivateDelegate = (OnDeactivationEventHandler) Delegate.Remove(_onDeactivateDelegate, value); }
    }

    public bool objectActive;

    public virtual void ActivateObject(object sender)
    {
        if (objectActive) return;
        objectActive = true;

        if (_onActivateDelegate != null)
            _onActivateDelegate(this);
    }

    public virtual void DeactivateObject(object sender)
    {
        if (!objectActive) return;
        objectActive = false;

        if (_onDeactivateDelegate != null)
            _onDeactivateDelegate(this);
    }

    public virtual void ToggleObject(object sender)
    {
        if (objectActive)
            DeactivateObject(sender);
        else
            ActivateObject(sender);
    }

    public virtual void SetObject(object sender, bool toSet)
    {
        if (toSet)
            ActivateObject(sender);
        else
            DeactivateObject(sender);
    }
}
