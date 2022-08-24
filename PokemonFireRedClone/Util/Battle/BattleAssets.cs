using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class BattleAssets
    {
        // TODO: Add sounds to all Animations

        // Battle menu animation
        private float barOriginalY;
        private float pokeNameOriginalY;
        private float pokeHPOriginalY;
        private float pokeHealthBarOriginalY;
        private float pokeEXPOriginalY;
        private float pokeBounceTimer = 0.2f;
        private float barBounceTimer = 0.3f;
        private bool barBounce;

        public enum BattleState
        {
            INTRO,
            WILD_POKEMON_FADE_IN,
            TRAINER_BALL_BAR,
            PLAYER_SEND_POKEMON,
            PLAYER_INTRO_SEND_POKEMON,
            OPPONENT_INTRO_SEND_POKEMON,
            BATTLE_MENU,
            DAMAGE_ANIMATION,
            STATUS_ANIMATION,
            POKEMON_SWITCH,
            POKEMON_SEND_OUT,
            OPPONENT_SEND_POKEMON,
            POKEMON_FAINT,
            EXP_ANIMATION,
            LEVEL_UP_ANIMATION
        }

        // General battle screen data
        public Image Background;
        public Image EnemyPlatform;
        public Image PlayerPlatform;
        public Image EnemyPokemon;
        public Image EnemySprite;
        public Image PlayerSprite;
        public Image PlayerPokemon;
        public Image PlayerHPBarBackground;
        public Image EnemyHPBarBackground;
        public Image PlayerHPBarLevelUp;
        public Image Pokeball;
        public Image StatChangeAnimationImage1;
        public Image StatChangeAnimationImage2;
        public bool IsTransitioning;
        public BattleState State;

        // HP bar data
        [XmlIgnore]
        public PokemonAssets PlayerPokemonAssets;
        [XmlIgnore]
        public PokemonAssets EnemyPokemonAssets;
        public Image EXPBar;

        // Trainer battle data
        public Image TrainerBallBar;
        public Image PlayerBallBar;
        public List<Image> TrainerBarBalls;
        public List<Image> PlayerBarBalls;

        public BattleAnimation Animation;

        public float PokeOriginalY;


        public void LoadContent()
        {
            // TODO: Load Background based on what environment the battle is in

            //Load battle images
            LoadBattleContent(BattleLogic.Battle.PlayerPokemon.Pokemon, BattleLogic.Battle.EnemyPokemon.Pokemon);
            if (ScreenManager.Instance.PreviousScreen is PokemonScreen)
            {
                State = BattleState.BATTLE_MENU;
                ScreenManager.Instance.BattleScreen.TextBox.NextPage = 4;
                ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning = true;
                ScreenManager.Instance.BattleScreen.TextBox.UpdateDialogue = true;
                PlayerPokemon.LoadContent();
                SetDefaultBattleImagePositions(ScreenManager.Instance.BattleScreen.TextBox);
            }
            else
            {
                LoadSprites();
                PlayerSprite.SpriteSheetEffect.AmountOfFrames = new Vector2(5, 1);
                PlayerSprite.SpriteSheetEffect.CurrentFrame = Vector2.Zero;

                State = BattleState.INTRO;
                Animation = new IntroAnimation();
                if (BattleLogic.Battle.IsWild)
                    EnemyPokemon.Tint = Color.LightGray;
                else
                {
                    EnemyPokemon.Scale = new Vector2(0.14f, 0.14f);
                    EnemyPokemon.Alpha = 0;
                    EnemyPokemon.LoadContent();
                    EnemyPokemon.Tint = Color.Red;
                }

                SetIntroBattleImagePositions(ScreenManager.Instance.BattleScreen.TextBox);


                barOriginalY = PlayerHPBarBackground.Position.Y;
                IsTransitioning = true;

                PlayerPokemon.Scale = new Vector2(0, 0);
                PlayerPokemon.LoadContent();
                PlayerPokemon.Tint = Color.Red;
            }

            SetAssetPositions();
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
            // REMOVE AFTER TESTING TRAINERS
            if (Pokeball.IsLoaded)
                Pokeball.UnloadContent();
            if (EnemySprite != null)
                EnemySprite.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {            
            if (Animation != null && IsTransitioning && !ScreenManager.Instance.IsTransitioning)
                Animation.Animate(gameTime);

            if (State == BattleState.POKEMON_SEND_OUT || State == BattleState.OPPONENT_INTRO_SEND_POKEMON || State == BattleState.OPPONENT_SEND_POKEMON)
                Pokeball.Update(gameTime);

            if (State == BattleState.INTRO || State == BattleState.WILD_POKEMON_FADE_IN || State == BattleState.PLAYER_SEND_POKEMON || State == BattleState.OPPONENT_INTRO_SEND_POKEMON || (PlayerSprite != null && State == BattleState.OPPONENT_SEND_POKEMON) || (PlayerSprite != null && State == BattleState.POKEMON_SEND_OUT))
                PlayerSprite.Update(gameTime);
            if (State == BattleState.BATTLE_MENU)
                AnimateBattleMenu(gameTime);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Background.Draw(spriteBatch);
            PlayerPlatform.Draw(spriteBatch);
            EnemyPlatform.Draw(spriteBatch);
            if ((State == BattleState.POKEMON_SWITCH || State == BattleState.POKEMON_SEND_OUT || State == BattleState.OPPONENT_INTRO_SEND_POKEMON || State == BattleState.OPPONENT_SEND_POKEMON) && Animation != null)
                Animation.Draw(spriteBatch);
            
            PlayerHPBarBackground.Draw(spriteBatch);
            if (State == BattleState.LEVEL_UP_ANIMATION)
                PlayerHPBarLevelUp.Draw(spriteBatch);
            EnemyHPBarBackground.Draw(spriteBatch);

            PlayerPokemonAssets.Draw(spriteBatch);
            EnemyPokemonAssets.Draw(spriteBatch);
            EXPBar.Draw(spriteBatch);

            if (State == BattleState.INTRO || State == BattleState.TRAINER_BALL_BAR || State == BattleState.OPPONENT_INTRO_SEND_POKEMON || State == BattleState.WILD_POKEMON_FADE_IN || State == BattleState.PLAYER_SEND_POKEMON || (PlayerSprite != null && State == BattleState.OPPONENT_SEND_POKEMON) || (PlayerSprite != null && State == BattleState.POKEMON_SEND_OUT))
                PlayerSprite.Draw(spriteBatch);

            if (State == BattleState.TRAINER_BALL_BAR || State == BattleState.OPPONENT_INTRO_SEND_POKEMON || State == BattleState.OPPONENT_SEND_POKEMON || (State == BattleState.PLAYER_SEND_POKEMON && !BattleLogic.Battle.IsWild))
            {
                TrainerBallBar.Draw(spriteBatch);
                foreach (Image image in TrainerBarBalls)
                    image.Draw(spriteBatch);

                if (PlayerBallBar != null)
                {
                    PlayerBallBar.Draw(spriteBatch);
                    foreach (Image image in PlayerBarBalls)
                        image.Draw(spriteBatch);
                }
            }

            PlayerPokemon.Draw(spriteBatch);

            if (!BattleLogic.Battle.IsWild)
                EnemySprite.Draw(spriteBatch);

            if (State == BattleState.POKEMON_SEND_OUT || State == BattleState.OPPONENT_INTRO_SEND_POKEMON || State == BattleState.OPPONENT_SEND_POKEMON)
                Pokeball.Draw(spriteBatch);

            EnemyPokemon.Draw(spriteBatch);

            if (State == BattleState.STATUS_ANIMATION)
            {
                StatChangeAnimationImage1.Draw(spriteBatch);
                StatChangeAnimationImage2.Draw(spriteBatch);
            }

            
        }

        private void AnimateBattleMenu(GameTime gameTime)
        {
            pokeBounceTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
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

        private void LoadBattleContent(CustomPokemon playerPokemon, CustomPokemon enemyPokemon)
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
            if (BattleLogic.Battle.IsWild || ScreenManager.Instance.PreviousScreen is PokemonScreen)
                EnemyPokemon.LoadContent();
            PlayerPlatform.LoadContent();
            PlayerHPBarBackground.LoadContent();
            EnemyHPBarBackground.LoadContent();
            PlayerHPBarLevelUp.LoadContent();
            StatChangeAnimationImage1.LoadContent();
            StatChangeAnimationImage2.LoadContent();
            if (!BattleLogic.Battle.IsWild)
            {
                Pokeball.Path = "BattleScreen/PokeballSpriteSheet";
                Pokeball.Effects = "SpriteSheetEffect";
            }
            Pokeball.LoadContent();
            if (!BattleLogic.Battle.IsWild)
                Pokeball.SpriteSheetEffect.AmountOfFrames = new Vector2(3, 1);
            PlayerHPBarLevelUp.Alpha = 0;
            StatChangeAnimationImage1.Alpha = 0;
            StatChangeAnimationImage2.Alpha = 0;

            // HP Bar assets
            PlayerPokemonAssets.LoadContent("Fonts/PokemonFireRedSmall", new Color(81, 81, 81, 255), new Color(224, 219, 192, 255));
            EnemyPokemonAssets.LoadContent("Fonts/PokemonFireRedSmall", new Color(81, 81, 81, 255), new Color(224, 219, 192, 255));
            EXPBar.LoadContent();

            // Trainer assets
            if (!BattleLogic.Battle.IsWild)
            {
                EnemySprite = new Image
                {
                    Path = BattleLogic.Battle.Trainer.BattleSprite.Path
                };

                if (!EnemySprite.IsLoaded)
                    EnemySprite.LoadContent();

                TrainerBallBar = new Image()
                {
                    Path = "BattleScreen/TrainerPokemonBar",
                    Flip = true
                };
                TrainerBarBalls = new List<Image>();

                for (int i = 0; i < BattleLogic.Battle.Trainer.Pokemon.Count; i++)
                {
                    Image ball = BattleLogic.Battle.Trainer.Pokemon[i].CurrentHP == 0 ?
                        new Image() { Path = "BattleScreen/FaintedTrainerBall" } : new Image() { Path = "BattleScreen/TrainerPokemonBall" };

                    TrainerBarBalls.Add(ball);
                }

                for (int i = TrainerBarBalls.Count; i < 6; i++)
                    TrainerBarBalls.Add(new Image() { Path = "BattleScreen/EmptyTrainerBall" });

                TrainerBallBar.LoadContent();
                foreach (Image image in TrainerBarBalls)
                    image.LoadContent();

                if (ScreenManager.Instance.PreviousScreen is GameplayScreen)
                {
                    PlayerBallBar = new Image()
                    {
                        Path = "BattleScreen/TrainerPokemonBar"
                    };
                    PlayerBarBalls = new List<Image>();

                    for (int i = 0; i < Player.PlayerJsonObject.PokemonInBag.Count; i++)
                    {
                        Image ball = Player.PlayerJsonObject.PokemonInBag[i].CurrentHP == 0 ?
                            new Image() { Path = "BattleScreen/FaintedTrainerBall" } : new Image() { Path = "BattleScreen/TrainerPokemonBall" };

                        PlayerBarBalls.Add(ball);
                    }

                    for (int i = PlayerBarBalls.Count; i < 6; i++)
                        PlayerBarBalls.Add(new Image() { Path = "BattleScreen/EmptyTrainerBall" });

                    PlayerBallBar.LoadContent();
                    foreach (Image image in PlayerBarBalls)
                        image.LoadContent();
                }
            }

        }

        private void LoadSprites()
        {
            PlayerSprite = new Image
            {
                Path = Player.PlayerJsonObject.Gender == Gender.MALE ? "BattleScreen/BattleRedSpriteSheet" : "BattleScreen/BattleBackground1",
                Effects = "SpriteSheetEffect"
            };
            PlayerSprite.LoadContent();
        }

        private void SetIntroBattleImagePositions(TextBox textBox)
        {
            // set battle image positions
            EnemyPlatform.Position = new Vector2(-EnemyPlatform.SourceRect.Width, 192);
            EnemyPokemon.Position = new Vector2(EnemyPlatform.Position.X + EnemyPlatform.SourceRect.Width / 2 - EnemyPokemon.SourceRect.Width / 2, EnemyPlatform.Position.Y + EnemyPlatform.SourceRect.Height * 0.75f - EnemyPokemon.SourceRect.Height);
            PlayerPlatform.Position = new Vector2(ScreenManager.Instance.Dimensions.X + PlayerPlatform.SourceRect.Width, textBox.Border.Position.Y - PlayerPlatform.SourceRect.Height);
            PlayerSprite.Position = new Vector2(PlayerPlatform.Position.X + PlayerPlatform.SourceRect.Width / 2 - 48, PlayerPlatform.Position.Y + PlayerPlatform.SourceRect.Height - PlayerSprite.SourceRect.Height);
            EnemyHPBarBackground.Position = new Vector2(-EnemyHPBarBackground.SourceRect.Width, EnemyPlatform.Position.Y - EnemyHPBarBackground.SourceRect.Height - 12);
            PlayerHPBarBackground.Position = new Vector2(ScreenManager.Instance.Dimensions.X, textBox.Border.Position.Y - PlayerHPBarBackground.SourceRect.Height - 4);
            PlayerHPBarLevelUp.Position = new Vector2(ScreenManager.Instance.Dimensions.X - PlayerHPBarBackground.SourceRect.Width - 40, PlayerHPBarBackground.Position.Y);

            // trainer battle image positions
            if (!BattleLogic.Battle.IsWild)
            {
                EnemySprite.Position = new Vector2(EnemyPlatform.Position.X + EnemyPlatform.SourceRect.Width / 2 - EnemySprite.SourceRect.Width / 2, EnemyPlatform.Position.Y + EnemyPlatform.SourceRect.Height * 0.75f - EnemySprite.SourceRect.Height);
                TrainerBallBar.Position = new Vector2(-TrainerBallBar.SourceRect.Width, EnemyPlatform.Position.Y - 32);
                PlayerBallBar.Position = new Vector2(ScreenManager.Instance.Dimensions.X, PlayerPlatform.Position.Y - 20);

                //TODO: set positions for palyer and trainer balls on ball bar
                float padX = 0;
                foreach (Image image in TrainerBarBalls)
                {
                    image.Position = new Vector2(-image.SourceRect.Width + padX, TrainerBallBar.Position.Y - image.SourceRect.Height - 4);
                    padX -= image.SourceRect.Width + 12;
                }

                padX = 0;

                foreach (Image image in PlayerBarBalls)
                {
                    image.Position = new Vector2(PlayerBallBar.Position.X + padX, PlayerBallBar.Position.Y - image.SourceRect.Height - 4);
                    padX += image.SourceRect.Width + 12;
                }
            }
        }

        // when battle menu option is selected
        public void Reset()
        {
            barBounce = false;
            barBounceTimer = 0.3f;
            pokeBounceTimer = 0.2f;
            PlayerPokemon.Position.Y = PokeOriginalY;
            PlayerHPBarBackground.Position.Y = barOriginalY;
            PlayerPokemonAssets.HPBar.Position.Y = pokeHealthBarOriginalY;
            PlayerPokemonAssets.Name.SetY(pokeNameOriginalY);
            PlayerPokemonAssets.Level.SetY(pokeNameOriginalY);
            if (PlayerPokemonAssets.Gender != null)
                PlayerPokemonAssets.Gender.SetY(pokeNameOriginalY);
            EXPBar.Position.Y = pokeEXPOriginalY;
            PlayerPokemonAssets.CurrentHP.SetY(pokeHPOriginalY);
            PlayerPokemonAssets.MaxHP.SetY(pokeHPOriginalY);
        }

        public void SetDefaultBattleImagePositions(TextBox textBox)
        {
            EnemyPlatform.Position = new Vector2(ScreenManager.Instance.Dimensions.X - EnemyPlatform.SourceRect.Width, 192);
            EnemyPokemon.Position = new Vector2(EnemyPlatform.Position.X + EnemyPlatform.SourceRect.Width / 2 - EnemyPokemon.SourceRect.Width / 2, EnemyPlatform.Position.Y + EnemyPlatform.SourceRect.Height * 0.75f - EnemyPokemon.SourceRect.Height);
            if (!BattleLogic.Battle.IsWild)
                EnemySprite.Position = new Vector2(ScreenManager.Instance.Dimensions.X + 8, EnemyPlatform.Position.Y + EnemyPlatform.SourceRect.Height * 0.75f - EnemySprite.SourceRect.Height);
            PlayerPlatform.Position = new Vector2(16, textBox.Border.Position.Y - PlayerPlatform.SourceRect.Height);
            EnemyHPBarBackground.Position = new Vector2(52, EnemyPlatform.Position.Y - EnemyHPBarBackground.SourceRect.Height - 12);
            PlayerHPBarBackground.Position = new Vector2(ScreenManager.Instance.Dimensions.X - PlayerHPBarBackground.SourceRect.Width - 40, textBox.Border.Position.Y - PlayerHPBarBackground.SourceRect.Height - 4);
            PlayerHPBarLevelUp.Position = PlayerHPBarBackground.Position;
            PlayerPokemon.Position = new Vector2(PlayerPlatform.Position.X + PlayerPlatform.SourceRect.Width * 0.55f - PlayerPokemon.SourceRect.Width / 2, PlayerPlatform.Position.Y + PlayerPlatform.SourceRect.Height - PlayerPokemon.SourceRect.Height);
            PokeOriginalY = PlayerPokemon.Position.Y;
            barOriginalY = PlayerHPBarBackground.Position.Y;

            if (!BattleLogic.Battle.IsWild)
            {
                TrainerBallBar.Position = new Vector2(-TrainerBallBar.SourceRect.Width, EnemyPlatform.Position.Y - 32);

                float padX = 0;
                foreach (Image image in TrainerBarBalls)
                {
                    image.Position = new Vector2(TrainerBallBar.Position.X + TrainerBallBar.SourceRect.Width - image.SourceRect.Width + padX - 80, TrainerBallBar.Position.Y - image.SourceRect.Height - 4);
                    padX -= image.SourceRect.Width + 12;
                }
            }
        }

        public void SetAssetPositions()
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

        public void RefreshTrainerBalls()
        {
            foreach (Image image in TrainerBarBalls)
                image.UnloadContent();

            TrainerBarBalls.Clear();

            for (int i = 0; i < BattleLogic.Battle.Trainer.Pokemon.Count; i++)
            {
                Image ball = BattleLogic.Battle.Trainer.Pokemon[i].CurrentHP == 0 ?
                    new Image() { Path = "BattleScreen/FaintedTrainerBall" } : new Image() { Path = "BattleScreen/TrainerPokemonBall" };

                TrainerBarBalls.Add(ball);
            }

            for (int i = TrainerBarBalls.Count; i < 6; i++)
                TrainerBarBalls.Add(new Image() { Path = "BattleScreen/EmptyTrainerBall" });

            foreach (Image image in TrainerBarBalls)
                image.LoadContent();

            float padX = 0;
            foreach (Image image in TrainerBarBalls)
            {
                image.Position = new Vector2(TrainerBallBar.Position.X + TrainerBallBar.SourceRect.Width - image.SourceRect.Width + padX - 80, TrainerBallBar.Position.Y - image.SourceRect.Height - 4);
                padX -= image.SourceRect.Width + 12;
            }
        }

        public void UpdatePlayerPokemon()
        {
            BattleLogic.Battle.UpdatePlayerPokemon();

            PlayerPokemon.Scale = Vector2.Zero;
            PlayerPokemon.Position = new Vector2(PlayerPlatform.Position.X + PlayerPlatform.SourceRect.Width * 0.55f - PlayerPokemon.SourceRect.Width / 2,
                PlayerPlatform.Position.Y + PlayerPlatform.SourceRect.Height - PlayerPokemon.SourceRect.Height);
            PokeOriginalY = PlayerPokemon.Position.Y;
            PlayerPokemon.UnloadContent();

            PlayerPokemon = BattleLogic.Battle.PlayerPokemon.Pokemon.Pokemon.Back;
            PlayerPokemon.Scale = Vector2.Zero;
            PlayerPokemon.LoadContent();
            PlayerPokemon.Tint = Color.Red;

            PlayerPokemonAssets.UnloadContent();
            PlayerPokemonAssets = new PokemonAssets(BattleLogic.Battle.PlayerPokemon.Pokemon, true);
            PlayerPokemonAssets.ScaleEXPBar(EXPBar);
            PlayerPokemonAssets.LoadContent("Fonts/PokemonFireRedSmall", new Color(81, 81, 81, 255), new Color(224, 219, 192, 255));
            SetDefaultBattleImagePositions(ScreenManager.Instance.BattleScreen.TextBox);
            PlayerHPBarBackground.Position = new Vector2(ScreenManager.Instance.Dimensions.X,
                ScreenManager.Instance.BattleScreen.TextBox.Border.Position.Y - PlayerHPBarBackground.SourceRect.Height - 4);
            SetAssetPositions();
        }

        public void UpdateOpponentPokemon()
        {
            EnemyPokemon.UnloadContent();

            EnemyPokemon = BattleLogic.Battle.EnemyPokemon.Pokemon.Pokemon.Front;
            EnemyPokemon.Scale = new Vector2(0.14f, 0.14f);
            EnemyPokemon.Alpha = 0;
            EnemyPokemon.LoadContent();
            EnemyPokemon.Tint = Color.Red;

            EnemyPokemonAssets.UnloadContent();
            EnemyPokemonAssets = new PokemonAssets(BattleLogic.Battle.EnemyPokemon.Pokemon, false);
            EnemyPokemonAssets.LoadContent("Fonts/PokemonFireRedSmall", new Color(81, 81, 81, 255), new Color(224, 219, 192, 255));
            EnemyPokemon.Position = new Vector2(EnemyPlatform.Position.X + EnemyPlatform.SourceRect.Width / 2 - EnemyPokemon.SourceRect.Width / 2, EnemyPlatform.Position.Y + EnemyPlatform.SourceRect.Height * 0.75f - EnemyPokemon.SourceRect.Height);
            EnemyHPBarBackground.Position = new Vector2(-EnemyHPBarBackground.SourceRect.Width, EnemyPlatform.Position.Y - EnemyHPBarBackground.SourceRect.Height - 12);
            EnemyHPBarBackground.Alpha = 1;
            SetAssetPositions();
        }

    }
}
