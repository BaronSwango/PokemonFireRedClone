using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class ScreenManager
    {


        private static ScreenManager instance;
        [XmlIgnore]
        public Vector2 Dimensions { set; get; }
        [XmlIgnore]
        public ContentManager Content { private set; get; }
        XmlManager<GameScreen> xmlGameScreenManager;

        [XmlIgnore]
        public GameScreen CurrentScreen;
        GameScreen newScreen;
        [XmlIgnore]
        public GraphicsDevice GraphicsDevice;
        [XmlIgnore]
        public SpriteBatch SpriteBatch;

        public Image Image;
        [XmlIgnore]
        public bool IsTransitioning { get; private set; }

        public static ScreenManager Instance
        {
            get
            {
                if (instance == null)
                {
                    XmlManager<ScreenManager> xml = new XmlManager<ScreenManager>();
                    instance = xml.Load("Load/ScreenManager.xml");
                }

                return instance;
            }
        }

        public void ChangeScreens(string screenName)
        {
            newScreen = (GameScreen) Activator.CreateInstance(Type.GetType("PokemonFireRedClone."+  screenName));
            Image.IsActive = true;
            Image.FadeEffect.Increase = true;
            Image.Alpha = 0.0f;
            IsTransitioning = true;
        }

        void Transition(GameTime gameTime)
        {
            if (IsTransitioning)
            {
                Image.Update(gameTime);
                if (Image.Alpha == 1.0f)
                {
                    CurrentScreen.UnloadContent();
                    CurrentScreen = newScreen;
                    xmlGameScreenManager.Type = CurrentScreen.Type;
                    if (File.Exists(CurrentScreen.XmlPath))
                        CurrentScreen = xmlGameScreenManager.Load(CurrentScreen.XmlPath);
                    CurrentScreen.LoadContent();
                }
                else if (Image.Alpha == 0.0f)
                {
                    Image.IsActive = false;
                    IsTransitioning = false;
                }

            }
        }

        public ScreenManager()
        {
            Dimensions = new Vector2(1280, 720);
            CurrentScreen = new GameplayScreen();
            xmlGameScreenManager = new XmlManager<GameScreen>();
            xmlGameScreenManager.Type = CurrentScreen.Type;
            //CurrentScreen = xmlGameScreenManager.Load("Load/SplashScreen.xml");
        }

        public void LoadContent(ContentManager Content)
        {
            this.Content = new ContentManager(Content.ServiceProvider, "Content");
            CurrentScreen.LoadContent();
            Image.LoadContent();
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
                Image.Draw(spriteBatch);
        }


    }
}
