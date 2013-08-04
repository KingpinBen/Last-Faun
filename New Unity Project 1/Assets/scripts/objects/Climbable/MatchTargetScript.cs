using UnityEngine;

public class MatchTargetScript : MonoBehaviour {

    private PlayerScript _playerScript;

    void Awake()
    {
        _playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue * 0.7f;
        Gizmos.DrawSphere(transform.position, 0.25f);
    }

    public void SetMatchTarget()
    {
        _playerScript.BoostJump(transform);
    }
}
