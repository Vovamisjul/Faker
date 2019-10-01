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
            var random = new Random();
            return (byte)(random.Next(2) == 0 ? random.Next(1, byte.MaxValue) : random.Next(byte.MinValue, -1));
        }

        public Type GeneratedType()
        {
            return typeof(byte);
        }
    }
}
