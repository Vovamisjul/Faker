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
        private const int nestedObjectsSize = 10;
        private TypeRecognizer recognizer = new TypeRecognizer();
        private Dictionary<Type, List<object>> nestedTypes = new Dictionary<Type, List<object>>();
        public T Create<T>()
        {
            nestedTypes = new Dictionary<Type, List<object>>();
            T obj = (T)CreateDTO(typeof(T));
            return obj;
        }

        private void FillDTO(object obj)
        {
            if (obj == null)
                return;
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
                        var nestedObj = CreateDTO(property.PropertyType);
                        FillDTO(nestedObj);
                        property.SetValue(obj, CreateDTO(property.PropertyType));
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
                    var nestedObj = CreateDTO(field.FieldType);
                    field.SetValue(obj, nestedObj);
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
                    collectionElem = CreateDTO(elem);
                }
                
                (typeof(ICollection<>).MakeGenericType(elem)).GetMethod("Add").Invoke(collectionClass, new object[] { collectionElem });
            }
            return collectionClass;
        }

        private bool IsAssignableToGenericType(Type givenType)
        {
            var interfaceTypes = givenType.GetInterfaces();
            var genericType = typeof(ICollection<>);
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

        private object CreateDTO(Type type)
        {
            if (nestedTypes.ContainsKey(type))
            {
                if (nestedTypes[type].Count < nestedObjectsSize)
                {
                    var obj = ConstructDTO(type);
                    nestedTypes[type].Add(obj);
                    FillDTO(obj);
                    return obj;
                }
                else
                {
                    return nestedTypes[type][new Random().Next(nestedObjectsSize - 1)];
                }
            }
            else
            {
                var obj = ConstructDTO(type);
                nestedTypes.Add(type, new List<object> { obj });
                FillDTO(obj);
                return obj;
            }

        }

        private object ConstructDTO(Type type)
        {
            try
            {
                var constructor = type.GetConstructors()[0];
                var parameters = new List<object>();
                foreach (var parameter in constructor.GetParameters())
                {
                    if (recognizer.isGeneration(parameter.ParameterType))
                    {
                        parameters.Add(recognizer.Generate(parameter.ParameterType));
                    }
                    else
                    {
                        if (nestedTypes.ContainsKey(parameter.ParameterType))
                        {
                            parameters.Add(nestedTypes[parameter.ParameterType]);
                        }
                        else
                        {
                            var obj = CreateDTO(parameter.ParameterType);
                            parameters.Add(obj);
                        }
                    }
                }
                return constructor.Invoke(parameters.ToArray());
            }
            catch
            {
                try
                {
                    return Activator.CreateInstance(type);
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
