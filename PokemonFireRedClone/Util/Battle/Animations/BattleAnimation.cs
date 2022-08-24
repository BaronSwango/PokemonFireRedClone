using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public abstract class BattleAnimation
    {

        protected float Counter;
        protected float CounterSpeed;
        protected Image WhiteEffect;
        protected bool WhiteEffectTransitioned;

        public BattleAssets BattleAssets
        {
            get { return ScreenManager.Instance.BattleScreen.BattleAssets; }
            private set { }
        }

        public abstract bool Animate(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

        protected void CreateWhiteEffect()
        {
            WhiteEffect = new Image
            {
                Texture = new Texture2D(ScreenManager.Instance.GraphicsDevice, ScreenManager.Instance.BattleScreen.BattleAssets.Background.SourceRect.Width, ScreenManager.Instance.BattleScreen.BattleAssets.Background.SourceRect.Height)
            };
            Color[] data = new Color[ScreenManager.Instance.BattleScreen.BattleAssets.Background.SourceRect.Width * ScreenManager.Instance.BattleScreen.BattleAssets.Background.SourceRect.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.White;
            WhiteEffect.Texture.SetData(data);
            WhiteEffect.Alpha = 0;
            WhiteEffect.LoadContent();
        }

        protected void ResetPokeball() {
            BattleAssets.Pokeball.Alpha = 0;
            BattleAssets.Pokeball.Position = new Vector2(156, 352);
        }

        protected void EndFightSequence()
        {
            ScreenManager.Instance.BattleScreen.MenuManager.MenuName = "BattleMenu";
            ScreenManager.Instance.BattleScreen.MenuManager.Menu.ID = "Load/Menus/BattleMenu.xml";
            ScreenManager.Instance.BattleScreen.TextBox.NextPage = 4;
            ScreenManager.Instance.BattleScreen.TextBox.IsTransitioning = true;
            ScreenManager.Instance.BattleScreen.BattleLogic.EnemyHasMoved = false;
            ScreenManager.Instance.BattleScreen.BattleLogic.PlayerHasMoved = false;
            ScreenManager.Instance.BattleScreen.BattleLogic.PlayerMoveUsed = false;
            ScreenManager.Instance.BattleScreen.BattleLogic.StatStageIncrease = false;
            ScreenManager.Instance.BattleScreen.BattleLogic.Stat = "";
            ScreenManager.Instance.BattleScreen.BattleLogic.SharplyStat = false;
            ScreenManager.Instance.BattleScreen.BattleLogic.PlayerMoveExecuted = false;
            ScreenManager.Instance.BattleScreen.BattleLogic.EnemyMoveExecuted = false;
            ScreenManager.Instance.BattleScreen.BattleLogic.State = BattleLogic.FightState.NONE;
            ScreenManager.Instance.BattleScreen.BattleAssets.State = BattleAssets.BattleState.BATTLE_MENU;
        }


    }
}
