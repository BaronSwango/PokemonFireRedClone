using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class PokemonMenuStarterInfoButton : PokemonMenuInfoButton
    {

        public PokemonMenuStarterInfoButton(CustomPokemon pokemon)
            : base(pokemon)
        {
            State = ButtonState.SELECTED;
            Bounce = true;
        }

        public override void UpdateInfoPositions(GameTime gameTime)
        {
            PokemonAssets.Name.SetPosition(new Vector2(BackgroundInUse.Position.X + 112, BackgroundInUse.Position.Y + 84));
            PokemonAssets.Level.SetPosition(new Vector2(PokemonAssets.Name.Position.X + 32, PokemonAssets.Name.Position.Y + PokemonAssets.Name.SourceRect.Height + 8)); 
            if (PokemonAssets.Gender != null)
                PokemonAssets.Gender.SetPosition(new Vector2(BackgroundInUse.Position.X + BackgroundInUse.SourceRect.Width - 28 - PokemonAssets.Gender.SourceRect.Width, PokemonAssets.Level.Position.Y));
            PokemonAssets.MaxHP.SetPosition(new Vector2(BackgroundInUse.Position.X + BackgroundInUse.SourceRect.Width - 21 - PokemonAssets.MaxHP.SourceRect.Width, BackgroundInUse.Position.Y + BackgroundInUse.SourceRect.Height - 16 - PokemonAssets.MaxHP.SourceRect.Height));
            PokemonAssets.CurrentHP.SetPosition(new Vector2(BackgroundInUse.Position.X + BackgroundInUse.SourceRect.Width - 101 - PokemonAssets.CurrentHP.SourceRect.Width, PokemonAssets.MaxHP.Position.Y));
            PokemonAssets.HPBar.Position = new Vector2(BackgroundInUse.Position.X + 120 - ((1 - PokemonAssets.HPBar.Scale.X) / 2 * PokemonAssets.HPBar.SourceRect.Width), BackgroundInUse.Position.Y + 164);
            if (!SpritePositioned)
            {
                MenuSprite.Position = new Vector2(BackgroundInUse.Position.X + 24 - (MenuSprite.SourceRect.Width / 4), BackgroundInUse.Position.Y + 56);
                SpritePositioned = true;
            }
            if (State == ButtonState.SELECTED || State == ButtonState.SWITCH_SELECTED)
            {
                MenuSprite.IsActive = false;
                //float CounterSpeed = (float)(gameTime.ElapsedGameTime.TotalMilliseconds * 8);
                //Counter += CounterSpeed;
                Counter.Update(gameTime);
                //if (Counter > 1000)
                if (Counter.Finished)
                {
                    if (Bounce)
                    {
                        MenuSprite.SpriteSheetEffect.CurrentFrame.X = 0;
                        MenuSprite.Position = new Vector2(BackgroundInUse.Position.X + 24 - (MenuSprite.SourceRect.Width / 4), BackgroundInUse.Position.Y + 72);
                        Bounce = false;
                    }
                    else
                    {
                        MenuSprite.SpriteSheetEffect.CurrentFrame.X = 1;
                        MenuSprite.Position = new Vector2(BackgroundInUse.Position.X + 24 - (MenuSprite.SourceRect.Width / 4), BackgroundInUse.Position.Y + 56);
                        Bounce = true;
                    }
                    //Counter = 0;
                    Counter.Reset();
                }
            }
            else
            {
                MenuSprite.IsActive = true;
                Bounce = true;
                MenuSprite.Position = new Vector2(BackgroundInUse.Position.X + 24 - (MenuSprite.SourceRect.Width / 4), BackgroundInUse.Position.Y + 56);
            }
        }

        protected override void LoadBackground()
        {
            base.LoadBackground();
            BackgroundUnselected.Path = "Menus/PokemonMenu/PokemonMenuMainButton";
            BackgroundSelected.Path = "Menus/PokemonMenu/PokemonMenuMainButtonSelected";
            BackgroundSwitchOriginal.Path = "Menus/PokemonMenu/PokemonMenuMainButtonSwitchOriginal";
            BackgroundSwitchSelected.Path = "Menus/PokemonMenu/PokemonMenuMainButtonSwitchSelected";
        }
    }
}
