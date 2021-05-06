using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using ConsoleTables;

namespace ADARewardsReporter
{
    public static class TableBuilder
    {
        public static ConsoleTable BuildTableFrom<T>(IEnumerable<T> values)
        {
            var table = new ConsoleTable();
            // {
            //     ColumnTypes = GetColumnsType<T>().ToArray()
            // };

            var columns = GetColumns<T>();
            var propertyNames = GetPropertyNames<T>();
            table.AddColumn(columns);
            
            foreach (
                var propertyValues
                in values.Select(value => propertyNames.Select(property => GetColumnValue<T>(value, property)))
            ) table.AddRow(propertyValues.ToArray());

            return table;
        }

        private static IEnumerable<Type> GetColumnsType<T>()
        {
            return typeof(T).GetProperties().Select(x => x.PropertyType).ToArray();
        }

        private static IEnumerable<string> GetColumns<T>()
        {
            var columns = typeof(T).GetProperties().Select(GetDisplayName);
            return columns;
        }

        private static IEnumerable<string> GetPropertyNames<T>()
        {
            var propertyNames = typeof(T).GetProperties().Select(x => x.Name);
            return propertyNames;
        }

        private static string GetDisplayName(PropertyInfo prop)
        {
            var attrs = prop.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>();
            var displayName = attrs.Any() ? attrs.First().DisplayName : prop.Name;
            return displayName;
        }

        private static object GetColumnValue<T>(object target, string column)
        {
            return typeof(T).GetProperty(column).GetValue(target, null);
        }
    }
}