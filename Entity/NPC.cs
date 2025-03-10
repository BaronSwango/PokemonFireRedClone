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
            [XmlEnum("HorizontalRotate")]
            HORIZONTAL_ROTATE,
            [XmlEnum("VerticalRotate")]
            VERTICAL_ROTATE,
            [XmlEnum("Zoned")]
            ZONED,
            [XmlEnum("SetPath")]
            SET_PATH
        }

        public enum TextBoxReaction
        {
            NORMAL,
            [XmlEnum("SnapBack")]
            SNAP_BACK,
            [XmlEnum("None")]
            NONE
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
        public TextBoxReaction TextBoxReactionType;
        [XmlIgnore]
        public bool UpdateMovement;

        public override void LoadContent()
        {
            UpdateMovement = MoveType != MovementType.STILL;

            NPCSprite = new Sprite(Sprite);
            NPCSprite.LoadContent(SpriteFrames, Direction);
            NPCSprite.SetPosition(SpawnLocation);

            switch (MoveType)
            {
                case MovementType.ROTATE:
                case MovementType.HORIZONTAL_ROTATE:
                case MovementType.VERTICAL_ROTATE:
                    MovementManager = new RotatingMovementManager(this);
                    break;
                case MovementType.SET_PATH:
                    MovementManager = new SetPathMovementManager(this);
                    MovementManager.LoadContent();
                    break;
                case MovementType.ZONED:
                    MovementManager = new ZonedMovementManager(this);
                    MovementManager.LoadContent();
                    break;
            }

        }

        public override void UnloadContent()
        {
            NPCSprite.UnloadContent();
        }

        public void Update(GameTime gameTime, Map map)
        {
            NPCSprite.Update(gameTime);
            if (UpdateMovement)
            {
                MovementManager.Update(gameTime, map);
            }
            else
            {
                IsMoving = false;
            }
        }

        public override void Spawn(Map map)
        {
            Tile currentTile = TileManager.GetCurrentTile(map, NPCSprite.Bottom, NPCSprite.Bottom.SourceRect.Width,
                NPCSprite.Bottom.SourceRect.Height);
            if (currentTile != null)
            {
                Destination = new(currentTile.Position.X, currentTile.Position.Y - 84);
                Vector2 centerTile = new(currentTile.Position.X, currentTile.Position.Y - 84);
                NPCSprite.SetPosition(centerTile);
            }
        }

        public void FacePlayer(Player player)
        {
            switch (player.Direction) {
                case EntityDirection.Left:
                    NPCSprite.SetDirection((int) EntityDirection.Right);
                    break;
            case EntityDirection.Right:
                    NPCSprite.SetDirection((int) EntityDirection.Left);
                    break;
                case EntityDirection.Down:
                    NPCSprite.SetDirection((int) EntityDirection.Up);
                    break;
                case EntityDirection.Up:
                    NPCSprite.SetDirection((int) EntityDirection.Down);
                    break;
            }
        }
    }
}