using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faking
{
    public class Faker
    {
        private Dictionary<Type, object> nestedTypes = new Dictionary<Type, object>();
        public T Create<T>()
        {
            T obj = Activator.CreateInstance<T>();
            FillDTO(obj);
            return obj;
        }

        private void FillDTO(object obj)
        {
            foreach (var property in obj.GetType().GetProperties())
            {
                property.SetValue(obj, null);
            }
            foreach (var field in obj.GetType().GetFields())
            {
                field.SetValue(obj, null);
            }
        }
    }
}
