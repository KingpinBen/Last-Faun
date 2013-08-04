using UnityEngine;
using System.Collections;

public class OscillateMovementScript : MonoBehaviour
{
    public float amount = 2.0f;
    public Vector3 direction = new Vector3(0, 0, 1); //  Make forward default.

    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.localPosition;
    }

    private void Update()
    {
        transform.localPosition = _startPosition + Mathf.PingPong(Time.time, amount) * direction.normalized;
    }
}
