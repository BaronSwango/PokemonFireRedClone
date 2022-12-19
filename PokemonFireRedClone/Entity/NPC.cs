using System.Collections.Generic;
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

        // SETPATHMOVEMANAGER ONLY
        [XmlElement("PathCoords")]
        public List<string> PathCoordsXML;

        // ZONEDMOVEMENTMANAGER ONLY
        [XmlElement("Zone")]
        public List<string> Zone;

        public string ID;
        [XmlIgnore]
        public Sprite NPCSprite;
        [XmlIgnore]
        public NPCMovementManager MovementManager;
        public MovementType MoveType;
        public bool UpdateMovement;

        public override void LoadContent()
        {
            UpdateMovement = MoveType != MovementType.STILL;

            NPCSprite = new Sprite(Sprite);
            NPCSprite.LoadContent(SpriteFrames, Direction);
            NPCSprite.SetPosition(SpawnLocation);

            switch(MoveType)
            {
                case MovementType.ROTATE:
                    MovementManager = new RotatingMovementManager(this);
                    break;
                case MovementType.SET_PATH:
                    MovementManager = new SetPathMovementManager(this);
                    MovementManager.LoadContent();
                    break;
                case MovementType.ZONED:
                    MovementManager = new ZonedMovementManager(this);
                    break;
            }


        }

        public override void UnloadContent()
        {
            NPCSprite.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            NPCSprite.Update(gameTime);
            if (UpdateMovement)
                MovementManager.Update(gameTime);
            else
                IsMoving = false;
        }

        public override void Spawn(ref Map map)
        {
            Tile currentTile = TileManager.GetCurrentTile(map, NPCSprite.Bottom, NPCSprite.Bottom.SourceRect.Width,
                NPCSprite.Bottom.SourceRect.Height);
            if (currentTile != null)
            {
                Destination = new(currentTile.Position.X, currentTile.Position.Y - 84);
                Vector2 centerTile = new(currentTile.Position.X,
                currentTile.Position.Y - 84);
                NPCSprite.SetPosition(centerTile);
            }
        }

    }
}
