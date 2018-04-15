using Arcanoid.Models;
using System.Collections.Generic;

namespace Arcanoid
{
    public class MultiPlayerMode : GameMode
    {

        public override List<Player> CreatePlayers()
        {
            var player1 = new Player();
            var player2 = new Player();


            return new List<Player>() { player1, player2 };
        }
    }
}
