using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Trady.Core;

namespace Trady.Exporter.Helper
{
    internal static class ExporterExtension
    {
        public static IEnumerable<string> GetGenericParameterPropertiesNames(this object obj)
        {
            return obj
                .GetType()
                .GetGenericArguments()[0]
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Select(p => p.Name);
        }
    }
}
