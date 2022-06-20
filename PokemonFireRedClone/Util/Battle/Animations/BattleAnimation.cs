using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public abstract class BattleAnimation
    {

        protected BattleScreen BattleScreen
        {
            get { return (BattleScreen)ScreenManager.Instance.CurrentScreen; }
            set { }
        }

        protected BattleAnimations BattleAnimations
        {
            get { return BattleScreen.BattleAnimations; }
            set { }
        }

        protected float counter;
        protected float counterSpeed;

        public abstract bool Animate(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);


        protected void resetPokeball() {
            BattleAnimations.Pokeball.Alpha = 0;
            BattleAnimations.Pokeball.Position = new Vector2(156, 352);
        }

        protected void endFightSequence()
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
            BattleAnimations.State = BattleAnimations.BattleState.BATTLE_MENU;
        }


    }
}
