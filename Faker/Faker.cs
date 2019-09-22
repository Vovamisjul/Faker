using Faking.ValueGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faking
{
    public class Faker
    {
        private TypeRecognizer recognizer = new TypeRecognizer();
        private Dictionary<Type, object> nestedTypes = new Dictionary<Type, object>();
        public T Create<T>()
        {
            nestedTypes = new Dictionary<Type, object>();
            T obj = Activator.CreateInstance<T>();
            FillDTO(obj);
            return obj;
        }

        private void FillDTO(object obj)
        {
            nestedTypes.Add(obj.GetType(), obj);
            bool initialized = false;
            foreach (var property in obj.GetType().GetProperties())
            {
                if (property.SetMethod != null)
                {
                    if (nestedTypes.ContainsKey(property.PropertyType))
                    {
                        property.SetValue(obj, nestedTypes[property.PropertyType]);
                        initialized = true;
                        continue;
                    }
                    if (recognizer.isGeneration(property.PropertyType))
                    {
                        property.SetValue(obj, recognizer.Generate(property.PropertyType));
                        initialized = true;
                    }
                    else
                    {
                        try
                        {
                            object nestedObject = Activator.CreateInstance(property.PropertyType);
                            FillDTO(nestedObject);
                            property.SetValue(obj, nestedObject);
                        }
                        catch (Exception)
                        {
                            property.SetValue(obj, null);
                        }
                        initialized = true;
                    }
                }
            }
            foreach (var field in obj.GetType().GetFields())
            {
                if (nestedTypes.ContainsKey(field.FieldType))
                {
                    field.SetValue(obj, nestedTypes[field.FieldType]);
                    initialized = true;
                    continue;
                }
                if (recognizer.isGeneration(field.FieldType))
                {
                    field.SetValue(obj, recognizer.Generate(field.FieldType));
                    initialized = true;
                }
                else
                {
                    object nestedObject = Activator.CreateInstance(field.FieldType);
                    FillDTO(nestedObject);
                    field.SetValue(obj, nestedObject);
                    initialized = true;
                }
            }
            if (!initialized)
                obj = null;
        }
    }
}
