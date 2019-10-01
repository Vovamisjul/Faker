using Generation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Faking.ValueGenerator
{
    class TypeRecognizer
    {

        private delegate object _generate();
        private List<IGenerator> _igeneartors = new List<IGenerator> {new IntGenerator(), new ShortGenerator(), new StringGenerator()};
        private Dictionary<Type, _generate> _generators = new Dictionary<Type, _generate>();

        public TypeRecognizer()
        {
            LoadAssemblies();
            foreach(var generator in _igeneartors)
            {
                try
                {
                    _generators.Add(generator.GeneratedType(), generator.Generate);
                }
                catch { }
            }
        }

        private void LoadAssemblies()
        {
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Generators";
            var assemblies = Directory.GetFiles(dir, "*.dll");

            foreach (var file in assemblies)
            {
                try
                {
                    var PaintAssembly = Assembly.LoadFrom(file);
                    foreach (Type t in PaintAssembly.GetExportedTypes())
                    {
                        if (t.IsClass && typeof(IGenerator).IsAssignableFrom(t))
                        {
                            _igeneartors.Add((IGenerator)Activator.CreateInstance(t));
                        }
                    }
                }
                catch { }
            }
        }

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
