using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TextGame2
{
    public abstract class UIComponent
    {
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public abstract void Update(GameTime gameTime);
    }
}

