using Higurashi_When_They_Cry_Hashiru.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Higurashi_When_They_Cry_Hashiru.Entities;

public class Keiichi : IGameEntity
{
    private const int IDLE_SPRITE_POS_X = 0;
    private const int IDLE_SPRITE_POS_Y = 0;
    private const int SPRITE_WIDTH = 85;
    private const int SPRITE_HEIGHT = 113;

    
    public int DrawOrder { get; }
    public Sprite Sprite { get; set; }

    public Keiichi(Texture2D spriteSheet)
    {
        Sprite = new Sprite(spriteSheet, IDLE_SPRITE_POS_X, IDLE_SPRITE_POS_Y, SPRITE_WIDTH, SPRITE_HEIGHT);
    }
    
    public void Update(GameTime gameTime)
    {

    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        Sprite.Draw(spriteBatch, new Vector2(0,0));
    }
}