using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TextGame2.GameStory
{
    public class CurrentChoiceText : StoryComponent
    {
        #region fields

        private string _choiceText;

        #endregion


        #region Properties

        public SpriteFont Font { get; set; }
        public Color Color { get; set; }
        public Vector2 Position { get; set; }

        public int NumOfChoices { get; set; }

        public string GetChoiceText()
        {
            return _choiceText;
        }
        public void SetChoiceText(string choiceText)
        {
            this._choiceText = choiceText;
        }
        #endregion

        public CurrentChoiceText() { }

        public CurrentChoiceText(string choiceText)
        {
            _choiceText = choiceText;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, _choiceText, new Vector2(Position.X, Position.Y), Color);
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
