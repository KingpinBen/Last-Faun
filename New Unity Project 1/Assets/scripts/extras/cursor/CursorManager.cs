using UnityEngine;
using System.Collections;

public enum CursorType
{
    None = -1, Normal, Inspect,
    PickUp, PutDown, Rotate, PullPush, 
    Wait, Cancel
}

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;
    public Texture2D cursors;
    public int cursorCount = 8;

    private CursorType _activeCursorType = CursorType.Normal;
    private CursorType _backUpCursorType;
    private Rect _cursorRect;
    private Vector2 _offset;
    private Vector2 _cursorDimensions;
    private Vector2 _cursorTileDimensions;
    private CursorGestureStatus _status;
    private Vector3 _mousePosition;
    private Rect _guiRect;

    private void Start()
    {
        const float rowCount = 4.0f;    //  4 status states. Can change if more get added.
                                        //  float value so the below divisions return float values.

        //Screen.showCursor = false;
        instance = this;

        _cursorDimensions = new Vector2(
            (float)cursors.width / cursorCount,
            cursors.height / rowCount);

        _cursorTileDimensions = new Vector2(
            1 / (float)cursorCount, 
            1 / rowCount);

        ChangeCursor(CursorType.Normal);
        ChangeCursorStatus(CursorGestureStatus.Normal);
    }

    private void Update()
    {
        if (InGamePause.instance.GetIsPaused()) return;

        if (_status != CursorGestureStatus.Capturing && (int)_backUpCursorType >= 0)
        {
            ChangeCursor(_backUpCursorType);
            _backUpCursorType = CursorType.None;
        }

        switch(_status)
        {
            case CursorGestureStatus.Successful:
            case CursorGestureStatus.Failed:
                StartCoroutine(ResetCursorStatus());
                break;
        }

        _mousePosition = Input.mousePosition;
        _guiRect = new Rect(
            _mousePosition.x - _offset.x, 
            Screen.height - _mousePosition.y - _offset.y, 64, 64);
    }

    private void OnGUI()
    {
        if (InGamePause.instance.GetIsPaused()) return;

        GUI.DrawTextureWithTexCoords(_guiRect,
            cursors, _cursorRect);
    }

    public void ChangeCursor(CursorType type)
    {
        if (_status == CursorGestureStatus.Capturing)
        {
            //  If we're capturing a gesture, we want to keep the current icon on screen
            //  so we'll save a copy of what it SHOULD be if we weren't capturing to 
            //  apply later.
            _backUpCursorType = type;
        }
        else
        {
            //  The normal type hand should use the top left as the mouse location
            //  while the others should use the middle of the icon.
            if (type != CursorType.Normal)
                _offset = _cursorDimensions * .25f;
            else
                _offset = new Vector2(10, 10);

            _activeCursorType = type;
            _cursorRect = new Rect(
                _cursorTileDimensions.x * (int)_activeCursorType, 
                _cursorTileDimensions.y * (int)_status, 
                _cursorTileDimensions.x, 
                _cursorTileDimensions.y);
        }
    }

    public void ChangeCursorStatus(CursorGestureStatus status)
    {
        //  We only have the one coroutine running in this script so we'll cancel it.
        //  We do this just to reset it for next time it's needed.
        if (_status == CursorGestureStatus.Successful ||
            _status == CursorGestureStatus.Failed)
            StopAllCoroutines();

        _status = status;
        _cursorRect = new Rect(_cursorTileDimensions.x * (int)_activeCursorType, _cursorTileDimensions.y * (int)_status, _cursorTileDimensions.x, _cursorTileDimensions.y);
    }

    private IEnumerator ResetCursorStatus()
    {
        yield return new WaitForSeconds(2.5f);

        //  We don't want to force it to change colour again if it's not necessary.
        if (_status == CursorGestureStatus.Successful || 
            _status == CursorGestureStatus.Failed)
                ChangeCursorStatus(CursorGestureStatus.Normal);
    }

    public enum CursorGestureStatus
    {
        Successful,
        Failed,
        Capturing,
        Normal
    }
}
