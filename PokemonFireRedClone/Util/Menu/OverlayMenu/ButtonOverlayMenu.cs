using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class ButtonOverlayMenu : OverlayMenu
    {
        bool inBattle
        {
            get { return BattleLogic.Battle != null && BattleLogic.Battle.InBattle; }
            set { }
        }

        public int SelectedIndex;

        public override void LoadContent()
        {
            string path = SelectedIndex == 0 && inBattle ? "Menus/PokemonMenu/StarterBattleButtonMenu" : "Menus/PokemonMenu/BattleButtonMenu";
            if (inBattle)
            {
                if (Background.IsLoaded)
                   Background.ReloadTexture(path);
                else
                    Background.Path = path;
                Items[1].Image.Text = Items[0].Image.Text;
                Items[0].Image.Text = "SHIFT";
                for (int i = 2; i < Items.Count - 1; i++)
                    Items.RemoveAt(i);
            }

            base.LoadContent();

            if (SelectedIndex == 0 && inBattle)
                itemNumber = 1;
        }

        public void Update()
        {
            if (IsOpen)
            {
                if (InputManager.Instance.KeyPressed(Keys.Q))
                    UnloadContent();
                else if (InputManager.Instance.KeyPressed(Keys.S))
                    itemNumber++;
                else if (InputManager.Instance.KeyPressed(Keys.W))
                    itemNumber--;
                else if (InputManager.Instance.KeyPressed(Keys.E)) {
                    PokemonMenu.SelectedIndex = SelectedIndex;
                    switch (itemNumber)
                    {
                        case 0:
                            if (inBattle)
                            {
                                BattleLogic.PlayerShift = true;
                                ScreenManager.Instance.ChangeScreens("BattleScreen");
                            } else
                                ScreenManager.Instance.ChangeScreens("SummaryScreen");
                            
                            break;
                        case 1:
                            if (inBattle)
                                ScreenManager.Instance.ChangeScreens("SummaryScreen");
                            else
                            {

                            }
                            break;
                        case 2:
                            if (inBattle)
                                UnloadContent();
                            else
                            {

                            }
                            break;
                        case 3:
                            UnloadContent();
                            break;
                        default:
                            break;
                    }
                }

                int smallestIndex = SelectedIndex == 0 && inBattle ? 1 : 0;
                if (itemNumber < smallestIndex)
                    itemNumber = Items.Count - 1;
                else if (itemNumber > Items.Count - 1)
                    itemNumber = smallestIndex;

                for (int i = smallestIndex; i < Items.Count; i++)
                {
                    if (i == itemNumber)
                    {
                        Items[i].Image.IsActive = true;
                        Arrow.Position = new Vector2(Items[i].Image.Position.X - Arrow.SourceRect.Width,
                            Items[i].Image.Position.Y + (Items[i].Image.SourceRect.Height / 4) - 2);
                    }
                    else
                        Items[i].Image.IsActive = false;

                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (SelectedIndex == 0 && inBattle && IsOpen)
            {
                Background.Draw(spriteBatch);
                Arrow.Draw(spriteBatch);
                for (int i = 1; i < Items.Count; i++)
                    Items[i].Image.Draw(spriteBatch);
            }
            else
                base.Draw(spriteBatch);
        }

    }
}
