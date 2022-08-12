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
                    if ((InputManager.Instance.KeyDown(Keys.D, Keys.W, Keys.S) && player.Direction == Entity.EntityDirection.Left)
                        || (InputManager.Instance.KeyDown(Keys.W, Keys.A, Keys.D) && player.Direction == Entity.EntityDirection.Down)
                        || (InputManager.Instance.KeyDown(Keys.S, Keys.D, Keys.A) && player.Direction == Entity.EntityDirection.Up)
                        || (InputManager.Instance.KeyDown(Keys.S, Keys.W, Keys.A) && player.Direction == Entity.EntityDirection.Right)
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
                    Tile currentTile = TileManager.GetCurrentTile(map, player.Sprite, player.Sprite.SourceRect.Width / 2, player.Sprite.SourceRect.Height);

                    if (InputManager.Instance.KeyPressed(Keys.E) && player.State == Entity.MoveState.Idle)
                    {
                        if (TileManager.UpTile(map, currentTile) != null && player.Direction == Entity.EntityDirection.Up)
                            LoadContent(TileManager.UpTile(map, currentTile).ID, ref player);

                        if (TileManager.DownTile(map, currentTile) != null && player.Direction == Entity.EntityDirection.Down)
                            LoadContent(TileManager.DownTile(map, currentTile).ID, ref player);

                        if (TileManager.LeftTile(map, currentTile) != null && player.Direction == Entity.EntityDirection.Left)
                            LoadContent(TileManager.LeftTile(map, currentTile).ID, ref player);

                        if (TileManager.RightTile(map, currentTile) != null && player.Direction == Entity.EntityDirection.Right)
                            LoadContent(TileManager.RightTile(map, currentTile).ID, ref player);
                    }
                    else if (currentTile != null && InputManager.Instance.KeyPressed(Keys.W) && TileManager.UpTile(map, currentTile) != null && player.Direction == Entity.EntityDirection.Up)
                        LoadContent(TileManager.UpTile(map, currentTile).ID, ref player);

                    else if (currentTile != null && InputManager.Instance.KeyPressed(Keys.S) && TileManager.DownTile(map, currentTile) != null && player.Direction == Entity.EntityDirection.Down)
                        LoadContent(TileManager.DownTile(map, currentTile).ID, ref player);

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
