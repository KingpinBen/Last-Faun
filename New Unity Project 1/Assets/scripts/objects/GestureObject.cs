using UnityEngine;

/// <summary>
/// Not fussy over what type of collider, it just needs one.
/// </summary>
//[RequireComponent(typeof(Collider))]
public class GestureObject : InteractiveObject
{
    public CursorType mouseOverCursor = CursorType.Inspect;
    public AINode aiTargetNode;

    protected bool _isMouseOvered;
    protected bool _listening;
    protected bool _successfulGesture;
    protected bool _objectUsed;
    protected GradientBasedGestureScript _gestureScript;
    protected GestureType _receivedGesture = GestureType.None;
    protected AINode _player;
    protected CompanionScript _companionScript;

    protected virtual void Awake()
    {
        var player = GameObject.FindWithTag("Player");
        var companion = GameObject.FindWithTag("Companion");

        _gestureScript = player.GetComponent<GradientBasedGestureScript>();
        _player = player.GetComponentInChildren<AINode>();

        _companionScript = companion.GetComponent<CompanionScript>();
    }

    protected override void Start()
    {
    }

    /// <summary>
    /// Call this (base.Update) at the start of the update methods
    /// </summary>
    protected virtual void Update()
    {
        if (!objectActive) return;
        if (InGamePause.instance.GetIsPaused()) return;
        if (_companionScript.IsIdle() == false) return;

        if (_isMouseOvered && !_listening)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartListening();
                aiTargetNode.ActivateObject(this);
            }
        }
    }

    protected void StartListening()
    {
        _listening = true;
        _objectUsed = true;
        _gestureScript.StartCapture(this);
    }

    /// <summary>
    /// This method should be overwritten.
    /// Override should include switch statements to act on the received gesture.
    /// </summary>
    protected virtual void ProcessReceivedGesture()
    {
        _receivedGesture = GestureType.None;
        _successfulGesture = false;
    }

    /// <summary>
    /// Used by the Gesture capturing script.
    /// </summary>
    /// <param name="sentGesture"></param>
    public void ReceiveGesture(GestureType sentGesture)
    {
        _receivedGesture = sentGesture;

        if (_receivedGesture == GestureType.None)
            CursorManager.instance.ChangeCursorStatus(CursorManager.CursorGestureStatus.Normal);
        else
            ProcessReceivedGesture();

        _listening = false;
    }

    protected virtual void OnMouseEnter()
    {
        if (InGamePause.instance.GetIsPaused()) return;
        if (objectActive == false) return;

        _isMouseOvered = true;
        CursorManager.instance.ChangeCursor(mouseOverCursor);
    }

    protected virtual void OnMouseExit()
    {
        if (_isMouseOvered)
        {
            CursorManager.instance.ChangeCursor(CursorType.Normal);
            _isMouseOvered = false;
        }
    }

    protected virtual void SuccessfulGesture()
    {
        _successfulGesture = true;

        CursorManager.instance.ChangeCursorStatus(CursorManager.CursorGestureStatus.Successful);
        _companionScript.GetEmotions().InstantiateEmoticon(Emotions.Good);
    }

    protected  virtual void FailedGesture()
    {
        CursorManager.instance.ChangeCursorStatus(CursorManager.CursorGestureStatus.Failed);
        _companionScript.GetEmotions().InstantiateEmoticon(Emotions.Bad);
    }
}
