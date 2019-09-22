using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faking.ValueGenerator
{
    class StringGenerator : IGenerator
    {
        public object Generate()
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < random.Next(1, 128); i++)
            {
                ch = Convert.ToChar(random.Next(0, char.MaxValue));
                builder.Append(ch);
            }
            return builder.ToString();
        }
    }
}
