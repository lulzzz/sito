using System;
using System.Data;

namespace Maddalena.Ai.Extensions
{
    internal static class DatabaseTableExtensions
    {
        public static object ToObject(this IDataRecord record, Type type)
        {
            var obj = Activator.CreateInstance(type);
            foreach (var prop in obj.GetType().GetProperties())
                try
                {
                    if (!Equals(record[prop.Name], DBNull.Value)) prop.SetValue(obj, record[prop.Name], null);
                }
                catch (Exception)
                {
                }

            return obj;
        }
    }
}