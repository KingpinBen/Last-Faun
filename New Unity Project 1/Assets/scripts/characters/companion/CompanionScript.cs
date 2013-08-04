using UnityEngine;

public class CompanionScript : AiCharacter
{
    private PlayerScript _player;
    private EmoticonDisplayScript _emoticon;

    protected override void Start()
    {
        if (_target == null)
        {
            _player = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
            _target = _player.GetComponentInChildren<AINode>();
        }

        _emoticon = GetComponent<EmoticonDisplayScript>();

        base.Start();
    }

    public EmoticonDisplayScript GetEmotions()
    {
        return _emoticon;
    }
}
