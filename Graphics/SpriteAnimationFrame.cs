namespace Higurashi_Game.Graphics;

public class SpriteAnimationFrame
{
    private Sprite _sprite;
    public float TimeStamp { get; }
    public Sprite Sprite
    {
        get => _sprite;
        set
        {
            _sprite = value;
        }
    }

    public SpriteAnimationFrame(Sprite sprite, float timeStamp)
    {
        Sprite = sprite;
        TimeStamp = timeStamp;
    }
}