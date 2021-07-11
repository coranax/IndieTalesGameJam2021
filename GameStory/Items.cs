using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TextGame2.GameStory
{
    public class Items
    {
        private Texture2D _texture;
        private Vector2 _position;
        private Rectangle _sourceRectangle;

        private int _itemSpriteIndex;

        public static List<string> itemsList = new List<string>();

        private readonly string[] _masterList = { 
            "rectangle",        //0
            "drawing",          //1
            "plushie",          //2
            "figurine",         //3
            "tshirt",           //4
            "hat",              //5
            "backpack",         //6
            "gamecart",         //7
            "gamedisc",         //8
            "autographman",     //9
            "autographwoman",   //10
            "autographmanimal", //11
            "soda",             //12
            "water",            //13
            "lemonade",         //14
            "energy",           //15 
            "coffee"            //16
        };

        public Items() { }

        public Items(Texture2D texture)
        {
            _texture = texture;
        }
        private int GetItemSpriteIndex(string name)
        {
            foreach (string str in _masterList)
            {
                if (str.Equals(name))
                    _itemSpriteIndex = Array.IndexOf(_masterList, name);
            }
            return _itemSpriteIndex;
        }

        // methods for use outside of this class... you might even call them... public... eheheh
        public static void AddItemToList(string item)
        {
            itemsList.Add(item);
        }

        public static void EmptyItemList()
        {
            if (itemsList != null)
                if (itemsList.Count > 0)
                    itemsList.Clear();
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (itemsList != null)
            {
                for (int i = 0; i < itemsList.Count; i++) // MATH!
                {
                    // x is position constant. y position is (index of item in list) * (height + buffer).
                    _position = new Vector2(677, 54 + (i * 70));

                    // source rectangle x vale is (index of item in sprite) * width. source rectangle y value is constant.
                    _sourceRectangle = new Rectangle((GetItemSpriteIndex(itemsList[i]) * 70), 0, 70, 60);

                    spriteBatch.Draw(_texture, _position, _sourceRectangle, Color.White);
                }
            }                
        }
    }
}
