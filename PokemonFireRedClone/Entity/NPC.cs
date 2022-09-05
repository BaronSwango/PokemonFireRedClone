using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class NPC : Entity
    {

        public enum MovementType
        {
            [XmlEnum("Still")]
            STILL,
            [XmlEnum("Rotate")]
            ROTATE,
            [XmlEnum("Zoned")]
            ZONED,
            [XmlEnum("SetPath")]
            SET_PATH
        }

        public string ID;
        [XmlIgnore]
        public Sprite NPCSprite;
        [XmlIgnore]
        public NPCMovementManager movementManager;
        public MovementType MoveType;
        public bool UpdateMovement;

        public override void LoadContent()
        {
            UpdateMovement = MoveType != MovementType.STILL;
            movementManager = new NPCMovementManager(this);
            NPCSprite = new Sprite(Sprite);
            NPCSprite.LoadContent(SpriteFrames, Direction);
            NPCSprite.SetPosition(SpawnLocation);
        }

        public override void UnloadContent()
        {
            NPCSprite.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            NPCSprite.Update(gameTime);
            if (UpdateMovement)
                movementManager.Update(gameTime);
        }

        public override void Spawn(ref Map map)
        {
            Tile currentTile = TileManager.GetCurrentTile(map, NPCSprite.Bottom, NPCSprite.Bottom.SourceRect.Width,
                NPCSprite.Bottom.SourceRect.Height);
            if (currentTile != null)
            {
                Vector2 centerTile = new(currentTile.Position.X,
                currentTile.Position.Y - 84);
                NPCSprite.SetPosition(centerTile);
            }
        }

    }
}
