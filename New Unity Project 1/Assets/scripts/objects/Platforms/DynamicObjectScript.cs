using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This should be applied to a separate gameobject and made child to the object you're
/// trying to attach the object too. E.g: Platform>MovingPlatform(contains scripts and parent target)
/// </summary>
[RequireComponent(typeof (BoxCollider))]
public class DynamicObjectScript : MonoBehaviour
{
    private readonly Dictionary<string, Transform> _connectedObjects = new Dictionary<string, Transform>();
    private readonly Dictionary<string, Transform> _connectedObjectsParents = new Dictionary<string, Transform>();

    private void OnTriggerEnter(Collider body)
    {
        if (body.isTrigger) return;

        var parent = body.transform.parent;
        _connectedObjectsParents.Add(body.name, parent);
        body.transform.parent = transform;
        _connectedObjects.Add(body.name, body.transform);
    }

    private void OnTriggerExit(Collider body)
    {
        if (body.isTrigger) return;
        Transform objTransform = null;
        Transform pastParent = null;

        for (var i = 0; i < _connectedObjects.Count; i++)
        {
            _connectedObjects.TryGetValue(body.name, out objTransform);
            if (!objTransform) continue;

            _connectedObjectsParents.TryGetValue(body.name, out pastParent);
            break;
        }

        if (!objTransform) return;

        objTransform.parent = pastParent;
        _connectedObjects.Remove(body.name);
        _connectedObjectsParents.Remove(body.name);
    }
}
