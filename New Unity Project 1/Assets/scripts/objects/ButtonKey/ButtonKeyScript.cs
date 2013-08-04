using UnityEngine;

/// <summary>
/// Object interaction is pretty much all carried out in the Button object code.
/// This is simply to pick up the object.
/// </summary>
public class ButtonKeyScript : MonoBehaviour
{
    public Transform handle;

    private KeyState _currentState = KeyState.Grounded;
    private Transform _transform;

    void Start()
    {
        _transform = transform;

        _transform.position += new Vector3(0,.5f,0);
    }

    void Update()
    {
        switch(_currentState)
        {
            case KeyState.Grounded:
                {
                    var pos = _transform.position;
                    pos.y += Mathf.Sin(Time.time * Time.timeScale * 2) * 0.002f;
                    _transform.position = pos;
                }
                break;
            case KeyState.PickedUp:
                {
                }
                break;
            case KeyState.OnButton:
                {
                    _transform.Rotate(0, 10 * Time.deltaTime, 0);
                }
                break;
        }
    }

    private void OnTriggerEnter(Collider body)
    {
        if (body.tag == "Player")
        {
            var handScript = body.GetComponent<PlayerScript>();

            /*
             * Check if the item holster already has an item attached.
             */
            if (handScript.itemHolster.childCount == 0)
            {
                transform.parent = handScript.itemHolster;
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.Euler(new Vector3(7.5f, 174, 0));
                handle.transform.localRotation = Quaternion.identity;
                _currentState = KeyState.PickedUp;
            }
        }
    }

    public void SetKeyState(KeyState state)
    {
        _currentState = state;
    }
    
    public enum KeyState
    {
        Grounded,
        PickedUp,
        OnButton
    }
}
