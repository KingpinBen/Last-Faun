[System.Serializable]
public class ObjectState
{
    public InteractiveObject targetObject;
    public bool requiredState;

    public static implicit operator bool(ObjectState obj)
    {
        return obj != null;
    }
}