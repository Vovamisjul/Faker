using Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generators
{
    public class LongGenerator : IGenerator
    {
        public object Generate()
        {
            byte[] buf = new byte[8];
            new Random().NextBytes(buf);
            return BitConverter.ToInt64(buf, 0);
        }

        public Type GeneratedType()
        {
            return typeof(long);
        }
    }
}
