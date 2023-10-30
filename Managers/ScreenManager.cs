using System;
using System.Xml.Serialization;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class ScreenManager
    {

        private readonly XmlManager<GameScreen> xmlGameScreenManager;
        private GameScreen newScreen;
        private readonly Counter counter;
        private static ScreenManager instance;

        public Vector2 Dimensions;
        public ContentManager Content { get; private set; }

        public GameScreen CurrentScreen;
        public GameScreen PreviousScreen;
        public GraphicsDevice GraphicsDevice;
        public SpriteBatch SpriteBatch;

        public Image Image;
        [XmlIgnore]
        public bool IsTransitioning { get; private set; }

        public BattleScreen BattleScreen {
            get { return (BattleScreen) CurrentScreen; }
            private set { }
        }

        public static ScreenManager Instance
        {
            get
            {
                instance ??= new ScreenManager();

                return instance;
            }
        }

        public ScreenManager()
        {
            Dimensions = new Vector2(1280, 720);
            CurrentScreen = new SplashScreen();
            xmlGameScreenManager = new XmlManager<GameScreen>
            {
                Type = CurrentScreen.Type
            };
            CurrentScreen = xmlGameScreenManager.Load("Load/SplashScreen.xml");
            counter = new Counter(500);
        }

        public void LoadContent(ContentManager content)
        {
            Content = new ContentManager(content.ServiceProvider, "Content");
            CurrentScreen.LoadContent();
            LoadWhiteScreen();
        }

        public void UnloadContent()
        {
            CurrentScreen.UnloadContent();
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            CurrentScreen.Update(gameTime);
            Transition(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentScreen.Draw(spriteBatch);
            if (IsTransitioning)
            {
                Image.Draw(spriteBatch);
            }
        }

        private void LoadFadeImage()
        {
            Color[] data = new Color[Image.Texture.Width * Image.Texture.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Black;
            Image.Texture.SetData(data);
            Image.ReloadTexture();
        }

        private void LoadWhiteScreen()
        {
            Image = new Image
            {
                Texture = new Texture2D(GraphicsDevice, (int)Dimensions.X + 8, (int)Dimensions.Y + 8)
            };
            Color[] data = new Color[Image.Texture.Width * Image.Texture.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.White;
            Image.Texture.SetData(data);
            Image.Effects = "FadeEffect";
            Image.LoadContent();
        }

        private void Transition(GameTime gameTime)
        {
            if (IsTransitioning)
            {
                Image.Update(gameTime);
                if (Image.Alpha == 1.0f)
                {
                    //if (counter < 500)
                    if (!counter.Finished)
                    {
                        //counter += gameTime.ElapsedGameTime.TotalMilliseconds;
                        counter.Update(gameTime);
                        Image.IsActive = false;
                        return;
                    }

                    if (!Image.IsActive)
                    {
                        Image.IsActive = true;
                    }

                    CurrentScreen.UnloadContent();
                    PreviousScreen = CurrentScreen;
                    CurrentScreen = newScreen;
                    xmlGameScreenManager.Type = CurrentScreen.Type;

                    if (File.Exists(CurrentScreen.XmlPath))
                    {
                        CurrentScreen = xmlGameScreenManager.Load(CurrentScreen.XmlPath);
                    }

                    if (CurrentScreen is BattleScreen battleScreen)
                    {
                        battleScreen.Trainer = ((BattleScreen)newScreen).Trainer;
                    }

                    CurrentScreen.LoadContent();

                    if (CurrentScreen is GameplayScreen screen)
                    {
                        Vector2 playerPos = screen.Player.Sprite.Position;
                        Image.Position = new Vector2(playerPos.X - (Dimensions.X / 2) + 32,
                        playerPos.Y - (Dimensions.Y / 2) + 40);
                    }
                    else
                    {
                        Image.Position = Vector2.Zero;
                    }

                    counter.Reset();
                }
                else if (Image.Alpha == 0.0f)
                {
                    Image.IsActive = false;
                    IsTransitioning = false;
                }
                else
                {
                    if (CurrentScreen is GameplayScreen screen)
                    {
                        Vector2 playerPos = screen.Player.Sprite.Position;
                        Image.Position = new Vector2(playerPos.X - (Dimensions.X / 2) + 32,
                        playerPos.Y - (Dimensions.Y / 2) + 40);
                    }
                }
            }
        }

        public void ChangeScreens(string screenName, params Trainer[] members)
        {
            newScreen = (GameScreen)Activator.CreateInstance(System.Type.GetType("PokemonFireRedClone." + screenName));

            if (newScreen is BattleScreen screen && members.Length > 0)
            {
                screen.Trainer = members[0];
            }

            if (newScreen is not TitleScreen && !Image.IsReloaded)
            {
                LoadFadeImage();
            }

            Image.IsActive = true;
            Image.FadeEffect.Increase = true;
            Image.Alpha = 0.0f;
            IsTransitioning = true;
            Image.FadeEffect.FadeSpeed = CurrentScreen is PokemonScreen || newScreen is PokemonScreen ||
                CurrentScreen is PokedexScreen || newScreen is PokedexScreen ||
                CurrentScreen is BagScreen || newScreen is BagScreen ? 3.5f : 1.5f;
        }
    }
}
