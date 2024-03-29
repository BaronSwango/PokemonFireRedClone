﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class Image
    {

        private Vector2 origin;
        private ContentManager content;
        private SpriteFont font;

        [XmlIgnore]
        public Texture2D Texture;
        [XmlIgnore]
        public RenderTarget2D RenderTarget;
        [XmlIgnore]
        public Dictionary<string, ImageEffect> EffectList;
        public float Alpha;
        public string Text, FontName, Path;
        public Vector2 Position, Scale;
        public Rectangle SourceRect;
        public bool IsActive, IsLoaded, IsReloaded;
        public string Effects;
        public Color Tint;
        public float Angle;
        public Color FontColor;
        public int R, G, B, A;
        public bool UseFontColor;
        public bool Flip;

        public FadeEffect FadeEffect;
        public SpriteSheetEffect SpriteSheetEffect;
        public GrayOutEffect GrayOutEffect;


        void SetEffect<T>(ref T effect) where T:ImageEffect
        {
            if (effect == null)
            {
                effect = (T)Activator.CreateInstance(typeof(T));
            }
            else
            {
                effect.IsActive = true;
                var obj = this;
                effect.LoadContent(ref obj);
            }
            EffectList.Add(effect.GetType().ToString().Replace("PokemonFireRedClone.", ""), effect);
        }

        public void ActivateEffect(string effect)
        {
            if (EffectList.ContainsKey(effect))
            {
                EffectList[effect].IsActive = true;
                var obj = this;
                EffectList[effect].LoadContent(ref obj);
            }
        }

        public void DeactivateEffect(string effect)
        {
            if (EffectList.ContainsKey(effect))
            {
                EffectList[effect].IsActive = false;
                EffectList[effect].UnloadContent();
            }
        }

        public void StoreEffects()
        {
            Effects = string.Empty;
            foreach(var effect in EffectList)
            {
                if (effect.Value.IsActive)
                {
                    Effects += effect.Key + ":";
                }
            }
            if (Effects != string.Empty)
            {
                _ = Effects.TrimEnd(':');
            }
        }

        public void RestoreEffects()
        {
            foreach (var effect in EffectList)
            {
                DeactivateEffect(effect.Key);
            }

            string[] split = Effects.Split(':');

            foreach (string s in split)
            {
                ActivateEffect(s);
            }
        }

        public Image()
        {
            Path = Effects = Text = string.Empty;
            FontName = "Fonts/Arial";
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Alpha = 1.0f;
            SourceRect = Rectangle.Empty;
            EffectList = new Dictionary<string, ImageEffect>();
            IsLoaded = false;
            Tint = Color.White;
            Angle = 0;
            FontColor = Color.Black;
            A = 255;
        }

        public void ReloadText()
        {
            content.Unload();
            font = content.Load<SpriteFont>(FontName);
            if (!UseFontColor)
            {
                FontColor = new Color(R, G, B, A);
            }

            Vector2 dimensions = Vector2.Zero;
            dimensions.Y = font.MeasureString(Text).Y;
            dimensions.X += font.MeasureString(Text).X;

            SourceRect = new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y);

            RenderTarget = new RenderTarget2D(ScreenManager.Instance.GraphicsDevice,
                (int)dimensions.X, (int)dimensions.Y);
            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(RenderTarget);
            ScreenManager.Instance.GraphicsDevice.Clear(Color.Transparent);
            ScreenManager.Instance.SpriteBatch.Begin();
            ScreenManager.Instance.SpriteBatch.DrawString(font, Text, Vector2.Zero,
                FontColor);
            ScreenManager.Instance.SpriteBatch.End();

            Texture = RenderTarget;

            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(null);
            IsReloaded = true;
        }

        public void ReloadTexture()
        {
            content.Unload();
            if (Path != string.Empty)
            {
                Texture = content.Load<Texture2D>(Path);
            }
            
            Vector2 dimensions = Vector2.Zero;

            if (Texture != null)
            {
                dimensions.X += Texture.Width;
                dimensions.Y = Texture.Height;
            }

            SourceRect = new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y);

            RenderTarget = new RenderTarget2D(ScreenManager.Instance.GraphicsDevice,
                (int)dimensions.X, (int)dimensions.Y);
            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(RenderTarget);
            ScreenManager.Instance.GraphicsDevice.Clear(Color.Transparent);
            ScreenManager.Instance.SpriteBatch.Begin();
            if (Texture != null)
            {
                ScreenManager.Instance.SpriteBatch.Draw(Texture, Vector2.Zero, Tint);
            }
            ScreenManager.Instance.SpriteBatch.End();

            Texture = RenderTarget;

            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(null);
            IsReloaded = true;

            if (Effects != string.Empty)
            {
                string[] split = Effects.Split(':');
                foreach (string item in split)
                {
                    EffectList[item].IsActive = true;
                }
            }
        }

        public void ReloadTexture(string newPath)
        {
            content.Unload();
            if (newPath != string.Empty)
            {
                Texture = content.Load<Texture2D>(newPath);
            }

            Vector2 dimensions = Vector2.Zero;

            if (Texture != null)
            {
                dimensions.X += Texture.Width;
                dimensions.Y += Texture.Height;
            }

            SourceRect = new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y);

            RenderTarget = new RenderTarget2D(ScreenManager.Instance.GraphicsDevice,
                (int)dimensions.X, (int)dimensions.Y);
            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(RenderTarget);
            ScreenManager.Instance.GraphicsDevice.Clear(Color.Transparent);
            ScreenManager.Instance.SpriteBatch.Begin();
            if (Texture != null)
            {
                ScreenManager.Instance.SpriteBatch.Draw(Texture, Vector2.Zero, Tint);
            }
            ScreenManager.Instance.SpriteBatch.End();

            Texture = RenderTarget;

            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(null);
            IsReloaded = true;
        }

        public void LoadContent()
        {
            content = new ContentManager(
                ScreenManager.Instance.Content.ServiceProvider, "Content");

            if (Path != string.Empty)
            {
                Texture = content.Load<Texture2D>(Path);
            }

            font = content.Load<SpriteFont>(FontName);
            if (!UseFontColor)
            {
                FontColor = new Color(R, G, B, A);
            }

            Vector2 dimensions = Vector2.Zero;

            if (Texture != null)
            {
                dimensions.X += Texture.Width;
                dimensions.Y = Math.Max(Texture.Height, font.MeasureString(Text).Y);
            }
            else
            {
                dimensions.Y = font.MeasureString(Text).Y;
            }
            dimensions.X += font.MeasureString(Text).X;

            if (SourceRect == Rectangle.Empty)
            {
                SourceRect = new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y);
            }

            RenderTarget = new RenderTarget2D(ScreenManager.Instance.GraphicsDevice, 
                (int) dimensions.X, (int) dimensions.Y);
            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(RenderTarget);
            ScreenManager.Instance.GraphicsDevice.Clear(Color.Transparent);
            ScreenManager.Instance.SpriteBatch.Begin();
            if (Texture != null)
            {
                ScreenManager.Instance.SpriteBatch.Draw(Texture, Vector2.Zero, Tint);
            }
            ScreenManager.Instance.SpriteBatch.DrawString(font, Text, Vector2.Zero,
                FontColor);
            ScreenManager.Instance.SpriteBatch.End();
 
            Texture = RenderTarget;
             
            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(null);

            SetEffect(ref FadeEffect);
            SetEffect(ref SpriteSheetEffect);
            SetEffect(ref GrayOutEffect);

            if (Effects != string.Empty)
            {
                string[] split = Effects.Split(':');
                foreach (string item in split)
                {
                    ActivateEffect(item);
                }
            }
            IsLoaded = true;
        }

        public void UnloadContent()
        {
            content.Unload();
            foreach (var effect in EffectList)
            {
                DeactivateEffect(effect.Key);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var effect in EffectList)
            {
                if (effect.Value.IsActive)
                {
                    effect.Value.Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            origin = new Vector2(SourceRect.Width / 2,
                SourceRect.Height / 2);
            SpriteEffects s = Flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(Texture, Position + origin, SourceRect, Tint * Alpha,
                Angle, origin, Scale, s, 0.0f);

            foreach (var effect in EffectList)
            {
                if (effect.Value.IsActive)
                {
                    effect.Value.Draw(spriteBatch);
                }
            }
        }

    }
}
