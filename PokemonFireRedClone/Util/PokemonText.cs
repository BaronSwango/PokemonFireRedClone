using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class PokemonText
    {
        private readonly Image textShadowX;
        private readonly Image textShadowY;
        private readonly Image textShadowXY;

        public Image Text;

        [XmlIgnore]
        public Vector2 Position
        {
            get { return Text.Position; }
            set { Text.Position = value; }
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
            textShadowX = new Image
            {
                FontName = fontName,
                Text = text,
                UseFontColor = true,
                FontColor = shadowColor
            };
            textShadowY = new Image
            {
                FontName = fontName,
                Text = text,
                UseFontColor = true,
                FontColor = shadowColor
            };
            textShadowXY = new Image
            {
                FontName = fontName,
                Text = text,
                UseFontColor = true,
                FontColor = shadowColor
            };
        }

        public void LoadContent()
        {
            if (!textShadowX.Text.Equals(Text.Text))
            {
                textShadowX.Text = textShadowY.Text = textShadowXY.Text = Text.Text;
                textShadowX.FontName = textShadowY.FontName = textShadowXY.FontName = Text.FontName;
            }

            Text.LoadContent();
            textShadowX.LoadContent();
            textShadowY.LoadContent();
            textShadowXY.LoadContent();
        }

        public void UnloadContent()
        {
            Text.UnloadContent();
            textShadowX.UnloadContent();
            textShadowY.UnloadContent();
            textShadowXY.UnloadContent();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            textShadowX.Draw(spriteBatch);
            textShadowY.Draw(spriteBatch);
            textShadowXY.Draw(spriteBatch);
            Text.Draw(spriteBatch);
        }

        public void UpdateText(string text)
        {
            Text.Text = textShadowX.Text = textShadowY.Text = textShadowXY.Text = text;
            Text.ReloadText();
            textShadowX.ReloadText();
            textShadowY.ReloadText();
            textShadowXY.ReloadText();
        }

        public void SetPosition(Vector2 position)
        {
            Text.Position = position;
            textShadowX.Position = new Vector2(Text.Position.X + 4, Text.Position.Y);
            textShadowY.Position = new Vector2(Text.Position.X, Text.Position.Y + 4);
            textShadowXY.Position = new Vector2(Text.Position.X + 4, Text.Position.Y + 4);
        }

        public void OffsetX(float value)
        {
            SetX(Text.Position.X + value);
        }

        public void OffsetY(float value)
        {
            SetY(Text.Position.Y + value);
        }

        public void SetX(float coord)
        {
            Text.Position.X = textShadowY.Position.X = coord; 
            textShadowX.Position.X = textShadowXY.Position.X = coord + 4;
        }

        public void SetY(float coord)
        {
            Text.Position.Y = textShadowX.Position.Y = coord;
            textShadowY.Position.Y = textShadowXY.Position.Y = coord + 4;
        }

        public void SetAlpha(float alpha)
        {
            Text.Alpha = textShadowX.Alpha = textShadowY.Alpha = textShadowXY.Alpha = alpha;
        }

    }
}
