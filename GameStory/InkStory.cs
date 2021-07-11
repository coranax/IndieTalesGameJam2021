using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Ink.Runtime;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TextGame2.GameStory;

namespace TextGame2
{
    public class InkStory
    {
        #region Fields

        private static string _inkJson;
        private Story _inkStory;

        private string _storyText;
        private string _choiceText;

        private bool _hasStoryEnded;

        private CurrentStoryText _currentStory;
        private CurrentChoiceText _currentChoice;

        #endregion


        #region Methods

        // constructors
        public InkStory() {} // empty constructor
        
        // constuctor takes only the json file address and font
        public InkStory(string inkJson, SpriteFont _font) 
        {
            _inkJson = inkJson;
            _inkStory = new Story(_inkJson);

            _storyText = "";
            _choiceText = "";

            _hasStoryEnded = false;

            _currentStory = new CurrentStoryText(_storyText)
            {
                Font = _font,
                Color = Color.Black,
                Position = new Vector2(40, 40),
                MaxLineWidth = 590,
            };

            _currentChoice = new CurrentChoiceText(_choiceText)
            {
                Font = _font,
                Color = Color.Black,
                Position = new Vector2(32, 316), // for testing only
            };
        }

        // wraps text based on given length. cannot deal with line breaks.
        private string WrapText(string text, SpriteFont spriteFont, float maxLineWidth)
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
                    if (word.Contains("\n"))
                    {
                        builder.Append(word);
                        lineWidth = size.X + spaceWidth;
                    }
                    else
                    {
                        builder.Append(word + " ");
                        lineWidth += size.X + spaceWidth;
                    }
                }
                else
                {
                    if (word.Contains("\n"))
                    {
                        builder.Append("\n" + word);
                        lineWidth = size.X + spaceWidth;
                    } else
                    {
                        builder.Append("\n" + word + " ");
                        lineWidth = size.X + spaceWidth;
                    }
                }
            }
            return builder.ToString();
        }

        public void LoopStory()
        {
            // reset values
            //_storyText = ""; reset this in SetChoice()
            _choiceText = "";
            _currentChoice.NumOfChoices = 0;

            // parse story text
            if (_inkStory.canContinue)
            {
                _storyText += WrapText(_inkStory.Continue(), _currentStory.Font, _currentStory.MaxLineWidth) + "\n";
            } else if (!_inkStory.canContinue && (_inkStory.currentChoices.Count == 0))
            {
                setHasStoryEnded(true);
                Trace.WriteLine("YOU HAVE REACHED THE END...!");
            }
            
            // parse choices
            if (_inkStory.currentChoices.Count > 0)
            {
                for (int i = 0; i < _inkStory.currentChoices.Count; ++i)
                {
                    Choice choice = _inkStory.currentChoices[i];
                    _choiceText += "Choice " + (i + 1) + ". " + choice.text + "\n";
                    _currentChoice.NumOfChoices++;
                }
            }
            _currentStory.SetStoryText(_storyText);
            _currentChoice.SetChoiceText(_choiceText);
        }

        public int GetNumberOfChoices() // simplify this using _inkStory.currentChoices.Count?
        {
            return _currentChoice.NumOfChoices;
        }

        public void SetChoice(int choice)
        {
            switch (choice)
            {
                case 0:
                    _inkStory.ChooseChoiceIndex(0);
                    _storyText = "";
                    break;
                case 1:
                    _inkStory.ChooseChoiceIndex(1);
                    _storyText = "";
                    break;
                case 2:
                    _inkStory.ChooseChoiceIndex(2);
                    _storyText = "";
                    break;
                default:
                    Trace.WriteLine("ERROR in choices?");
                    _storyText = "";
                    break;
            }
            LoopStory();
        }

        public string GetChoiceAtIndex(int num)
        {
            List<string> list = new List<string>();

            if (_inkStory.currentChoices.Count >= num)
            {
                foreach (Choice choice in _inkStory.currentChoices)
                {
                    list.Add(choice.text);
                }
                return list[num];
            } else
            {
                return "ERROR Choice not Found :(";
            }
        }

        public void setHasStoryEnded(bool state)
        {
            _hasStoryEnded = state;
        }

        public bool getHasStoryEnded()
        {
            return _hasStoryEnded;
        }

        public void ParseTags()
        {
            // todo
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
            _currentStory.Draw(gameTime, spriteBatch);
            //_currentChoice.Draw(gameTime, spriteBatch);
        }

/*        public virtual void Update(GameTime gameTime)
        {
            
        }*/ //todo do i need this

        #endregion

    }
}
