using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class PokemonText
    {
        public Image Text;
        public Image TextShadow;

        [XmlIgnore]
        public Vector2 Position
        {
            get { return Text.Position; }
            private set { }
        }

        [XmlIgnore]
        public Rectangle SourceRect
        {
            get { return Text.SourceRect; }
            private set { }
        }

        public PokemonText() { }

        public PokemonText(string text, string fontName, Color textColor, Color shadowColor)
        {
            Text = new Image
            {
                FontName = fontName,
                Text = text,
                UseFontColor = true,
                FontColor = textColor
            };
            TextShadow = new Image
            {
                FontName = fontName,
                Text = text,
                UseFontColor = true,
                FontColor = shadowColor
            };

        }

        public void LoadContent()
        {
            if (!TextShadow.Text.Equals(Text.Text))
            {
                TextShadow.Text = Text.Text;
                TextShadow.FontName = Text.FontName;
            }

            Text.LoadContent();
            TextShadow.LoadContent();
        }

        public void UnloadContent()
        {
            Text.UnloadContent();
            TextShadow.UnloadContent();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            TextShadow.Draw(spriteBatch);
            Text.Draw(spriteBatch);
        }


        public void SetPosition(Vector2 position)
        {
            Text.Position = position;
            TextShadow.Position = new Vector2(Text.Position.X + 2, Text.Position.Y + 2);
        }

    }
}
