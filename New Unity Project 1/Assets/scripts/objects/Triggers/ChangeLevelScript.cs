using UnityEngine;
using System;

public class ChangeLevelScript : MonoBehaviour
{
    public string nextLevelName;
    public Transform arrow;

    private TransitionState _transitionState = TransitionState.Showing;
    private float _elapsed;
    private float _arrowPositionDelta = -1.5f;
    private bool _playerInRange;
    private bool _companionInRange;
	private Texture2D _pixel;
	private Color _color = Color.black;

    private void Awake()
    {
		if (string.IsNullOrEmpty(nextLevelName)) 
			Debug.LogException(new Exception("No Level to change to"));
		
        _pixel = new Texture2D(1, 1);
		_pixel.SetPixel(1, 1, Color.black);
		_pixel.Apply();
    }

    private void Update()
    {
		switch(_transitionState) 
		{
			case TransitionState.Idle:
				
				break;
			case TransitionState.Hiding:
				_color.a = Mathf.Lerp(_color.a, 1, Time.deltaTime * 3);
				break;
			case TransitionState.Showing:
				_color.a = Mathf.Lerp(_color.a, 0, Time.deltaTime * 3);
			
				if (_color.a <= Time.deltaTime)
					_transitionState = TransitionState.Idle;
				break;
		}

        arrow.transform.localPosition = new Vector3(0, 0.25f, (Mathf.Sin(Time.time*1.45f)*_arrowPositionDelta) + 2);
    }

    private void OnTriggerEnter(Collider body)
    {
        if (_transitionState == TransitionState.Hiding) return;

        if (body.tag == "Player")
            _playerInRange = true;
        else if (body.tag == "Companion")
            _companionInRange = true;

        if (_playerInRange && _companionInRange)
        {
            _transitionState = TransitionState.Hiding;
        }
    }

    private void OnTriggerExit(Collider body)
    {
        if (body.tag == "Player")
            _playerInRange = false;
        else if (body.tag == "Companion")
            _companionInRange = false;
    }
	
	void OnGUI(){
		if (_transitionState != TransitionState.Idle) {
			GUI.color = _color;
			GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), _pixel);
			
			if (_transitionState == TransitionState.Hiding && _color.a >= 255-Time.deltaTime)
				Application.LoadLevel(nextLevelName);
			else{
				if (_transitionState == TransitionState.Showing && _color.a <= Mathf.Epsilon )
				{
					_transitionState = TransitionState.Idle;
					_color.a = 0;
				}
			}
		}
	}
	
	enum TransitionState {
		Idle, Hiding, Showing	
	}
}
