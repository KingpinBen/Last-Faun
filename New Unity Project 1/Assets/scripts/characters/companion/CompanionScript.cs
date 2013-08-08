using UnityEngine;

public class CompanionScript : AiCharacter
{
    private PlayerScript _player;
    private EmoticonDisplayScript _emoticon;

    protected override void Awake()
    {
        base.Awake();

        _emoticon = GetComponent<EmoticonDisplayScript>();
    }

    protected override void Start()
    {
        if (_target == null)
        {
            _player = GameObject.FindWithTag("Player").GetComponent<PlayerScript>();
            _target = _player.GetComponentInChildren<AINode>();
        }

        base.Start();
    }

    public EmoticonDisplayScript GetEmotions()
    {
        return _emoticon;
    }
}
