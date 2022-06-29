using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class BattleLevelUp
    {

        private int page;
        private Image levelUpBorder;
        private List<List<Image>> levelUpPages;

        public bool IsActive;

        public void LoadContent(CustomPokemon pokemon, int level)
        {
            IsActive = true;
            page = 1;
            levelUpBorder = new Image
            {
                Path = "BattleScreen/levelUpBorder"
            };
            levelUpPages = new List<List<Image>>(2)
            {
                new List<Image>(),
                new List<Image>()
            };

            for (int i = 0; i < 6; i++)
            {
                levelUpPages[0].Add(new Image());
                levelUpPages[0][i].FontName = "Fonts/PokemonFireRedDialogue";
                levelUpPages[0][i].R = levelUpPages[0][i].G = levelUpPages[0][i].B = 81;
                levelUpPages[0][i].Text = "+     ";
                levelUpPages[1].Add(new Image());
                levelUpPages[1][i].FontName = "Fonts/PokemonFireRedDialogue";
                levelUpPages[1][i].R = levelUpPages[1][i].G = levelUpPages[1][i].B = 81;
            }

            LoadLevelInfo(pokemon, level);

            levelUpBorder.LoadContent();
            levelUpBorder.Position = new Vector2(ScreenManager.Instance.Dimensions.X - levelUpBorder.SourceRect.Width - 4,
                ScreenManager.Instance.Dimensions.Y - levelUpBorder.SourceRect.Height - 4);

            foreach (List<Image> images in levelUpPages)
            {
                int dimensionY = 0;
                foreach (Image image in images)
                {
                    image.LoadContent();
                    image.Position = new Vector2(levelUpBorder.Position.X + 348 - image.SourceRect.Width, levelUpBorder.Position.Y + 26 + dimensionY);
                    dimensionY += 60;
                }
            }
        }

        void UnloadContent()
        {
            levelUpBorder.UnloadContent();
            foreach (List<Image> images in levelUpPages)
            {
                foreach (Image image in images)
                    image.UnloadContent();
            }
            IsActive = false;
            ScreenManager.Instance.BattleScreen.BattleAssets.State = BattleAssets.BattleState.EXP_ANIMATION;
            ScreenManager.Instance.BattleScreen.BattleAssets.Animation = new EXPAnimation();
            ScreenManager.Instance.BattleScreen.BattleAssets.IsTransitioning = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                levelUpBorder.Draw(spriteBatch);
                foreach (Image image in levelUpPages[page - 1])
                    image.Draw(spriteBatch);
            }
        }

        private void LoadLevelInfo(CustomPokemon pokemon, int level)
        {
            StatList newStats = PokemonManager.Instance.StatsOfLevel(pokemon, level);
            StatList oldStats = PokemonManager.Instance.StatsOfLevel(pokemon, level - 1);

            levelUpPages[0][0].Text += newStats.HP - oldStats.HP;
            levelUpPages[0][1].Text += newStats.Attack - oldStats.Attack;
            levelUpPages[0][2].Text += newStats.Defense - oldStats.Defense;
            levelUpPages[0][3].Text += newStats.SpecialAttack - oldStats.SpecialAttack;
            levelUpPages[0][4].Text += newStats.SpecialDefense - oldStats.SpecialDefense;
            levelUpPages[0][5].Text += newStats.Speed - oldStats.Speed;

            levelUpPages[1][0].Text = newStats.HP.ToString();
            levelUpPages[1][1].Text = newStats.Attack.ToString();
            levelUpPages[1][2].Text = newStats.Defense.ToString();
            levelUpPages[1][3].Text = newStats.SpecialAttack.ToString();
            levelUpPages[1][4].Text = newStats.SpecialDefense.ToString();
            levelUpPages[1][5].Text = newStats.Speed.ToString();
        }

        public void NextPage()
        {
            if (IsActive)
            {
                if (page == 2)
                    UnloadContent();
                else
                    page++;
            }
        }

    }
}
