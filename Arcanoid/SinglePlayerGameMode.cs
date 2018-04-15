using Arcanoid.Models;
using System.Collections.Generic;

namespace Arcanoid
{
    class SinglePlayerGameMode : GameMode
    {
        public Player Player { get; set; }

        public override List<Player> CreatePlayers()
        {
            Player = new Player();

            return new List<Player>() { Player };
        }

        public Record CreateRecord()
        {
            Record record = new Record();
            //Creating new record
            record.Create(Player);

            return record;
        }
    }
}
