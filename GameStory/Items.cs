using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TextGame2.GameStory
{
    public class Items
    {
        private string _name;

        private Texture2D _texture;
        private Vector2 _position;
        private Rectangle _sourceRectangle;

        private List<Items> _itemsList;



        /*
         * List of items index, name/tag:
         * 0 : rectangle
         * 1 : drawing  
         * 2 : plushie
         * 3 : figurine
         * 4 : tshirt
         * 5 : hat
         * 6 : backpack
         * 7 : gamecart
         * 8 : gamedisc
         * 9 : autographman
         * 10 : autographwoman
         * 11 : autographmanimal
         * 12 : soda
         * 13 : water
         * 14 : lemonade
         * 15 : energy
         * 16 : coffee
        */

        /*
         * item dimensions: 70 x 60
        */

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 startingPosition = new Vector2(677, 54);
            Rectangle startingSourceRectangle = new Rectangle(0, 0, 70, 60);

            for (int i = 0; i < _itemsList.Count; i++)
            {
                int itemSpriteIndex = 0; // TODO YOU ARE HERE - make a list

                // load tags from api in loop method
                // pass this draw method through main game class

                _position = new Vector2(677, 54 + (i * 70)); // x is constant. y position is (index of item in list) * (height + buffer).
                _sourceRectangle = new Rectangle((itemSpriteIndex * 70), 0, 70, 60); // x source rectangle is (index of item in sprite) * width. y is constant.
                spriteBatch.Draw(_texture, _position, _sourceRectangle, Color.White);
            }
        }

    }
}
