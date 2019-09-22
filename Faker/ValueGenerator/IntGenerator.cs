using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faking.ValueGenerator
{
    class IntGenerator : IGenerator
    {
        public object Generate()
        {
            return new Random().Next(1, int.MaxValue);
        }
    }
}
