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

        private Texture2D _titleTexture;
        private Texture2D _creditsTexture;

        private Texture2D _uiTexture;
        private Texture2D _itemTexture;
        private Texture2D _mapTexture;
        private Texture2D _catIconTexture;

        private Texture2D _bigButtonTexture;
        private Texture2D _littleButtonTexture;
        private Texture2D _xButtonTexture;

        // game window
        int gameWidth;
        int gameHeight;

        // game state enum
        public enum GameState { Title, Desktop, CheckMap, Play, Credits }; // todo desktop
        GameState gameState;

        // buttons, A, B, and C are choice buttons
        private Button startButton, buttonA, buttonB, buttonC, nextButton, mapButton, closeButton;

        // ink
        private static string _inkJson;
        private InkStory story;

        // list of ui components
        private List<UIComponent> _uiComponents;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // change window size
            gameWidth = 800;
            gameHeight = 608;
        }

        protected override void Initialize()
        {
            // change window size
            _graphics.PreferredBackBufferWidth = gameWidth;
            _graphics.PreferredBackBufferHeight = gameHeight;
            _graphics.ApplyChanges();

            _inkJson = File.ReadAllText("Ink/story.json"); // json file containing raw ink data

            // set starting gamestate
            gameState = GameState.Title;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("font");

            _titleTexture = Content.Load<Texture2D>("Sprites/title");
            _creditsTexture = Content.Load<Texture2D>("Sprites/credits");

            _uiTexture = Content.Load<Texture2D>("Sprites/uiSheet");
            _itemTexture = Content.Load<Texture2D>("Sprites/inventorySheet");
            _mapTexture = Content.Load<Texture2D>("Sprites/map");
            _catIconTexture = Content.Load<Texture2D>("Sprites/catIcon");

            _bigButtonTexture = Content.Load<Texture2D>("Sprites/bigButton");
            _littleButtonTexture = Content.Load<Texture2D>("Sprites/littleButton");
            _xButtonTexture = Content.Load<Texture2D>("Sprites/xButton");

            // ink story
            story = new InkStory(_inkJson, _font);

            #region buttons

            // !-- IMPORTANT --! do not let button positions+rectanges intersect!

            // choice buttons
            buttonA = new Button(_bigButtonTexture, _font)
            {
                Position = new Vector2(176, 432),                
                Text = "",
            };
            buttonA.Click += ButtonA_Click;

            buttonB = new Button(_bigButtonTexture, _font)
            {
                Position = new Vector2(176, 484),
                Text = "",
            };
            buttonB.Click += ButtonB_Click;

            buttonC = new Button(_bigButtonTexture, _font)
            {
                Position = new Vector2(176, 536),
                Text = "",
            };
            buttonC.Click += ButtonC_Click;

            // other UI buttons

            startButton = new Button(_littleButtonTexture, _font)
            {
                Position = new Vector2((gameWidth / 2) - (_littleButtonTexture.Width / 2), 350),
                Text = "Start!",
            };
            startButton.Click += StartButton_Click;

            nextButton = new Button(_littleButtonTexture, _font)
            {
                Position = new Vector2(656, 448),
                Text = "Next",
            };
            nextButton.Click += NextButton_Click;

            mapButton = new Button(_littleButtonTexture, _font)
            {
                Position = new Vector2(656, 512),
                Text = "Map",
            };
            mapButton.Click += MapButton_Click;

            closeButton = new Button(_xButtonTexture, _font)
            {
                Position = new Vector2(772, 2),
                Text = "",
            };
            closeButton.Click += CloseButton_Click;

            #endregion

            // add all ui components to list 
            _uiComponents = new List<UIComponent>()
              {
                buttonA,
                buttonB,
                buttonC,
                startButton,
                nextButton,
                mapButton,
                closeButton
              };
        }


        #region button methods

        // choice buttons

        private void ButtonA_Click(object sender, System.EventArgs e)
        {
            if (gameState == GameState.Play && (story.GetNumberOfChoices() >= 1))
            {
                story.SetChoice(0);
            }
            Trace.WriteLine("Button A Pressed");
        }

        private void ButtonB_Click(object sender, System.EventArgs e)
        {
            if (gameState == GameState.Play && (story.GetNumberOfChoices() >= 2))
            {
                story.SetChoice(1);
            }
            Trace.WriteLine("Button B Pressed");
        }

        private void ButtonC_Click(object sender, EventArgs e)
        {
            if (gameState == GameState.Play && (story.GetNumberOfChoices() >= 3))
            {
                story.SetChoice(2);
            }
            Trace.WriteLine("Button C Pressed");
        }

        // other ui buttons

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (gameState == GameState.Title)
            {
                gameState = GameState.Play;
                story.LoopStory();
            }
            Trace.WriteLine("Button START Pressed");
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if (gameState == GameState.Play)
            {
                story.LoopStory();
            }
            Trace.WriteLine("Button NEXT Pressed");
        }

        private void MapButton_Click(object sender, EventArgs e)
        {
            if (gameState == GameState.Play)
            {
                gameState = GameState.CheckMap;
            } else if (gameState == GameState.CheckMap)
            {
                gameState = GameState.Play;
            }

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

            if (story.getHasStoryEnded())
                gameState = GameState.Credits;

            // update ui components
            foreach (Button button in _uiComponents)
                button.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null);

            // always draw background
            _spriteBatch.Draw(_uiTexture, Vector2.Zero, new Rectangle(0, 0, gameWidth, gameHeight), Color.White);

            if (gameState == GameState.Title)
            {
                // draw title screen
                _spriteBatch.Draw(_titleTexture, Vector2.Zero, new Rectangle(0, 0, gameWidth, gameHeight), Color.White);

                // draw start button
                startButton.Draw(gameTime, _spriteBatch);
            }

            if (gameState == GameState.Play || gameState == GameState.CheckMap)
            {
                // draw ui and cat icon
                _spriteBatch.Draw(_uiTexture, Vector2.Zero, new Rectangle(800, 0, gameWidth, gameHeight), Color.White);
                _spriteBatch.Draw(_catIconTexture, new Vector2(35, 451), new Rectangle(0, 0, 122, 122), Color.White);

                // draw map button
                mapButton.Draw(gameTime, _spriteBatch);
            }

            if (gameState == GameState.Play)
            {
                // draw buttons based on number of current choices
                switch (story.GetNumberOfChoices())
                {
                    case 0:
                        nextButton.Draw(gameTime, _spriteBatch);
                        break;
                    case 1:
                        buttonA.Draw(gameTime, _spriteBatch);
                        buttonA.Text = story.GetChoiceAtIndex(0);
                        break;
                    case 2:
                        buttonA.Draw(gameTime, _spriteBatch);
                        buttonA.Text = story.GetChoiceAtIndex(0);
                        buttonB.Draw(gameTime, _spriteBatch);
                        buttonB.Text = story.GetChoiceAtIndex(1);
                        break;
                    case 3:
                        buttonA.Draw(gameTime, _spriteBatch);
                        buttonA.Text = story.GetChoiceAtIndex(0);
                        buttonB.Draw(gameTime, _spriteBatch);
                        buttonB.Text = story.GetChoiceAtIndex(1);
                        buttonC.Draw(gameTime, _spriteBatch);
                        buttonC.Text = story.GetChoiceAtIndex(2);
                        break;
                    default:
                        Trace.WriteLine("ERROR default choice case hit");
                        break;
                }

                // draw story and choice text
                story.Draw(gameTime, _spriteBatch);

            }

            // draw map
            if (gameState == GameState.CheckMap)
            {
                _spriteBatch.Draw(_mapTexture, Vector2.Zero, Color.White);
            }

            if (gameState == GameState.Credits)
            {
                _spriteBatch.Draw(_creditsTexture, Vector2.Zero, new Rectangle(0, 0, gameWidth, gameHeight), Color.White);
            }

            // always draw close button
            closeButton.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
