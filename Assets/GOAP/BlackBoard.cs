using System.Collections.Generic;
using UnityEngine;
namespace GOAP
{
    public class BlackBoard
    {
        public Dictionary<string, WorldState> worldStates;

        public BlackBoard() { }
        public BlackBoard(Dictionary<string, WorldState> worldStates)
        {
            this.worldStates = new Dictionary<string, WorldState>(worldStates);
        }
        public BlackBoard(BlackBoard blackBoard)
        {
            this.worldStates = new Dictionary<string, WorldState>(blackBoard.worldStates);
        }
    }

}
