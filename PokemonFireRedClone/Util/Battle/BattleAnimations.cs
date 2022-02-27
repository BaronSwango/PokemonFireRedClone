using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class BattleAnimations
    {

        public enum BattleState
        {
            INTRO,
            WILD_POKEMON_FADE_IN,
            PLAYER_SEND_POKEMON,
            ENEMY_SEND_POKEMON,
            BATTLE_MENU,
            PLAYER_MOVE,
            ENEMY_MOVE,
            PLAYER_SWITCH,
            ENEMY_SWITCH,
            THROW_POKEBALL,
            PLAYER_POKEMON_FAINT,
            ENEMY_POKEMON_FAINT
        }

        // General battle screen data
        Image Background;
        Image EnemyPlatform;
        Image PlayerPlatform;
        Image EnemySprite;
        Image PlayerSprite;
        Image PlayerPokemon;
        public Image PlayerHPBarBackground;
        public Image EnemyHPBarBackground;
        public Image Pokeball;
        public bool IsTransitioning;
        public BattleState state;

        // HP bar data
        public Image PlayerPokemonName;
        public Image EnemyPokemonName;
        public Image PlayerPokemonGender;
        public Image EnemyPokemonGender;
        public Image PlayerHPBar;
        public Image PlayerPokemonHP;
        public Image PlayerPokemonMaxHP;
        public Image EnemyHPBar;
        public Image PlayerPokemonLevel;
        public Image EnemyPokemonLevel;
        public Image EXPBar;


        // Transition data
        float pokeballSpeedY = 4f;
        bool maxHeight;
        bool whiteBackgroundTransitioned;
        Image whiteBackground;

        private void Transition(GameTime gameTime, BattleTextBox textBox)
        {
            if (IsTransitioning && !ScreenManager.Instance.IsTransitioning)
            {
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
                            EnemySprite.Position.X += enemySpeed;
                            return;
                        }

                        PlayerPlatform.Position.X = playerPlatformDestinationX;
                        PlayerSprite.Position.X = PlayerPlatform.Position.X + PlayerPlatform.SourceRect.Width / 2 - 48;
                        EnemyPlatform.Position.X = enemyPlatformDestinationX;
                        EnemySprite.Position.X = EnemyPlatform.Position.X + EnemyPlatform.SourceRect.Width / 2 - EnemySprite.SourceRect.Width / 2;

                        if (BattleScreen.Wild)
                            state = BattleState.WILD_POKEMON_FADE_IN;
                        else
                            IsTransitioning = false;
                        break;
                    case BattleState.WILD_POKEMON_FADE_IN:
                        float enemyHPDestinationX = 52;
                        enemySpeed = (float)(1.2 * gameTime.ElapsedGameTime.TotalMilliseconds);
                        if (EnemySprite.Tint != Color.White || EnemyHPBarBackground.Position.X + enemySpeed < enemyHPDestinationX)
                        {
                            if (EnemyHPBarBackground.Position.X + enemySpeed < enemyHPDestinationX)
                            {
                                EnemyHPBarBackground.Position.X += enemySpeed;
                                EnemyPokemonName.Position.X += enemySpeed;
                                EnemyPokemonGender.Position.X += enemySpeed;
                                EnemyPokemonLevel.Position.X += enemySpeed;
                                EnemyHPBar.Position.X += enemySpeed;
                            }
                            if (EnemySprite.Tint != Color.White)
                                EnemySprite.Tint = new Color(EnemySprite.Tint.R + 3, EnemySprite.Tint.G + 3, EnemySprite.Tint.B + 3, 255);
                            return;
                        }
                        EnemyHPBarBackground.Position.X = enemyHPDestinationX;
                        EnemyPokemonName.Position = new Vector2(EnemyHPBarBackground.Position.X + 24, EnemyHPBarBackground.Position.Y + 19);
                        EnemyPokemonGender.Position = new Vector2(EnemyPokemonName.Position.X + EnemyPokemonName.SourceRect.Width, EnemyPokemonName.Position.Y);
                        EnemyPokemonLevel.Position = new Vector2(EnemyHPBarBackground.Position.X + EnemyHPBarBackground.SourceRect.Width - 56 - EnemyPokemonLevel.SourceRect.Width, EnemyPokemonName.Position.Y);
                        EnemyHPBar.Position = new Vector2(EnemyHPBarBackground.Position.X + 156 - ((1 - EnemyHPBar.Scale.X) / 2 * EnemyHPBar.SourceRect.Width), EnemyHPBarBackground.Position.Y + 68);
                        break;
                    case BattleState.PLAYER_SEND_POKEMON:
                        float playerSpriteDestinationX = -PlayerSprite.SourceRect.Width - 8;
                        float pokeballMaxHeight = textBox.Border.Position.Y - PlayerSprite.SourceRect.Height - 36;
                        float pokeballSpeedX = (float)(0.2 * gameTime.ElapsedGameTime.TotalMilliseconds);

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
                            if (Pokeball.Position.X < PlayerSprite.SourceRect.Width - 100)
                                Pokeball.Position = new Vector2(PlayerSprite.SourceRect.Width - 100, textBox.Border.Position.Y - PlayerSprite.SourceRect.Height + 20);

                            if (!maxHeight)
                            {
                                Pokeball.Position.Y -= pokeballSpeedY;
                                if (Pokeball.Position.Y <= pokeballMaxHeight)
                                {
                                    maxHeight = true;
                                    pokeballSpeedY = 1;
                                }
                            }
                            else
                            {
                                if (Pokeball.Position.Y < textBox.Border.Position.Y)
                                {
                                    pokeballSpeedX = (float)(0.1 * gameTime.ElapsedGameTime.TotalMilliseconds);
                                    Pokeball.Angle += 0.7f;
                                    Pokeball.Position.Y += pokeballSpeedY;
                                    pokeballSpeedY *= 1.2f;
                                }
                            }

                            Pokeball.Position.X += pokeballSpeedX;
                        }

                        if (!(PlayerSprite.Position.X - playerSpeed < playerSpriteDestinationX) || Pokeball.Position.Y < textBox.Border.Position.Y)
                        {
                            PlayerSprite.Position.X -= (int)playerSpeed;
                            return;
                        }

                        PlayerSprite.UnloadContent();

                        if ((PlayerPokemon.Scale.X < 1 && PlayerPokemon.Scale.Y < 1) || !whiteBackgroundTransitioned)
                        {
                            whiteBackground.Alpha += 0.05f;

                            if (whiteBackground.Alpha >= 1)
                                whiteBackgroundTransitioned = true;

                            PlayerPokemon.Scale = new Vector2(PlayerPokemon.Scale.X + 0.05f, PlayerPokemon.Scale.Y + 0.05f);
                            PlayerPokemon.Position = new Vector2(PlayerPlatform.Position.X + PlayerPlatform.SourceRect.Width * 0.55f - PlayerPokemon.SourceRect.Width / 2, PlayerPlatform.Position.Y + PlayerPlatform.SourceRect.Height - (int)(PlayerPokemon.SourceRect.Height * PlayerPokemon.Scale.Y));
                            pokeOriginalY = PlayerPokemon.Position.Y;
                            return;
                        }

                        PlayerPokemon.Scale = Vector2.One;
                        PlayerPokemon.Position = new Vector2(PlayerPlatform.Position.X + PlayerPlatform.SourceRect.Width * 0.55f - PlayerPokemon.SourceRect.Width / 2, PlayerPlatform.Position.Y + PlayerPlatform.SourceRect.Height - PlayerPokemon.SourceRect.Height);

                        float playerHPDestinationX = ScreenManager.Instance.Dimensions.X - PlayerHPBarBackground.SourceRect.Width - 40;
                        playerSpeed = (float)(1.2 * gameTime.ElapsedGameTime.TotalMilliseconds);
                        if (PlayerPokemon.Tint != Color.White || whiteBackground.Alpha > 0 || PlayerHPBarBackground.Position.X - playerSpeed > playerHPDestinationX)
                        {
                            if (PlayerHPBarBackground.Position.X - playerSpeed > playerHPDestinationX)
                            {
                                PlayerHPBarBackground.Position.X -= playerSpeed;
                                PlayerPokemonName.Position.X -= playerSpeed;
                                PlayerPokemonGender.Position.X -= playerSpeed;
                                PlayerPokemonLevel.Position.X -= playerSpeed;
                                PlayerPokemonMaxHP.Position.X -= playerSpeed;
                                PlayerPokemonHP.Position.X -= playerSpeed;
                                PlayerHPBar.Position.X -= playerSpeed;
                                EXPBar.Position.X -= playerSpeed;
                            }

                            if (whiteBackground.Alpha > 0)
                                whiteBackground.Alpha -= 0.05f;
                            PlayerPokemon.Tint = new Color(PlayerPokemon.Tint.R + 20, PlayerPokemon.Tint.G + 20, PlayerPokemon.Tint.B + 20, 255);
                            return;
                        }

                        PlayerHPBarBackground.Position.X = playerHPDestinationX;
                        PlayerPokemonName.Position = new Vector2(PlayerHPBarBackground.Position.X + 64, PlayerHPBarBackground.Position.Y + 19);
                        PlayerPokemonGender.Position = new Vector2(PlayerPokemonName.Position.X + PlayerPokemonName.SourceRect.Width, PlayerPokemonName.Position.Y);
                        PlayerPokemonLevel.Position = new Vector2(PlayerHPBarBackground.Position.X + PlayerHPBarBackground.SourceRect.Width - 36 - PlayerPokemonLevel.SourceRect.Width, PlayerPokemonName.Position.Y);
                        PlayerPokemonMaxHP.Position = new Vector2(PlayerHPBarBackground.Position.X + PlayerHPBarBackground.SourceRect.Width - 36 - PlayerPokemonMaxHP.SourceRect.Width, PlayerHPBarBackground.Position.Y + 92);
                        PlayerPokemonHP.Position = new Vector2(PlayerHPBarBackground.Position.X + PlayerHPBarBackground.SourceRect.Width - 116 - PlayerPokemonHP.SourceRect.Width, PlayerPokemonMaxHP.Position.Y);
                        PlayerHPBar.Position = new Vector2(PlayerHPBarBackground.Position.X + 192 - ((1 - PlayerHPBar.Scale.X) / 2 * PlayerHPBar.SourceRect.Width), PlayerHPBarBackground.Position.Y + 68);
                        EXPBar.Position = new Vector2(PlayerHPBarBackground.Position.X + 128 - ((1 - EXPBar.Scale.X) / 2 * EXPBar.SourceRect.Width), PlayerHPBarBackground.Position.Y + PlayerHPBarBackground.SourceRect.Height - 16);
                        whiteBackground.Alpha = 0;
                        IsTransitioning = false;
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

        private void animateBattleMenu(GameTime gameTime)
        {
            pokeBounceTimer -= (float) gameTime.ElapsedGameTime.TotalSeconds;
            barBounceTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (barBounceTimer < 0)
            {
                barBounce = !barBounce;
                PlayerHPBarBackground.Position.Y += barBounce ? 4 : -4;
                PlayerPokemonName.Position.Y += barBounce ? 4 : -4;
                PlayerPokemonGender.Position.Y += barBounce ? 4 : -4;
                PlayerPokemonLevel.Position.Y += barBounce ? 4 : -4;
                PlayerPokemonMaxHP.Position.Y += barBounce ? 4 : -4;
                PlayerPokemonHP.Position.Y += barBounce ? 4 : -4;
                PlayerHPBar.Position.Y += barBounce ? 4 : -4;
                EXPBar.Position.Y += barBounce ? 4 : -4;
                barBounceTimer = 0.3f;
            }

            if (pokeBounceTimer < 0)
            {
                PlayerPokemon.Position.Y += barBounce ? -4 : 4;
                pokeBounceTimer = 0.3f;
            }

        }

        // when battle menu option is selected
        public void reset()
        {
            barBounce = false;
            barBounceTimer = 0.3f;
            pokeBounceTimer = 0.2f;
            maxHeight = false;
            whiteBackgroundTransitioned = false;
            pokeballSpeedY = 4f;
            PlayerPokemon.Position.Y = pokeOriginalY;
            PlayerHPBarBackground.Position.Y = barOriginalY;

        }


        public void LoadContent(BattleTextBox textBox, CustomPokemon playerPokemon, CustomPokemon enemyPokemon)
        {
            // TODO: Load Background based on what environment the battle is in

            //Load battle images
            loadBattleContent(playerPokemon, enemyPokemon);

            PlayerSprite.SpriteSheetEffect.AmountOfFrames = new Vector2(5, 1);
            PlayerSprite.SpriteSheetEffect.CurrentFrame = Vector2.Zero;

            state = BattleState.INTRO;

            if (BattleScreen.Wild)
                EnemySprite.Tint = Color.LightGray;

            setBattleImagePositions(textBox);


            barOriginalY = PlayerHPBarBackground.Position.Y;
            IsTransitioning = true;


            PlayerPokemon = playerPokemon.Pokemon.Back;
            PlayerPokemon.Scale = new Vector2(0.01f, 0.01f);
            PlayerPokemon.LoadContent();
            PlayerPokemon.Tint = Color.Red;
        }

        public void UnloadContent()
        {
            Background.UnloadContent();
            EnemyPlatform.UnloadContent();
            EnemySprite.UnloadContent();
            PlayerPlatform.UnloadContent();
            PlayerSprite.UnloadContent();
            PlayerPokemon.UnloadContent();
        }

        public void Update(GameTime gameTime, BattleTextBox textBox)
        {
            Transition(gameTime, textBox);
            PlayerSprite.Update(gameTime);
            if (state == BattleState.BATTLE_MENU)
                animateBattleMenu(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, BattleTextBox textBox)
        {
            Background.Draw(spriteBatch);
            PlayerPlatform.Draw(spriteBatch);
            EnemyPlatform.Draw(spriteBatch);
            if (state == BattleState.PLAYER_SEND_POKEMON && PlayerSprite.SpriteSheetEffect.CurrentFrame.X == 4)
                whiteBackground.Draw(spriteBatch);
            PlayerHPBarBackground.Draw(spriteBatch);
            EnemyHPBarBackground.Draw(spriteBatch);

            drawHPBarStats(spriteBatch);

            PlayerSprite.Draw(spriteBatch);
            if (Pokeball.Position.Y >= textBox.Border.Position.Y)
                PlayerPokemon.Draw(spriteBatch);
            EnemySprite.Draw(spriteBatch);
            if (state == BattleState.PLAYER_SEND_POKEMON && PlayerSprite.SpriteSheetEffect.CurrentFrame.X == 4)
                Pokeball.Draw(spriteBatch);
        }


        private void loadBattleContent(CustomPokemon playerPokemon, CustomPokemon enemyPokemon)
        {
            // Battle assets
            Background = new Image();
            EnemyPlatform = new Image();
            PlayerPlatform = new Image();
            PlayerSprite = new Image();
            EnemySprite = new Image();
            PlayerPokemon = new Image();

            // Battle assets
            Background.Path = "BattleScreen/BattleBackground1";
            EnemyPlatform.Path = "BattleScreen/BattleBackground1EnemyPlatform";
            PlayerPlatform.Path = "BattleScreen/BattleBackground1PlayerPlatform";
            PlayerSprite.Path = Player.PlayerJsonObject.Gender == Gender.MALE ? "BattleScreen/BattleRedSpriteSheet" : "BattleScreen/BattleBackground1";
            PlayerSprite.Effects = "SpriteSheetEffect";
            EnemySprite = enemyPokemon.Pokemon.Front;

            // HP Bar assets
            PlayerPokemonName.Text = playerPokemon.Name;
            EnemyPokemonName.Text = enemyPokemon.PokemonName.ToUpper();
            PlayerPokemonGender.Text = playerPokemon.Gender == 0 ? "♂" : "♀";
            PlayerPokemonGender.FontColor = playerPokemon.Gender == 0 ? new Color(119, 208, 250, 255) : new Color(243, 169, 161, 255);

            EnemyPokemonGender.Text = enemyPokemon.Gender == 0 ? "♂" : "♀";
            EnemyPokemonGender.FontColor = enemyPokemon.Gender == 0 ? new Color(119, 208, 250, 255) : new Color(243, 169, 161, 255);

            PlayerPokemonLevel.Text = "Lv" + playerPokemon.Level;
            EnemyPokemonLevel.Text = "Lv" + enemyPokemon.Level;
            PlayerPokemonHP.Text = playerPokemon.CurrentHP.ToString();
            PlayerPokemonMaxHP.Text = playerPokemon.Stats.HP.ToString();

            // Handle Health ane exp bars
            float playerHealthRatio = (float)playerPokemon.CurrentHP / playerPokemon.Stats.HP;
            float expRatio = (float)playerPokemon.CurrentEXP / playerPokemon.LevelEXP;
            float enemyHealthRatio = (float)enemyPokemon.CurrentHP / enemyPokemon.Stats.HP;

            PlayerHPBar.Scale.X = playerHealthRatio;
            EXPBar.Scale.X = expRatio;
            EnemyHPBar.Scale.X = enemyHealthRatio;

            if (playerHealthRatio > 0.5)
            {
                PlayerHPBar.Tint = new Color(175, 252, 175, 1);
                PlayerHPBar.Alpha = 0.5f;
            }
            else if (playerHealthRatio > 0.2 && playerHealthRatio <= 0.5)
            {
                PlayerHPBar.Tint = new Color(255, 232, 0, 0);
                PlayerHPBar.Alpha = 0.5f;
            }
            else
            {
                PlayerHPBar.Tint = new Color(255, 100, 0, 100);
                PlayerHPBar.Alpha = 0.4f;
            }

            if (enemyHealthRatio > 0.5)
            {
                EnemyHPBar.Tint = new Color(175, 252, 175, 1);
                EnemyHPBar.Alpha = 0.5f;
            }
            else if (enemyHealthRatio > 0.2 && enemyHealthRatio <= 0.5)
            {
                EnemyHPBar.Tint = new Color(255, 232, 0, 0);
                EnemyHPBar.Alpha = 0.5f;
            }
            else
            {
                EnemyHPBar.Tint = new Color(255, 100, 0, 100);
                EnemyHPBar.Alpha = 0.4f;
            }

            // Battle assets
            Background.LoadContent();
            EnemyPlatform.LoadContent();
            EnemySprite.LoadContent();
            PlayerPlatform.LoadContent();
            PlayerSprite.LoadContent();
            Pokeball.LoadContent();
            PlayerHPBarBackground.LoadContent();
            EnemyHPBarBackground.LoadContent();

            // HP Bar assets
            PlayerPokemonName.LoadContent();
            EnemyPokemonName.LoadContent();
            PlayerPokemonGender.LoadContent();
            EnemyPokemonGender.LoadContent();
            EnemyPokemonLevel.LoadContent();
            PlayerPokemonLevel.LoadContent();
            PlayerPokemonHP.LoadContent();
            PlayerPokemonMaxHP.LoadContent();
            PlayerHPBar.LoadContent();
            EnemyHPBar.LoadContent();
            EXPBar.LoadContent();


            /* POKEBALL TRANSITION CODE */
            whiteBackground = new Image();
            whiteBackground.Texture = new Texture2D(ScreenManager.Instance.GraphicsDevice, Background.SourceRect.Width, Background.SourceRect.Height);
            whiteBackground.LoadContent();
            Color[] data = new Color[Background.SourceRect.Width * Background.SourceRect.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.White;
            whiteBackground.Texture.SetData(data);
            whiteBackground.Alpha = 0;
            /* END POKEBALL TRANSITION CODE */
        }

        private void setBattleImagePositions(TextBox textBox)
        {
            // set battle image positions
            EnemyPlatform.Position = new Vector2(-EnemyPlatform.SourceRect.Width, 192);
            EnemySprite.Position = new Vector2(EnemyPlatform.Position.X + EnemyPlatform.SourceRect.Width / 2 - EnemySprite.SourceRect.Width / 2, EnemyPlatform.Position.Y + EnemyPlatform.SourceRect.Height * 0.75f - EnemySprite.SourceRect.Height);
            PlayerPlatform.Position = new Vector2(ScreenManager.Instance.Dimensions.X + PlayerPlatform.SourceRect.Width, textBox.Border.Position.Y - PlayerPlatform.SourceRect.Height);
            PlayerSprite.Position = new Vector2(PlayerPlatform.Position.X + PlayerPlatform.SourceRect.Width / 2 - 48, PlayerPlatform.Position.Y + PlayerPlatform.SourceRect.Height - PlayerSprite.SourceRect.Height);
            EnemyHPBarBackground.Position = new Vector2(-EnemyHPBarBackground.SourceRect.Width, EnemyPlatform.Position.Y - EnemyHPBarBackground.SourceRect.Height - 12);
            PlayerHPBarBackground.Position = new Vector2(ScreenManager.Instance.Dimensions.X, textBox.Border.Position.Y - PlayerHPBarBackground.SourceRect.Height - 4);

            // set hp bar image positions
            PlayerPokemonName.Position = new Vector2(PlayerHPBarBackground.Position.X + 64, PlayerHPBarBackground.Position.Y + 19);
            PlayerPokemonGender.Position = new Vector2(PlayerPokemonName.Position.X + PlayerPokemonName.SourceRect.Width, PlayerPokemonName.Position.Y);
            PlayerPokemonLevel.Position = new Vector2(PlayerHPBarBackground.Position.X + PlayerHPBarBackground.SourceRect.Width - 36 - PlayerPokemonLevel.SourceRect.Width, PlayerPokemonName.Position.Y);
            PlayerPokemonMaxHP.Position = new Vector2(PlayerHPBarBackground.Position.X + PlayerHPBarBackground.SourceRect.Width - 36 - PlayerPokemonMaxHP.SourceRect.Width, PlayerHPBarBackground.Position.Y + 92);
            PlayerPokemonHP.Position = new Vector2(PlayerHPBarBackground.Position.X + PlayerHPBarBackground.SourceRect.Width - 116 - PlayerPokemonHP.SourceRect.Width, PlayerPokemonMaxHP.Position.Y);
            PlayerHPBar.Position = new Vector2(PlayerHPBarBackground.Position.X + 192 - ((1 - PlayerHPBar.Scale.X) / 2 * PlayerHPBar.SourceRect.Width), PlayerHPBarBackground.Position.Y + 68);
            EXPBar.Position = new Vector2(PlayerHPBarBackground.Position.X + 128 - ((1 - EXPBar.Scale.X) / 2 * EXPBar.SourceRect.Width), PlayerHPBarBackground.Position.Y + PlayerHPBarBackground.SourceRect.Height - 16);

            EnemyPokemonName.Position = new Vector2(EnemyHPBarBackground.Position.X + 24, EnemyHPBarBackground.Position.Y + 19);
            EnemyPokemonGender.Position = new Vector2(EnemyPokemonName.Position.X + EnemyPokemonName.SourceRect.Width, EnemyPokemonName.Position.Y);
            EnemyPokemonLevel.Position = new Vector2(EnemyHPBarBackground.Position.X + EnemyHPBarBackground.SourceRect.Width - 56 - EnemyPokemonLevel.SourceRect.Width, EnemyPokemonName.Position.Y);
            EnemyHPBar.Position = new Vector2(EnemyHPBarBackground.Position.X + 156 - ((1 - EnemyHPBar.Scale.X) / 2 * EnemyHPBar.SourceRect.Width), EnemyHPBarBackground.Position.Y + 68);

        }

        private void drawHPBarStats(SpriteBatch spriteBatch) {
            EnemyPokemonName.Draw(spriteBatch);
            EnemyPokemonGender.Draw(spriteBatch);
            EnemyPokemonLevel.Draw(spriteBatch);
            EnemyHPBar.Draw(spriteBatch);

            PlayerPokemonName.Draw(spriteBatch);
            PlayerPokemonGender.Draw(spriteBatch);
            PlayerPokemonLevel.Draw(spriteBatch);
            PlayerPokemonHP.Draw(spriteBatch);
            PlayerPokemonMaxHP.Draw(spriteBatch);
            PlayerHPBar.Draw(spriteBatch);
            EXPBar.Draw(spriteBatch);
        }


    }
}
