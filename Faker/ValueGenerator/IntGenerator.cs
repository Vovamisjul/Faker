using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Generation;

namespace Faking.ValueGenerator
{
    class IntGenerator : IGenerator
    {
        public object Generate()
        {
            var random = new Random();
            return random.Next(2) == 0 ? random.Next(1, int.MaxValue) : random.Next(int.MinValue, -1);
        }

        public Type GeneratedType()
        {
            return typeof(int);
        }
    }
}
