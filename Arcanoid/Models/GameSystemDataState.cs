using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using Arcanoid.Models;

namespace Arcanoid
{
    [Serializable]
    public class GameSystemDataState
    {
        public int _currentStage = 1;
        public List<Brick> bricks;
        public int skipTick = 5;
        public double RedGameBallLeft = 40;
        public double RedGameBallTop = 200;
        public int RedBallCurrentDirection = 3;
        public float motionRatio = 4;
        public bool _isClockWise = true; // true = clockwise , false = anti-clockwise


        public GameSystemDataState()
        {
            bricks = BrickLoader.load(_currentStage);
        }
    }
}
