using System;

namespace Arcanoid.Patterns.Strategy
{
    public class ArraySortingContext
    {
        private Strategy Strategy;

        public ArraySortingContext(object[] array)
        {

            Strategy = new ArraySortingStrategy(array);

            Strategy.Algorithm();
            Object result = Strategy.Result;
        }
    }
}