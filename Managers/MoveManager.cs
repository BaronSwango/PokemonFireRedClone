﻿using System.Collections.Generic;
using System.Xml.Serialization;

namespace PokemonFireRedClone
{
    public class MoveManager
    {

        private static MoveManager instance;

        [XmlElement("Moves")]
        public List<Move> Moves;

        public static MoveManager Instance
        {
            get
            {
                XmlManager<MoveManager> xml = new();
                instance = xml.Load("Load/Pokemon/MoveManager.xml");

                return instance;
            }
        }

        public Move GetMove(string name)
        {
            foreach (Move move in Moves)
            {
                if (move.Name.ToLower().Equals(name.ToLower()))
                    return move;
            }
            return null;
        }


    }
}
