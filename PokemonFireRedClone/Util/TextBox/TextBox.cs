﻿using System;
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
        public int TotalPages;
        Image border;
        [XmlIgnore]
        public bool IsDisplayed;
        public bool IsTransitioning;

        int positionOffset;
        int dialogueOffsetX;
        int dialogueOffsetY;

        Image transitionRect;
        Image transitionRect2;
        List<Image> currentDialogue;
        bool updateDialogue;

        private void Transition(GameTime gameTime)
        {
            //TODO: Add text animation slide animation with two white rectangles getting smaller through the update function
            if (IsTransitioning)
            {
                if (updateDialogue)
                {
                    if (Page <= TotalPages)
                    {
                        Page++;
                        currentDialogue.Clear();
                        foreach (Image image in Dialogue)
                        {
                            if (image.Page == Page)
                                currentDialogue.Add(image);
                        }

                        currentDialogue[0].Position = new Vector2(border.Position.X + dialogueOffsetX, border.Position.Y + dialogueOffsetY);

                        transitionRect.Scale = new Vector2(currentDialogue[0].SourceRect.Width, currentDialogue[0].SourceRect.Height);
                        transitionRect.Position = new Vector2(currentDialogue[0].Position.X, currentDialogue[0].Position.Y);

                        if (currentDialogue.Count == 2)
                        {
                            currentDialogue[1].Position = new Vector2(border.Position.X + dialogueOffsetX, border.Position.Y + dialogueOffsetY + 60);

                            transitionRect2.Scale = new Vector2(currentDialogue[1].SourceRect.Width, currentDialogue[1].SourceRect.Height);
                            transitionRect2.Position = new Vector2(currentDialogue[1].Position.X, currentDialogue[1].Position.Y);
                        }
                    }

                    updateDialogue = false;

                }



                float speed = (float)(1.5 * gameTime.ElapsedGameTime.TotalMilliseconds);

                if (transitionRect.Scale.X > 1)
                {
                    transitionRect.Scale = new Vector2(transitionRect.Scale.X - speed, transitionRect.Scale.Y);
                    transitionRect.Position = new Vector2(transitionRect.Position.X + speed, transitionRect.Position.Y);
                }
                else
                {
                    if (currentDialogue.Count == 2 && transitionRect2.Scale.X > 1)
                    {
                        transitionRect2.Scale = new Vector2(transitionRect2.Scale.X - speed, transitionRect2.Scale.Y);
                        transitionRect2.Position = new Vector2(transitionRect2.Position.X + speed, transitionRect2.Position.Y);
                        return;
                    }

                    IsTransitioning = false;
                    updateDialogue = true;
                }
            }
                

        }


        public TextBox()
        {
            Dialogue = new List<Image>();
            currentDialogue = new List<Image>();
            Page = 1;
        }

        public void LoadContent(ref Player player)
        {

            if (player.Image.SpriteSheetEffect.CurrentFrame.Y > 3)
                player.Image.SpriteSheetEffect.CurrentFrame.Y -= 4;
            player.Image.SpriteSheetEffect.CurrentFrame.X = 0;
            player.CanUpdate = false;

            IsDisplayed = true;
            IsTransitioning = true;

            border = new Image();

            if (Type == "Tile")
            {
                border.Path = "TextBoxes/GrayTextBox";
                positionOffset = 216;
                dialogueOffsetX = 48;
                dialogueOffsetY = 24;
            }
            else if (Type == "NPC")
            {
                border.Path = "TextBoxes/BlueTextBox";
                positionOffset = 220;
                dialogueOffsetX = 44;
                dialogueOffsetY = 20;
            }
            border.LoadContent();
            border.Position = new Vector2(player.Image.Position.X - (ScreenManager.Instance.Dimensions.X -
                border.SourceRect.Width) - 64, player.Image.Position.Y + positionOffset);
            foreach (Image image in Dialogue)
            {
                image.LoadContent();

            }

            foreach (Image image in Dialogue)
            {
                if (image.Page == 1)
                    currentDialogue.Add(image);
            }

            currentDialogue[0].Position = new Vector2(border.Position.X + dialogueOffsetX, border.Position.Y + dialogueOffsetY);

            //TRANSITION
            transitionRect = new Image();
            transitionRect.Path = "TextBoxes/TextBoxEffectPixel";
            transitionRect.LoadContent();
            transitionRect.Scale = new Vector2(currentDialogue[0].SourceRect.Width, currentDialogue[0].SourceRect.Height);
            transitionRect.Position = new Vector2(currentDialogue[0].Position.X, currentDialogue[0].Position.Y);


            if (currentDialogue.Count == 2)
            {
                currentDialogue[1].Position = new Vector2(border.Position.X + dialogueOffsetX, border.Position.Y + dialogueOffsetY + 60);
                transitionRect2 = new Image();
                transitionRect2.Path = "TextBoxes/TextBoxEffectPixel";
                transitionRect2.LoadContent();
                transitionRect2.Scale = new Vector2(currentDialogue[1].SourceRect.Width, currentDialogue[1].SourceRect.Height);
                transitionRect2.Position = new Vector2(currentDialogue[1].Position.X, currentDialogue[1].Position.Y);
            }

            //END TRANSITION
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


                Transition(gameTime);

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsDisplayed)
            {
                border.Draw(spriteBatch);
                foreach (Image image in currentDialogue)
                {
                    if (image.Page == Page)
                        image.Draw(spriteBatch);
                }

                if (IsTransitioning)
                {
                    transitionRect.Draw(spriteBatch);
                    if (currentDialogue.Count == 2)
                        transitionRect2.Draw(spriteBatch);
                }
                //foreach (Image image in Dialogue)
                    //image.Draw(spriteBatch);
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