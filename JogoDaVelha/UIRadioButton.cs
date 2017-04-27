using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JogoDaVelha
{
    class UIRadioButton
    {
        private Vector2 position, size;
        private Texture2D background;
        private String text;
        private SpriteFont font;

        public UIRadioButton(Vector2 position, Vector2 size, Texture2D background, string text, SpriteFont font)
        {
            this.position = position;
            this.size = size;
            this.background = background;
            this.text = text;
            this.font = font;
        }


    }
}
