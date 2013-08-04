using UnityEngine;

public class SwipeEffectScript : MonoBehaviour
{
    public enum Transition
    {
        Hidden, 
        FadingIn, 
        FadingOut
    }

    private Transition _transitionState = Transition.FadingIn;
    private bool _isCompleted;
    private const float TRANSITION_SPEED = 1.0f;

    void Awake()
    {
        SwitchToFadeIn();
    }

    void Update()
    {
        if (_isCompleted) return;
        if (_transitionState == Transition.FadingIn)
        {
            Vector3 position = transform.position;
            position.x = position.x + (Time.deltaTime * 1);
            transform.position = position;

            if (transform.position.x > 1.49)
            {
                transform.position = new Vector3(1.5f, 0.49f, 0.0f);

                _isCompleted = true;
            }
        }
        else
        {
            if (_transitionState == Transition.FadingOut)
            {
                var position = transform.position;
                position.x = position.x + (Time.deltaTime * 1);
                transform.position = position;

                if (transform.position.x > 0.49f)
                {
                    transform.position = new Vector3(.5f, 0.49f, 0.0f);

                    _isCompleted = true;
                }
            }
        }
    }

    public void SwitchToFadeIn()
    {
        transform.position = new Vector3(0.5f, 0.49f, 0);
        _transitionState = Transition.FadingIn;
        _isCompleted = false;
    }

    public void SwitchToFadeOut()
    {
        transform.position = new Vector3(-0.5f, 0.49f, 0);
        _transitionState = Transition.FadingOut;
        _isCompleted = false;
    }

    public bool IsCompleted()
    {
        return _isCompleted;
    }
}
