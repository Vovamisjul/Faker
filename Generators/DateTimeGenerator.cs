using Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generators
{
    public class DateTimeGenerator : IGenerator
    {
        public object Generate()
        {
            var random = new Random();
            return new DateTime(random.Next(1, 9999), random.Next(1, 12), random.Next(1, 28), random.Next(23), random.Next(59), random.Next(59), random.Next(999));
        }

        public Type GeneratedType()
        {
            return typeof(DateTime);
        }
    }
}
