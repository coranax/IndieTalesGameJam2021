using Ink.Runtime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace TextGame2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteFont _font;
        private Texture2D _buttonTexture;

        // game window
        int gameWidth;
        int gameHeight;

        // game state enum
        private enum GameState { intro, desktop, gameplay, credits };

        // buttons, A, B, and C are choice buttons
        private Button buttonA, buttonB, buttonC, nextButton, mapButton, closeButton;

        // ink
        private static string _inkJson;

        private GameStory story;
        private GameStory choice;

        // list of ui components
        private List<UIComponent> _uiComponents;

        // list of story components
        private List<StoryComponent> _storyComponents;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // change window size
            gameWidth = 800;
            gameHeight = 600;
        }

        protected override void Initialize()
        {
            // change window size
            _graphics.PreferredBackBufferWidth = gameWidth;
            _graphics.PreferredBackBufferHeight = gameHeight;
            _graphics.ApplyChanges();

            _inkJson = File.ReadAllText("Ink/story.json"); // json file containing raw ink data
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _buttonTexture = Content.Load<Texture2D>("button");
            _font = Content.Load<SpriteFont>("font");

            story = new GameStory(_inkJson)
            {
                Font = _font,
                Color = Color.Black,
                MaxTextWidth = 400,
                StoryPosition = new Vector2(16, 16),
            };
            
            choice = new GameStory(_inkJson)
            {
                Font = _font,
                Color = Color.Black,
                ChoicePosition = new Vector2(432, 16),
            };


            #region buttons

            // choice buttons

            buttonA = new Button(_buttonTexture, _font)
            {
                Position = new Vector2(64, 400),
                Text = "A",
            };
            buttonA.Click += ButtonA_Click;

            buttonB = new Button(_buttonTexture, _font)
            {
                Position = new Vector2(192, 400),
                Text = "B",
            };
            buttonB.Click += ButtonB_Click;

            buttonC = new Button(_buttonTexture, _font)
            {
                Position = new Vector2(320, 400),
                Text = "C",
            };
            buttonC.Click += ButtonC_Click;

            // other UI buttons

            nextButton = new Button(_buttonTexture, _font)
            {
                Position = new Vector2(64, 464),
                Text = "Next",
            };
            nextButton.Click += NextButton_Click;

            mapButton = new Button(_buttonTexture, _font)
            {
                Position = new Vector2(192, 464),
                Text = "Map",
            };
            mapButton.Click += MapButton_Click;

            closeButton = new Button(_buttonTexture, _font)
            {
                Position = new Vector2(320, 464),
                Text = "Close",
            };
            closeButton.Click += CloseButton_Click;

            #endregion

            // add all ui components to list 
            _uiComponents = new List<UIComponent>()
              {
                buttonA,
                buttonB,
                buttonC,
                nextButton,
                mapButton,
                closeButton
              };

            // add all story components to list
            _storyComponents = new List<StoryComponent>()
            {
                story, choice
            };
        }

        // button click methods
        #region button methods

        // choice buttons

        private void ButtonA_Click(object sender, System.EventArgs e)
        {
            
            buttonA.Clicked = true;
            Trace.WriteLine("Button A Pressed");
        }

        private void ButtonB_Click(object sender, System.EventArgs e)
        {
            buttonB.Clicked = true;
            Trace.WriteLine("Button B Pressed");
        }

        private void ButtonC_Click(object sender, EventArgs e)
        {
            buttonC.Clicked = true;
            Trace.WriteLine("Button C Pressed");
        }

        // other ui buttons

        private void NextButton_Click(object sender, EventArgs e)
        {
            story.LoopStory();
            nextButton.Clicked = true;
            Trace.WriteLine("Button NEXT Pressed");
        }

        private void MapButton_Click(object sender, EventArgs e)
        {
            mapButton.Clicked = true;
            Trace.WriteLine("Button MAP Pressed");
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Exit();
        }

        #endregion


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // update ui components
            foreach (Button button in _uiComponents)
                button.Update(gameTime);

            // update story components
            foreach (GameStory gameStory in _storyComponents)
                gameStory.Update(gameTime);


            // reset bools

            buttonA.Clicked = false;
            buttonB.Clicked = false;
            buttonC.Clicked = false;
            nextButton.Clicked = false;
            mapButton.Clicked = false;
            closeButton.Clicked = false;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);

            // draw buttons based on number of current choices
            switch (choice.NumOfChoices)
            {
                case 0:
                    nextButton.Draw(gameTime, _spriteBatch);
                    break;
                case 1:
                    buttonA.Draw(gameTime, _spriteBatch);
                    break;
                case 2:
                    buttonA.Draw(gameTime, _spriteBatch);
                    buttonB.Draw(gameTime, _spriteBatch);
                    break;
                case 3:
                    buttonA.Draw(gameTime, _spriteBatch);
                    buttonB.Draw(gameTime, _spriteBatch);
                    buttonC.Draw(gameTime, _spriteBatch);
                    break;
                default:
                    Trace.WriteLine("Choice Error");
                    break;
            }

            // more buttons
            mapButton.Draw(gameTime, _spriteBatch);
            closeButton.Draw(gameTime, _spriteBatch);

            // draw story and choice text
            story.Draw(gameTime, _spriteBatch);
            choice.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
