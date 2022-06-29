using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class TextBoxManager
    {

        // fix when opening pokemon menu in front of sign

        private TextBox textBox;

        [XmlElement("TextBoxes")]
        public List<TextBox> TextBoxes;
        public bool Closed;

        [XmlIgnore]
        public bool IsDisplayed
        {
            get
            {
                if (textBox != null)
                    return textBox.IsDisplayed;
                return false;
            }
            set {
                if (textBox != null)
                    textBox.IsDisplayed = value;
            }
        }

        public void LoadXML()
        {
            XmlManager<TextBoxManager> textBoxLoader = new();
            TextBoxes = textBoxLoader.Load("Load/Gameplay/TextBoxManager.xml").TextBoxes;
        }

        public void LoadContent(string ID, ref Player player)
        {
            LoadXML();

            foreach (TextBox textBox in TextBoxes)
            {
                if (textBox.ID == ID)
                {
                    this.textBox = textBox;
                    this.textBox.LoadContent(ref player);
                    break;
                }
            }
        }

        public void UnloadContent(ref Player player)
        {
            textBox.UnloadContent(ref player);
            textBox = null;
        }

        public void Update(GameTime gameTime, ref Map map, ref Player player)
        {

            if (textBox != null)
            {
                textBox.Update(gameTime);
                if (textBox.IsDisplayed && !textBox.IsTransitioning)
                {
                    if ((InputManager.Instance.KeyDown(Keys.D, Keys.W, Keys.S) && player.Direction == Player.PlayerDirection.Left)
                        || (InputManager.Instance.KeyDown(Keys.W, Keys.A, Keys.D) && player.Direction == Player.PlayerDirection.Down)
                        || (InputManager.Instance.KeyDown(Keys.S, Keys.D, Keys.A) && player.Direction == Player.PlayerDirection.Up)
                        || (InputManager.Instance.KeyDown(Keys.S, Keys.W, Keys.A) && player.Direction == Player.PlayerDirection.Right)
                        || InputManager.Instance.KeyPressed(Keys.E))
                    {
                        if (InputManager.Instance.KeyPressed(Keys.E) && textBox.Page != textBox.TotalPages)
                        {
                            textBox.IsTransitioning = true;
                            return;
                        }

                        UnloadContent(ref player);
                        Closed = true;
                    }

                }
            } else
            {
                if (player.CanUpdate)
                {
                    Tile currentTile = TileManager.Instance.GetCurrentTile(map, player.Image, player.Image.SourceRect.Width / 2, player.Image.SourceRect.Height);

                    if (InputManager.Instance.KeyPressed(Keys.E) && player.State == Player.PlayerState.Idle)
                    {
                        if (TileManager.Instance.UpTile(map, currentTile) != null && player.Direction == Player.PlayerDirection.Up)
                            LoadContent(TileManager.Instance.UpTile(map, currentTile).ID, ref player);

                        if (TileManager.Instance.DownTile(map, currentTile) != null && player.Direction == Player.PlayerDirection.Down)
                            LoadContent(TileManager.Instance.DownTile(map, currentTile).ID, ref player);

                        if (TileManager.Instance.LeftTile(map, currentTile) != null && player.Direction == Player.PlayerDirection.Left)
                            LoadContent(TileManager.Instance.LeftTile(map, currentTile).ID, ref player);

                        if (TileManager.Instance.RightTile(map, currentTile) != null && player.Direction == Player.PlayerDirection.Right)
                            LoadContent(TileManager.Instance.RightTile(map, currentTile).ID, ref player);
                    }
                    else if (currentTile != null && InputManager.Instance.KeyPressed(Keys.W) && TileManager.Instance.UpTile(map, currentTile) != null && player.Direction == Player.PlayerDirection.Up)
                        LoadContent(TileManager.Instance.UpTile(map, currentTile).ID, ref player);

                    else if (currentTile != null && InputManager.Instance.KeyPressed(Keys.S) && TileManager.Instance.DownTile(map, currentTile) != null && player.Direction == Player.PlayerDirection.Down)
                        LoadContent(TileManager.Instance.DownTile(map, currentTile).ID, ref player);

                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (textBox != null)
                textBox.Draw(spriteBatch);
        }

    }
}
