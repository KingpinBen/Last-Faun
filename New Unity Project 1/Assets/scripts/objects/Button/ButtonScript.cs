using UnityEngine;

/// <summary>
/// If single use is true, objectActive is turned false on use. 
/// Logic containers wanting to check if a button has been pushed should check for false.
/// </summary>
public class ButtonScript : InteractiveObject
{
    public ButtonKeyScript keyObject;
    public bool requiresKey;

    private InteractiveObject _stateObject;

    private bool _requiredObjectInRange;
    private ButtonKeyScript _keyInRange;

    void Awake()
    {
        _stateObject = transform.parent.GetComponentInChildren< ToggleStateObject >();
        if (!_stateObject)
        {
            gameObject.SetActive(false);
            Debug.Log("Could not find a ToggleStateObject to use! Disabling this object.");

        }
            
    }

    void Update()
    {
        if (objectActive && _requiredObjectInRange)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                _stateObject.ToggleObject(this);

                if (_keyInRange)
                {
                    _keyInRange.collider.enabled = false;
                    //  TODO: Remove this. Only want to get it off the character for now.
                    _keyInRange.transform.parent = transform;
                    _keyInRange.transform.localPosition = new Vector3(0, 2, 0);
                    _keyInRange.SetKeyState(ButtonKeyScript.KeyState.OnButton);
                    _requiredObjectInRange = false;
                }
            }
        }
    }

    void OnTriggerEnter(Collider body)
    {
        if (requiresKey)
        {
            var key = body.GetComponent< ButtonKeyScript >();

            if (!key)
                return;

            if (keyObject)
            {
                if (key.gameObject == keyObject.gameObject)
                {
                    _requiredObjectInRange = true;
                    _keyInRange = key;
                }
            }
            else
            {
                _requiredObjectInRange = true;
                _keyInRange = key;
            }
        }
        else
        {
            if (body.tag == "Player")
            {
                _requiredObjectInRange = true;
            }
        }
    }
    void OnTriggerExit(Collider body)
    {
        if (requiresKey)
        {
            var key = body.GetComponent<ButtonKeyScript>();

            if (!key)
                return;

            if (keyObject)
            {
                if (key.gameObject == keyObject.gameObject)
                {
                    _requiredObjectInRange = false;
                    _keyInRange = null;
                }
            }
            else
            {
                _requiredObjectInRange = false;
                _keyInRange = null;
            }
        }
        else
        {
            if (body.tag == "Player")
            {
                _requiredObjectInRange = false;
            }
        }
    }
}
