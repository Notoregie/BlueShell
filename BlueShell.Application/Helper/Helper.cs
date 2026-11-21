using System;
using System.Collections.Generic;
using System.Text;

namespace BlueShell.Application.Helper
{
    public static class Helper
    {
        public static bool TryExpandEnvironmentVariable(string value, out string expandedValue)
        {
            expandedValue = value;

            if (!value.StartsWith("%") || !value.EndsWith("%"))
                return false;

            string environmentVariableName = value[1..^1];
            expandedValue = Environment.GetEnvironmentVariable(environmentVariableName) ?? "";
            return !string.IsNullOrWhiteSpace(expandedValue);
        }

        public static string[] ExpandEnvironmentVariables(bool includeUnexpanded = false, params string[] values)
        {
            var expandedArguments = new List<string>();

            foreach (var value in values)
            {
                if (TryExpandEnvironmentVariable(value, out string expandedValue))
                {
                    expandedArguments.Add(expandedValue);
                    continue;
                }
               
                if (includeUnexpanded)
                    expandedArguments.Add(value);
            }

            return expandedArguments.ToArray();
        }
    }
}
