using Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generators
{
    public class ByteGenerator : IGenerator
    {
        public object Generate()
        {
            return (byte)new Random().Next(byte.MaxValue, byte.MaxValue);
        }

        public Type GeneratedType()
        {
            return typeof(byte);
        }
    }
}
