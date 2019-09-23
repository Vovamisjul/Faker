using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faking.ValueGenerator
{
    class TypeRecognizer
    {

        delegate object Generator();
        private Dictionary<Type, Generator> _generators = new Dictionary<Type, Generator>()
        {
            {typeof(int), new IntGenerator().Generate },
            {typeof(short), new ShortGenerator().Generate },
            {typeof(string), new StringGenerator().Generate }
        };

        public object Generate(Type type)
        {
            if (_generators.ContainsKey(type))
            {
                return _generators[type]();
            }
            return null;
        }

        public bool isGeneration(Type type)
        {
            if (_generators.ContainsKey(type))
            {
                return true;
            }
            return false;
        }
    }
}
