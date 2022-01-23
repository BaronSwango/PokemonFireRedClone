using System;
using System.Collections.Generic;

using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class TextBox
    {

        //TODO: Also add bouncing red triangle after text if applicable

        //public event EventHandler OnPageChange;

        public string Type;
        [XmlElement("Dialogue")]
        public List<Image> Dialogue;
        public string ID;
        [XmlIgnore]
        public int Page;
        Image border;
        [XmlIgnore]
        public bool IsDisplayed;

        private void Transition()
        {
            //TODO: Add text animation slide animation with two white rectangles getting smaller through the update function
        }


        public TextBox()
        {
            Dialogue = new List<Image>();
            Page = 0;
        }

        public void LoadContent(ref Player player)
        {

            if (player.Image.SpriteSheetEffect.CurrentFrame.Y > 3)
                player.Image.SpriteSheetEffect.CurrentFrame.Y -= 4;
            player.Image.SpriteSheetEffect.CurrentFrame.X = 0;
            player.CanUpdate = false;

            IsDisplayed = true;

            border = new Image();

            if (Type == "Tile")
                border.Path = "TextBoxes/GrayTextBox";
            border.LoadContent();
            border.Position = new Vector2(player.Image.Position.X - (ScreenManager.Instance.Dimensions.X -
                border.SourceRect.Width) - 64, player.Image.Position.Y + 216);
            foreach (Image image in Dialogue)
            {
                image.LoadContent();
                image.Position = new Vector2(border.Position.X + 48, border.Position.Y + 24);
                //image.ActivateEffect("TextBoxEffect");
            }

        }

        public void UnloadContent(ref Player player)
        {
            IsDisplayed = false;
            player.CanUpdate = true;
            border.UnloadContent();
            foreach (Image image in Dialogue)
                image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            if (IsDisplayed)
            {
                border.Update(gameTime);
                foreach (Image image in Dialogue)
                    image.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsDisplayed)
            {
                border.Draw(spriteBatch);
                foreach (Image image in Dialogue)
                    image.Draw(spriteBatch);
            }
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
 * - Use effect on the List<Image> in TextBox class
 * 
 * 
 * 
 * Contents of TextBox class:
 * - Type (string: Blue or gray currently)
 * - List of Image (Text in the box, only have red arrow if list of images > 1 && index < images.Count)
 * - 
 * - 
 * - 
 * 
 * TextBox comes up when running into or interacting with sign
 * TextBox must be unique to each sign tile and NPC
 * 
 * border color must match NPC or Tile
 * TextBox drawn after player and map (same area as menu)
 * Player can't move when TextBox displayed
 * Press E to close Box, can only be done after animation
 * 
 * 
 * 
 */