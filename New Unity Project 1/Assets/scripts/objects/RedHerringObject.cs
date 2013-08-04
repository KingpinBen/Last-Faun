using UnityEngine;
using System.Collections;

public class RedHerringObject : GestureObject
{

    public bool disableOnUse = true;
    public Emotions emotionOnUse = Emotions.None;
    public float timeBeforeReset;

    private bool _waitingToReset;

    protected override void Start()
    {
        mouseOverCursor = CursorType.Inspect;

        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (_successfulGesture)
        {
            //  If it's been deactivated, we no longer want to use this object.
            if (aiTargetNode.objectActive == false)
                _successfulGesture = false;

            if (_waitingToReset)
                return;

            if (_companionScript.waitingAtTarget)
            {
                _waitingToReset = true;

                if (disableOnUse)
                    DeactivateObject(null);

                StopAllCoroutines();

                if (timeBeforeReset > .35f)
                    StartCoroutine(ResetCompanion());
                else
                    CompleteHerring();
            }
        }
        else
        {
            if (_waitingToReset)
                return;

            if (_objectUsed &&
                _listening == false)
            {
                _objectUsed = false;
                _player.ActivateObject(this);
            }
        }
    }

    protected override void ProcessReceivedGesture()
    {
        if (_receivedGesture == GestureType.None) 
            return;

        _successfulGesture = true;
        _receivedGesture = GestureType.None;
    }

    IEnumerator ResetCompanion()
    {
        yield return new WaitForSeconds(timeBeforeReset);

        CompleteHerring();
    }

    void CompleteHerring()
    {
        _successfulGesture = false;
        _waitingToReset = false;
        _companionScript.GetEmotions().InstantiateEmoticon(emotionOnUse);

        switch (emotionOnUse)
        {
            case Emotions.Good:
                _companionScript.GetEmotions().InstantiateEmoticon(Emotions.Good);
                CursorManager.instance.ChangeCursorStatus(CursorManager.CursorGestureStatus.Successful);
                break;
            case Emotions.Bad:
                _companionScript.GetEmotions().InstantiateEmoticon(Emotions.Bad);
                FailedGesture();
                break;
            case Emotions.None:
                break;
        }
    }
}
