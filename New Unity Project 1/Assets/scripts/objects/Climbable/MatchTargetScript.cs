using UnityEngine;

public class MatchTargetScript : MonoBehaviour {

    private PlayerScript _playerScript;

    void Awake()
    {
        _playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
    }

    public void SetMatchTarget()
    {
        _playerScript.BoostJump(transform);
    }
}
