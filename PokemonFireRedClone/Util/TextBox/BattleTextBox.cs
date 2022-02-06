using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class BattleTextBox : TextBox
    {

        int nextPage;
        Pokemon wildEncounterPoke;
        Pokemon playerPokemon;
        double nextPageCounter;

        /*
         * 
         * Very dynamic textbox
         * Need to adjust to different trainers, wild pokemon, etc
         * 
         */

        private void Transition(GameTime gameTime)
        {
            if (IsTransitioning)
            {
                if (updateDialogue)
                {
                    if (Page != 10 && Page != 14)
                    {

                        Page = nextPage;

                        foreach (TextBoxImage image in currentDialogue)
                            image.UnloadContent();
                        currentDialogue.Clear();
                        foreach (TextBoxImage image in Dialogue)
                        {
                            if (image.Page == Page)
                                currentDialogue.Add(image);
                        }

                        foreach (TextBoxImage image in currentDialogue)
                        {
                            switch (Page)
                            {
                                case 3:
                                    image.Text = "Go !   " + playerPokemon.Name.ToUpper() + " !";
                                    break;
                                case 4:
                                    if (image.Text.Contains("P"))
                                        image.Text = playerPokemon.Name.ToUpper() + "   do?";
                                    break;
                                default:
                                    return;
                            }
                            image.LoadContent();
                        }

                        currentDialogue[0].Position = new Vector2(Border.Position.X + DialogueOffsetX, Border.Position.Y + DialogueOffsetY);

                        transitionRect.Scale = new Vector2(currentDialogue[0].SourceRect.Width, currentDialogue[0].SourceRect.Height);
                        transitionRect.Position = new Vector2(currentDialogue[0].Position.X, currentDialogue[0].Position.Y);

                        if (currentDialogue.Count == 2)
                        {
                            currentDialogue[1].Position = new Vector2(Border.Position.X + DialogueOffsetX, Border.Position.Y + DialogueOffsetY + 64);

                            transitionRect2.Scale = new Vector2(currentDialogue[1].SourceRect.Width, currentDialogue[1].SourceRect.Height);
                            transitionRect2.Position = new Vector2(currentDialogue[1].Position.X, currentDialogue[1].Position.Y);
                        }
                    }

                    updateDialogue = false;

                }



                float speed = (float)(1.35 * gameTime.ElapsedGameTime.TotalMilliseconds);
                if (Page != 4)
                {
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
            } else
            {
                foreach (TextBoxImage image in currentDialogue)
                {
                    if (!image.Skippable && !image.Arrow)
                    {
                        nextPageCounter += gameTime.ElapsedGameTime.TotalSeconds;
                        if (nextPageCounter >= 2)
                        {
                            switch (Page)
                            {
                                case 3:
                                    nextPage = 4;
                                    IsTransitioning = true;
                                    break;
                                case 4:
                                    break;
                                default:
                                    return;
                            }
                            nextPageCounter = 0;
                        }
                    }
                       
                }
            }
        }

        
        public BattleTextBox()
        {
            wildEncounterPoke = PokemonManager.Instance.GetPokemon("Squirtle");
            playerPokemon = PokemonManager.Instance.GetPokemon(Player.PlayerJsonObject.Pokemon);
        }


        public void LoadContent()
        {
            IsTransitioning = true;

            Border.LoadContent();
            Arrow.LoadContent();

            Border.Position = new Vector2(ScreenManager.Instance.Dimensions.X - Border.SourceRect.Width, ScreenManager.Instance.Dimensions.Y - Border.SourceRect.Height);


            foreach (TextBoxImage image in Dialogue)
            {
                if (image.Page == 1)
                {
                    if (BattleScreen.Wild)
                    {
                        image.Text = "Wild   " + wildEncounterPoke.Name.ToUpper() + "   appeared !";
                        nextPage = 3;
                        currentDialogue.Add(image);
                        image.LoadContent();
                        break;
                    }
                    else
                    {
                        // TRAINER NAME AND STUFF
                        currentDialogue.Add(image);
                        image.LoadContent();
                    }
                }

            }

            currentDialogue[0].Position = new Vector2(Border.Position.X + DialogueOffsetX, Border.Position.Y + DialogueOffsetY);

            Arrow.Position = new Vector2(currentDialogue[0].Position.X + currentDialogue[0].SourceRect.Width + 8, currentDialogue[0].Position.Y + 12);
            arrowOriginalY = Arrow.Position.Y;

            transitionRect = new Image
            {
                Path = "TextBoxes/TextBoxEffectPixelBattle"
            };
            transitionRect.LoadContent();
            transitionRect.Scale = new Vector2(currentDialogue[0].SourceRect.Width, currentDialogue[0].SourceRect.Height);
            transitionRect.Position = new Vector2(currentDialogue[0].Position.X, currentDialogue[0].Position.Y);

            transitionRect2 = new Image
            {
                Path = "TextBoxes/TextBoxEffectPixelBattle"
            };
            transitionRect2.LoadContent();
        }

        public void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            Transition(gameTime);

            if (InputManager.Instance.KeyPressed(Keys.E) && !IsTransitioning)
            {
                foreach (TextBoxImage image in currentDialogue)
                {
                    if (image.Skippable || image.Arrow)
                        IsTransitioning = true;
                }
            }

            animateRedArrow(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Border.Draw(spriteBatch);

            foreach (TextBoxImage image in currentDialogue)
            {
                image.Draw(spriteBatch);
                if (image.Arrow && !IsTransitioning)
                    Arrow.Draw(spriteBatch);
            }
            if (IsTransitioning && Page != 4)
            {
                transitionRect.Draw(spriteBatch);
                if (currentDialogue.Count == 2)
                    transitionRect2.Draw(spriteBatch);
            }
        }
    }
}
