﻿using System;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class BattleScreen : GameScreen
    {

        public enum BattleState {
            INTRO,
            WILD_POKEMON_FADE_IN,
            PLAYER_SEND_POKEMON,
            ENEMY_SEND_POKEMON,
            BATTLE_MENU,
            PLAYER_MOVE,
            ENEMY_MOVE,
            THROW_POKEBALL,
            PLAYER_POKEMON_FAINT,
            ENEMY_POKEMON_FAINT
        }

        public BattleTextBox TextBox;
        Image Background;
        Image EnemyPlatform;
        Image PlayerPlatform;
        Image EnemySprite;
        Image PlayerSprite;
        Image PlayerPokemon;
        public Image PlayerHPBar;
        public Image EnemyHPBar;
        public Image Pokeball;
        MenuManager menuManager;
        public static bool IsTransitioning;
        public static BattleState state;

        public static bool Wild;

        /*
         * Battle screen:
         * - Pokemon sprites
         * - Battle menu
         *      - Fight menu
         *      - Access to pokemon menu
         *      - Access to item menu
         *      - Run away
         * - If wild, Battle starts with "Wild POKEMONNAME appeared!"
         * - Special textbox at bottom
         * - Exp bar
         * - Health bar
         * - Icon whether opposite pokemon has been caught
         * - Pokemon Name
         * - Pokemon Level
         * 
         * 
         * 
         */

        float POKEBALL_SPEED_Y = 4f;
        bool maxHeight;
        bool whiteBackgroundTransitioned;
        Image whiteBackground;

        private void Transition(GameTime gameTime)
        {
            if (IsTransitioning && !ScreenManager.Instance.IsTransitioning)
            {
                switch(state)
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

                        if (Wild)
                            state = BattleState.WILD_POKEMON_FADE_IN;
                        else
                            IsTransitioning = false;
                        break;
                    case BattleState.WILD_POKEMON_FADE_IN:
                        float enemyHPDestinationX = 52;
                        enemySpeed = (float)(1.2 * gameTime.ElapsedGameTime.TotalMilliseconds);
                        if (EnemySprite.Tint != Color.White || EnemyHPBar.Position.X + enemySpeed < enemyHPDestinationX)
                        {
                            if (EnemyHPBar.Position.X + enemySpeed < enemyHPDestinationX)
                                EnemyHPBar.Position.X += enemySpeed;
                            if (EnemySprite.Tint != Color.White)
                                EnemySprite.Tint = new Color(EnemySprite.Tint.R + 3, EnemySprite.Tint.G + 3, EnemySprite.Tint.B + 3, 255);
                            return;
                        }
                        EnemyHPBar.Position.X = enemyHPDestinationX;
                        IsTransitioning = false;
                        break;
                    case BattleState.PLAYER_SEND_POKEMON:
                        float playerSpriteDestinationX = -PlayerSprite.SourceRect.Width-8;
                        float pokeballMaxHeight = TextBox.Border.Position.Y - PlayerSprite.SourceRect.Height - 36;
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
                                Pokeball.Position = new Vector2(PlayerSprite.SourceRect.Width - 100, TextBox.Border.Position.Y - PlayerSprite.SourceRect.Height + 20);

                            if (!maxHeight)
                            {
                                Pokeball.Position.Y -= POKEBALL_SPEED_Y;
                                if (Pokeball.Position.Y <= pokeballMaxHeight)
                                {
                                    maxHeight = true;
                                    POKEBALL_SPEED_Y = 1;
                                }
                            }
                            else
                            {
                                if (Pokeball.Position.Y < TextBox.Border.Position.Y)
                                {
                                    pokeballSpeedX = (float)(0.1 * gameTime.ElapsedGameTime.TotalMilliseconds);
                                    Pokeball.Angle += 0.7f;
                                    Pokeball.Position.Y += POKEBALL_SPEED_Y;
                                    POKEBALL_SPEED_Y *= 1.2f;
                                }
                            }

                            Pokeball.Position.X += pokeballSpeedX;
                        }

                        if (!(PlayerSprite.Position.X - playerSpeed < playerSpriteDestinationX) || Pokeball.Position.Y < TextBox.Border.Position.Y)
                        {
                            PlayerSprite.Position.X -= (int) playerSpeed;
                            return;
                        }

                        PlayerSprite.UnloadContent();

                        if ((PlayerPokemon.Scale.X < 1 && PlayerPokemon.Scale.Y < 1) || !whiteBackgroundTransitioned)
                        {
                            whiteBackground.Alpha += 0.05f;

                            if (whiteBackground.Alpha >= 1)
                                whiteBackgroundTransitioned = true;

                            PlayerPokemon.Scale = new Vector2(PlayerPokemon.Scale.X + 0.05f, PlayerPokemon.Scale.Y + 0.05f);
                            PlayerPokemon.Position = new Vector2(PlayerPlatform.Position.X + PlayerPlatform.SourceRect.Width / 2 - 48, PlayerPlatform.Position.Y + PlayerPlatform.SourceRect.Height - (int) (PlayerPokemon.SourceRect.Height * PlayerPokemon.Scale.Y));
                            return;
                        }

                        PlayerPokemon.Scale = Vector2.One;
                        PlayerPokemon.Position = new Vector2(PlayerPlatform.Position.X + PlayerPlatform.SourceRect.Width / 2 - 48, PlayerPlatform.Position.Y + PlayerPlatform.SourceRect.Height - PlayerPokemon.SourceRect.Height);

                        float playerHPDestinationX = ScreenManager.Instance.Dimensions.X - PlayerHPBar.SourceRect.Width - 40;
                        playerSpeed = (float)(1.2 * gameTime.ElapsedGameTime.TotalMilliseconds);
                        if (PlayerPokemon.Tint != Color.White || whiteBackground.Alpha > 0 || PlayerHPBar.Position.X - playerSpeed > playerHPDestinationX)
                        {
                            if (PlayerHPBar.Position.X - playerSpeed > playerHPDestinationX)
                                PlayerHPBar.Position.X -= playerSpeed;

                            if (whiteBackground.Alpha > 0)
                                whiteBackground.Alpha -= 0.05f;
                            PlayerPokemon.Tint = new Color(PlayerPokemon.Tint.R + 20, PlayerPokemon.Tint.G + 20, PlayerPokemon.Tint.B + 20, 255);
                            return;
                        }

                        PlayerHPBar.Position.X = playerHPDestinationX;
                        whiteBackground.Alpha = 0;
                        IsTransitioning = false;
                        break;
                    default:
                        break;
                }
            }
        }


        public BattleScreen()
        {
            Wild = true;
            menuManager = new MenuManager("BattleMenu");
        }


        public override void LoadContent()
        {
            // TODO: Load Background based on what environment the battle is in
            Background = new Image();
            EnemyPlatform = new Image();
            PlayerPlatform = new Image();
            PlayerSprite = new Image();
            EnemySprite = new Image();
            PlayerPokemon = new Image();

            Background.Path = "BattleScreen/BattleBackground1";
            EnemyPlatform.Path = "BattleScreen/BattleBackground1EnemyPlatform";
            PlayerPlatform.Path = "BattleScreen/BattleBackground1PlayerPlatform";
            PlayerSprite.Path = Player.PlayerJsonObject.Gender == Gender.MALE ? "BattleScreen/BattleRedSpriteSheet" : "BattleScreen/BattleBackground1";
            PlayerSprite.Effects = "SpriteSheetEffect";
            EnemySprite = TextBox.wildEncounterPoke.Front;

            Background.LoadContent();

            EnemyPlatform.LoadContent();
            EnemySprite.LoadContent();
            PlayerPlatform.LoadContent();
            PlayerSprite.LoadContent();
            TextBox.LoadContent();
            Pokeball.LoadContent();
            PlayerHPBar.LoadContent();
            EnemyHPBar.LoadContent();

            PlayerSprite.SpriteSheetEffect.AmountOfFrames = new Vector2(5, 1);
            PlayerSprite.SpriteSheetEffect.CurrentFrame = Vector2.Zero;


            /* POKEBALL TRANSITION CODE */
            whiteBackground = new Image();
            whiteBackground.Texture = new Texture2D(ScreenManager.Instance.GraphicsDevice, Background.SourceRect.Width, Background.SourceRect.Height);
            whiteBackground.LoadContent();
            Color[] data = new Color[Background.SourceRect.Width * Background.SourceRect.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.White;
            whiteBackground.Texture.SetData(data);
            whiteBackground.Alpha = 0;
            /* END POKEBALL TRANSITION CODE */

            state = BattleState.INTRO;

            if (Wild)
                EnemySprite.Tint = Color.LightGray;

            EnemyPlatform.Position = new Vector2(-EnemyPlatform.SourceRect.Width, 192);
            EnemySprite.Position = new Vector2(EnemyPlatform.Position.X + EnemyPlatform.SourceRect.Width / 2 - EnemySprite.SourceRect.Width / 2, EnemyPlatform.Position.Y - EnemySprite.SourceRect.Height / 3);
            PlayerPlatform.Position = new Vector2(ScreenManager.Instance.Dimensions.X + PlayerPlatform.SourceRect.Width, TextBox.Border.Position.Y - PlayerPlatform.SourceRect.Height);
            PlayerSprite.Position = new Vector2(PlayerPlatform.Position.X + PlayerPlatform.SourceRect.Width / 2 - 48, PlayerPlatform.Position.Y + PlayerPlatform.SourceRect.Height - PlayerSprite.SourceRect.Height);
            EnemyHPBar.Position = new Vector2(-EnemyHPBar.SourceRect.Width, EnemyPlatform.Position.Y - EnemyHPBar.SourceRect.Height - 12);
            PlayerHPBar.Position = new Vector2(ScreenManager.Instance.Dimensions.X, TextBox.Border.Position.Y - PlayerHPBar.SourceRect.Height - 4);
            IsTransitioning = true;
            

            CustomPokemon poke = PokemonManager.createRandomPokemon(PokemonManager.Instance.GetPokemon("Bulbasaur"), 100);
            PlayerPokemon = poke.Pokemon.Back;
            PlayerPokemon.Scale = new Vector2(0.01f, 0.01f);
            PlayerPokemon.LoadContent();
            PlayerPokemon.Tint = Color.Red;
            Console.WriteLine(poke.Stats.HP);
            Console.WriteLine(poke.Stats.Attack);
            Console.WriteLine(poke.Stats.Defense);
            Console.WriteLine(poke.Stats.SpecialAttack);
            Console.WriteLine(poke.Stats.SpecialDefense);
            Console.WriteLine(poke.Stats.Speed);
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            Background.UnloadContent();
            EnemyPlatform.UnloadContent();
            EnemySprite.UnloadContent();
            PlayerPlatform.UnloadContent();
            PlayerSprite.UnloadContent();
            PlayerPokemon.UnloadContent();
            TextBox.UnloadContent();
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Transition(gameTime);
            PlayerSprite.Update(gameTime);
            
            if (InputManager.Instance.KeyPressed(Keys.K) && !IsTransitioning)
                ScreenManager.Instance.ChangeScreens("GameplayScreen");

            if (TextBox.Page == 4 && !menuManager.IsLoaded)
                menuManager.LoadContent("Load/Menus/BattleMenu.xml");
            else if (TextBox.Page != 4 && menuManager.IsLoaded)
                menuManager.UnloadContent();

            if (menuManager.IsLoaded)
                menuManager.Update(gameTime);
            Player.ElapsedTime += (double)gameTime.ElapsedGameTime.TotalSeconds / 3600;
            if ((!IsTransitioning && !ScreenManager.Instance.IsTransitioning) || state == BattleState.WILD_POKEMON_FADE_IN)
                TextBox.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Background.Draw(spriteBatch);
            PlayerPlatform.Draw(spriteBatch);
            EnemyPlatform.Draw(spriteBatch);
            if (state == BattleState.PLAYER_SEND_POKEMON && PlayerSprite.SpriteSheetEffect.CurrentFrame.X == 4)
                whiteBackground.Draw(spriteBatch);
            PlayerHPBar.Draw(spriteBatch);
            EnemyHPBar.Draw(spriteBatch);
            PlayerSprite.Draw(spriteBatch);
            if (Pokeball.Position.Y >= TextBox.Border.Position.Y)
                PlayerPokemon.Draw(spriteBatch);
            EnemySprite.Draw(spriteBatch);
            if (state == BattleState.PLAYER_SEND_POKEMON && PlayerSprite.SpriteSheetEffect.CurrentFrame.X == 4)
                Pokeball.Draw(spriteBatch);
            
            TextBox.Draw(spriteBatch);
            if (menuManager.IsLoaded)
                menuManager.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
