using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class PokemonAssets
    {
        public Image Name;
        public Image Gender;
        public Image CurrentHP;
        public Image MaxHP;
        public Image HPBar;
        public Image Level;

        CustomPokemon pokemon;
        bool player;
        bool HPLoaded;

        public PokemonAssets(CustomPokemon pokemon, bool player)
        {
            this.pokemon = pokemon;
            this.player = player;
        }

        public void LoadContent(string font, Color fontColor)
        {
            initializeImages(font, fontColor);

            Name.LoadContent();
            if (Gender != null)
                Gender.LoadContent();
            SetUpHealthBar();
            HPBar.LoadContent();
            Level.LoadContent();

            if (player)
            {
                CurrentHP.LoadContent();
                MaxHP.LoadContent();
            }
            HPLoaded = true;
        }

        public void UnloadContent()
        {
            Name.UnloadContent();
            if (Gender != null)
                Gender.UnloadContent();
            HPBar.UnloadContent();
            Level.UnloadContent();

            if (player)
            {
                CurrentHP.UnloadContent();
                MaxHP.UnloadContent();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Name.Draw(spriteBatch);
            if (Gender != null)
                Gender.Draw(spriteBatch);
            HPBar.Draw(spriteBatch);
            Level.Draw(spriteBatch);

            if (player)
            {
                CurrentHP.Draw(spriteBatch);
                MaxHP.Draw(spriteBatch);
            }

        }

        void initializeImages(string font, Color fontColor)
        {
            Name = new Image
            {
                FontName = font,
                UseFontColor = true,
                FontColor = fontColor,
                Text = pokemon.Name
            };
            if (pokemon.Gender != PokemonFireRedClone.Gender.GENDERLESS && !pokemon.Name.Contains("Nidoran"))
            {
                Gender = new Image
                {
                    FontName = font,
                    UseFontColor = true,
                    FontColor = pokemon.Gender == PokemonFireRedClone.Gender.MALE ? new Color(119, 208, 250, 255) : new Color(243, 169, 161, 255),
                    Text = pokemon.Gender == PokemonFireRedClone.Gender.MALE ? "♂" : "♀"
                };
            }
            HPBar = new Image
            {
                Path = "BattleScreen/HPBar"
            };
            Level = new Image
            {
                FontName = font,
                UseFontColor = true,
                FontColor = fontColor,
                Text = "Lv" + pokemon.Level
            };

            if (player)
            {
                MaxHP = new Image
                {
                    FontName = font,
                    UseFontColor = true,
                    FontColor = fontColor,
                    Text = pokemon.Stats.HP.ToString()
                };

                CurrentHP = new Image
                {
                    FontName = font,
                    UseFontColor = true,
                    FontColor = fontColor,
                    Text = pokemon.CurrentHP.ToString()
                };
            }

        }

        public void SetUpHealthBar()
        {
            float healthRatio = (float)pokemon.CurrentHP / pokemon.Stats.HP;

            HPBar.Scale.X = healthRatio;

            calculateHealthBarColor(healthRatio);

        }

        public void ScaleEXPBar(Image image)
        {
            float expRatio = (float)pokemon.EXPTowardsLevelUp / pokemon.EXPNeededToLevelUp;
            image.Scale.X = expRatio;
        }

        void calculateHealthBarColor(float ratio)
        {
            if (ratio > 0.5)
            {
                HPBar.Tint = new Color(175, 252, 175, 1);
                HPBar.Alpha = 0.5f;
            }
            else if (ratio > 0.2 && ratio <= 0.5)
            {
                HPBar.Tint = new Color(255, 232, 0, 100);
                HPBar.Alpha = 0.57f;
            }
            else
            {
                HPBar.Tint = new Color(255, 100, 0, 50);
                HPBar.Alpha = 0.4f;
            }

            if (HPLoaded)
                HPBar.ReloadTexture();

        }

    }
}
