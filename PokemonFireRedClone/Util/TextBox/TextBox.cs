
using System.Collections.Generic;

using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class TextBox
    {

        //TODO: Also add bouncing red triangle after text if applicable

        //public event EventHandler OnPageChange;

        public string Type;
        [XmlElement("Dialogue")]
        public List<TextBoxImage> Dialogue;
        public string ID;
        [XmlIgnore]
        public int Page;
        public int TotalPages;
        public Image Border;
        [XmlIgnore]
        public bool IsDisplayed;
        [XmlIgnore]
        public bool IsTransitioning;
        public bool Menu;

        public Image Arrow;
        public float ArrowOffset;
        protected bool increase;
        protected float arrowOriginalY;

        int positionOffset;
        public int DialogueOffsetX;
        public int DialogueOffsetY;

        protected Image transitionRect;
        protected Image transitionRect2;
        protected List<TextBoxImage> currentDialogue;
        protected bool updateDialogue;

        private void Transition(GameTime gameTime)
        {
            //TODO: Add text animation slide animation with two white rectangles getting smaller through the update function
            if (IsTransitioning)
            {
                if (updateDialogue)
                {
                    if (Page <= TotalPages)
                    {
                        if (!Menu)
                            Arrow.IsActive = false;
                        Page++;
                        currentDialogue.Clear();
                        foreach (TextBoxImage image in Dialogue)
                        {
                            if (image.Page == Page)
                                currentDialogue.Add(image);
                        }

                        currentDialogue[0].Position = new Vector2(Border.Position.X + DialogueOffsetX, Border.Position.Y + DialogueOffsetY);

                        transitionRect.Scale = new Vector2(currentDialogue[0].SourceRect.Width, currentDialogue[0].SourceRect.Height);
                        transitionRect.Position = new Vector2(currentDialogue[0].Position.X, currentDialogue[0].Position.Y);

                        if (currentDialogue.Count == 2)
                        {
                            currentDialogue[1].Position = new Vector2(Border.Position.X + DialogueOffsetX, Border.Position.Y + DialogueOffsetY + 60);

                            if (transitionRect2 == null)
                            {
                                transitionRect2 = new Image();
                                transitionRect2.Path = "TextBoxes/TextBoxEffectPixel";
                                transitionRect2.LoadContent();
                            }
                            transitionRect2.Scale = new Vector2(currentDialogue[1].SourceRect.Width, currentDialogue[1].SourceRect.Height);
                            transitionRect2.Position = new Vector2(currentDialogue[1].Position.X, currentDialogue[1].Position.Y);
                        }
                    }

                    updateDialogue = false;

                }



                float speed = (float)(1.35 * gameTime.ElapsedGameTime.TotalMilliseconds);

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

        protected void animateRedArrow(GameTime gameTime)
        {
            float speed = (float)(30 * gameTime.ElapsedGameTime.TotalSeconds);

            if (ArrowOffset >= 8)
                increase = false;
            else if (ArrowOffset <= 0)
                increase = true;

            if (increase)
                ArrowOffset += speed;
            else
                ArrowOffset -= speed;

            //if (ArrowOffset % 4 < 0.1)
                Arrow.Position = new Vector2(Arrow.Position.X, arrowOriginalY + ArrowOffset);
        }


        public TextBox()
        {
            Dialogue = new List<TextBoxImage>();
            currentDialogue = new List<TextBoxImage>();
            Page = 1;
            Menu = false;
        }

        public void LoadContent(ref Player player)
        {

            if (player.Image.SpriteSheetEffect.CurrentFrame.Y > 3)
                player.Image.SpriteSheetEffect.CurrentFrame.Y -= 4;
            player.Image.SpriteSheetEffect.CurrentFrame.X = 0;
            player.Image.IsActive = false;
            player.CanUpdate = false;

            IsDisplayed = true;
            IsTransitioning = true;

            Border = new Image();

            if (TotalPages > 1 && !Menu)
            {
                Arrow = new Image
                {
                    Path = "TextBoxes/RedArrow"
                };
                Arrow.LoadContent();
                ArrowOffset = 0;
            }

            if (Type.Contains("Tile"))
            {
                Border.Path = "TextBoxes/GrayTextBox";
                positionOffset = 216;
                DialogueOffsetX = 48;
                DialogueOffsetY = 24;
            }
            else if (Type == "NPC")
            {
                Border.Path = "TextBoxes/BlueTextBox";
                positionOffset = 220;
                DialogueOffsetX = 44;
                DialogueOffsetY = 20;
            }
            Border.LoadContent();
            Border.Position = new Vector2(player.Image.Position.X - (ScreenManager.Instance.Dimensions.X -
                Border.SourceRect.Width) - 64, player.Image.Position.Y + positionOffset);
            foreach (Image image in Dialogue)
            {
                if (Type == "TilePlayer")
                    image.Text = Player.PlayerJsonObject.Name + "`s   house";
                else if (Type == "TileRival")
                    image.Text = Player.PlayerJsonObject.RivalName + "`s   house";
                image.LoadContent();

            }

            foreach (TextBoxImage image in Dialogue)
            {
                if (image.Page == 1)
                    currentDialogue.Add(image);
            }



            currentDialogue[0].Position = new Vector2(Border.Position.X + DialogueOffsetX, Border.Position.Y + DialogueOffsetY);

            //TRANSITION
            if (currentDialogue.Count >= 1)
            {
                transitionRect = new Image
                {
                    Path = "TextBoxes/TextBoxEffectPixel"
                };
                transitionRect.LoadContent();
                transitionRect.Scale = new Vector2(currentDialogue[0].SourceRect.Width, currentDialogue[0].SourceRect.Height);
                transitionRect.Position = new Vector2(currentDialogue[0].Position.X, currentDialogue[0].Position.Y);
                if (TotalPages > 1 && !Menu)
                {
                    Arrow.Position = new Vector2(currentDialogue[0].Position.X + currentDialogue[0].SourceRect.Width, currentDialogue[0].Position.Y + 12);
                    arrowOriginalY = Arrow.Position.Y;
                }
            }
            if (currentDialogue.Count == 2)
            {
                currentDialogue[1].Position = new Vector2(Border.Position.X + DialogueOffsetX, Border.Position.Y + DialogueOffsetY + 60);
                transitionRect2 = new Image
                {
                    Path = "TextBoxes/TextBoxEffectPixel"
                };
                transitionRect2.LoadContent();
                transitionRect2.Scale = new Vector2(currentDialogue[1].SourceRect.Width, currentDialogue[1].SourceRect.Height);
                transitionRect2.Position = new Vector2(currentDialogue[1].Position.X, currentDialogue[1].Position.Y);
                if (TotalPages > 1 && !Menu)
                {
                    Arrow.Position = new Vector2(currentDialogue[1].Position.X + currentDialogue[1].SourceRect.Width, currentDialogue[1].Position.Y + 12);
                    arrowOriginalY = Arrow.Position.Y;
                }
            }

            //END TRANSITION
        }

        public void UnloadContent(ref Player player)
        {
            IsDisplayed = false;
            player.CanUpdate = true;
            Border.UnloadContent();
            foreach (Image image in Dialogue)
                image.UnloadContent();

            if (TotalPages > 1 && !Menu)
                Arrow.UnloadContent();
        }

        public virtual void Update(GameTime gameTime)
        {
            if (IsDisplayed)
            {
                Border.Update(gameTime);
                foreach (Image image in Dialogue)
                    image.Update(gameTime);


                Transition(gameTime);
                if (!Menu && TotalPages > 1 && Arrow.IsActive)
                    animateRedArrow(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (IsDisplayed)
            {
                Border.Draw(spriteBatch);
                foreach (TextBoxImage image in currentDialogue)
                {
                    image.Draw(spriteBatch);
                }

                if (IsTransitioning)
                {
                    transitionRect.Draw(spriteBatch);
                    if (currentDialogue.Count == 2)
                        transitionRect2.Draw(spriteBatch);
                }
                else if (!Menu && TotalPages > 1 && Page < TotalPages)
                {
                    Arrow.Draw(spriteBatch);
                    if (!Arrow.IsActive)
                        Arrow.IsActive = true;
                }

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
 * - List of Image (Text in the box, only have red Arrow if list of images > 1 && index < images.Count)
 * - 
 * - 
 * - 
 * 
 * TextBox comes up when running into or interacting with sign
 * TextBox must be unique to each sign tile and NPC
 * 
 * Border color must match NPC or Tile
 * TextBox drawn after player and map (same area as Menu)
 * Player can't move when TextBox displayed
 * Press E to close Box, can only be done after animation
 * 
 * 
 * 
 */