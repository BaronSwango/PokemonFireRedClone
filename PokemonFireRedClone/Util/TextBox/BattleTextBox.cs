using System.Collections.Generic;
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

                if (UpdateDialogue)
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
                                    image.Text = "Go !   " + BattleLogic.Battle.PlayerPokemon.Pokemon.Name + " !";
                                    break;
                                case 4:
                                    if (image == currentDialogue[1])
                                        image.Text = BattleLogic.Battle.PlayerPokemon.Pokemon.Name + "   do?";
                                    break;
                                case 5:
                                    
                                    if (currentDialogue[0] == image)
                                    {
                                        if (battleScreen.BattleLogic.State == BattleLogic.FightState.PLAYER_DEFEND)
                                        {
                                            string encounter = BattleLogic.Battle.IsWild ? "Wild   " : "Foe   ";
                                            image.Text = encounter + BattleLogic.Battle.EnemyPokemon.Pokemon.Name + "   used";
                                        } else 
                                            image.Text = BattleLogic.Battle.PlayerPokemon.Pokemon.Name + "   used";

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
                                        if (battleScreen.BattleLogic.State == BattleLogic.FightState.PLAYER_FAINT)
                                            image.Text = BattleLogic.Battle.PlayerPokemon.Pokemon.Name;

                                        else if (battleScreen.BattleLogic.State == BattleLogic.FightState.ENEMY_FAINT)
                                        {
                                            string encounter = BattleLogic.Battle.IsWild ? "Wild   " : "Foe   ";
                                            image.Text = encounter + BattleLogic.Battle.EnemyPokemon.Pokemon.Name;

                                            if (BattleLogic.Battle.PlayerPokemon.Pokemon.Level < 100 || battleScreen.BattleLogic.LevelUp)
                                                NextPage = 16;
                                            battleScreen.BattleLogic.State = BattleLogic.FightState.NONE;
                                        }
                                    }
                                    break;
                                case 16:

                                    if (currentDialogue[0] == image)
                                        image.Text = BattleLogic.Battle.PlayerPokemon.Pokemon.Name + image.Text;

                                    else if (currentDialogue[1] == image)
                                        image.Text = battleScreen.BattleLogic.GainedEXP + image.Text;

                                    break;
                                case 17:
                                    if (currentDialogue[0] == image)
                                        image.Text = BattleLogic.Battle.PlayerPokemon.Pokemon.Name + "   grew   to";

                                    else if (currentDialogue[1] == image)
                                        image.Text = "LV.   " + battleScreen.BattleAssets.PlayerPokemonAssets.Level.Text.Text[2..]+ " !";
                                    break;
                                case 18:
                                    if (currentDialogue[0] == image)
                                    {
                                        if (battleScreen.BattleLogic.State == BattleLogic.FightState.PLAYER_STATUS)
                                            image.Text = BattleLogic.Battle.PlayerPokemon.Pokemon.Name + "`s   " + battleScreen.BattleLogic.Stat;
                                        else if (battleScreen.BattleLogic.State == BattleLogic.FightState.ENEMY_STATUS)
                                        {
                                            string encounter = BattleLogic.Battle.IsWild ? "Wild   " : "Foe   ";
                                            image.Text = encounter + BattleLogic.Battle.EnemyPokemon.Pokemon.Name + "`s   " + battleScreen.BattleLogic.Stat;
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
                                case 19:
                                    if (currentDialogue[0] == image)
                                    {
                                        image.Text = BattleLogic.Battle.PlayerPokemon.Pokemon.Name + " ,   that`s   enough !";
                                    }
                                    else if (currentDialogue[1] == image)
                                    {
                                        image.Text = "Come   back !";
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

                    UpdateDialogue = false;

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
                UpdateDialogue = true;
            } else
            {
                foreach (TextBoxImage image in currentDialogue)
                {
                    if (!image.Skippable && !image.Arrow)
                    {
                        if (!battleScreen.BattleAssets.IsTransitioning)
                        {
                            switch (Page)
                            {
                                case 3:
                                    //NextPage = 4;
                                    //IsTransitioning = true;
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
                    if (BattleLogic.Battle.IsWild)
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
                        if (battleScreen.BattleLogic.State == BattleLogic.FightState.PLAYER_FAINT || (BattleLogic.Battle.PlayerPokemon.Pokemon.Level == 100 && !battleScreen.BattleLogic.LevelUp)) {
                            ScreenManager.Instance.ChangeScreens("GameplayScreen");
                            BattleLogic.EndBattle();
                            return;
                        }
                        IsTransitioning = true;
                        break;
                    case 16:
                        battleScreen.BattleAssets.State = BattleAssets.BattleState.EXP_ANIMATION;
                        battleScreen.BattleAssets.Animation = new EXPAnimation();
                        battleScreen.BattleAssets.IsTransitioning = true;
                        break;
                    case 17:
                        BattleLevelUp.LoadContent(BattleLogic.Battle.PlayerPokemon.Pokemon, int.Parse(battleScreen.BattleAssets.PlayerPokemonAssets.Level.Text.Text[2..]));
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
