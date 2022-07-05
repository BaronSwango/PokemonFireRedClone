using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class KnownMoves : SummaryPage
    {
        private readonly List<Image> types;
        private readonly List<PokemonText> moveNames;
        private readonly List<PokemonText> movePPs;
        private readonly List<PokemonText> pps;

        public KnownMoves(CustomPokemon pokemon)
            : base(pokemon)
        {
            Background.Path = "Menus/SummaryMenu/KnownMovesBackground";

            types = new List<Image>();
            moveNames = new List<PokemonText>();
            pps = new List<PokemonText>();
            movePPs = new List<PokemonText>();

            foreach (Move move in pokemon.Moves.Keys)
            {
                types.Add(TypeProperties.ImageOf(move.Type));
                moveNames.Add(new PokemonText(move.Name.ToUpper(), "Fonts/PokemonFireRedDialogue", new Color(49, 49, 49), new Color(225, 225, 225)));
                movePPs.Add(new PokemonText(pokemon.Moves[move]+"/"+move.PP, "Fonts/PokemonFireRedDialogue", new Color(49, 49, 49), new Color(225, 225, 225)));
            }

            for (int i = pokemon.Moves.Count; i < 4; i++)
            {
                moveNames.Add(new PokemonText("-", "Fonts/PokemonFireRedDialogue", new Color(49, 49, 49), new Color(225, 225, 225)));
                movePPs.Add(new PokemonText("--", "Fonts/PokemonFireRedDialogue", new Color(49, 49, 49), new Color(225, 225, 225)));
            }

            for (int i = 0; i < 4; i++)
                pps.Add(new PokemonText("PP", "Fonts/PokemonFireRedSmall", new Color(49, 49, 49), new Color(225, 225, 225)));
            

        }

        public override void LoadContent()
        {
            base.LoadContent();
            int yPad = 0;
            foreach (Image image in types)
            {
                image.LoadContent();
                image.Position = new Vector2(492, 84 + yPad);
                yPad += image.SourceRect.Height + 64;
            }

            yPad = 0;

            foreach (PokemonText text in moveNames)
            {
                text.LoadContent();
                text.SetPosition(new Vector2(492 + types[0].SourceRect.Width + 32, 80 + yPad));
                yPad += text.SourceRect.Height + 56;
            }

            yPad = 0;

            foreach (PokemonText text in pps)
            {
                text.LoadContent();
                text.SetPosition(new Vector2(784, moveNames[0].Position.Y + moveNames[0].SourceRect.Height + 8 + yPad));
                yPad += text.SourceRect.Height + 84;
            }

            yPad = 0;

            foreach (PokemonText text in movePPs)
            {
                text.LoadContent();
                text.SetPosition(new Vector2(944 - text.SourceRect.Width, pps[0].Position.Y - (text.SourceRect.Height / 2) + 8 + yPad));
                yPad += text.SourceRect.Height + 56;
            }

        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            foreach (Image image in types)
                image.UnloadContent();
            types.Clear();
            foreach (PokemonText text in moveNames)
                text.UnloadContent();
            moveNames.Clear();
            foreach (PokemonText text in pps)
                text.UnloadContent();
            pps.Clear();
            foreach (PokemonText text in movePPs)
                text.UnloadContent();
            movePPs.Clear();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            foreach (Image image in types)
                image.Draw(spriteBatch);

            foreach (PokemonText text in moveNames)
                text.Draw(spriteBatch);

            foreach (PokemonText text in pps)
                text.Draw(spriteBatch);

            foreach (PokemonText text in movePPs)
                text.Draw(spriteBatch);
        }


    }
}
