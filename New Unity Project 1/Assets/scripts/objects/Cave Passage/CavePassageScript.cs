using UnityEngine;

public class CavePassageScript : MonoBehaviour
{
    public CavePassageScript cavePortalPartner;
    public Transform myTeleportGameObject;

    private Transform _player;
    private bool _enabled;

    void Start ()
    {
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (!_enabled) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            _player.position = cavePortalPartner.GetTeleportLocation().position;
            _enabled = false;
        }
    }

    public Transform GetTeleportLocation()
    {
        return myTeleportGameObject;
    }

    private void OnTriggerEnter(Collider body)
    {
        if (body.tag == "Player")
        {
            if (!_enabled)
                _enabled = true;
        }
    }

    private void OnTriggerExit(Collider body)
    {
        if (body.tag == "Player")
            if (_enabled)
                _enabled = false;
    }
}
