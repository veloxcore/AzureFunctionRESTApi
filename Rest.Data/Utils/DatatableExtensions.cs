using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Rest.Data.Utils
{
    /// <summary>
    /// DatatableExtensions
    /// </summary>
    public static class DatatableExtensions
    {
        /// <summary>
        /// Convert DataTable to List<T>
        /// </summary>
        /// <typeparam name="T">Type of list to be created</typeparam>
        /// <param name="dt">DataTable to be converted.</param>
        /// <returns>Type T list from DataTable</returns>
        public static List<T> ToList<T>(this DataTable dt, bool ignoreMissingFields = false)
        {
            List<T> lst = new List<T>();
            T obj = default(T);
            var member = TypeAccessor.Create(typeof(T));
            object value;

            PropertyInfo[] props = typeof(T).GetProperties();
            PropertyInfo property;

            // Work with FieldName attribute
            Dictionary<string, string> fieldNames = new Dictionary<string, string>();
            string fieldName;
            foreach (PropertyInfo item in props)
            {
                fieldName = GetFieldName(item);
                if (!string.IsNullOrEmpty(fieldName))
                {
                    fieldNames.Add(item.Name, fieldName.ToLower());
                }
            }

            foreach (DataRow dr in dt.Rows)
            {
                obj = Activator.CreateInstance<T>();

                foreach (DataColumn item in dt.Columns)
                {
                    value = (object)(dr[item.ColumnName]);
                    var prop = props.Where(o => o.Name.Equals(item.ColumnName, StringComparison.CurrentCultureIgnoreCase) || (fieldNames.ContainsKey(o.Name) && fieldNames[o.Name].Equals(item.ColumnName, StringComparison.CurrentCultureIgnoreCase)));
                    if (prop.Any())
                    {
                        property = prop.FirstOrDefault();
                        try
                        {
                            if (value == DBNull.Value)
                                continue;

                            if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                if (value == null)
                                {
                                    member[obj, property.Name] = null;
                                }
                                else
                                {
                                    member[obj, property.Name] = Convert.ChangeType(value, Nullable.GetUnderlyingType(property.PropertyType));
                                }

                            }
                            else if (property.PropertyType.IsEnum)
                            {
                                member[obj, property.Name] = Enum.Parse(property.PropertyType, value.ToString());
                            }
                            else
                            {
                                member[obj, property.Name] = Convert.ChangeType(value, property.PropertyType);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error while casting: " + typeof(T) + "." + property.Name + Environment.NewLine + ex.Message, ex);
                        }
                    }
                    else if (!ignoreMissingFields)
                    {
                        throw new Exception("Field not found: " + item.ColumnName);
                    }
                }

                lst.Add(obj);
            }

            return lst;
        }

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <param name="prop">The prop.</param>
        /// <returns>FieldName attribute value for the property</returns>
        private static string GetFieldName(PropertyInfo prop)
        {
            Attribute attr = prop.GetCustomAttribute(typeof(ColumnAttribute), false);
            if (attr != null)
            {
                var fieldAttr = attr as ColumnAttribute;
                if (fieldAttr != null)
                    return fieldAttr.Name;
            }
            return string.Empty;
        }

    }
}
