using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PokemonFireRedClone
{
    public class PokemonInfo : SummaryPage
    {

        private readonly PokemonText pokedexNum,
            pokemonName, playerName, trainerID, heldItem, nature, meeting;
        private readonly List<Image> types;

        public PokemonInfo(CustomPokemon pokemon)
            : base(pokemon)
        {
            Background.Path = "Menus/SummaryMenu/PokemonInfoBackground";

            string font = "Fonts/PokemonFireRedDialogue";
            Color fontColor = new(81,81,81);
            Color shadowColor = new(225,225,206);

            pokedexNum = new PokemonText(pokemon.Pokemon.Index.ToString().PadLeft(3, '0'), font, fontColor, shadowColor);
            pokemonName = new PokemonText(pokemon.Pokemon.Name.ToUpper(), font, fontColor, shadowColor);

            types = new List<Image>();
            foreach (Type type in pokemon.Pokemon.Types)
                types.Add(TypeProperties.ImageOf(type));


            playerName = new PokemonText(Player.PlayerJsonObject.Name, font, fontColor, shadowColor);
            trainerID = new PokemonText(Player.PlayerJsonObject.TrainerID, font, fontColor, shadowColor);
            heldItem = new PokemonText("NONE", font, fontColor, shadowColor);
            nature = new PokemonText(pokemon.Nature.ToString() + "   nature .", font, fontColor, shadowColor);
            meeting = new PokemonText("Met   in   PALLET   TOWN   at   Lv   5 .", font, fontColor, shadowColor);

        }

        public override void LoadContent()
        {
            base.LoadContent();
            pokedexNum.LoadContent();
            pokedexNum.SetPosition(new Vector2(668,80));
            pokemonName.LoadContent();
            pokemonName.SetPosition(new Vector2(pokedexNum.Position.X, pokedexNum.Position.Y + pokedexNum.SourceRect.Height));
            int dimensionX = 0;
            foreach (Image image in types)
            {
                image.LoadContent();
                image.Position = new Vector2(pokemonName.Position.X + dimensionX, pokemonName.Position.Y + pokemonName.SourceRect.Height + 12);
                dimensionX += image.SourceRect.Width + 16;
            }
            playerName.LoadContent();
            playerName.SetPosition(new Vector2(types[0].Position.X, types[0].Position.Y + types[0].SourceRect.Height + 4));
            trainerID.LoadContent();
            trainerID.SetPosition(new Vector2(playerName.Position.X, playerName.Position.Y + pokemonName.SourceRect.Height + 4));
            heldItem.LoadContent();
            heldItem.SetPosition(new Vector2(trainerID.Position.X, trainerID.Position.Y + trainerID.SourceRect.Height + 4));

            nature.LoadContent();
            nature.SetPosition(new Vector2(32, 520));
            meeting.LoadContent();
            meeting.SetPosition(new Vector2(nature.Position.X, nature.Position.Y + nature.SourceRect.Height));
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            pokedexNum.UnloadContent();
            pokemonName.UnloadContent();
            playerName.UnloadContent();
            foreach (Image image in types)
                image.UnloadContent();
            trainerID.UnloadContent();
            heldItem.UnloadContent();
            nature.UnloadContent();
            meeting.UnloadContent();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            pokedexNum.Draw(spriteBatch);
            pokemonName.Draw(spriteBatch);
            foreach (Image image in types)
                image.Draw(spriteBatch);
            playerName.Draw(spriteBatch);
            trainerID.Draw(spriteBatch);
            heldItem.Draw(spriteBatch);
            nature.Draw(spriteBatch);
            meeting.Draw(spriteBatch);
        }

    }
}
