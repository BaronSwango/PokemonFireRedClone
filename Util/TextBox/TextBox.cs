
using System.Collections.Generic;

using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class TextBox
    {
        //public event EventHandler OnPageChange;

        private int positionOffset;
        private bool increase;

        protected Image TransitionRect;
        protected Image TransitionRect2;
        protected List<TextBoxText> CurrentDialogue;
        protected float ArrowOriginalY;

        public string Type;
        [XmlElement("Dialogue")]
        public List<TextBoxText> Dialogue;
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

        public int DialogueOffsetX;
        public int DialogueOffsetY;

        public bool UpdateDialogue;

        public TextBox()
        {
            Dialogue = new List<TextBoxText>();
            CurrentDialogue = new List<TextBoxText>();
            Page = 1;
            Menu = false;
        }

        private void Transition(GameTime gameTime)
        {
            if (IsTransitioning)
            {
                if (UpdateDialogue)
                {
                    if (Page <= TotalPages)
                    {
                        if (!Menu)
                            Arrow.IsActive = false;
                        Page++;
                        CurrentDialogue.Clear();
                        foreach (TextBoxText image in Dialogue)
                        {
                            if (image.Page == Page)
                                CurrentDialogue.Add(image);
                        }

                        CurrentDialogue[0].SetPosition(new Vector2(Border.Position.X + DialogueOffsetX, Border.Position.Y + DialogueOffsetY));

                        TransitionRect.Scale = new Vector2(CurrentDialogue[0].SourceRect.Width+4, CurrentDialogue[0].SourceRect.Height+4);
                        TransitionRect.Position = new Vector2(CurrentDialogue[0].Position.X, CurrentDialogue[0].Position.Y);

                        if (CurrentDialogue.Count == 2)
                        {
                            CurrentDialogue[1].SetPosition(new Vector2(Border.Position.X + DialogueOffsetX, Border.Position.Y + DialogueOffsetY + 60));

                            if (TransitionRect2 == null)
                            {
                                TransitionRect2 = new Image
                                {
                                    Path = "TextBoxes/TextBoxEffectPixel"
                                };
                                TransitionRect2.LoadContent();
                            }
                            TransitionRect2.Scale = new Vector2(CurrentDialogue[1].SourceRect.Width+4, CurrentDialogue[1].SourceRect.Height+4);
                            TransitionRect2.Position = new Vector2(CurrentDialogue[1].Position.X, CurrentDialogue[1].Position.Y);
                        }
                    }
                    if (TotalPages > 1 && !Menu)
                        SetArrowPosition();

                    UpdateDialogue = false;

                }



                float speed = (float)(1.35 * gameTime.ElapsedGameTime.TotalMilliseconds);

                if (TransitionRect.Scale.X > 1)
                {
                    TransitionRect.Scale = new Vector2(TransitionRect.Scale.X - speed, TransitionRect.Scale.Y);
                    TransitionRect.Position = new Vector2(TransitionRect.Position.X + speed, TransitionRect.Position.Y);
                }
                else
                {
                    if (CurrentDialogue.Count == 2 && TransitionRect2.Scale.X > 1)
                    {
                        TransitionRect2.Scale = new Vector2(TransitionRect2.Scale.X - speed, TransitionRect2.Scale.Y);
                        TransitionRect2.Position = new Vector2(TransitionRect2.Position.X + speed, TransitionRect2.Position.Y);
                        return;
                    }

                    IsTransitioning = false;
                    UpdateDialogue = true;
                }
            }
        }

        public void LoadContent(ref Player player)
        {

            if (player.Sprite.SpriteSheetEffect.CurrentFrame.Y > 3)
                player.Sprite.SpriteSheetEffect.CurrentFrame.Y -= 4;
            player.Sprite.SpriteSheetEffect.CurrentFrame.X = 0;
            player.Sprite.IsActive = false;
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
            Border.Position = new Vector2(player.Sprite.Position.X - (ScreenManager.Instance.Dimensions.X -
                Border.SourceRect.Width) - 64, player.Sprite.Position.Y + positionOffset);
            foreach (TextBoxText image in Dialogue)
            {
                if (Type == "TilePlayer")
                    image.Text = Player.PlayerJsonObject.Name + "`s   house";
                else if (Type == "TileRival")
                    image.Text = Player.PlayerJsonObject.RivalName + "`s   house";
                image.LoadContent();

            }

            foreach (TextBoxText image in Dialogue)
            {
                if (image.Page == 1)
                    CurrentDialogue.Add(image);
            }



            CurrentDialogue[0].SetPosition(new Vector2(Border.Position.X + DialogueOffsetX, Border.Position.Y + DialogueOffsetY));

            //TRANSITION
            if (CurrentDialogue.Count >= 1)
            {
                TransitionRect = new Image
                {
                    Path = "TextBoxes/TextBoxEffectPixel"
                };
                TransitionRect.LoadContent();
                TransitionRect.Scale = new Vector2(CurrentDialogue[0].SourceRect.Width+4, CurrentDialogue[0].SourceRect.Height+4);
                TransitionRect.Position = new Vector2(CurrentDialogue[0].Position.X, CurrentDialogue[0].Position.Y);

                if (CurrentDialogue.Count == 2)
                {
                    CurrentDialogue[1].SetPosition(new Vector2(Border.Position.X + DialogueOffsetX, Border.Position.Y + DialogueOffsetY + 60));
                    TransitionRect2 = new Image
                    {
                        Path = "TextBoxes/TextBoxEffectPixel"
                    };
                    TransitionRect2.LoadContent();
                    TransitionRect2.Scale = new Vector2(CurrentDialogue[1].SourceRect.Width + 4, CurrentDialogue[1].SourceRect.Height + 4);
                    TransitionRect2.Position = new Vector2(CurrentDialogue[1].Position.X, CurrentDialogue[1].Position.Y);
                }

                if (TotalPages > 1 && !Menu)
                    SetArrowPosition();
                
            }

            //END TRANSITION
        }

        public virtual void UnloadContent()
        {
            IsDisplayed = false;
            Border.UnloadContent();
            foreach (PokemonText image in Dialogue)
                image.UnloadContent();

            if (TotalPages > 1 && !Menu)
                Arrow.UnloadContent();

            TransitionRect.UnloadContent();
            if (TransitionRect2 != null)
                TransitionRect2.UnloadContent();
        }

        public void UnloadContent(ref Player player)
        {
            UnloadContent();
            player.CanUpdate = true;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (IsDisplayed)
            {
                Border.Update(gameTime);
                foreach (PokemonText image in Dialogue)
                    image.Update(gameTime);


                Transition(gameTime);
                if (!Menu && TotalPages > 1 && Arrow.IsActive)
                    AnimateRedArrow(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (IsDisplayed)
            {
                Border.Draw(spriteBatch);
                foreach (TextBoxText image in CurrentDialogue)
                {
                    image.Draw(spriteBatch);
                }

                if (IsTransitioning)
                {
                    TransitionRect.Draw(spriteBatch);
                    if (CurrentDialogue.Count == 2)
                        TransitionRect2.Draw(spriteBatch);
                }
                else if (!Menu && TotalPages > 1 && Page < TotalPages)
                {
                    Arrow.Draw(spriteBatch);
                    if (!Arrow.IsActive)
                        Arrow.IsActive = true;
                }

            }
        }

        protected void AnimateRedArrow(GameTime gameTime)
        {
            float speed = (float)(32 * gameTime.ElapsedGameTime.TotalSeconds);

            if (ArrowOffset >= 8)
                increase = false;
            else if (ArrowOffset <= 0)
                increase = true;

            if (increase)
                ArrowOffset += speed;
            else
                ArrowOffset -= speed;

            if (ArrowOffset % 4 < 1)
                Arrow.Position.Y = ArrowOriginalY + (int) ArrowOffset;
        }

        private void SetArrowPosition()
        {
            int index = CurrentDialogue.Count == 2 ? 1 : 0;
            Arrow.Position = new Vector2(CurrentDialogue[index].Position.X + CurrentDialogue[index].SourceRect.Width, CurrentDialogue[index].Position.Y + 12);
            ArrowOriginalY = Arrow.Position.Y;
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