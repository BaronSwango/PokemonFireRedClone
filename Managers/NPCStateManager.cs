using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;

namespace PokemonFireRedClone
{
    public class NPCStateManager
    {
        private static NPCStateManager instance;
        private readonly Dictionary<string, NPCState> mapNPCStates;

        public static NPCStateManager Instance
        {
            get
            {
                instance ??= new NPCStateManager();
                return instance;
            }
        }

        private NPCStateManager()
        {
            mapNPCStates = new Dictionary<string, NPCState>();
        }

        public void SaveNPCStates(List<NPC> npcs)
        {
            foreach (NPC npc in npcs)
            {
                mapNPCStates[npc.ID] = new NPCState
                {
                    Position = npc.IsMoving ? npc.Destination : npc.Sprite.Position,
                    Direction = (int) npc.Sprite.SpriteSheetEffect.CurrentFrame.Y,
                    CurrentState = npc.State
                };
            }
        }

        public void LoadNPCStates(List<NPC> npcs)
        {
            foreach (NPC npc in npcs)
            {
                if (mapNPCStates.TryGetValue(npc.ID, out NPCState state))
                {
                    npc.Sprite.Position = state.Position;
                    npc.Sprite.SpriteSheetEffect.CurrentFrame.Y = state.Direction;
                    npc.State = state.CurrentState;
                }
            }
        }
    }

    public class NPCState
    {
        public Vector2 Position { get; set; }
        public int Direction { get; set; }
        public Entity.MoveState CurrentState { get; set; }
    }
}