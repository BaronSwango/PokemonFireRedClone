using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class BattleLevelUp
    {
        public bool IsActive;

        int Page;
        Image LevelUpBorder;
        List<List<Image>> LevelUpPages;

        BattleScreen battleScreen
        {
            get { return (BattleScreen) ScreenManager.Instance.CurrentScreen; }
            set { }
        }

        public void LoadContent(CustomPokemon pokemon, int level)
        {
            IsActive = true;
            Page = 1;
            LevelUpBorder = new Image();
            LevelUpBorder.Path = "BattleScreen/LevelUpBorder";
            LevelUpPages = new List<List<Image>>(2);
            LevelUpPages.Add(new List<Image>());
            LevelUpPages.Add(new List<Image>());

            for (int i = 0; i < 6; i++)
            {
                LevelUpPages[0].Add(new Image());
                LevelUpPages[0][i].FontName = "Fonts/PokemonFireRedDialogue";
                LevelUpPages[0][i].R = LevelUpPages[0][i].G = LevelUpPages[0][i].B = 81;
                LevelUpPages[0][i].Text = "+     ";
                LevelUpPages[1].Add(new Image());
                LevelUpPages[1][i].FontName = "Fonts/PokemonFireRedDialogue";
                LevelUpPages[1][i].R = LevelUpPages[1][i].G = LevelUpPages[1][i].B = 81;
            }

            loadLevelInfo(pokemon, level);

            LevelUpBorder.LoadContent();
            LevelUpBorder.Position = new Vector2(ScreenManager.Instance.Dimensions.X - LevelUpBorder.SourceRect.Width - 4,
                ScreenManager.Instance.Dimensions.Y - LevelUpBorder.SourceRect.Height - 4);

            foreach (List<Image> images in LevelUpPages)
            {
                int dimensionY = 0;
                foreach (Image image in images)
                {
                    image.LoadContent();
                    image.Position = new Vector2(LevelUpBorder.Position.X + 348 - image.SourceRect.Width, LevelUpBorder.Position.Y + 26 + dimensionY);
                    dimensionY += 60;
                }
            }
        }

        void UnloadContent()
        {
            LevelUpBorder.UnloadContent();
            foreach (List<Image> images in LevelUpPages)
            {
                foreach (Image image in images)
                    image.UnloadContent();
            }
            IsActive = false;
            battleScreen.BattleAnimations.state = BattleAnimations.BattleState.EXP_ANIMATION;
            battleScreen.BattleAnimations.IsTransitioning = true;
        }

        public void NextPage()
        {
            if (IsActive)
            {
                if (Page == 2)
                    UnloadContent();
                else
                    Page++;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                LevelUpBorder.Draw(spriteBatch);
                foreach (Image image in LevelUpPages[Page - 1])
                    image.Draw(spriteBatch);
            }
        }

        void loadLevelInfo(CustomPokemon pokemon, int level)
        {
            StatList newStats = PokemonManager.Instance.StatsOfLevel(pokemon, level);
            StatList oldStats = PokemonManager.Instance.StatsOfLevel(pokemon, level - 1);

            LevelUpPages[0][0].Text += newStats.HP - oldStats.HP;
            LevelUpPages[0][1].Text += newStats.Attack - oldStats.Attack;
            LevelUpPages[0][2].Text += newStats.Defense - oldStats.Defense;
            LevelUpPages[0][3].Text += newStats.SpecialAttack - oldStats.SpecialAttack;
            LevelUpPages[0][4].Text += newStats.SpecialDefense - oldStats.SpecialDefense;
            LevelUpPages[0][5].Text += newStats.Speed - oldStats.Speed;

            LevelUpPages[1][0].Text = newStats.HP.ToString();
            LevelUpPages[1][1].Text = newStats.Attack.ToString();
            LevelUpPages[1][2].Text = newStats.Defense.ToString();
            LevelUpPages[1][3].Text = newStats.SpecialAttack.ToString();
            LevelUpPages[1][4].Text = newStats.SpecialDefense.ToString();
            LevelUpPages[1][5].Text = newStats.Speed.ToString();
        }

    }
}
