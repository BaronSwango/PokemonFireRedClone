using System.Collections.Generic;

using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class TextBox
    {

        public string Type;
        Image border;
        [XmlElement("Contents")]
        public List<Image> Contents;
        public Tile Tile;

        public TextBox()
        {
            Contents = new List<Image>();
            if (Type == "NPC")
                border.Path = "TextBoxes/BlueTextBox";
            else if (Type == "Tile")
                border.Path = "TextBoxes/GrayTextBox";
            // Add more text boxes if needed
        }

        public void LoadContent()
        {
            border.LoadContent();
            foreach (Image image in Contents)
                image.LoadContent();
        }

        public void UnloadContent()
        {
            border.UnloadContent();
            foreach (Image image in Contents)
                image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            border.Update(gameTime);
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            border.Draw(spriteBatch);
        }

    }
}

/*
 * 
 * Dictionary with text box and tile/entity?
 * - Check if player is interacting with a particular tile, 
 *   pull up specific textbox associated with that tile from 
 *   Dictionary
 * 
 * 
 * 
 * Create TextBoxEffect (Animation)
 * - Add effect to Image class
 * - Use effect on the List<Image> in TextBox class
 * 
 * 
 * 
 * Contents of TextBox class:
 * - Type (string: Blue or gray currently)
 * - List of Image (Text in the box, only have red arrow if list of images > 1)
 * - 
 * - 
 * - 
 * 
 */