﻿using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class BattleTextBox : TextBox
    {

        public int NextPage;
        [XmlIgnore]
        public BattleLevelUp BattleLevelUp;

        BattleScreen battleScreen
        {
            get { return (BattleScreen)ScreenManager.Instance.CurrentScreen; }
            set { }
        }

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
                                    image.Text = "Go !   " + Player.PlayerJsonObject.PokemonInBag[0].Name + " !";
                                    break;
                                case 4:
                                    if (image == currentDialogue[1])
                                        image.Text = Player.PlayerJsonObject.PokemonInBag[0].Name + "   do?";
                                    break;
                                case 5:
                                    
                                    if (currentDialogue[0] == image)
                                    {
                                        if (battleScreen.BattleLogic.State == BattleLogic.FightState.PLAYER_DEFEND)
                                        {
                                            string encounter = BattleScreen.Wild ? "Wild   " : "Foe   ";
                                            image.Text = encounter + battleScreen.enemyPokemon.Name + "   used";
                                        } else 
                                            image.Text = Player.PlayerJsonObject.PokemonInBag[0].Name + "   used";

                                    }
                                    
                                    else if (currentDialogue[1] == image)
                                    {
                                        image.Text = battleScreen.BattleLogic.State == BattleLogic.FightState.PLAYER_DEFEND ? battleScreen.BattleLogic.EnemyMoveOption.Name.ToUpper() + " !"
                                            : battleScreen.BattleLogic.PlayerMoveOption.Name.ToUpper() + " !";

                                        if (battleScreen.BattleLogic.EnemyMoveOption != null && battleScreen.BattleLogic.EnemyMoveOption.Category == "Status" && battleScreen.BattleLogic.State == BattleLogic.FightState.PLAYER_DEFEND)
                                        {
                                            battleScreen.BattleLogic.State = battleScreen.BattleLogic.EnemyMoveOption.Self ? BattleLogic.FightState.ENEMY_STATUS : BattleLogic.FightState.PLAYER_STATUS;
                                            NextPage = 18;
                                        } else if (battleScreen.BattleLogic.PlayerMoveOption != null && battleScreen.BattleLogic.PlayerMoveOption.Category == "Status" && battleScreen.BattleLogic.State == BattleLogic.FightState.ENEMY_DEFEND)
                                        {
                                            battleScreen.BattleLogic.State = battleScreen.BattleLogic.PlayerMoveOption.Self ? BattleLogic.FightState.PLAYER_STATUS : BattleLogic.FightState.ENEMY_STATUS;
                                            NextPage = 18;
                                        }

                                    }

                                    break;
                                case 9:
                                    if (currentDialogue[0] == image)
                                    {
                                        if (battleScreen.BattleAnimations.state == BattleAnimations.BattleState.PLAYER_POKEMON_FAINT)
                                            image.Text = Player.PlayerJsonObject.PokemonInBag[0].Name;

                                        else if (battleScreen.BattleAnimations.state == BattleAnimations.BattleState.ENEMY_POKEMON_FAINT)
                                        {
                                            string encounter = BattleScreen.Wild ? "Wild   " : "Foe   ";
                                            image.Text = encounter + battleScreen.enemyPokemon.Name;

                                            if (Player.PlayerJsonObject.PokemonInBag[0].Level < 100 || battleScreen.BattleLogic.LevelUp)
                                                NextPage = 16;
                                        }
                                    }
                                    break;
                                case 16:

                                    if (currentDialogue[0] == image)
                                        image.Text = Player.PlayerJsonObject.PokemonInBag[0].Name + image.Text;

                                    else if (currentDialogue[1] == image)
                                        image.Text = battleScreen.BattleLogic.GainedEXP + image.Text;

                                    break;
                                case 17:
                                    if (currentDialogue[0] == image)
                                        image.Text = Player.PlayerJsonObject.PokemonInBag[0].Name + "   grew   to";

                                    else if (currentDialogue[1] == image)
                                        image.Text = "LV.   " + battleScreen.BattleAnimations.PlayerPokemonAssets.Level.Text.Text[2..]+ " !";
                                    break;
                                case 18:
                                    if (currentDialogue[0] == image)
                                    {
                                        if (battleScreen.BattleLogic.State == BattleLogic.FightState.PLAYER_STATUS)
                                            image.Text = Player.PlayerJsonObject.PokemonInBag[0].Name + "`s   " + battleScreen.BattleLogic.Stat;
                                        else if (battleScreen.BattleLogic.State == BattleLogic.FightState.ENEMY_STATUS)
                                        {
                                            string encounter = BattleScreen.Wild ? "Wild   " : "Foe   ";
                                            image.Text = encounter + battleScreen.enemyPokemon.Name + "`s   " + battleScreen.BattleLogic.Stat;
                                        }
                                    }

                                    else if (currentDialogue[1] == image)
                                    {
                                        if (battleScreen.BattleLogic.StageMaxed)
                                        {
                                            string change = battleScreen.BattleLogic.StatStageIncrease ? "higher" : "lower";
                                            image.Text = "won`t   go   " + change + " !";
                                        }
                                        else
                                        {
                                            string sharply = battleScreen.BattleLogic.SharplyStat ? "sharply   " : "";
                                            string change = battleScreen.BattleLogic.StatStageIncrease ? "rose" : "fell";
                                            image.Text = sharply + change + " !";
                                        }
                                    }
                                        
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
            BattleLevelUp = new BattleLevelUp();

            Border.LoadContent();
            Arrow.LoadContent();

            Border.Position = new Vector2(ScreenManager.Instance.Dimensions.X - Border.SourceRect.Width, ScreenManager.Instance.Dimensions.Y - Border.SourceRect.Height);


            foreach (TextBoxImage image in Dialogue)
            {
                if (image.Page == 1)
                {
                    if (BattleScreen.Wild)
                    {
                        image.Text = "Wild   " + enemyPokemon.Name + "   appeared !";
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

        public override void Update(GameTime gameTime)
        {
            Transition(gameTime);

            if (InputManager.Instance.KeyPressed(Keys.E) && !IsTransitioning)
            {
                switch (Page)
                {
                    case 1:
                        IsTransitioning = true;
                        break;
                    case 9:
                        if (battleScreen.BattleAnimations.state == BattleAnimations.BattleState.PLAYER_POKEMON_FAINT || (Player.PlayerJsonObject.PokemonInBag[0].Level == 100 && !battleScreen.BattleLogic.LevelUp)) {
                            ScreenManager.Instance.ChangeScreens("GameplayScreen");
                            return;
                        }
                        IsTransitioning = true;
                        break;
                    case 16:
                        battleScreen.BattleAnimations.state = BattleAnimations.BattleState.EXP_ANIMATION;
                        battleScreen.BattleAnimations.IsTransitioning = true;
                        break;
                    case 17:
                        BattleLevelUp.LoadContent(Player.PlayerJsonObject.PokemonInBag[0], int.Parse(battleScreen.BattleAnimations.PlayerPokemonAssets.Level.Text.Text[2..]));
                        break;
                    default:
                        break;
                }

                /*
                foreach (TextBoxImage image in currentDialogue)
                {
                    if (image.Skippable || image.Arrow)
                        IsTransitioning = true;
                }
                */
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


            BattleLevelUp.Draw(spriteBatch);
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
