using UnityEngine;
using System.Collections;

public class CharacterAnimationHashes : MonoBehaviour
{

    public int speed;
    public int emotionState;
    public int completeAction;
    public int currentAction;
    public int slowed;

    void Awake()
    {
        speed = Animator.StringToHash( "Speed" );
        emotionState = Animator.StringToHash( "EmotionalState" );
        completeAction = Animator.StringToHash( "CompleteAction" );
        currentAction = Animator.StringToHash( "CurrentAction" );
        slowed = Animator.StringToHash( "Slowed" );
    }
}
