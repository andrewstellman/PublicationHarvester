using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using Microsoft.Win32;

namespace PubMed
{
    /// <summary>
    /// Save values in a subkey under HKEY_CURRENT_USER 
    /// </summary>
    public static class Settings
    {
        /* For more information, see:
         * http://msdn.microsoft.com/en-us/library/microsoft.win32.registrykey.aspx
         */

        const string SUBKEY_NAME = "PublicationHarvester";

        /// <summary>
        /// Create/retrieve the subkey called SUBKEY_NAME under HKEY_CURRENT_USER
        /// </summary>
        static RegistryKey subKey = Registry.CurrentUser.CreateSubKey(SUBKEY_NAME);

        /// <summary>
        /// Create/update a subkey value
        /// </summary>
        /// <param name="key">Key to save the value under</param>
        /// <param name="value">Value to save</param>
        public static void SetValue(string key, object value)
        {
            using (RegistryKey valueKey = subKey.CreateSubKey(key)) 
            {
                // Set the value for the key
                valueKey.SetValue(key, value);
            }
        }

        public static object GetValue(string key, object defaultValue)
        {
            foreach (string subKeyName in subKey.GetSubKeyNames())
            {
                using (RegistryKey tempKey = subKey.OpenSubKey(subKeyName))
                {
                    if (tempKey == null) return defaultValue;
                    foreach (string valueName in tempKey.GetValueNames())
                    {
                        if (!String.IsNullOrEmpty(valueName) && (valueName == key))
                        {
                            return tempKey.GetValue(valueName);
                        }
                    }
                }
            }
            return defaultValue;
        }

        public static string GetValueString(string key, string defaultValue)
        {
            object value = GetValue(key, defaultValue);
            return value != null ? value.ToString() : null;
        }

        public static bool GetValueBool(string key, bool defaultValue)
        {
            string value = Settings.GetValueString(key, defaultValue.ToString());
            bool result;
            if (!bool.TryParse(value, out result))
                result = defaultValue;
            return result;
        }

        public static decimal GetValueDecimal(string key, decimal defaultValue)
        {
            string value = Settings.GetValueString(key, defaultValue.ToString());
            decimal result;
            if (!decimal.TryParse(value, out result))
                result = defaultValue;
            return result;
        }
    }
}
