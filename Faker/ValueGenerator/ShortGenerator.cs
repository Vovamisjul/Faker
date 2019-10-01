using Faking.ValueGenerator;
using Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faking.ValueGenerator
{
    class ShortGenerator : IGenerator
    {
        public object Generate()
        {
            var random = new Random();
            return (short)(random.Next(2) == 0 ? random.Next(1, short.MaxValue) : random.Next(short.MinValue, -1));
        }

        public Type GeneratedType()
        {
            return typeof(short);
        }
    }
}
