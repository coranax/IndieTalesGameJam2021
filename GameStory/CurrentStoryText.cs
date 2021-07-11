using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TextGame2.GameStory
{
    public class CurrentStoryText : StoryComponent
    {

        #region fields

        private string _storyText;

        #endregion


        #region Properties

        public SpriteFont Font { get; set; }
        public Color Color { get; set; }
        public Vector2 Position { get; set; }

        public int MaxLineWidth { get; set; }
       
        public string GetStoryText()
        {
            return _storyText;
        }
        public  void SetStoryText(string storyText)
        {
            this._storyText = storyText;
        }

        #endregion

        public CurrentStoryText() { }
        public CurrentStoryText(string storyText)
        {

            _storyText = storyText;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, _storyText, new Vector2(Position.X, Position.Y), Color);
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
