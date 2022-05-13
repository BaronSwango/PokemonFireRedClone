using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class BattleAnimations
    {
        // TODO: Add sounds to all animations

        BattleScreen BattleScreen
        {
            get { return (BattleScreen)ScreenManager.Instance.CurrentScreen; }
            set { }
        }

        public enum BattleState
        {
            INTRO,
            WILD_POKEMON_FADE_IN,
            PLAYER_SEND_POKEMON,
            ENEMY_SEND_POKEMON,
            BATTLE_MENU,
            MOVE_EXCHANGE,
            DAMAGE_ANIMATION,
            STATUS_ANIMATION,
            FADE_TRANSITION,
            POKEMON_SWITCH,
            POKEMON_SEND_OUT,
            PLAYER_POKEMON_FAINT,
            ENEMY_POKEMON_FAINT,
            EXP_ANIMATION,
            LEVEL_UP_ANIMATION
        }

        // General battle screen data
        Image Background;
        Image EnemyPlatform;
        Image PlayerPlatform;
        Image EnemyPokemon;
        Image EnemySprite;
        Image PlayerSprite;
        Image PlayerPokemon;
        public Image PlayerHPBarBackground;
        public Image EnemyHPBarBackground;
        public Image PlayerHPBarLevelUp;
        public Image Pokeball;
        public Image StatChangeAnimationImage1;
        public Image StatChangeAnimationImage2;
        public bool IsTransitioning;
        public BattleState state;
        public static bool FromMenu;

        // HP bar data
        [XmlIgnore]
        public PokemonAssets PlayerPokemonAssets;
        [XmlIgnore]
        public PokemonAssets EnemyPokemonAssets;

        public Image EXPBar;

        // Level up transition data
        bool levelUpTransitioned;

        // Send out pokemon transition data
        float pokeballSpeedY = 4f;
        bool maxHeight;
        bool whiteBackgroundTransitioned;
        Image whiteBackground;

        // time counters and animation ints
        float counter = 0;
        int blinkCounter = 0;

        // temp stat changes
        bool increase;
        int reveal;
        float spinCounter;

        private void Transition(GameTime gameTime)
        {
            if (IsTransitioning && !ScreenManager.Instance.IsTransitioning)
            {
                float counterSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                switch (state)
                {
                    case BattleState.INTRO:
                        float enemySpeed = (float)(0.596 * gameTime.ElapsedGameTime.TotalMilliseconds);
                        float playerSpeed = (float)(0.807 * gameTime.ElapsedGameTime.TotalMilliseconds);
                        float enemyPlatformDestinationX = ScreenManager.Instance.Dimensions.X - EnemyPlatform.SourceRect.Width;
                        float playerPlatformDestinationX = 16;

                        if (!(PlayerPlatform.Position.X - playerSpeed < playerPlatformDestinationX) && !(EnemyPlatform.Position.X + enemySpeed > enemyPlatformDestinationX))
                        {
                            PlayerPlatform.Position.X -= playerSpeed;
                            PlayerSprite.Position.X -= playerSpeed;
                            EnemyPlatform.Position.X += enemySpeed;
                            EnemyPokemon.Position.X += enemySpeed;
                            break;
                        }

                        PlayerPlatform.Position.X = playerPlatformDestinationX;
                        PlayerSprite.Position.X = PlayerPlatform.Position.X + PlayerPlatform.SourceRect.Width / 2 - 48;
                        EnemyPlatform.Position.X = enemyPlatformDestinationX;
                        EnemyPokemon.Position.X = EnemyPlatform.Position.X + EnemyPlatform.SourceRect.Width / 2 - EnemyPokemon.SourceRect.Width / 2;

                        if (BattleLogic.Battle.IsWild)
                            state = BattleState.WILD_POKEMON_FADE_IN;
                        else
                            IsTransitioning = false;
                        break;
                    case BattleState.WILD_POKEMON_FADE_IN:
                        float enemyHPDestinationX = 52;
                        enemySpeed = (float)(1.2 * gameTime.ElapsedGameTime.TotalMilliseconds);
                        if (EnemyPokemon.Tint != Color.White || EnemyHPBarBackground.Position.X + enemySpeed < enemyHPDestinationX)
                        {
                            if (EnemyHPBarBackground.Position.X + enemySpeed < enemyHPDestinationX)
                            {
                                EnemyHPBarBackground.Position.X += enemySpeed;
                                EnemyPokemonAssets.Name.OffsetX(enemySpeed);
                                if (EnemyPokemonAssets.Gender != null)
                                    EnemyPokemonAssets.Gender.OffsetX(enemySpeed);
                                EnemyPokemonAssets.Level.OffsetX(enemySpeed);
                                EnemyPokemonAssets.HPBar.Position.X += enemySpeed;
                            }
                            if (EnemyPokemon.Tint != Color.White)
                                EnemyPokemon.Tint = new Color(EnemyPokemon.Tint.R + 3, EnemyPokemon.Tint.G + 3, EnemyPokemon.Tint.B + 3, 255);
                            break;
                        }
                        EnemyHPBarBackground.Position.X = enemyHPDestinationX;
                        EnemyPokemonAssets.Name.SetPosition(new Vector2(EnemyHPBarBackground.Position.X + 24, EnemyHPBarBackground.Position.Y + 19));
                        if (EnemyPokemonAssets.Gender != null)
                            EnemyPokemonAssets.Gender.SetPosition(new Vector2(EnemyPokemonAssets.Name.Position.X + EnemyPokemonAssets.Name.SourceRect.Width, EnemyPokemonAssets.Name.Position.Y));
                        EnemyPokemonAssets.Level.SetPosition(new Vector2(EnemyHPBarBackground.Position.X + EnemyHPBarBackground.SourceRect.Width - 56 - EnemyPokemonAssets.Level.SourceRect.Width, EnemyPokemonAssets.Name.Position.Y));
                        EnemyPokemonAssets.HPBar.Position = new Vector2(EnemyHPBarBackground.Position.X + 156 - ((1 - EnemyPokemonAssets.HPBar.Scale.X) / 2 * EnemyPokemonAssets.HPBar.SourceRect.Width), EnemyHPBarBackground.Position.Y + 68);
                        
                        if (BattleScreen.TextBox.Page == 3 && !BattleScreen.TextBox.IsTransitioning)
                            state = BattleState.PLAYER_SEND_POKEMON;

                        break;
                    case BattleState.PLAYER_SEND_POKEMON:
                        float playerSpriteDestinationX = -PlayerSprite.SourceRect.Width - 8;

                        playerSpeed = (float)(0.6 * gameTime.ElapsedGameTime.TotalMilliseconds);

                        if (PlayerSprite.Position.X > 0)
                            PlayerSprite.SpriteSheetEffect.CurrentFrame.X = 1;
                        else if (PlayerSprite.Position.X <= 0 && PlayerSprite.Position.X > -PlayerSprite.SourceRect.Width / 6)
                            PlayerSprite.SpriteSheetEffect.CurrentFrame.X = 2;
                        else if (PlayerSprite.Position.X <= -PlayerSprite.SourceRect.Width / 6 && PlayerSprite.Position.X > -PlayerSprite.SourceRect.Width / 3)
                            PlayerSprite.SpriteSheetEffect.CurrentFrame.X = 3;
                        else
                        {
                            PlayerSprite.SpriteSheetEffect.CurrentFrame.X = 4;
                            state = BattleState.POKEMON_SEND_OUT;
                            Pokeball.LoadContent();
                            resetPokeball();

                        }
                        if (!(PlayerSprite.Position.X - playerSpeed < playerSpriteDestinationX) || Pokeball.Position.Y < BattleScreen.TextBox.Border.Position.Y)
                        {
                            PlayerSprite.Position.X -= (int)playerSpeed;
                            break;
                        }
                   
                        break;
                    case BattleState.POKEMON_SEND_OUT:

                        if (Pokeball.Alpha == 0)
                        {
                            Pokeball.Alpha = 1;
                        }

                        if (!whiteBackground.IsLoaded)
                            whiteBackground.LoadContent();

                        float ballMaxHeight = 296;
                        float ballSpeedX = (float)(0.2 * gameTime.ElapsedGameTime.TotalMilliseconds);

                        if (!maxHeight)
                        {
                            Pokeball.Position.Y -= pokeballSpeedY;
                            if (Pokeball.Position.Y <= ballMaxHeight)
                            {
                                maxHeight = true;
                                pokeballSpeedY = 1;
                            }
                        }
                        else
                        {
                            if (Pokeball.Position.Y < BattleScreen.TextBox.Border.Position.Y)
                            {
                                ballSpeedX = (float)(0.1 * gameTime.ElapsedGameTime.TotalMilliseconds);
                                Pokeball.Angle += 0.7f;
                                Pokeball.Position.Y += pokeballSpeedY;
                                pokeballSpeedY *= 1.2f;
                            }
                        }

                        Pokeball.Position.X += ballSpeedX;

                        if (PlayerSprite != null)
                        {
                            playerSpeed = (float)(0.6 * gameTime.ElapsedGameTime.TotalMilliseconds);
                            playerSpriteDestinationX = -PlayerSprite.SourceRect.Width - 8;
                            if (!(PlayerSprite.Position.X - playerSpeed < playerSpriteDestinationX) || Pokeball.Position.Y < BattleScreen.TextBox.Border.Position.Y)
                            {
                                PlayerSprite.Position.X -= (int)playerSpeed;
                                break;
                            }

                            PlayerSprite.UnloadContent();
                        }

                        if (Pokeball.Position.Y < BattleScreen.TextBox.Border.Position.Y) break;

                        if ((PlayerPokemon.Scale.X + 0.05f < 1 && PlayerPokemon.Scale.Y + 0.05f < 1) || !whiteBackgroundTransitioned)
                        {
                            whiteBackground.Alpha += 0.05f;

                            if (whiteBackground.Alpha >= 1)
                                whiteBackgroundTransitioned = true;

                            if (PlayerPokemon.Scale.X + 0.05f < 1 && PlayerPokemon.Scale.Y + 0.05f < 1)
                            {
                                PlayerPokemon.Scale = new Vector2(PlayerPokemon.Scale.X + 0.05f, PlayerPokemon.Scale.Y + 0.05f);
                                PlayerPokemon.Position = new Vector2(PlayerPlatform.Position.X + PlayerPlatform.SourceRect.Width * 0.55f - PlayerPokemon.SourceRect.Width / 2, PlayerPlatform.Position.Y + PlayerPlatform.SourceRect.Height - (int)(PlayerPokemon.SourceRect.Height * PlayerPokemon.Scale.Y));
                            }
                            break;
                        }

                        PlayerPokemon.Scale = Vector2.One;
                        PlayerPokemon.Position = new Vector2(PlayerPlatform.Position.X + PlayerPlatform.SourceRect.Width * 0.55f - PlayerPokemon.SourceRect.Width / 2, PlayerPlatform.Position.Y + PlayerPlatform.SourceRect.Height - PlayerPokemon.SourceRect.Height);
                        pokeOriginalY = PlayerPokemon.Position.Y;

                        float playerHPDestX = ScreenManager.Instance.Dimensions.X - PlayerHPBarBackground.SourceRect.Width - 40;
                        playerSpeed = (float)(1.2 * gameTime.ElapsedGameTime.TotalMilliseconds);
                        if (PlayerPokemon.Tint != Color.White || whiteBackground.Alpha > 0 || PlayerHPBarBackground.Position.X - playerSpeed > playerHPDestX)
                        {
                            if (PlayerHPBarBackground.Position.X - playerSpeed > playerHPDestX)
                            {
                                PlayerHPBarBackground.Position.X -= playerSpeed;
                                PlayerPokemonAssets.Name.OffsetX(-playerSpeed);
                                if (PlayerPokemonAssets.Gender != null)
                                    PlayerPokemonAssets.Gender.OffsetX(-playerSpeed);
                                PlayerPokemonAssets.Level.OffsetX(-playerSpeed);
                                PlayerPokemonAssets.MaxHP.OffsetX(-playerSpeed);
                                PlayerPokemonAssets.CurrentHP.OffsetX(-playerSpeed);
                                PlayerPokemonAssets.HPBar.Position.X -= playerSpeed;
                                EXPBar.Position.X -= playerSpeed;
                            }

                            if (whiteBackground.Alpha > 0)
                                whiteBackground.Alpha -= 0.05f;
                            PlayerPokemon.Tint = new Color(PlayerPokemon.Tint.R + 20, PlayerPokemon.Tint.G + 20, PlayerPokemon.Tint.B + 20, 255);
                            break;
                        }

                        PlayerHPBarBackground.Position.X = playerHPDestX;
                        PlayerPokemonAssets.Name.SetPosition(new Vector2(PlayerHPBarBackground.Position.X + 64, PlayerHPBarBackground.Position.Y + 19));
                        if (PlayerPokemonAssets.Gender != null)
                            PlayerPokemonAssets.Gender.SetPosition(new Vector2(PlayerPokemonAssets.Name.Position.X + PlayerPokemonAssets.Name.SourceRect.Width, PlayerPokemonAssets.Name.Position.Y));
                        PlayerPokemonAssets.Level.SetPosition(new Vector2(PlayerHPBarBackground.Position.X + PlayerHPBarBackground.SourceRect.Width - 36 - PlayerPokemonAssets.Level.SourceRect.Width, PlayerPokemonAssets.Name.Position.Y));
                        PlayerPokemonAssets.MaxHP.SetPosition(new Vector2(PlayerHPBarBackground.Position.X + PlayerHPBarBackground.SourceRect.Width - 36 - PlayerPokemonAssets.MaxHP.SourceRect.Width, PlayerHPBarBackground.Position.Y + 92));
                        PlayerPokemonAssets.CurrentHP.SetPosition(new Vector2(PlayerHPBarBackground.Position.X + PlayerHPBarBackground.SourceRect.Width - 116 - PlayerPokemonAssets.CurrentHP.SourceRect.Width, PlayerPokemonAssets.MaxHP.Position.Y));
                        PlayerPokemonAssets.HPBar.Position = new Vector2(PlayerHPBarBackground.Position.X + 192 - ((1 - PlayerPokemonAssets.HPBar.Scale.X) / 2 * PlayerPokemonAssets.HPBar.SourceRect.Width), PlayerHPBarBackground.Position.Y + 68);
                        EXPBar.Position = new Vector2(PlayerHPBarBackground.Position.X + 128 - ((1 - EXPBar.Scale.X) / 2 * EXPBar.SourceRect.Width), PlayerHPBarBackground.Position.Y + PlayerHPBarBackground.SourceRect.Height - 16);

                        if (PlayerSprite == null)
                        {
                            if (counter < 500)
                            {
                                counter += counterSpeed;
                                break;
                            }

                            counter = 0;

                        }

                        whiteBackground.Alpha = 0;
                        whiteBackground.UnloadContent();
                        resetPokeball();
                        Pokeball.UnloadContent();
                        IsTransitioning = false;

                        if (PlayerSprite != null)
                        {
                            BattleScreen.TextBox.NextPage = 4;
                            BattleScreen.TextBox.IsTransitioning = true;
                        } else
                            BattleScreen.BattleLogic.PlayerHasMoved = true;

                        break;
                    case BattleState.DAMAGE_ANIMATION:
                        if (!BattleScreen.TextBox.IsTransitioning)
                        {
                            bool player = BattleScreen.BattleLogic.State == BattleLogic.FightState.PLAYER_DEFEND;

                            if (blinkCounter < 4)
                            {
                                if (counter > 60)
                                {
                                    if (player)
                                    {
                                        if (PlayerPokemon.Alpha == 1)
                                            PlayerPokemon.Alpha = 0;
                                        else if (PlayerPokemon.Alpha == 0)
                                        {
                                            PlayerPokemon.Alpha = 1;
                                            blinkCounter++;
                                        }
                                    } else
                                    {
                                        if (EnemyPokemon.Alpha == 1)
                                            EnemyPokemon.Alpha = 0;
                                        else if (EnemyPokemon.Alpha == 0)
                                        {
                                            EnemyPokemon.Alpha = 1;
                                            blinkCounter++;
                                        }
                                    }
                                    counter = 0;
                                    if (blinkCounter == 4) break;
                                }
                                counter += counterSpeed;
                                break;
                            }


                            float goalScale = player ? (float)BattleLogic.Battle.PlayerPokemon.Pokemon.CurrentHP / BattleLogic.Battle.PlayerPokemon.Pokemon.Stats.HP : (float)BattleLogic.Battle.EnemyPokemon.Pokemon.CurrentHP / BattleLogic.Battle.EnemyPokemon.Pokemon.Stats.HP;
                            int goalHP = 0;
                            if (player)
                                goalHP = BattleLogic.Battle.PlayerPokemon.Pokemon.CurrentHP;
                            float speed = 0.01f;

                            if ((player && BattleLogic.Battle.PlayerPokemon.Pokemon.Stats.HP < 50) || BattleLogic.Battle.EnemyPokemon.Pokemon.Stats.HP < 50)
                                speed = 0.04f;
                            else if ((player && BattleLogic.Battle.PlayerPokemon.Pokemon.Stats.HP >= 100) || BattleLogic.Battle.EnemyPokemon.Pokemon.Stats.HP >= 100)
                                speed = 0.005f;


                            if (player)
                            {
                                if (PlayerPokemonAssets.HPBar.Scale.X - speed > goalScale)
                                {
                                    PlayerPokemonAssets.HPBar.Scale.X -= speed;
                                    PlayerPokemonAssets.CalculateHealthBarColor(PlayerPokemonAssets.HPBar.Scale.X);
                                    PlayerPokemonAssets.HPBar.Position = new Vector2(PlayerHPBarBackground.Position.X + 192 - ((1 - PlayerPokemonAssets.HPBar.Scale.X) / 2 * PlayerPokemonAssets.HPBar.SourceRect.Width), PlayerHPBarBackground.Position.Y + 68);
                                    PlayerPokemonAssets.CurrentHP.UnloadContent();
                                    PlayerPokemonAssets.CurrentHP.UpdateText(((int)(PlayerPokemonAssets.HPBar.Scale.X * BattleLogic.Battle.PlayerPokemon.Pokemon.Stats.HP)).ToString());
                                    PlayerPokemonAssets.CurrentHP.SetPosition(new Vector2(PlayerHPBarBackground.Position.X + PlayerHPBarBackground.SourceRect.Width - 116 - PlayerPokemonAssets.CurrentHP.SourceRect.Width, PlayerPokemonAssets.MaxHP.Position.Y));
                                    break;
                                }

                                PlayerPokemonAssets.CurrentHP.UnloadContent();
                                PlayerPokemonAssets.CurrentHP.UpdateText(goalHP.ToString());
                                PlayerPokemonAssets.CurrentHP.SetPosition(new Vector2(PlayerHPBarBackground.Position.X + PlayerHPBarBackground.SourceRect.Width - 116 - PlayerPokemonAssets.CurrentHP.SourceRect.Width, PlayerPokemonAssets.MaxHP.Position.Y));
                                PlayerPokemonAssets.HPBar.Scale.X = goalScale;
                                PlayerPokemonAssets.CalculateHealthBarColor(goalScale);
                                PlayerPokemonAssets.HPBar.Position = new Vector2(PlayerHPBarBackground.Position.X + 192 - ((1 - PlayerPokemonAssets.HPBar.Scale.X) / 2 * PlayerPokemonAssets.HPBar.SourceRect.Width), PlayerHPBarBackground.Position.Y + 68);

                            } else
                            {
                                if (EnemyPokemonAssets.HPBar.Scale.X - speed > goalScale)
                                {
                                    EnemyPokemonAssets.HPBar.Scale.X -= speed;
                                    EnemyPokemonAssets.CalculateHealthBarColor(EnemyPokemonAssets.HPBar.Scale.X);
                                    EnemyPokemonAssets.HPBar.Position = new Vector2(EnemyHPBarBackground.Position.X + 156 - ((1 - EnemyPokemonAssets.HPBar.Scale.X) / 2 * EnemyPokemonAssets.HPBar.SourceRect.Width), EnemyHPBarBackground.Position.Y + 68);
                                    break;
                                }

                                EnemyPokemonAssets.HPBar.Scale.X = goalScale;
                                EnemyPokemonAssets.CalculateHealthBarColor(goalScale);
                                EnemyPokemonAssets.HPBar.Position = new Vector2(EnemyHPBarBackground.Position.X + 156 - ((1 - EnemyPokemonAssets.HPBar.Scale.X) / 2 * EnemyPokemonAssets.HPBar.SourceRect.Width), EnemyHPBarBackground.Position.Y + 68);
                            }

                            

                            if (counter < 1000.0f)
                            {
                                counter += counterSpeed;
                                break;
                            }

                            if (BattleScreen.BattleLogic.Crit)
                            {
                                BattleScreen.TextBox.NextPage = 15;
                                BattleScreen.TextBox.IsTransitioning = true;
                                BattleScreen.BattleLogic.Crit = false;
                                counter = 0;
                                break;
                            }

                            if (BattleScreen.BattleLogic.SuperEffective)
                            {
                                BattleScreen.TextBox.NextPage = 7;
                                BattleScreen.TextBox.IsTransitioning = true;
                                BattleScreen.BattleLogic.SuperEffective = false;
                                counter = 0;
                                break;
                            }
                            else if (BattleScreen.BattleLogic.NotVeryEffective)
                            {
                                BattleScreen.TextBox.NextPage = 8;
                                BattleScreen.TextBox.IsTransitioning = true;
                                BattleScreen.BattleLogic.NotVeryEffective = false;
                                counter = 0;
                                break;
                            }

                            blinkCounter = 0;
                            counter = 0;

                            if (player)
                                BattleScreen.BattleLogic.EnemyHasMoved = true;
                            else
                                BattleScreen.BattleLogic.PlayerHasMoved = true;

                            if (BattleScreen.BattleLogic.EnemyHasMoved && BattleScreen.BattleLogic.PlayerHasMoved)
                                endFightSequence(BattleScreen);


                            if (player)
                            {
                                if (BattleLogic.Battle.PlayerPokemon.Pokemon.CurrentHP == 0)
                                {
                                    state = BattleState.PLAYER_POKEMON_FAINT;
                                    IsTransitioning = true;
                                    BattleScreen.BattleLogic.PokemonFainted = true;
                                }
                                else
                                {
                                    IsTransitioning = false;
                                    BattleScreen.BattleLogic.State = BattleLogic.FightState.NONE;
                                }
                            } else
                            {
                                if (BattleLogic.Battle.EnemyPokemon.Pokemon.CurrentHP == 0)
                                {
                                    state = BattleState.ENEMY_POKEMON_FAINT;
                                    IsTransitioning = true;
                                    BattleScreen.BattleLogic.PokemonFainted = true;
                                }
                                else
                                {
                                    IsTransitioning = false;
                                    BattleScreen.BattleLogic.State = BattleLogic.FightState.NONE;
                                }
                            }
                        }
                        break;
  
                    case BattleState.STATUS_ANIMATION:
                        if (!BattleScreen.TextBox.IsTransitioning)
                        {
                            if (BattleScreen.TextBox.Page == 18)
                            {
                                if (counter < 1000.0f)
                                {
                                    counter += counterSpeed;
                                    break;
                                }

                                if (BattleScreen.BattleLogic.PlayerMoveExecuted)
                                    BattleScreen.BattleLogic.PlayerHasMoved = true;

                                if (BattleScreen.BattleLogic.EnemyMoveExecuted)
                                    BattleScreen.BattleLogic.EnemyHasMoved = true;


                                if (BattleScreen.BattleLogic.EnemyHasMoved && BattleScreen.BattleLogic.PlayerHasMoved)
                                    endFightSequence(BattleScreen);

                                StatChangeAnimationImage1.Position = new Vector2(-StatChangeAnimationImage1.SourceRect.Width, 0);
                                StatChangeAnimationImage2.Position = new Vector2(-StatChangeAnimationImage2.SourceRect.Width, 0);
                                counter = 0;
                                IsTransitioning = false;
                                break;
                            }

                            if (BattleScreen.BattleLogic.StageMaxed)
                            {
                                if (counter < 1000.0f)
                                {
                                    counter += counterSpeed;
                                    break;
                                }

                                counter = 0;
                                BattleScreen.TextBox.IsTransitioning = true;
                                break;
                            }

                            Vector2 animationPos = BattleScreen.BattleLogic.State == BattleLogic.FightState.PLAYER_STATUS ? new Vector2(PlayerPokemon.Position.X, PlayerPokemon.Position.Y)
                                : new Vector2(EnemyPokemon.Position.X, EnemyPokemon.Position.Y);
                            Rectangle pokeSourceRect = BattleScreen.BattleLogic.State == BattleLogic.FightState.PLAYER_STATUS ? PlayerPokemon.SourceRect
                                : EnemyPokemon.SourceRect;
                            if (StatChangeAnimationImage1.Position.X != animationPos.X)
                            {
                                
                                StatChangeAnimationImage1.Position = animationPos;
                                StatChangeAnimationImage2.Position = BattleScreen.BattleLogic.StatStageIncrease ? new Vector2(animationPos.X, animationPos.Y + pokeSourceRect.Height)
                                    : animationPos;
                                StatChangeAnimationImage1.SourceRect.Height = pokeSourceRect.Height;
                                StatChangeAnimationImage1.SourceRect.Width = pokeSourceRect.Width;
                                StatChangeAnimationImage1.SourceRect.Location = Point.Zero;
                                StatChangeAnimationImage2.SourceRect.Height = BattleScreen.BattleLogic.StatStageIncrease ? 0 : pokeSourceRect.Height;
                                StatChangeAnimationImage2.SourceRect.Width = pokeSourceRect.Width;
                                StatChangeAnimationImage2.SourceRect.Location = Point.Zero;
                                StatChangeAnimationImage1.Alpha = 0.00001f;
                                StatChangeAnimationImage1.Alpha = 0.00001f;
                                reveal = BattleScreen.BattleLogic.StatStageIncrease ? pokeSourceRect.Height : 0;
                                spinCounter = 0;
                                increase = false;
                                StatChangeAnimationImage1.Tint = BattleScreen.BattleLogic.StatStageIncrease ? Color.OrangeRed : Color.LightBlue;
                                StatChangeAnimationImage2.Tint = BattleScreen.BattleLogic.StatStageIncrease ? Color.OrangeRed : Color.LightBlue;
                            }
                            float speed = (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.3f;
                            float alphaSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.0005f;
                            

                            if (StatChangeAnimationImage1.Alpha > 0)
                            {

                                if (spinCounter < 1000)
                                {
                                    if (StatChangeAnimationImage1.Alpha + alphaSpeed < 0.5f)
                                    {
                                        StatChangeAnimationImage1.Alpha += alphaSpeed;
                                        StatChangeAnimationImage2.Alpha += alphaSpeed;
                                    }
                                    else
                                    {
                                        StatChangeAnimationImage1.Alpha = 0.5f;
                                        StatChangeAnimationImage2.Alpha = 0.5f;
                                    }
                                }

                                if (increase)
                                {
                                    
                                    if (BattleScreen.BattleLogic.StatStageIncrease)
                                    {
                                        reveal -= (int)speed;
                                        StatChangeAnimationImage2.SourceRect.Height = pokeSourceRect.Height;
                                        StatChangeAnimationImage2.SourceRect.Location = new Point(0, pokeSourceRect.Height - reveal);
                                        StatChangeAnimationImage2.SourceRect.Height -= pokeSourceRect.Height - reveal;
                                        StatChangeAnimationImage1.SourceRect.Height += (int)speed;
                                        StatChangeAnimationImage1.Position.Y -= (int)speed;
                                    }
                                    else
                                    
                                    {
                                        reveal += (int)speed;
                                        StatChangeAnimationImage1.SourceRect.Height = pokeSourceRect.Height;
                                        StatChangeAnimationImage1.SourceRect.Location = new Point(0, pokeSourceRect.Height - reveal);
                                        StatChangeAnimationImage1.SourceRect.Height -= pokeSourceRect.Height - reveal;
                                        StatChangeAnimationImage2.SourceRect.Height -= (int)speed;
                                        StatChangeAnimationImage2.Position.Y += (int)speed;
                                    }
                                }
                                else
                                {
                                    
                                    if (BattleScreen.BattleLogic.StatStageIncrease)
                                    {
                                        reveal -= (int)speed;
                                        StatChangeAnimationImage1.SourceRect.Height = pokeSourceRect.Height;
                                        StatChangeAnimationImage1.SourceRect.Location = new Point(0, pokeSourceRect.Height - reveal);
                                        StatChangeAnimationImage1.SourceRect.Height -= pokeSourceRect.Height - reveal;
                                        StatChangeAnimationImage2.SourceRect.Height += (int)speed;
                                        StatChangeAnimationImage2.Position.Y -= (int)speed;
                                    }
                                    else
                                    
                                    {
                                        reveal += (int)speed;
                                        StatChangeAnimationImage2.SourceRect.Height = pokeSourceRect.Height;
                                        StatChangeAnimationImage2.SourceRect.Location = new Point(0, pokeSourceRect.Height - reveal);
                                        StatChangeAnimationImage2.SourceRect.Height -= pokeSourceRect.Height - reveal;
                                        StatChangeAnimationImage1.SourceRect.Height -= (int)speed;
                                        StatChangeAnimationImage1.Position.Y += (int)speed;
                                    }
                                }



                                if (StatChangeAnimationImage1.SourceRect.Height <= 0)
                                {
                                    increase = true;
                                    StatChangeAnimationImage1.Position = BattleScreen.BattleLogic.StatStageIncrease ?
                                        new Vector2(animationPos.X, animationPos.Y + pokeSourceRect.Height) : animationPos;
                                    StatChangeAnimationImage1.SourceRect.Location = Point.Zero;
                                    StatChangeAnimationImage2.Position.Y = animationPos.Y;
                                    reveal = BattleScreen.BattleLogic.StatStageIncrease ? pokeSourceRect.Height : 0;
                                } else if (StatChangeAnimationImage2.SourceRect.Height <= 0)
                                {
                                    increase = false;
                                    StatChangeAnimationImage1.Position.Y = animationPos.Y;
                                    StatChangeAnimationImage2.Position = BattleScreen.BattleLogic.StatStageIncrease ?
                                        new Vector2(animationPos.X, animationPos.Y + pokeSourceRect.Height) : animationPos;
                                    StatChangeAnimationImage2.SourceRect.Location = Point.Zero;
                                    reveal = BattleScreen.BattleLogic.StatStageIncrease ? pokeSourceRect.Height : 0;
                                }

                                spinCounter += counterSpeed;

                                if (spinCounter > 1000)
                                {
                                    if (StatChangeAnimationImage1.Alpha - alphaSpeed > 0)
                                    {
                                        StatChangeAnimationImage1.Alpha -= alphaSpeed;
                                        StatChangeAnimationImage2.Alpha -= alphaSpeed;
                                    }
                                    else
                                    {
                                        StatChangeAnimationImage1.Alpha = 0;
                                        StatChangeAnimationImage2.Alpha = 0;
                                    }
                                }

                                break;
                            }

                            spinCounter = 0;

                            BattleScreen.TextBox.IsTransitioning = true;
                        }
                        break;
                    case BattleState.ENEMY_POKEMON_FAINT:
                        //TODO: REPLACE WITH SOUND ENDING TO TRIGGER INSTEAD OF COUNTER

                        if (counter < 1000.0f)
                        {
                            counter += counterSpeed;
                            break;
                        }

                        if (EnemyPokemon.SourceRect.Height - 12 > 0)
                        {
                            EnemyPokemon.SourceRect.Height -= 12;
                            EnemyPokemon.Position = new Vector2(EnemyPlatform.Position.X + EnemyPlatform.SourceRect.Width / 2 - EnemyPokemon.SourceRect.Width / 2, EnemyPlatform.Position.Y + EnemyPlatform.SourceRect.Height * 0.75f - EnemyPokemon.SourceRect.Height);
                            break;
                        }
                        EnemyPokemon.SourceRect.Height = 0;

                        IsTransitioning = false;

                        BattleScreen.TextBox.NextPage = 9;
                        BattleScreen.TextBox.IsTransitioning = true;
                        counter = 0;

                        // TODO: TEXTBOX FAINT MESSAGE WITH ARROW (CHECK WILD VS TRAINER FOR SPECIFIC MESSAGE)
                        // - AFTER CLICKING PAST ARROW, GO TO GAMEPLAY SCREEN

                        break;
                    case BattleState.PLAYER_POKEMON_FAINT:
                        //TODO: REPLACE WITH SOUND ENDING TO TRIGGER INSTEAD OF COUNTER

                        if (counter < 1000.0f)
                        {
                            counter += counterSpeed;
                            break;
                        }

                        if (PlayerPokemon.SourceRect.Height - 16 > 0)
                        {
                            PlayerPokemon.SourceRect.Height -= 16;
                            PlayerPokemon.Position = new Vector2(PlayerPlatform.Position.X + PlayerPlatform.SourceRect.Width * 0.55f - PlayerPokemon.SourceRect.Width / 2, PlayerPlatform.Position.Y + PlayerPlatform.SourceRect.Height - PlayerPokemon.SourceRect.Height);
                            break;
                        }
                        PlayerPokemon.SourceRect.Height = 0;
                        IsTransitioning = false;

                        BattleScreen.TextBox.NextPage = 9;
                        BattleScreen.TextBox.IsTransitioning = true;
                        counter = 0;
                        // TODO: TEXTBOX FAINT MESSAGE WITH ARROW (CHECK WILD VS TRAINER FOR SPECIFIC MESSAGE)
                        // - AFTER CLICKING PAST ARROW, GO TO GAMEPLAY SCREEN
                        break;
                    case BattleState.EXP_ANIMATION:
                        int goalLevel = BattleLogic.Battle.PlayerPokemon.Pokemon.Level;
                        float goalEXPScale = (float)BattleLogic.Battle.PlayerPokemon.Pokemon.EXPTowardsLevelUp / BattleLogic.Battle.PlayerPokemon.Pokemon.EXPNeededToLevelUp;

                        if (EXPBar.Scale.X + 0.01f < goalEXPScale || (EXPBar.Scale.X + 0.01f < 1 && int.Parse(PlayerPokemonAssets.Level.Text.Text[2..]) < goalLevel))
                        {
                            EXPBar.Scale.X += 0.01f;
                            EXPBar.Position = new Vector2(PlayerHPBarBackground.Position.X + 128 - ((1 - EXPBar.Scale.X) / 2 * EXPBar.SourceRect.Width), PlayerHPBarBackground.Position.Y + PlayerHPBarBackground.SourceRect.Height - 16);
                            break;
                        }
                        
                        if (EXPBar.Scale.X + 0.01f >= 1)
                        {
                            EXPBar.Scale.X = 1;
                            EXPBar.Position = new Vector2(PlayerHPBarBackground.Position.X + 128 - ((1 - EXPBar.Scale.X) / 2 * EXPBar.SourceRect.Width), PlayerHPBarBackground.Position.Y + PlayerHPBarBackground.SourceRect.Height - 16);
                            state = BattleState.LEVEL_UP_ANIMATION;
                            break;
                        }
                        else
                        {
                            EXPBar.Scale.X = goalEXPScale;
                            EXPBar.Position = new Vector2(PlayerHPBarBackground.Position.X + 128 - ((1 - EXPBar.Scale.X) / 2 * EXPBar.SourceRect.Width), PlayerHPBarBackground.Position.Y + PlayerHPBarBackground.SourceRect.Height - 16);
                        }

                        if (counter < 1000.0f)
                        {
                            counter += counterSpeed;
                            break;
                        }

                        counter = 0;

                        if (BattleLogic.Battle.IsWild)
                        {
                            ScreenManager.Instance.ChangeScreens("GameplayScreen");
                            BattleLogic.EndBattle();
                        }
                        BattleScreen.BattleLogic.LevelUp = false;
                        IsTransitioning = false;
                        break;
                    case BattleState.LEVEL_UP_ANIMATION:
                        if (!levelUpTransitioned)
                        {
                            PlayerHPBarLevelUp.Alpha += 0.1f;

                            if (PlayerHPBarLevelUp.Alpha >= 1)
                                levelUpTransitioned = true;

                            break;
                        }
                        else if (PlayerHPBarLevelUp.Alpha > 0)
                        {
                            PlayerHPBarLevelUp.Alpha -= 0.1f;
                            break;
                        }

                        PlayerHPBarLevelUp.Alpha = 0;

                        PlayerPokemonAssets.Level.UpdateText("Lv" + (int.Parse(PlayerPokemonAssets.Level.Text.Text[2..]) + 1).ToString());

                        int level = int.Parse(PlayerPokemonAssets.Level.Text.Text[2..]);
                        int newCurrentHP = int.Parse(PlayerPokemonAssets.CurrentHP.Text.Text) + (PokemonManager.Instance.StatsOfLevel(BattleLogic.Battle.PlayerPokemon.Pokemon, level).HP - PokemonManager.Instance.StatsOfLevel(BattleLogic.Battle.PlayerPokemon.Pokemon, level - 1).HP);
                        PlayerPokemonAssets.CurrentHP.UpdateText(newCurrentHP.ToString());
                        PlayerPokemonAssets.MaxHP.UpdateText(PokemonManager.Instance.StatsOfLevel(BattleLogic.Battle.PlayerPokemon.Pokemon, level).HP.ToString());

                        PlayerPokemonAssets.Level.SetPosition(new Vector2(PlayerHPBarBackground.Position.X + PlayerHPBarBackground.SourceRect.Width - 36 - PlayerPokemonAssets.Level.SourceRect.Width, PlayerPokemonAssets.Name.Position.Y));
                        PlayerPokemonAssets.MaxHP.SetPosition(new Vector2(PlayerHPBarBackground.Position.X + PlayerHPBarBackground.SourceRect.Width - 36 - PlayerPokemonAssets.MaxHP.SourceRect.Width, PlayerHPBarBackground.Position.Y + 92));
                        PlayerPokemonAssets.CurrentHP.SetPosition(new Vector2(PlayerHPBarBackground.Position.X + PlayerHPBarBackground.SourceRect.Width - 116 - PlayerPokemonAssets.CurrentHP.SourceRect.Width, PlayerPokemonAssets.MaxHP.Position.Y));

                        float healthScale = (float)newCurrentHP / PokemonManager.Instance.StatsOfLevel(BattleLogic.Battle.PlayerPokemon.Pokemon, level).HP;
                        PlayerPokemonAssets.HPBar.Scale.X = healthScale;
                        PlayerPokemonAssets.CalculateHealthBarColor(healthScale);
                        PlayerPokemonAssets.HPBar.Position = new Vector2(PlayerHPBarBackground.Position.X + 192 - ((1 - PlayerPokemonAssets.HPBar.Scale.X) / 2 * PlayerPokemonAssets.HPBar.SourceRect.Width), PlayerHPBarBackground.Position.Y + 68);

                        EXPBar.Scale.X = 0;
                        levelUpTransitioned = false;

                        IsTransitioning = false;
                        BattleScreen.TextBox.NextPage = 17;
                        BattleScreen.TextBox.IsTransitioning = true;
                        break;
                    case BattleState.POKEMON_SWITCH:
                        if (!BattleScreen.TextBox.IsTransitioning)
                        {
                            if (BattleScreen.TextBox.Page != 3)
                            {
                                if (!whiteBackground.IsLoaded)
                                    whiteBackground.LoadContent();

                                if (PlayerPokemon.Tint != Color.Red)
                                {
                                    PlayerPokemon.Tint = new Color(PlayerPokemon.Tint.R, PlayerPokemon.Tint.G - 20, PlayerPokemon.B - 20, 255);
                                    whiteBackground.Alpha += 0.0784f;
                                    break;
                                }
                                PlayerPokemon.Tint = Color.Red;

                                if (whiteBackground.Alpha < 1 && !whiteBackgroundTransitioned)
                                {
                                    whiteBackground.Alpha += 0.0784f;
                                    break;
                                }
                                whiteBackgroundTransitioned = true;

                                if ((PlayerPokemon.Scale.X - 0.05f > 0 && PlayerPokemon.Scale.Y - 0.05f > 0))
                                {
                                    whiteBackground.Alpha -= 0.05f;

                                    if (PlayerPokemon.Scale.X - 0.05f > 0 && PlayerPokemon.Scale.Y - 0.05f > 0)
                                    {
                                        PlayerPokemon.Scale = new Vector2(PlayerPokemon.Scale.X - 0.05f, PlayerPokemon.Scale.Y - 0.05f);
                                        PlayerPokemon.Position = new Vector2(PlayerPlatform.Position.X + PlayerPlatform.SourceRect.Width * 0.55f - PlayerPokemon.SourceRect.Width / 2, PlayerPlatform.Position.Y + PlayerPlatform.SourceRect.Height - (int)(PlayerPokemon.SourceRect.Height * PlayerPokemon.Scale.Y));
                                    }
                                    break;
                                }

                                if (whiteBackgroundTransitioned && whiteBackground.Alpha - 0.05f > 0)
                                {
                                    whiteBackground.Alpha -= 0.05f;
                                    break;
                                }
                                if (whiteBackground.Alpha != 0)
                                    whiteBackground.Alpha = 0;

            

                                if (PlayerPokemon.Scale != Vector2.Zero)
                                {
                                    BattleLogic.Battle.UpdatePlayerPokemon();

                                    PlayerPokemon.Scale = Vector2.Zero;
                                    PlayerPokemon.Position = new Vector2(PlayerPlatform.Position.X + PlayerPlatform.SourceRect.Width * 0.55f - PlayerPokemon.SourceRect.Width / 2, PlayerPlatform.Position.Y + PlayerPlatform.SourceRect.Height - PlayerPokemon.SourceRect.Height);
                                    pokeOriginalY = PlayerPokemon.Position.Y;
                                    PlayerPokemon.UnloadContent();

                                    PlayerPokemon = BattleLogic.Battle.PlayerPokemon.Pokemon.Pokemon.Back;
                                    PlayerPokemon.Scale = Vector2.Zero;
                                    PlayerPokemon.LoadContent();
                                    PlayerPokemon.Tint = Color.Red;

                                    PlayerPokemonAssets.UnloadContent();
                                    PlayerPokemonAssets = new PokemonAssets(BattleLogic.Battle.PlayerPokemon.Pokemon, true);
                                    PlayerPokemonAssets.ScaleEXPBar(EXPBar);
                                    PlayerPokemonAssets.LoadContent("Fonts/PokemonFireRedSmall", new Color(81, 81, 81, 255), new Color(224, 219, 192, 255));
                                    setDefaultBattleImagePositions(BattleScreen.TextBox);
                                    PlayerHPBarBackground.Position = new Vector2(ScreenManager.Instance.Dimensions.X, BattleScreen.TextBox.Border.Position.Y - PlayerHPBarBackground.SourceRect.Height - 4);
                                    setAssetPositions();
                                }

                                
                                
                                if (counter < 100.0f)
                                {
                                    counter += counterSpeed;
                                    break;
                                }

                                counter = 0;
                                
                                BattleScreen.TextBox.NextPage = 3;
                                BattleScreen.TextBox.IsTransitioning = true;
                                whiteBackgroundTransitioned = false;
                            }
                            else
                            {
                                state = BattleState.POKEMON_SEND_OUT;
                                Pokeball.LoadContent();
                                resetPokeball();
                            }
                            
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        bool barBounce;
        float pokeBounceTimer = 0.2f;
        float barBounceTimer = 0.3f;
        float pokeOriginalY;
        float barOriginalY;
        float pokeNameOriginalY;
        float pokeHPOriginalY;
        float pokeHealthBarOriginalY;
        float pokeEXPOriginalY;


        private void animateBattleMenu(GameTime gameTime)
        {
            pokeBounceTimer -= (float) gameTime.ElapsedGameTime.TotalSeconds;
            barBounceTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (barBounceTimer < 0)
            {
                barBounce = !barBounce;
                float yOffset = barBounce ? 4 : -4;
                PlayerHPBarBackground.Position.Y += yOffset;
                PlayerPokemonAssets.Name.OffsetY(yOffset);
                if (PlayerPokemonAssets.Gender != null)
                    PlayerPokemonAssets.Gender.OffsetY(yOffset);
                PlayerPokemonAssets.Level.OffsetY(yOffset);
                PlayerPokemonAssets.MaxHP.OffsetY(yOffset);
                PlayerPokemonAssets.CurrentHP.OffsetY(yOffset);
                PlayerPokemonAssets.HPBar.Position.Y += yOffset;
                EXPBar.Position.Y += yOffset;
                barBounceTimer = 0.3f;
            }

            if (pokeBounceTimer < 0)
            {
                PlayerPokemon.Position.Y += barBounce ? -4 : 4;
                pokeBounceTimer = 0.3f;
            }

        }

        // when battle menu option is selected
        public void Reset()
        {
            barBounce = false;
            barBounceTimer = 0.3f;
            pokeBounceTimer = 0.2f;
            maxHeight = false;
            whiteBackgroundTransitioned = false;
            pokeballSpeedY = 4f;
            PlayerPokemon.Position.Y = pokeOriginalY;
            PlayerHPBarBackground.Position.Y = barOriginalY;
            PlayerPokemonAssets.HPBar.Position.Y = pokeHealthBarOriginalY;
            PlayerPokemonAssets.Name.SetY(pokeNameOriginalY);
            PlayerPokemonAssets.Level.SetY(pokeNameOriginalY);
            if (PlayerPokemonAssets.Gender != null)
                PlayerPokemonAssets.Gender.SetY(pokeNameOriginalY);
            EXPBar.Position.Y = pokeEXPOriginalY;
            PlayerPokemonAssets.CurrentHP.SetY(pokeHPOriginalY);
            PlayerPokemonAssets.MaxHP.SetY(pokeHPOriginalY);
            counter = 0;
        }

        void resetPokeball()
        {
            pokeballSpeedY = 4;
            Pokeball.Alpha = 0;
            Pokeball.Position = new Vector2(156, 352);
        }


        public void LoadContent()
        {
            // TODO: Load Background based on what environment the battle is in

            //Load battle images
            loadBattleContent(BattleLogic.Battle.PlayerPokemon.Pokemon, BattleLogic.Battle.EnemyPokemon.Pokemon);
            if (FromMenu)
            {
                state = BattleState.BATTLE_MENU;
                BattleScreen.TextBox.NextPage = 4;
                BattleScreen.TextBox.UpdateDialogue = true;
                BattleScreen.TextBox.IsTransitioning = true;
                PlayerPokemon.LoadContent();
                setDefaultBattleImagePositions(BattleScreen.TextBox);
            }
            else
            {
                loadSprites();
                PlayerSprite.SpriteSheetEffect.AmountOfFrames = new Vector2(5, 1);
                PlayerSprite.SpriteSheetEffect.CurrentFrame = Vector2.Zero;

                state = BattleState.INTRO;

                if (BattleLogic.Battle.IsWild)
                    EnemyPokemon.Tint = Color.LightGray;

                setIntroBattleImagePositions(BattleScreen.TextBox);


                barOriginalY = PlayerHPBarBackground.Position.Y;
                IsTransitioning = true;

                PlayerPokemon.Scale = new Vector2(0.0f, 0.0f);
                PlayerPokemon.LoadContent();
                PlayerPokemon.Tint = Color.Red;
                FromMenu = true;
            }

            setAssetPositions();
        }

        public void UnloadContent()
        {
            Background.UnloadContent();
            EnemyPlatform.UnloadContent();
            EnemyPokemon.UnloadContent();
            PlayerPlatform.UnloadContent();
            PlayerPokemon.UnloadContent();
            PlayerHPBarBackground.UnloadContent();
            EnemyHPBarBackground.UnloadContent();
            PlayerHPBarLevelUp.UnloadContent();
            StatChangeAnimationImage1.UnloadContent();
            StatChangeAnimationImage2.UnloadContent();
            PlayerPokemonAssets.UnloadContent();
            EnemyPokemonAssets.UnloadContent();
            EXPBar.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            Transition(gameTime);
            if (state == BattleState.INTRO || state == BattleState.WILD_POKEMON_FADE_IN || state == BattleState.PLAYER_SEND_POKEMON || (PlayerSprite != null && state == BattleState.POKEMON_SEND_OUT))
                PlayerSprite.Update(gameTime);
            if (state == BattleState.BATTLE_MENU)
                animateBattleMenu(gameTime);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Background.Draw(spriteBatch);
            PlayerPlatform.Draw(spriteBatch);
            EnemyPlatform.Draw(spriteBatch);
            //if ((state == BattleState.PLAYER_SEND_POKEMON && PlayerSprite.SpriteSheetEffect.CurrentFrame.X == 4) || state == BattleState.POKEMON_SWITCH)
            if (state == BattleState.POKEMON_SWITCH || state == BattleState.POKEMON_SEND_OUT)
            {
                whiteBackground.Draw(spriteBatch);
            }
            PlayerHPBarBackground.Draw(spriteBatch);
            if (state == BattleState.LEVEL_UP_ANIMATION)
                PlayerHPBarLevelUp.Draw(spriteBatch);
            EnemyHPBarBackground.Draw(spriteBatch);

            PlayerPokemonAssets.Draw(spriteBatch);
            EnemyPokemonAssets.Draw(spriteBatch);
            EXPBar.Draw(spriteBatch);

            if (state == BattleState.INTRO || state == BattleState.WILD_POKEMON_FADE_IN || state == BattleState.PLAYER_SEND_POKEMON || (PlayerSprite != null && state == BattleState.POKEMON_SEND_OUT))
                PlayerSprite.Draw(spriteBatch);

            PlayerPokemon.Draw(spriteBatch);


            EnemyPokemon.Draw(spriteBatch);

            if (state == BattleState.STATUS_ANIMATION)
            {
                StatChangeAnimationImage1.Draw(spriteBatch);
                StatChangeAnimationImage2.Draw(spriteBatch);
            }

            //if ((state == BattleState.PLAYER_SEND_POKEMON && PlayerSprite.SpriteSheetEffect.CurrentFrame.X == 4) || state == BattleState.POKEMON_SWITCH && Pokeball.IsLoaded)
            if (state == BattleState.POKEMON_SEND_OUT)
            {
                Pokeball.Draw(spriteBatch);
            }
        }

        void loadSprites()
        {
            PlayerSprite = new Image
            {
                Path = Player.PlayerJsonObject.Gender == Gender.MALE ? "BattleScreen/BattleRedSpriteSheet" : "BattleScreen/BattleBackground1",
                Effects = "SpriteSheetEffect"
            };
            PlayerSprite.LoadContent();
        }


        void loadBattleContent(CustomPokemon playerPokemon, CustomPokemon enemyPokemon)
        {
            // Battle assets
            Background = new Image();
            EnemyPlatform = new Image();
            PlayerPlatform = new Image();
            EnemyPokemon = new Image();
            PlayerPokemon = playerPokemon.Pokemon.Back;

            // Battle assets
            Background.Path = "BattleScreen/BattleBackground1";
            EnemyPlatform.Path = "BattleScreen/BattleBackground1EnemyPlatform";
            PlayerPlatform.Path = "BattleScreen/BattleBackground1PlayerPlatform";
            EnemyPokemon = enemyPokemon.Pokemon.Front;

            // HP Bar assets
            PlayerPokemonAssets = new PokemonAssets(playerPokemon, true);
            EnemyPokemonAssets = new PokemonAssets(enemyPokemon, false);

            // Handle Health and exp bars
            PlayerPokemonAssets.ScaleEXPBar(EXPBar);

            // Battle assets
            Background.LoadContent();
            EnemyPlatform.LoadContent();
            EnemyPokemon.LoadContent();
            PlayerPlatform.LoadContent();
            //Pokeball.LoadContent();
            PlayerHPBarBackground.LoadContent();
            EnemyHPBarBackground.LoadContent();
            PlayerHPBarLevelUp.LoadContent();
            StatChangeAnimationImage1.LoadContent();
            StatChangeAnimationImage2.LoadContent();
            PlayerHPBarLevelUp.Alpha = 0;
            StatChangeAnimationImage1.Alpha = 0;
            StatChangeAnimationImage2.Alpha = 0;

            // HP Bar assets
            PlayerPokemonAssets.LoadContent("Fonts/PokemonFireRedSmall", new Color(81, 81, 81, 255), new Color(224, 219, 192, 255));
            EnemyPokemonAssets.LoadContent("Fonts/PokemonFireRedSmall", new Color(81, 81, 81, 255), new Color(224, 219, 192, 255));
            EXPBar.LoadContent();


            /* POKEBALL TRANSITION CODE */
            whiteBackground = new Image
            {
                Texture = new Texture2D(ScreenManager.Instance.GraphicsDevice, Background.SourceRect.Width, Background.SourceRect.Height)
            };
            //whiteBackground.LoadContent();
            Color[] data = new Color[Background.SourceRect.Width * Background.SourceRect.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.White;
            whiteBackground.Texture.SetData(data);
            whiteBackground.Alpha = 0;
            /* END POKEBALL TRANSITION CODE */
        }

        private void setIntroBattleImagePositions(TextBox textBox)
        {
            // set battle image positions
            EnemyPlatform.Position = new Vector2(-EnemyPlatform.SourceRect.Width, 192);
            EnemyPokemon.Position = new Vector2(EnemyPlatform.Position.X + EnemyPlatform.SourceRect.Width / 2 - EnemyPokemon.SourceRect.Width / 2, EnemyPlatform.Position.Y + EnemyPlatform.SourceRect.Height * 0.75f - EnemyPokemon.SourceRect.Height);
            PlayerPlatform.Position = new Vector2(ScreenManager.Instance.Dimensions.X + PlayerPlatform.SourceRect.Width, textBox.Border.Position.Y - PlayerPlatform.SourceRect.Height);
            PlayerSprite.Position = new Vector2(PlayerPlatform.Position.X + PlayerPlatform.SourceRect.Width / 2 - 48, PlayerPlatform.Position.Y + PlayerPlatform.SourceRect.Height - PlayerSprite.SourceRect.Height);
            EnemyHPBarBackground.Position = new Vector2(-EnemyHPBarBackground.SourceRect.Width, EnemyPlatform.Position.Y - EnemyHPBarBackground.SourceRect.Height - 12);
            PlayerHPBarBackground.Position = new Vector2(ScreenManager.Instance.Dimensions.X, textBox.Border.Position.Y - PlayerHPBarBackground.SourceRect.Height - 4);
            PlayerHPBarLevelUp.Position = new Vector2(ScreenManager.Instance.Dimensions.X - PlayerHPBarBackground.SourceRect.Width - 40, PlayerHPBarBackground.Position.Y);
        }

        void setDefaultBattleImagePositions(TextBox textBox)
        {
            EnemyPlatform.Position = new Vector2(ScreenManager.Instance.Dimensions.X - EnemyPlatform.SourceRect.Width, 192);
            EnemyPokemon.Position = new Vector2(EnemyPlatform.Position.X + EnemyPlatform.SourceRect.Width / 2 - EnemyPokemon.SourceRect.Width / 2, EnemyPlatform.Position.Y + EnemyPlatform.SourceRect.Height * 0.75f - EnemyPokemon.SourceRect.Height);
            PlayerPlatform.Position = new Vector2(16, textBox.Border.Position.Y - PlayerPlatform.SourceRect.Height);
            EnemyHPBarBackground.Position = new Vector2(52, EnemyPlatform.Position.Y - EnemyHPBarBackground.SourceRect.Height - 12);
            PlayerHPBarBackground.Position = new Vector2(ScreenManager.Instance.Dimensions.X - PlayerHPBarBackground.SourceRect.Width - 40, textBox.Border.Position.Y - PlayerHPBarBackground.SourceRect.Height - 4);
            PlayerHPBarLevelUp.Position = PlayerHPBarBackground.Position;
            PlayerPokemon.Position = new Vector2(PlayerPlatform.Position.X + PlayerPlatform.SourceRect.Width * 0.55f - PlayerPokemon.SourceRect.Width / 2, PlayerPlatform.Position.Y + PlayerPlatform.SourceRect.Height - PlayerPokemon.SourceRect.Height);
            pokeOriginalY = PlayerPokemon.Position.Y;
            barOriginalY = PlayerHPBarBackground.Position.Y;
        }

        void setAssetPositions()
        {
            StatChangeAnimationImage1.Position = new Vector2(ScreenManager.Instance.Dimensions.X - StatChangeAnimationImage1.SourceRect.Width, 0);
            StatChangeAnimationImage2.Position = new Vector2(ScreenManager.Instance.Dimensions.X - StatChangeAnimationImage2.SourceRect.Width, 0);

            // set hp bar image positions
            PlayerPokemonAssets.Name.SetPosition(new Vector2(PlayerHPBarBackground.Position.X + 64, PlayerHPBarBackground.Position.Y + 19));
            if (PlayerPokemonAssets.Gender != null)
                PlayerPokemonAssets.Gender.SetPosition(new Vector2(PlayerPokemonAssets.Name.Position.X + PlayerPokemonAssets.Name.SourceRect.Width, PlayerPokemonAssets.Name.Position.Y));
            PlayerPokemonAssets.Level.SetPosition(new Vector2(PlayerHPBarBackground.Position.X + PlayerHPBarBackground.SourceRect.Width - 36 - PlayerPokemonAssets.Level.SourceRect.Width, PlayerPokemonAssets.Name.Position.Y));
            PlayerPokemonAssets.MaxHP.SetPosition(new Vector2(PlayerHPBarBackground.Position.X + PlayerHPBarBackground.SourceRect.Width - 36 - PlayerPokemonAssets.MaxHP.SourceRect.Width, PlayerHPBarBackground.Position.Y + 92));
            PlayerPokemonAssets.CurrentHP.SetPosition(new Vector2(PlayerHPBarBackground.Position.X + PlayerHPBarBackground.SourceRect.Width - 116 - PlayerPokemonAssets.CurrentHP.SourceRect.Width, PlayerPokemonAssets.MaxHP.Position.Y));
            PlayerPokemonAssets.HPBar.Position = new Vector2(PlayerHPBarBackground.Position.X + 192 - ((1 - PlayerPokemonAssets.HPBar.Scale.X) / 2 * PlayerPokemonAssets.HPBar.SourceRect.Width), PlayerHPBarBackground.Position.Y + 68);
            EXPBar.Position = new Vector2(PlayerHPBarBackground.Position.X + 128 - ((1 - EXPBar.Scale.X) / 2 * EXPBar.SourceRect.Width), PlayerHPBarBackground.Position.Y + PlayerHPBarBackground.SourceRect.Height - 16);
            pokeNameOriginalY = PlayerPokemonAssets.Name.Position.Y;
            pokeHPOriginalY = PlayerPokemonAssets.MaxHP.Position.Y;
            pokeHealthBarOriginalY = PlayerPokemonAssets.HPBar.Position.Y;
            pokeEXPOriginalY = EXPBar.Position.Y;

            EnemyPokemonAssets.Name.SetPosition(new Vector2(EnemyHPBarBackground.Position.X + 24, EnemyHPBarBackground.Position.Y + 19));
            if (EnemyPokemonAssets.Gender != null)
                EnemyPokemonAssets.Gender.SetPosition(new Vector2(EnemyPokemonAssets.Name.Position.X + EnemyPokemonAssets.Name.SourceRect.Width, EnemyPokemonAssets.Name.Position.Y));
            EnemyPokemonAssets.Level.SetPosition(new Vector2(EnemyHPBarBackground.Position.X + EnemyHPBarBackground.SourceRect.Width - 56 - EnemyPokemonAssets.Level.SourceRect.Width, EnemyPokemonAssets.Name.Position.Y));
            EnemyPokemonAssets.HPBar.Position = new Vector2(EnemyHPBarBackground.Position.X + 156 - ((1 - EnemyPokemonAssets.HPBar.Scale.X) / 2 * EnemyPokemonAssets.HPBar.SourceRect.Width), EnemyHPBarBackground.Position.Y + 68);
        }

        private void endFightSequence(BattleScreen BattleScreen)
        {
            BattleScreen.menuManager.menuName = "BattleMenu";
            BattleScreen.menuManager.menu.ID = "Load/Menus/BattleMenu.xml";
            BattleScreen.TextBox.NextPage = 4;
            BattleScreen.TextBox.IsTransitioning = true;
            BattleScreen.BattleLogic.EnemyHasMoved = false;
            BattleScreen.BattleLogic.PlayerHasMoved = false;
            BattleScreen.BattleLogic.PlayerMoveUsed = false;
            BattleScreen.BattleLogic.StatStageIncrease = false;
            BattleScreen.BattleLogic.Stat = "";
            BattleScreen.BattleLogic.SharplyStat = false;
            BattleScreen.BattleLogic.PlayerMoveExecuted = false;
            BattleScreen.BattleLogic.EnemyMoveExecuted = false;
            BattleScreen.BattleLogic.State = BattleLogic.FightState.NONE;
            state = BattleState.BATTLE_MENU;
        }

    }
}
