using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class OutOfBoundsZone : MonoBehaviour {
	
    

	public Transform targetRespawnPoint;
	
	private Texture2D _guiOverlayTexture;
	public Color _guiColor;
	private PlayerScript _player;
	private float _startTime;
    private FadingState _activeState = FadingState.Hidden;
	
	private const float FADE_DURATION = 1.0f;
    private const float TIME_DURING_BLANK_SCREEN = 1.0f;
	
	void Awake ()
	{
		_guiOverlayTexture = new Texture2D(1, 1);
	    _guiOverlayTexture.SetPixels(new[] { Color.white } );
        _guiOverlayTexture.Apply();
	}

	void Start () {
        if (!targetRespawnPoint)
        {
            Debug.LogWarning(name + " doesn't have a valid target respawn point, so I'm disabling it.");
            gameObject.SetActive(false);
        }

		collider.isTrigger = true;
	}
	
	void Update()
	{
		HandleFading();
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (_activeState != FadingState.Hidden) 
            return;
		
		if (other.tag == "Player")
		{
			if (!_player) 
				_player = other.GetComponent<PlayerScript>();
			
			StartCoroutine(RespawnPlayer());
		}
	}
	
	void OnDrawGizmos()
	{
		var world = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
		Gizmos.matrix = world;
	    Gizmos.color = Color.black*.7f;
		Gizmos.DrawCube(Vector3.zero, Vector3.one);
	}
	
	void HandleFading()
	{
        switch (_activeState)
        {
            case FadingState.Hidden:
            case FadingState.Shown:
                return;
            case FadingState.FadingIn:
                _guiColor.a = Mathf.PingPong(Time.time - _startTime, FADE_DURATION) / FADE_DURATION;
                break;
            case FadingState.FadingOut:
                _guiColor.a = 1 - Mathf.PingPong(Time.time - _startTime, FADE_DURATION) / FADE_DURATION;
                break;
        }
	}
	
	void OnGUI()
	{
		if ( _activeState == FadingState.Hidden ) 
            return;

		GUI.color = _guiColor;
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _guiOverlayTexture);
	}
	
	IEnumerator RespawnPlayer()
	{
	    _player.IsControllable = false;
        //  Start fading in
        _startTime = Time.time;
        _activeState = FadingState.FadingIn;
        yield return new WaitForSeconds(FADE_DURATION);

        //  After _fadeDuration time, show solid black
	    _guiColor.a = 1;
        _activeState = FadingState.Shown;
	    yield return new WaitForEndOfFrame();

        //  Add a solid black screen to hide stuff.
        yield return new WaitForSeconds(TIME_DURING_BLANK_SCREEN);
        _activeState = FadingState.FadingOut;
        _startTime = Time.time;
        _player.transform.position = targetRespawnPoint.position;

        yield return new WaitForSeconds(FADE_DURATION);
	    _guiColor.a = 0;
        _activeState = FadingState.Hidden;

	    _player.IsControllable = true;
	}

    private enum FadingState
    {
        Hidden,
        FadingIn,
        Shown,
        FadingOut
    }
}
