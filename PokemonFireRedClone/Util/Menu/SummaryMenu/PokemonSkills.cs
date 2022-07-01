using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class PokemonSkills : SummaryPage
    {

        private readonly PokemonText health,
            attack, defense, spAttack, spDefense, speed, totalEXP, levelEXP, ability, abilityDesc;

        private readonly Image expBar;

        public PokemonSkills(CustomPokemon pokemon)
            :base(pokemon)
        {
            Background.Path = "Menus/SummaryMenu/PokemonSkillsBackground";

            string font = "Fonts/PokemonFireRedDialogue";
            Color fontColor = new(81, 81, 81);
            Color shadowColor = new(225, 225, 206);

            health = new PokemonText(pokemon.CurrentHP + "/" + pokemon.Stats.HP, font, fontColor, shadowColor);
            attack = new PokemonText(pokemon.Stats.Attack.ToString(), font, fontColor, shadowColor);
            defense = new PokemonText(pokemon.Stats.Defense.ToString(), font, fontColor, shadowColor);
            spAttack = new PokemonText(pokemon.Stats.SpecialAttack.ToString(), font, fontColor, shadowColor);
            spDefense = new PokemonText(pokemon.Stats.SpecialDefense.ToString(), font, fontColor, shadowColor);
            speed = new PokemonText(pokemon.Stats.Speed.ToString(), font, fontColor, shadowColor);

            totalEXP = new PokemonText(pokemon.CurrentEXP.ToString(), font, fontColor, shadowColor);
            levelEXP = new PokemonText((pokemon.EXPNeededToLevelUp - pokemon.EXPTowardsLevelUp).ToString(), font, fontColor, shadowColor);

            expBar = new Image
            {
                Path = "Menus/SummaryMenu/PokemonSkillsEXPBar"
            };

            ability = new PokemonText("ABILITY", font, fontColor, shadowColor);
            abilityDesc = new PokemonText("Ability   description   goes   here .", font, fontColor, shadowColor);
        }

        //TODO: fix healthbar color, change text to PokemonText obj, remove slash from menu background
        public override void LoadContent()
        {
            base.LoadContent();

            float xCoord = 948;

            health.LoadContent();
            health.SetPosition(new Vector2(xCoord - health.SourceRect.Width, 76)); 

            attack.LoadContent();
            attack.SetPosition(new Vector2(xCoord - attack.SourceRect.Width, health.Position.Y + health.SourceRect.Height + 16));

            defense.LoadContent();
            defense.SetPosition(new Vector2(xCoord - defense.SourceRect.Width, attack.Position.Y + attack.SourceRect.Height - 4));

            spAttack.LoadContent();
            spAttack.SetPosition(new Vector2(xCoord - spAttack.SourceRect.Width, defense.Position.Y + defense.SourceRect.Height - 4));

            spDefense.LoadContent();
            spDefense.SetPosition(new Vector2(xCoord - spDefense.SourceRect.Width, spAttack.Position.Y + spAttack.SourceRect.Height - 4));

            speed.LoadContent();
            speed.SetPosition(new Vector2(xCoord - speed.SourceRect.Width, spDefense.Position.Y + spDefense.SourceRect.Height - 4));

            xCoord += 4;

            totalEXP.LoadContent();
            totalEXP.SetPosition(new Vector2(xCoord - totalEXP.SourceRect.Width, speed.Position.Y + speed.SourceRect.Height + 44));

            levelEXP.LoadContent();
            levelEXP.SetPosition(new Vector2(xCoord - levelEXP.SourceRect.Width, totalEXP.Position.Y + totalEXP.SourceRect.Height - 4));

            PokemonAssets.HPBar.Position = new Vector2(736 - ((1 - PokemonAssets.HPBar.Scale.X) / 2 * PokemonAssets.HPBar.SourceRect.Width), health.Position.Y + health.SourceRect.Height + 4);

            expBar.LoadContent();
            expBar.Position = new Vector2(levelEXP.Position.X + levelEXP.SourceRect.Width - expBar.SourceRect.Width - 24, levelEXP.Position.Y + levelEXP.SourceRect.Height + 4);
            expBar.SourceRect.Width = (int)(expBar.SourceRect.Width * ((float)Pokemon.EXPTowardsLevelUp / Pokemon.EXPNeededToLevelUp));

            ability.LoadContent();
            ability.SetPosition(new Vector2(296, expBar.Position.Y - 8));

            abilityDesc.LoadContent();
            abilityDesc.SetPosition(new Vector2(40, ability.Position.Y + ability.SourceRect.Height));
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            health.UnloadContent();
            attack.UnloadContent();
            defense.UnloadContent();
            spAttack.UnloadContent();
            spDefense.UnloadContent();
            speed.UnloadContent();
            totalEXP.UnloadContent();
            levelEXP.UnloadContent();
            expBar.UnloadContent();
            ability.UnloadContent();
            abilityDesc.UnloadContent();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            health.Draw(spriteBatch);
            attack.Draw(spriteBatch);
            defense.Draw(spriteBatch);
            spAttack.Draw(spriteBatch);
            spDefense.Draw(spriteBatch);
            speed.Draw(spriteBatch);

            totalEXP.Draw(spriteBatch);
            levelEXP.Draw(spriteBatch);

            PokemonAssets.HPBar.Draw(spriteBatch);
            expBar.Draw(spriteBatch);

            ability.Draw(spriteBatch);
            abilityDesc.Draw(spriteBatch);
        }
    }
}
