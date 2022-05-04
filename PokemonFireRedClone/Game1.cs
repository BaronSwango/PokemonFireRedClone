using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace PokemonFireRedClone
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RenderTarget2D renderTarget;
        Rectangle sourceRect;
        //For the eventual multiple options of scaling the window partially without full screen
        //int scaled
        bool scaled;

        int defaultWidth, defaultHeight;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            defaultWidth = graphics.PreferredBackBufferWidth = (int)ScreenManager.Instance.Dimensions.X;
            defaultHeight = graphics.PreferredBackBufferHeight = (int)ScreenManager.Instance.Dimensions.Y;
            graphics.ApplyChanges();
            renderTarget = new RenderTarget2D(GraphicsDevice, defaultWidth, defaultHeight);
            sourceRect = new Rectangle(0, 0, defaultWidth, defaultHeight);
            
            scaled = false;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ScreenManager.Instance.GraphicsDevice = GraphicsDevice;
            ScreenManager.Instance.SpriteBatch = spriteBatch;
            ScreenManager.Instance.LoadContent(Content);

        }

        protected override void Update(GameTime gameTime)
        {
            if (InputManager.Instance.KeyPressed(Keys.F11) || InputManager.Instance.KeyPressed(Keys.OemTilde) || InputManager.Instance.KeyPressed(Keys.RightShift))
            {
                if (InputManager.Instance.KeyPressed(Keys.RightShift))
                {
                    scaled = !scaled;
                    sourceRect = scaled ? new Rectangle(0, 0, 1920, 1080) : new Rectangle(0, 0, defaultWidth, defaultHeight);
                }
                else
                {
                    graphics.IsFullScreen = !graphics.IsFullScreen;
                    sourceRect = graphics.IsFullScreen ? new Rectangle(0, 0,
                        GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
                        GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height) :
                        new Rectangle(0, 0, defaultWidth, defaultHeight);
                }
                graphics.PreferredBackBufferWidth = sourceRect.Width;
                graphics.PreferredBackBufferHeight = sourceRect.Height;
                graphics.ApplyChanges();
            }

            ScreenManager.Instance.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);

            if (ScreenManager.Instance.Image.Alpha == 1)
                GraphicsDevice.Clear(Color.Black);
            else
                GraphicsDevice.Clear(new Color(94, 99, 150));

            if (ScreenManager.Instance.CurrentScreen.Type.Name == "GameplayScreen")
                spriteBatch.Begin(transformMatrix: ((GameplayScreen)ScreenManager.Instance.CurrentScreen).Camera.Transform);
            else 
                spriteBatch.Begin();
            ScreenManager.Instance.Draw(spriteBatch);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            
            //render target to back buffer
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            spriteBatch.Draw(renderTarget, sourceRect, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
