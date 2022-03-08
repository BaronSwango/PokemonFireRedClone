using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class BattleTextBox : TextBox
    {

        public int NextPage;

        /*
         * 
         * Very dynamic textbox
         * Need to adjust to different trainers, wild pokemon, etc
         * 
         */

        private void Transition(GameTime gameTime, BattleScreen battleScreen)
        {
            if (IsTransitioning)
            {    

                if (updateDialogue)
                {
                    if (Page != 10 && Page != 14)
                    {

                        Page = NextPage;

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
                                    image.Text = "Go !   " + Player.PlayerJsonObject.Pokemon.Name + " !";
                                    break;
                                case 4:
                                    if (image == currentDialogue[1])
                                        image.Text = Player.PlayerJsonObject.Pokemon.Name + "   do?";
                                    break;
                                case 5:

                                    if (currentDialogue[0] == image)
                                    {
                                        if (battleScreen.BattleAnimations.state == BattleAnimations.BattleState.ENEMY_DAMAGE_ANIMATION)
                                        {
                                            image.Text = Player.PlayerJsonObject.Pokemon.Name + "   used";
                                        } else if (battleScreen.BattleAnimations.state == BattleAnimations.BattleState.PLAYER_DAMAGE_ANIMATION)
                                        {
                                            image.Text = battleScreen.enemyPokemon.Pokemon.Name.ToUpper() + "   used";
                                        }
                                    }
                                    
                                    else if (currentDialogue[1] == image)
                                    {

                                        if (battleScreen.BattleAnimations.state == BattleAnimations.BattleState.ENEMY_DAMAGE_ANIMATION)
                                        {
                                            image.Text = battleScreen.BattleLogic.PlayerMoveOption.Name.ToUpper() + " !";
                                        }
                                        else if (battleScreen.BattleAnimations.state == BattleAnimations.BattleState.PLAYER_DAMAGE_ANIMATION)
                                        {
                                            image.Text = battleScreen.BattleLogic.EnemyMoveOption.Name.ToUpper() + " !";
                                        }
                                    }
                                    
                                    break;
                                case 9:
                                    if (currentDialogue[0] == image)
                                    {
                                        if (battleScreen.BattleAnimations.state == BattleAnimations.BattleState.PLAYER_POKEMON_FAINT)
                                            image.Text = Player.PlayerJsonObject.Pokemon.Name;

                                        else if (battleScreen.BattleAnimations.state == BattleAnimations.BattleState.ENEMY_POKEMON_FAINT)
                                        {
                                            if (BattleScreen.Wild)
                                                image.Text = "Wild   " + battleScreen.enemyPokemon.PokemonName.ToUpper();
                                            else
                                                image.Text = "Foe   " + battleScreen.enemyPokemon.PokemonName.ToUpper();
                                            if (Player.PlayerJsonObject.Pokemon.Level < 100)
                                                NextPage = 16;
                                        }
                                    }
                                    break;
                                case 16:

                                    if (currentDialogue[0] == image)
                                        image.Text = Player.PlayerJsonObject.Pokemon.Name + image.Text;

                                    else if (currentDialogue[1] == image)
                                        image.Text = battleScreen.BattleLogic.GainedEXP + image.Text;

                                    break;
                            }

                            if (image.EffectList.Count == 0)
                                image.LoadContent();
                            else
                                image.ReloadText();
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
                        return;
                    }
                    else
                    {
                        if (currentDialogue.Count == 2 && transitionRect2.Scale.X > 1)
                        {
                            transitionRect2.Scale = new Vector2(transitionRect2.Scale.X - speed, transitionRect2.Scale.Y);
                            transitionRect2.Position = new Vector2(transitionRect2.Position.X + speed, transitionRect2.Position.Y);
                            return;
                        }


                    }
                }
                IsTransitioning = false;
                updateDialogue = true;
            } else
            {
                foreach (TextBoxImage image in currentDialogue)
                {
                    if (!image.Skippable && !image.Arrow)
                    {
                        if (!battleScreen.BattleAnimations.IsTransitioning)
                        {
                            switch (Page)
                            {
                                case 3:
                                    NextPage = 4;
                                    IsTransitioning = true;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                       
                }

            }
        }


        public void LoadContent(CustomPokemon enemyPokemon)
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
                        image.Text = "Wild   " + enemyPokemon.Pokemon.Name.ToUpper() + "   appeared !";
                        NextPage = 3;
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

            setArrowPosition();

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
        { }

        public void Update(GameTime gameTime, BattleScreen battleScreen)
        {
            Transition(gameTime, battleScreen);

            if (InputManager.Instance.KeyPressed(Keys.E) && !IsTransitioning)
            {
                // FOR RIGHT NOW, CHANGE TO WHITING OUT OR SWITCH POKEMON
                if (Page == 16 || (Page == 9 && (battleScreen.BattleAnimations.state == BattleAnimations.BattleState.PLAYER_POKEMON_FAINT || Player.PlayerJsonObject.Pokemon.Level == 100)))
                {
                    ScreenManager.Instance.ChangeScreens("GameplayScreen");
                    return;
                }
                foreach (TextBoxImage image in currentDialogue)
                {
                    if (image.Skippable || image.Arrow)
                        IsTransitioning = true;
                }
            }

            setArrowPosition();
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

        private void setArrowPosition()
        {
            foreach (TextBoxImage image in currentDialogue)
            {
                if (image.Arrow && Arrow.Position.X != image.Position.X + image.SourceRect.Width + 8)
                {
                    Arrow.Position = new Vector2(image.Position.X + image.SourceRect.Width + 8, image.Position.Y + 12);
                    arrowOriginalY = Arrow.Position.Y;
                    return;
                }
            }

        }
    }
}
