using System.Linq;

namespace Arcanoid.Patterns.Strategy
{
    public class ArraySortingStrategy : Strategy
    {
        public override object Result { get; set; }
        private object[] array;

        public ArraySortingStrategy(object[] array)
        {
            this.array = array;
        }
        public override void Algorithm()
        {
            array.ToList().Sort();
        }
    }
}