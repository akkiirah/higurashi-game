using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Higurashi_When_They_Cry_Hashiru.Entities;

public interface IGameEntity
{
    int DrawOrder { get; }
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch, GameTime gameTime);
}