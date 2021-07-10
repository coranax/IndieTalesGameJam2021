using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Ink.Runtime;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TextGame2
{
    public class GameStory : StoryComponent
    {
        #region Fields

        private static string _inkJson;
        private Story _inkStory;

        private string _storyText;
        private string _choiceText;

        // private CurrentText;
        //private CurrentChoices;
        #endregion


        #region Properties


        public SpriteFont Font { get; set; }
        public Color Color { get; set; }
        public int MaxTextWidth { get; set; }

        public Vector2 StoryPosition { get; set; }
        public Vector2 ChoicePosition { get; set; }

        public int NumOfChoices { get; private set; }

        #endregion


        #region Methods

        // constructors
        public GameStory() {
            _inkStory = new Story(_inkJson);

            

        } // empty constructor
        public GameStory(string inkJson) // constuctor takes only the json file address
        {
            _inkJson = inkJson;
            _inkStory = new Story(_inkJson);

            _storyText = "";
            _choiceText = "";
            NumOfChoices = 0;
        }

        // wraps text based on given length. cannot deal with line breaks.
        public string WrapText(string text, SpriteFont spriteFont, float maxLineWidth)
        {
            string[] words = text.Split(' ');

            StringBuilder builder = new StringBuilder();

            float lineWidth = 0f;
            float spaceWidth = spriteFont.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = spriteFont.MeasureString(word);

                if (lineWidth + size.X < maxLineWidth)
                {
                    builder.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    builder.Append("\n" + word + " ");
                    lineWidth = size.X + spaceWidth;
                }
            }
            return builder.ToString();
        }

        public void LoopStory()
        {
            // clear story text
            // storyText =  blahblah
            // if story can continue && (next?) button has been clicked, story text += blahblah
            // if it can't continue, load choices?

            _storyText = "";
            _choiceText = "";
            NumOfChoices = 0;
            while (_inkStory.canContinue)
            {
                _storyText += WrapText(_inkStory.Continue(), Font, 600) + "\n";
                Debug.WriteLine("Story Text: " + _storyText);
            }

            // break into separate method
            if (_inkStory.currentChoices.Count > 0)
            {
                for (int i = 0; i < _inkStory.currentChoices.Count; ++i)
                {
                    Choice choice = _inkStory.currentChoices[i];
                    _choiceText += "Choice " + (i + 1) + ". " + choice.text + "\n";
                    NumOfChoices++;
                    Debug.WriteLine("Choice Text: " + _choiceText);

                }
            }
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Trace.WriteLine("StoryPosition: " + StoryPosition);
            Trace.WriteLine("ChoicePosition: " + ChoicePosition);

            
            spriteBatch.DrawString(Font, _storyText, new Vector2(StoryPosition.X, StoryPosition.Y), Color);
            spriteBatch.DrawString(Font, _choiceText, new Vector2(ChoicePosition.X, ChoicePosition.Y), Color);
        }

        public override void Update(GameTime gameTime)
        {
            
        }


        #endregion

    }
}
