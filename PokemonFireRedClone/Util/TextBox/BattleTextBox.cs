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
        bool PageSkipped;

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
                        PageSkipped = false;
                        Page = NextPage;

                        foreach (TextBoxText image in CurrentDialogue)
                            image.UnloadContent();
                        CurrentDialogue.Clear();

                        if (ScreenManager.Instance.BattleScreen.BattleAssets.State == BattleAssets.BattleState.TRAINER_BATTLE_VICTORY)
                        {
                            foreach (TextBoxText image in BattleLogic.Battle.Trainer.BattleText)
                            {
                                if (image.Page == Page) { 
                                    CurrentDialogue.Add(image);
                                }
                            }

                            if (Page < BattleLogic.Battle.Trainer.TotalBattleTextPages)
                            {
                                int index = CurrentDialogue.Count == 2 ? 1 : 0;
                                CurrentDialogue[index].Arrow = true;
                                SetArrowPosition();
                            }
                        }
                        else
                        {
                            foreach (TextBoxText image in Dialogue)
                            {
                                if (image.Page == Page)
                                    CurrentDialogue.Add(image);
                            }
                        }
                        
                        foreach (TextBoxText image in CurrentDialogue)
                        {

                            if (ScreenManager.Instance.BattleScreen.BattleAssets.State != BattleAssets.BattleState.TRAINER_BATTLE_VICTORY)
                            {
                                switch (Page)
                                {
                                    case 2:
                                        if (CurrentDialogue[0] == image)
                                            image.Text = BattleLogic.Battle.Trainer.Name + "   sent";
                                        else if (CurrentDialogue[1] == image)
                                            image.Text = "out   " + BattleLogic.Battle.EnemyPokemon.Pokemon.PokemonName.ToUpper() + " ! ";
                                        break;
                                    case 3:
                                        image.Text = "Go !   " + BattleLogic.Battle.PlayerPokemon.Pokemon.Name + " !";
                                        break;
                                    case 4:
                                        if (image == CurrentDialogue[1])
                                            image.Text = BattleLogic.Battle.PlayerPokemon.Pokemon.Name + "   do?";
                                        break;
                                    case 5:

                                        if (CurrentDialogue[0] == image)
                                        {
                                            if (ScreenManager.Instance.BattleScreen.BattleLogic.State == BattleLogic.FightState.PLAYER_DEFEND)
                                            {
                                                string encounter = BattleLogic.Battle.IsWild ? "Wild   " : "Foe   ";
                                                image.Text = encounter + BattleLogic.Battle.EnemyPokemon.Pokemon.Name + "   used";
                                            }
                                            else
                                                image.Text = BattleLogic.Battle.PlayerPokemon.Pokemon.Name + "   used";

                                        }

                                        else if (CurrentDialogue[1] == image)
                                        {
                                            image.Text = ScreenManager.Instance.BattleScreen.BattleLogic.State == BattleLogic.FightState.PLAYER_DEFEND ? ScreenManager.Instance.BattleScreen.BattleLogic.EnemyMoveOption.Name.ToUpper() + " !"
                                                : ScreenManager.Instance.BattleScreen.BattleLogic.PlayerMoveOption.Name.ToUpper() + " !";

                                            if (ScreenManager.Instance.BattleScreen.BattleLogic.EnemyMoveOption != null && ScreenManager.Instance.BattleScreen.BattleLogic.EnemyMoveOption.Category == "Status" && ScreenManager.Instance.BattleScreen.BattleLogic.State == BattleLogic.FightState.PLAYER_DEFEND)
                                            {
                                                ScreenManager.Instance.BattleScreen.BattleLogic.State = ScreenManager.Instance.BattleScreen.BattleLogic.EnemyMoveOption.Self ? BattleLogic.FightState.ENEMY_STATUS : BattleLogic.FightState.PLAYER_STATUS;
                                                NextPage = 18;
                                            }
                                            else if (ScreenManager.Instance.BattleScreen.BattleLogic.PlayerMoveOption != null && ScreenManager.Instance.BattleScreen.BattleLogic.PlayerMoveOption.Category == "Status" && ScreenManager.Instance.BattleScreen.BattleLogic.State == BattleLogic.FightState.ENEMY_DEFEND)
                                            {
                                                ScreenManager.Instance.BattleScreen.BattleLogic.State = ScreenManager.Instance.BattleScreen.BattleLogic.PlayerMoveOption.Self ? BattleLogic.FightState.PLAYER_STATUS : BattleLogic.FightState.ENEMY_STATUS;
                                                NextPage = 18;
                                            }

                                        }

                                        break;
                                    case 9:
                                        if (CurrentDialogue[0] == image)
                                        {
                                            if (ScreenManager.Instance.BattleScreen.BattleLogic.State == BattleLogic.FightState.PLAYER_FAINT)
                                                image.Text = BattleLogic.Battle.PlayerPokemon.Pokemon.Name;

                                            else if (ScreenManager.Instance.BattleScreen.BattleLogic.State == BattleLogic.FightState.ENEMY_FAINT)
                                            {
                                                string encounter = BattleLogic.Battle.IsWild ? "Wild   " : "Foe   ";
                                                image.Text = encounter + ScreenManager.Instance.BattleScreen.BattleAssets.EnemyPokemonAssets.Name.Text;

                                                if (BattleLogic.Battle.PlayerPokemon.Pokemon.Level < 100 || ScreenManager.Instance.BattleScreen.BattleLogic.LevelUp)
                                                    NextPage = 16;
                                            }
                                        }
                                        break;
                                    case 16:

                                        if (CurrentDialogue[0] == image)
                                            image.Text = BattleLogic.Battle.PlayerPokemon.Pokemon.Name + "   gained";

                                        else if (CurrentDialogue[1] == image)
                                            image.Text = ScreenManager.Instance.BattleScreen.BattleLogic.GainedEXP + "   EXP.   Points !";

                                        break;
                                    case 17:
                                        if (CurrentDialogue[0] == image)
                                            image.Text = BattleLogic.Battle.PlayerPokemon.Pokemon.Name + "   grew   to";

                                        else if (CurrentDialogue[1] == image)
                                            image.Text = "LV.   " + ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.Level.Image.Text[2..] + " !";
                                        break;
                                    case 18:
                                        if (CurrentDialogue[0] == image)
                                        {
                                            if (ScreenManager.Instance.BattleScreen.BattleLogic.State == BattleLogic.FightState.PLAYER_STATUS)
                                                image.Text = BattleLogic.Battle.PlayerPokemon.Pokemon.Name + "`s   " + ScreenManager.Instance.BattleScreen.BattleLogic.Stat;
                                            else if (ScreenManager.Instance.BattleScreen.BattleLogic.State == BattleLogic.FightState.ENEMY_STATUS)
                                            {
                                                string encounter = BattleLogic.Battle.IsWild ? "Wild   " : "Foe   ";
                                                image.Text = encounter + BattleLogic.Battle.EnemyPokemon.Pokemon.Name + "`s   " + ScreenManager.Instance.BattleScreen.BattleLogic.Stat;
                                            }
                                        }

                                        else if (CurrentDialogue[1] == image)
                                        {
                                            if (ScreenManager.Instance.BattleScreen.BattleLogic.StageMaxed)
                                            {
                                                string change = ScreenManager.Instance.BattleScreen.BattleLogic.StatStageIncrease ? "higher" : "lower";
                                                image.Text = "won`t   go   " + change + " !";
                                            }
                                            else
                                            {
                                                string sharply = ScreenManager.Instance.BattleScreen.BattleLogic.SharplyStat ? "sharply   " : "";
                                                string change = ScreenManager.Instance.BattleScreen.BattleLogic.StatStageIncrease ? "rose" : "fell";
                                                image.Text = sharply + change + " !";
                                            }
                                        }

                                        break;
                                    case 19:
                                        if (CurrentDialogue[0] == image)
                                            image.Text = BattleLogic.Battle.PlayerPokemon.Pokemon.Name + " ,   that`s   enough !";

                                        else if (CurrentDialogue[1] == image)
                                            image.Text = "Come   back !";

                                        break;
                                    case 20:
                                        if (CurrentDialogue[0] == image)
                                        {
                                            if (ScreenManager.Instance.BattleScreen.BattleLogic.State == BattleLogic.FightState.ENEMY_DEFEND || ScreenManager.Instance.BattleScreen.BattleLogic.State == BattleLogic.FightState.ENEMY_STATUS)
                                            {
                                                image.Text = BattleLogic.Battle.PlayerPokemon.Pokemon.Name + "`s";
                                                ScreenManager.Instance.BattleScreen.BattleLogic.PlayerHasMoved = true;
                                            }
                                            else if (ScreenManager.Instance.BattleScreen.BattleLogic.State == BattleLogic.FightState.PLAYER_DEFEND || ScreenManager.Instance.BattleScreen.BattleLogic.State == BattleLogic.FightState.PLAYER_STATUS)
                                            {
                                                string encounter = BattleLogic.Battle.IsWild ? "Wild   " : "Foe   ";
                                                image.Text = encounter + BattleLogic.Battle.EnemyPokemon.Pokemon.Name + "`s";
                                                ScreenManager.Instance.BattleScreen.BattleLogic.EnemyHasMoved = true;
                                            }
                                        }
                                        else if (CurrentDialogue[1] == image)
                                            image.Text = "attack   missed !";

                                        break;
                                    case 22:
                                        if (CurrentDialogue[0] == image)
                                            image.Text = Player.PlayerJsonObject.Name + image.Text;
                                        else if (CurrentDialogue[1] == image)
                                            image.Text = BattleLogic.Battle.Trainer.Name + image.Text;

                                        break;
                                    case 23:
                                        if (CurrentDialogue[0] == image)
                                            image.Text = Player.PlayerJsonObject.Name + "   got   " + "$" + BattleLogic.Battle.Trainer.Reward;
                                        break;
                                }
                            }
                            if (!image.IsLoaded)
                                image.LoadContent();
                            else
                                image.ReloadText();
                        }

                        CurrentDialogue[0].SetPosition(new Vector2(Border.Position.X + DialogueOffsetX, Border.Position.Y + DialogueOffsetY));

                        TransitionRect.Scale = new Vector2(CurrentDialogue[0].SourceRect.Width+4, CurrentDialogue[0].SourceRect.Height+4);
                        TransitionRect.Position = new Vector2(CurrentDialogue[0].Position.X, CurrentDialogue[0].Position.Y);

                        if (CurrentDialogue.Count == 2)
                        {
                            CurrentDialogue[1].SetPosition(new Vector2(Border.Position.X + DialogueOffsetX, Border.Position.Y + DialogueOffsetY + 64));

                            TransitionRect2.Scale = new Vector2(CurrentDialogue[1].SourceRect.Width+4, CurrentDialogue[1].SourceRect.Height+4);
                            TransitionRect2.Position = new Vector2(CurrentDialogue[1].Position.X, CurrentDialogue[1].Position.Y);
                        }
                    }

                    UpdateDialogue = false;

                }


                float speed = (float)(1.35 * gameTime.ElapsedGameTime.TotalMilliseconds);
                if (Page != 4 || ScreenManager.Instance.BattleScreen.BattleAssets.State == BattleAssets.BattleState.TRAINER_BATTLE_VICTORY)
                {
                    if (TransitionRect.Scale.X > 1)
                    {
                        TransitionRect.Scale = new Vector2(TransitionRect.Scale.X - speed, TransitionRect.Scale.Y);
                        TransitionRect.Position = new Vector2(TransitionRect.Position.X + speed, TransitionRect.Position.Y);
                        return;
                    }
                    else
                    {
                        if (CurrentDialogue.Count == 2 && TransitionRect2.Scale.X > 1)
                        {
                            TransitionRect2.Scale = new Vector2(TransitionRect2.Scale.X - speed, TransitionRect2.Scale.Y);
                            TransitionRect2.Position = new Vector2(TransitionRect2.Position.X + speed, TransitionRect2.Position.Y);
                            return;
                        }


                    }
                }
                IsTransitioning = false;
                UpdateDialogue = true;
            }
        }


        public void LoadContent(CustomPokemon enemyPokemon)
        {
            IsTransitioning = true;
            BattleLevelUp = new BattleLevelUp();

            Border.LoadContent();
            Arrow.LoadContent();

            Border.Position = new Vector2(ScreenManager.Instance.Dimensions.X - Border.SourceRect.Width, ScreenManager.Instance.Dimensions.Y - Border.SourceRect.Height);

            if (!BattleLogic.Battle.IsWild)
            {
                foreach (TextBoxText image in BattleLogic.Battle.Trainer.BattleText)
                {
                    image.Image.FontName = "Fonts/PokemonFireRedDialogue";
                    image.Image.R = image.Image.G = image.Image.B = 255;
                    image.R = 118;
                    image.G = 105;
                    image.B = 126;
                }
            }

            foreach (TextBoxText image in Dialogue)
            {
                image.Image.FontName = "Fonts/PokemonFireRedDialogue";
                image.Image.R = image.Image.G = image.Image.B = 255;
                image.R = 118;
                image.G = 105;
                image.B = 126;

                if (image.Page == 1)
                {
                    if (BattleLogic.Battle.IsWild)
                    {
                        image.Text = "Wild   " + enemyPokemon.Name + "   appeared !";
                        NextPage = 3;
                        CurrentDialogue.Add(image);
                        image.LoadContent();
                        image.Arrow = true;
                        break;
                    }
                    else
                    {
                        if (CurrentDialogue.Count == 0)
                        {
                            image.Text = BattleLogic.Battle.Trainer.Name;
                            NextPage = 2;
                        }
                        CurrentDialogue.Add(image);
                        image.LoadContent();
                    }
                }

            }

            CurrentDialogue[0].SetPosition(new Vector2(Border.Position.X + DialogueOffsetX, Border.Position.Y + DialogueOffsetY));

            SetArrowPosition();

            TransitionRect = new Image
            {
                Path = "TextBoxes/TextBoxEffectPixelBattle"
            };
            TransitionRect.LoadContent();
            TransitionRect.Scale = new Vector2(CurrentDialogue[0].SourceRect.Width+4, CurrentDialogue[0].SourceRect.Height+4);
            TransitionRect.Position = new Vector2(CurrentDialogue[0].Position.X, CurrentDialogue[0].Position.Y);

            TransitionRect2 = new Image
            {
                Path = "TextBoxes/TextBoxEffectPixelBattle"
            };
            TransitionRect2.LoadContent();

            if (CurrentDialogue.Count == 2)
            {
                CurrentDialogue[1].SetPosition(new Vector2(Border.Position.X + DialogueOffsetX, Border.Position.Y + DialogueOffsetY + 64));

                TransitionRect2.Scale = new Vector2(CurrentDialogue[1].SourceRect.Width + 4, CurrentDialogue[1].SourceRect.Height + 4);
                TransitionRect2.Position = new Vector2(CurrentDialogue[1].Position.X, CurrentDialogue[1].Position.Y);
            }
        }

        public override void UnloadContent()
        {
            Border.UnloadContent();
            Arrow.UnloadContent();
            foreach (TextBoxText image in CurrentDialogue)
                image.UnloadContent();

            TransitionRect.UnloadContent();
            TransitionRect2.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Transition(gameTime);

            if (InputManager.Instance.KeyPressed(Keys.E) && !IsTransitioning && !PageSkipped)
            {
                if (ScreenManager.Instance.BattleScreen.BattleAssets.State == BattleAssets.BattleState.TRAINER_BATTLE_VICTORY)
                {
                    NextPage = Page < BattleLogic.Battle.Trainer.TotalBattleTextPages ? NextPage + 1 : 23;
                    if (Page == BattleLogic.Battle.Trainer.TotalBattleTextPages)
                        ScreenManager.Instance.BattleScreen.BattleAssets.State = BattleAssets.BattleState.END_TRAINER_BATTLE;
                    IsTransitioning = true;
                }
                else
                {
                    switch (Page)
                    {
                        case 1:
                            IsTransitioning = true;
                            break;
                        case 9:
                            if (ScreenManager.Instance.BattleScreen.BattleLogic.State == BattleLogic.FightState.PLAYER_FAINT || (BattleLogic.Battle.PlayerPokemon.Pokemon.Level == 100 && !ScreenManager.Instance.BattleScreen.BattleLogic.LevelUp && BattleLogic.Battle.IsWild))
                            {
                                ScreenManager.Instance.ChangeScreens("GameplayScreen");
                                BattleLogic.EndBattle();
                                return;
                            }

                            if (ScreenManager.Instance.BattleScreen.BattleLogic.State == BattleLogic.FightState.ENEMY_FAINT && BattleLogic.Battle.PlayerPokemon.Pokemon.Level == 100)
                            {
                                if (!BattleLogic.Battle.IsEnded)
                                {
                                    ScreenManager.Instance.BattleScreen.BattleAssets.State = BattleAssets.BattleState.OPPONENT_SEND_POKEMON;
                                    ScreenManager.Instance.BattleScreen.BattleAssets.Animation = new TrainerBallBarAnimation();
                                    ScreenManager.Instance.BattleScreen.BattleAssets.IsTransitioning = true;
                                }
                                else if (!BattleLogic.Battle.IsWild)
                                {
                                    NextPage = 22;
                                    IsTransitioning = true;
                                }
                            }

                            if (BattleLogic.Battle.PlayerPokemon.Pokemon.Level < 100)
                                IsTransitioning = true;

                            ScreenManager.Instance.BattleScreen.BattleLogic.State = BattleLogic.FightState.NONE;
                            ScreenManager.Instance.BattleScreen.BattleLogic.EXPGainApplied = false;
                            break;
                        case 16:
                            ScreenManager.Instance.BattleScreen.BattleAssets.State = BattleAssets.BattleState.EXP_ANIMATION;
                            ScreenManager.Instance.BattleScreen.BattleAssets.Animation = new EXPAnimation();
                            ScreenManager.Instance.BattleScreen.BattleAssets.IsTransitioning = true;
                            break;
                        case 17:
                            BattleLevelUp.LoadContent(BattleLogic.Battle.PlayerPokemon.Pokemon, int.Parse(ScreenManager.Instance.BattleScreen.BattleAssets.PlayerPokemonAssets.Level.Image.Text[2..]));
                            break;
                        case 21:
                            ScreenManager.Instance.BattleScreen.MenuManager.LoadContent("Load/Menus/MoveMenu.xml");
                            NextPage = 4;
                            Page = 4;
                            IsTransitioning = true;
                            break;
                        case 22:
                            //TODO: Handle trainer post-battle text and $ earnings
                            /*
                            ScreenManager.Instance.ChangeScreens("GameplayScreen");
                            BattleLogic.EndBattle();
                            */
                            ScreenManager.Instance.BattleScreen.BattleAssets.State = BattleAssets.BattleState.TRAINER_BATTLE_VICTORY;
                            ScreenManager.Instance.BattleScreen.BattleAssets.Animation = new TrainerVictoryAnimation();
                            ScreenManager.Instance.BattleScreen.BattleAssets.IsTransitioning = true;
                            break;
                        case 23:
                            ScreenManager.Instance.ChangeScreens("GameplayScreen");
                            BattleLogic.EndBattle();
                            return;
                        default:
                            break;
                    }
                }
                PageSkipped = true;
            }

            SetArrowPosition();
            if (ScreenManager.Instance.BattleScreen.BattleAssets.State != BattleAssets.BattleState.OPPONENT_SEND_POKEMON)
                AnimateRedArrow(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Border.Draw(spriteBatch);

            foreach (TextBoxText image in CurrentDialogue)
            {
                image.Draw(spriteBatch);
                if (image.Arrow && !IsTransitioning)
                    Arrow.Draw(spriteBatch);
            }


            if (IsTransitioning && (Page != 4 || ScreenManager.Instance.BattleScreen.BattleAssets.State == BattleAssets.BattleState.TRAINER_BATTLE_VICTORY))
            {
                TransitionRect.Draw(spriteBatch);
                if (CurrentDialogue.Count == 2)
                    TransitionRect2.Draw(spriteBatch);
            }


            BattleLevelUp.Draw(spriteBatch);
        }

        private void SetArrowPosition()
        {
            foreach (TextBoxText image in CurrentDialogue)
            {
                if (image.Arrow && Arrow.Position.X != image.Position.X + image.SourceRect.Width + 8)
                {
                    Arrow.Position = new Vector2(image.Position.X + image.SourceRect.Width + 8, image.Position.Y + 12);
                    ArrowOriginalY = Arrow.Position.Y;
                    return;
                }
            }

        }
    }
}
