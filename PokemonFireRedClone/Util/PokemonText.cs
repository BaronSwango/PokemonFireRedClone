using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class PokemonText
    {
        private Image textShadowX;
        private Image textShadowY;
        private Image textShadowXY;
        private Color shadowColor;
        private readonly bool fromXML;

        public Image Image;
        public int R, G, B;

        [XmlIgnore]
        public Vector2 Position
        {
            get { return Image.Position; }
            private set { }
        }

        [XmlIgnore]
        public Rectangle SourceRect
        {
            get { return Image.SourceRect; }
            private set { }
        }

        [XmlIgnore]
        public string Text
        {
            get { return Image.Text; }
            set
            {
                Image.Text = value;
                if (textShadowX != null)
                {
                    textShadowX.Text = value;
                    textShadowY.Text = value;
                    textShadowXY.Text = value;
                }
            }
        }

        [XmlIgnore]
        public bool IsLoaded
        {
            get { return Image.EffectList.Count != 0; }
            private set { }
        }

        public PokemonText() { fromXML = true; }

        public PokemonText(string text, string fontName, Color textColor, Color shadowColor)
        {
            Image = new Image
            {
                FontName = fontName,
                Text = text,
                UseFontColor = true,
                FontColor = textColor
            };

            this.shadowColor = shadowColor;
        }
        
        public void LoadContent()
        {
            if (fromXML)
                shadowColor = new Color(R, G, B);

            textShadowX = new Image
            {
                FontName = Image.FontName,
                Text = Image.Text,
                UseFontColor = true,
                FontColor = shadowColor
            };
            textShadowY = new Image
            {
                FontName = Image.FontName,
                Text = Image.Text,
                UseFontColor = true,
                FontColor = shadowColor
            };
            textShadowXY = new Image
            {
                FontName = Image.FontName,
                Text = Image.Text,
                UseFontColor = true,
                FontColor = shadowColor
            };

            if (!textShadowX.Equals(Image.Text))
            {
                textShadowX.Text = textShadowY.Text = textShadowXY.Text = Image.Text;
                textShadowX.FontName = textShadowY.FontName = textShadowXY.FontName = Image.FontName;
            }

            Image.LoadContent();
            textShadowX.LoadContent();
            textShadowY.LoadContent();
            textShadowXY.LoadContent();
        }

        public void UnloadContent()
        {
            Image.UnloadContent();
            textShadowX.UnloadContent();
            textShadowY.UnloadContent();
            textShadowXY.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            Image.Update(gameTime);
            textShadowX.Update(gameTime);
            textShadowY.Update(gameTime);
            textShadowXY.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            textShadowX.Draw(spriteBatch);
            textShadowY.Draw(spriteBatch);
            textShadowXY.Draw(spriteBatch);
            Image.Draw(spriteBatch);
        }

        public void ReloadText()
        {
            Image.ReloadText();
            textShadowX.ReloadText();
            textShadowY.ReloadText();
            textShadowXY.ReloadText();
        }

        public void UpdateText(string text)
        {
            Image.Text = textShadowX.Text = textShadowY.Text = textShadowXY.Text = text;
            ReloadText();
        }

        public void SetPosition(Vector2 position)
        {
            Image.Position = position;
            textShadowX.Position = new Vector2(Image.Position.X + 4, Image.Position.Y);
            textShadowY.Position = new Vector2(Image.Position.X, Image.Position.Y + 4);
            textShadowXY.Position = new Vector2(Image.Position.X + 4, Image.Position.Y + 4);
        }

        public void OffsetX(float value)
        {
            SetX(Image.Position.X + value);
        }

        public void OffsetY(float value)
        {
            SetY(Image.Position.Y + value);
        }

        public void SetX(float coord)
        {
            Image.Position.X = textShadowY.Position.X = coord; 
            textShadowX.Position.X = textShadowXY.Position.X = coord + 4;
        }

        public void SetY(float coord)
        {
            Image.Position.Y = textShadowX.Position.Y = coord;
            textShadowY.Position.Y = textShadowXY.Position.Y = coord + 4;
        }

        public void SetAlpha(float alpha)
        {
            Image.Alpha = textShadowX.Alpha = textShadowY.Alpha = textShadowXY.Alpha = alpha;
        }

        public void SetActive(bool active)
        {
            Image.IsActive = textShadowX.IsActive = textShadowY.IsActive = textShadowXY.IsActive = active;
        }

        public void ActivateEffect(string s)
        {
            Image.ActivateEffect(s);
            textShadowX.ActivateEffect(s);
            textShadowY.ActivateEffect(s);
            textShadowXY.ActivateEffect(s);
        }

        public void StoreEffects()
        {
            Image.StoreEffects();
            textShadowX.StoreEffects();
            textShadowY.StoreEffects();
            textShadowXY.StoreEffects();
        }

        public void RestoreEffects()
        {
            Image.RestoreEffects();
            textShadowX.RestoreEffects();
            textShadowY.RestoreEffects();
            textShadowXY.RestoreEffects();
        }

        public void SetFadeEffectIncrease(bool increase)
        {
            Image.FadeEffect.Increase = textShadowX.FadeEffect.Increase = textShadowY.FadeEffect.Increase = textShadowXY.FadeEffect.Increase = increase;
        }

    }
}
