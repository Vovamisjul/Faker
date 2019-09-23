using Faking.ValueGenerator;
using System;
using System.Collections;
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
            foreach (var property in obj.GetType().GetProperties())
            {
                try
                {
                    if (property.SetMethod != null)
                    {
                        if (recognizer.isGeneration(property.PropertyType))
                        {
                            property.SetValue(obj, recognizer.Generate(property.PropertyType));
                            continue;
                        }
                        if (typeof(ICollection<>).IsAssignableFrom(property.PropertyType))
                        {
                            FillCollection(property.PropertyType, property.PropertyType.GetGenericArguments().Single());
                            continue;
                        }
                        if (nestedTypes.ContainsKey(property.PropertyType))
                        {
                            property.SetValue(obj, nestedTypes[property.PropertyType]);
                        }
                        else
                        {
                            object nestedObject = Activator.CreateInstance(property.PropertyType);
                            nestedTypes.Add(property.PropertyType, nestedObject);
                            FillDTO(nestedObject);
                            property.SetValue(obj, nestedObject);
                        }
                    }
                }
                catch (Exception)
                {
                    property.SetValue(obj, null);
                }
            }
            foreach (var field in obj.GetType().GetFields())
            {
                try
                {
                    if (recognizer.isGeneration(field.FieldType))
                    {
                        field.SetValue(obj, recognizer.Generate(field.FieldType));
                        continue;
                    }
                    if (IsAssignableToGenericType(field.FieldType))
                    {
                        field.SetValue(obj, FillCollection(field.FieldType, field.FieldType.GetGenericArguments().Single()));
                        continue;
                    }
                    if (nestedTypes.ContainsKey(field.FieldType))
                    {
                        field.SetValue(obj, nestedTypes[field.FieldType]);
                    }
                    else
                    {
                        object nestedObject = Activator.CreateInstance(field.FieldType);
                        nestedTypes.Add(field.FieldType, nestedObject);
                        FillDTO(nestedObject);
                        field.SetValue(obj, nestedObject);
                    }
                }
                catch (Exception)
                {
                    field.SetValue(obj, null);
                }
            }
        }

        private object FillCollection(Type collection, Type elem)
        {
            object collectionClass = Activator.CreateInstance(collection);
            for (int i = 0; i < new Random().Next(64); i++)
            {
                object collectionElem = null;
                if (recognizer.isGeneration(elem))
                {
                    recognizer.Generate(elem);
                }
                else
                {
                    collectionElem = Activator.CreateInstance(elem);
                    FillDTO(collectionElem);
                }
                collection.GetMethod("Add").Invoke(collectionClass, new object[] { collectionElem });
            }
            return collectionClass;
        }

        private bool IsAssignableToGenericType(Type givenType)
        {
            var interfaceTypes = givenType.GetInterfaces();
            var genericType = typeof(IEnumerable<>);
            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            Type baseType = givenType.BaseType;
            if (baseType == null) return false;

            return IsAssignableToGenericType(baseType);
        }
    }
}
