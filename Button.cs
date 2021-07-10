using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace TextGame2
{
    public class Button : UIComponent
    {
        #region fields

        private MouseState _previousMouse;
        private MouseState _currentMouse;
        private bool _isHovering;

        private SpriteFont _font;
        private Texture2D _texture;

        #endregion

        #region properties

        public event EventHandler Click;
        public bool Clicked { get; set; }

        public string Text { get; set; } // button text
        public Color TextColor { get; set; } // button text color (default black, set in constructor)

        public Vector2 Position { get; set; } // position on game screen

        public Rectangle Rectangle // use position and size to draw button
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }

        // the way this is written, all buttons must share a texture or have their own.
        // todo modify this to allow for different sprite per button if needed.

        #endregion

        #region Methods

        // button constructor
        public Button (Texture2D texture, SpriteFont font)
        {
            _texture = texture;
            _font = font;
            TextColor = Color.Black;
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var colour = Color.White; // button color

            // button color will change when mouse hovers over it
            if (_isHovering)
                colour = Color.Gray;

            spriteBatch.Draw(_texture, Rectangle, colour); // todo use one of the overrides to call a different place on the button sprite sheet?

            if (!string.IsNullOrEmpty(Text))
            {
                // centering text on button
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), TextColor);
            }
        }

        public override void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse; // set previous mouse to whatever the mouse was just doing
            _currentMouse = Mouse.GetState(); // set current mouse from it's state

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1); // 1x1 rectange. smol.

            _isHovering = false; // needs to be false by default

            // if smol mouse rectangle intersects with the button rectangle
            if (mouseRectangle.Intersects(Rectangle)) // 
            {
                _isHovering = true;

                // if the mouse was pressed and then released
                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs()); // todo understand this
                }
            }
        }

        #endregion
    }
}
