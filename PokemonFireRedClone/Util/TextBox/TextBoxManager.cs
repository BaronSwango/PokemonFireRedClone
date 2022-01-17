using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class TextBoxManager
    {

        [XmlElement("TextBoxes")]
        public List<TextBox> TextBoxes;
        [XmlIgnore]
        public TextBox ActiveTextBox;

        public void LoadXML()
        {
            XmlManager<TextBoxManager> textBoxLoader = new XmlManager<TextBoxManager>();
            TextBoxes = textBoxLoader.Load("Load/Gameplay/TextBoxManager.xml").TextBoxes;
        }

        public void LoadContent(string ID, ref Player player)
        {
            LoadXML();

            foreach (TextBox textBox in TextBoxes)
            {
                if (textBox.ID == ID)
                {
                    ActiveTextBox = textBox;
                    ActiveTextBox.LoadContent(ref player);
                }
            }
        }

        public void Update(GameTime gameTime, ref Map map, ref Player player)
        {

            if (ActiveTextBox != null)
            {
                ActiveTextBox.Update(gameTime);
                if (ActiveTextBox.IsDisplayed)
                {
                    if ((InputManager.Instance.KeyPressed(Keys.D, Keys.W, Keys.S) && player.direction == Player.Direction.Left)
                        || (InputManager.Instance.KeyPressed(Keys.W, Keys.A, Keys.D) && player.direction == Player.Direction.Down)
                        || (InputManager.Instance.KeyPressed(Keys.S, Keys.D, Keys.A) && player.direction == Player.Direction.Up)
                        || (InputManager.Instance.KeyPressed(Keys.S, Keys.W, Keys.A) && player.direction == Player.Direction.Right)
                        || InputManager.Instance.KeyPressed(Keys.E))
                    {
                        ActiveTextBox.UnloadContent(ref player);
                        ActiveTextBox = null;
                    }

                }
            } else
            {
                if (InputManager.Instance.KeyPressed(Keys.E) && player.CanUpdate && player.state == Player.State.Idle)
                {

                    Tile currentTile = TileManager.Instance.GetCurrentTile(map, player.Image, player.Image.SourceRect.Width / 2, player.Image.SourceRect.Height);

                    if (TileManager.Instance.UpTile(map, currentTile) != null && player.direction == Player.Direction.Up)
                        LoadContent(TileManager.Instance.UpTile(map, currentTile).ID, ref player);

                    if (TileManager.Instance.DownTile(map, currentTile) != null && player.direction == Player.Direction.Down)
                        LoadContent(TileManager.Instance.DownTile(map, currentTile).ID, ref player);

                    if (TileManager.Instance.LeftTile(map, currentTile) != null && player.direction == Player.Direction.Left)
                        LoadContent(TileManager.Instance.LeftTile(map, currentTile).ID, ref player);

                    if (TileManager.Instance.RightTile(map, currentTile) != null && player.direction == Player.Direction.Right)
                        LoadContent(TileManager.Instance.RightTile(map, currentTile).ID, ref player);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (ActiveTextBox != null)
                ActiveTextBox.Draw(spriteBatch);
        }

    }
}
