using UnityEngine;
using System;
using System.Collections;

public class ChangeLevelScript : MonoBehaviour
{
    public string nextLevelName;

    private TransitionState _transitionState = TransitionState.Showing;
    private float _arrowPositionDelta = -1.5f;
    private bool _playerInRange;
    private bool _companionInRange;
	private Texture2D _pixel;
	private Color _color = Color.black;
    private float _fadeStartTime;

    private const float FADE_DURATION = 1.0f;

    private void Awake()
    {
        

		if (string.IsNullOrEmpty(nextLevelName))
		{
            Debug.LogException(new Exception("No Level to change to. Disabling"));
            gameObject.SetActive(false);
		}
        else
		{
            //  Do it in the else so we don't have to load extra things.
            _pixel = new Texture2D(1, 1);
            _pixel.SetPixel(1, 1, Color.black);
            _pixel.Apply();

            //  Start the fade in.
            StartCoroutine(FadeIn());
		}
    }

    private void Update()
    {
		switch(_transitionState) 
		{
			case TransitionState.Idle:
				
				break;
			case TransitionState.Hiding:
                _color.a = Mathf.PingPong(Time.time - _fadeStartTime, FADE_DURATION) / FADE_DURATION;
				break;
			case TransitionState.Showing:
		        _color.a = 1 - Mathf.PingPong(Time.time - _fadeStartTime, FADE_DURATION)/FADE_DURATION;
			
				if (_color.a <= Time.deltaTime)
					_transitionState = TransitionState.Idle;
				break;
		}
    }

    private void OnTriggerEnter(Collider body)
    {
        if (_transitionState == TransitionState.Hiding) return;

        if (body.tag == "Player")
        {
            _playerInRange = true;
        }
        else if (body.tag == "Companion")
        {
            _companionInRange = true;
        }
            
        if (_playerInRange && _companionInRange)
        {
            StartCoroutine(FadeOut());
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
		}
	}

    IEnumerator FadeIn()
    {
        _fadeStartTime = Time.time;
        _transitionState = TransitionState.Showing;

        yield return new WaitForSeconds(FADE_DURATION);

        _transitionState = TransitionState.Idle;
    }

    IEnumerator FadeOut()
    {
        _fadeStartTime = Time.time;
        _transitionState = TransitionState.Hiding;

        yield return new WaitForSeconds(FADE_DURATION);

        SaveManager.ChangeLevel(nextLevelName);
    }
	
	enum TransitionState {
		Idle, Hiding, Showing	
	}
}
