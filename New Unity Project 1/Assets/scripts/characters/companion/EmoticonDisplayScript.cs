using UnityEngine;
using System.Collections;

public enum Emotions {
    None = -1, 
    Bad,
    Good
}

public class EmoticonDisplayScript : MonoBehaviour
{

    public GameObject emoticonObject;
    public Transform head;

    private EmoticonScript _emoticon;
    private Animator _animator;
    private CompanionScript _companionScript;
    private CharacterAnimationHashes _hashes;

    private int[][] _emoticonStates = new[]
                                          {
                                              new[]
                                                  {
                                                      2, 9, 10, 11, 12, 13, 14, 15, 16
                                                  },
                                              new[]
                                                  {
                                                      1, 3, 4, 5, 6, 7, 8
                                                  }
                                          };

    private void Awake()
    {
        _animator = GetComponent< Animator >();
        _companionScript = GetComponent< CompanionScript >();
        _hashes = GetComponent< CharacterAnimationHashes >();

        _animator.SetLayerWeight( 1, 1 );

        var obj = Instantiate( emoticonObject, head.position + Vector3.up, Quaternion.identity ) as GameObject;
        _emoticon = obj.GetComponent< EmoticonScript >();
        _emoticon.transform.parent = transform;
    }

    public void InstantiateEmoticon( Emotions emoticon )
    {
        _emoticon.SetTexture( ( int ) emoticon );

        StopAllCoroutines();
        StartCoroutine( ResetCurrentEmotionState() );

        //  We'll check if he's currently already doing something with his hands
        //  so, for instance, he doesn't cheer whilst holding a boulder
        if ( _companionScript.currentAction == Character.CharacterAction.None ||
             _companionScript.currentAction == Character.CharacterAction.Waiting )
        {
            _animator.SetInteger( _hashes.emotionState,
                                  _emoticonStates[( int ) emoticon][
                                      Random.Range( 0, _emoticonStates[( int ) emoticon].Length )] );
        }

    }

    private IEnumerator ResetCurrentEmotionState()
    {
        yield return new WaitForSeconds( 0.5f );

        _animator.SetInteger( _hashes.emotionState, 0 );
    }
}
