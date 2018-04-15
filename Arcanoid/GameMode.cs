using Arcanoid.Models;
using System.Collections.Generic;

namespace Arcanoid
{
    public abstract class GameMode
    {
        public abstract List<Player> CreatePlayers();
    }
}
