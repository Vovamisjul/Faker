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
            return (short)new Random().Next(1, short.MaxValue);
        }

        public Type GeneratedType()
        {
            return typeof(short);
        }
    }
}
