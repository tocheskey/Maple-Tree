﻿// Created: 2017/09/29 8:45 PM
// Updated: 2017/09/29 8:45 PM
// 
// Project: MapleLib
// Filename: IniManager.cs
// Created By: Jared T

using System.Collections.Generic;
using System.IO;

namespace MapleLib.Common
{
    /// <summary>
    /// A class for reading values by section and key from a standard ".ini" initialization file.
    /// </summary>
    /// <remarks>
    /// Section and key names are not case-sensitive. Values are loaded into a hash table for fast access.
    /// Sections in the initialization file must have the following form:
    /// <code>
    ///     ; comment line
    ///     [section]
    ///     key=value
    /// </code>
    /// </remarks>
    public class IniManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IniManager"/> class.
        /// </summary>
        /// <param name="text">The ini text.</param>
        /// <param name="commentDelimiter">The comment delimiter string (default value is ";").
        /// </param>
        public IniManager(string text, string commentDelimiter = ";")
        {
            CommentDelimiter = commentDelimiter;
            TheFile = text;
        }

        /// <summary>
        /// The comment delimiter string (default value is ";").
        /// </summary>
        private string CommentDelimiter { get; }

        private string _theFile;

        /// <summary>
        /// The initialization file path.
        /// </summary>
        private string TheFile
        {
            set
            {
                _theFile = null;
                _dictionary.Clear();
                //if (!File.Exists(value)) return;
                _theFile = value;
                using (var sr = new StringReader(_theFile))
                {
                    string line, section = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (line.Length == 0) continue;  // empty line
                        if (!string.IsNullOrEmpty(CommentDelimiter) && line.StartsWith(CommentDelimiter))
                            continue;  // comment

                        if (line.StartsWith("[") && line.Contains("]"))  // [section]
                        {
                            int index = line.IndexOf(']');
                            section = line.Substring(1, index - 1).Trim();
                            continue;
                        }

                        if (!line.Contains("=")) continue;
                        {
                            int index = line.IndexOf('=');
                            var key = line.Substring(0, index).Trim();
                            var val = line.Substring(index + 1).Trim();
                            var key2 = $"[{section}]{key}".ToLower();

                            if (val.StartsWith("\"") && val.EndsWith("\""))  // strip quotes
                                val = val.Substring(1, val.Length - 2);

                            if (_dictionary.ContainsKey(key2))  // multiple values can share the same key
                            {
                                index = 1;
                                while (true)
                                {
                                    var key3 = $"{key2}~{++index}";
                                    if (!_dictionary.ContainsKey(key3))
                                    {
                                        _dictionary.Add(key3, val);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                _dictionary.Add(key2, val);
                            }
                        }
                    }
                }
            }
        }

        // "[section]key"   -> "value1"
        // "[section]key~2" -> "value2"
        // "[section]key~3" -> "value3"
        private readonly Dictionary<string, string> _dictionary = new Dictionary<string, string>();

        private bool TryGetValue(string section, string key, out string value)
        {
            var key2 = string.Format(section.StartsWith("[") ? "{0}{1}" : "[{0}]{1}", section, key);

            return _dictionary.TryGetValue(key2.ToLower(), out value);
        }

        /// <summary>
        /// Gets a string value by section and key.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The value.</returns>
        public string GetValue(string section, string key, string defaultValue = "")
        {
            string value;
            if (!TryGetValue(section, key, out value))
                return defaultValue;

            return value;
        }
    }
}