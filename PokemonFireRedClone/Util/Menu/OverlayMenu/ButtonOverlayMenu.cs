using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PokemonFireRedClone
{
    public class ButtonOverlayMenu : OverlayMenu
    {
        bool InBattle
        {
            get { return ScreenManager.Instance.CurrentScreen is BattleScreen; }
            set { }
        }

        public override void LoadContent()
        {
            if (InBattle)
            {
                Background.Path = "Menus/PokemonMenu/BattleButtonMenu";
                Items[1].Image.Text = Items[0].Image.Text;
                Items[0].Image.Text = "SHIFT";
                for (int i = 2; i < Items.Count - 1; i++)
                    Items.RemoveAt(i);
            }
            base.LoadContent();
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
                    switch(itemNumber)
                    {
                        case 0:
                            if (InBattle)
                            {

                            } else
                            {

                            }
                            break;
                        case 1:
                            if (InBattle)
                            {

                            }
                            else
                            {

                            }
                            break;
                        case 2:
                            if (InBattle)
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

                if (itemNumber < 0)
                    itemNumber = Items.Count - 1;
                else if (itemNumber > Items.Count - 1)
                    itemNumber = 0;

                for (int i = 0; i < Items.Count; i++)
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

    }
}
