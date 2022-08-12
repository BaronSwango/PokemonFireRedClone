using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class NPC : Entity
    {

        [XmlElement("TextBoxes")]
        public List<TextBox> TextBoxes;
        [XmlIgnore]
        public Sprite NPCSprite;

        public override void LoadContent()
        {
            //base.LoadContent();
            NPCSprite = new Sprite(Sprite);
            NPCSprite.LoadContent();
            NPCSprite.SetPosition(SpawnLocation);
        }

        public override void UnloadContent()
        {
            NPCSprite.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            NPCSprite.Update(gameTime);
        }

        public override void Spawn(Map map)
        {
            Tile currentTile = TileManager.GetCurrentTile(map, NPCSprite.Top, NPCSprite.Top.SourceRect.Width / 8, NPCSprite.Top.SourceRect.Height / (int)NPCSprite.Top.SpriteSheetEffect.AmountOfFrames.Y); ;
            if (currentTile != null)
            {
                Vector2 centerTile = new(currentTile.Position.X,
                currentTile.Position.Y - 84);
                NPCSprite.SetPosition(centerTile);
            }
        }

    }
}
