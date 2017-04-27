using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace JogoDaVelha
{
    class UIButton
    {

        private Vector2 position, size;
        private Texture2D background;
        private String text;
        private SpriteFont font;

        public UIButton(Vector2 position, Vector2 size, Texture2D background, string text, SpriteFont font)
        {
            this.position = position;
            this.size = size;
            this.background = background;
            this.text = text;
            this.font = font;
        }


        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(background, position, null, Color.White, 0.0f, Vector2.Zero, new Vector2(size.X / background.Width, size.Y / background.Height), SpriteEffects.None, 0.0f);
            Vector2 textSize = font.MeasureString(text);

            spriteBatch.DrawString(
                font,
                text,
                position + (size / 2.0f),   //position 
                Color.White,                //color
                0.0f,                       //rotation
                textSize / 2.0f,            //origin (pivot)
                Vector2.One,                //scale
                SpriteEffects.None,
                0.0f
                );
        }

        public bool TesteClick(Vector2 pTeste)
        {
            if (((pTeste.X > position.X) && (pTeste.X < position.X + size.X)) && ((pTeste.Y > position.Y) && (pTeste.Y < position.Y + size.Y)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool setBackground(Texture2D background)
        {
            this.background = background;
            return true;
        }

        public Texture2D getBackground()
        {
            return background;
        }

    }
}
