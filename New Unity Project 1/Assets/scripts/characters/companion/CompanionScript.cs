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

    protected override void Update()
    {
        base.Update();

        if (_canReachTarget)
        {
            if (_emoticon.currentEmoticon == 17)
                _emoticon.currentEmoticon = 0;
        }
        else
        {
            if (_emoticon.currentEmoticon == 0)
                _emoticon.currentEmoticon = 17;
        }
    }

    public EmoticonDisplayScript GetEmotions()
    {
        return _emoticon;
    }
}
