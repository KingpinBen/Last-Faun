using UnityEngine;

/// <summary>
/// If single use is true, objectActive is turned false on use. 
/// Logic containers wanting to check if a button has been pushed should check for false.
/// </summary>
public class ButtonScript : InteractiveObject
{
    public ButtonKeyScript keyObject;
    public InteractiveObject targetObject;

    private bool _playerInRange;

    void Update()
    {
        if (objectActive && _playerInRange)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                targetObject.ToggleObject(this);

                if (keyObject)
                {
                    keyObject.collider.enabled = false;
                    //  TODO: Remove this. Only want to get it off the character for now.
                    keyObject.transform.parent = transform;
                    keyObject.transform.localPosition = new Vector3(0, 2, 0);
                    keyObject.SetKeyState(ButtonKeyScript.KeyState.OnButton);
                    _playerInRange = false;
                }
            }
        }
    }

    void OnTriggerEnter(Collider body)
    {
        if (keyObject)
        {
            if (body.name == keyObject.name)
                _playerInRange = true;
        }
        else
        {
            if (body.tag == "Player")
                _playerInRange = true;
        }
    }
    void OnTriggerExit(Collider body)
    {
        if (keyObject)
        {
            if (body.name == keyObject.name)
                _playerInRange = false;
        }
        else
        {
            if (body.tag == "Player")
                _playerInRange = false;
        }
    }
}
