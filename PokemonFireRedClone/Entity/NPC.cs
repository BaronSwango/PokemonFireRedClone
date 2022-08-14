using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class NPC : Entity
    {

        public string ID;
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

        public override void Spawn(ref Map map)
        {
            Tile currentTile = TileManager.GetCurrentTile(map, NPCSprite.Bottom, NPCSprite.Bottom.SourceRect.Width / 8, NPCSprite.Bottom.SourceRect.Height / (int)NPCSprite.Bottom.SpriteSheetEffect.AmountOfFrames.Y);
            if (currentTile != null)
            {
                Vector2 centerTile = new(currentTile.Position.X,
                currentTile.Position.Y - 84);
                NPCSprite.SetPosition(centerTile);
            }
        }

    }
}
