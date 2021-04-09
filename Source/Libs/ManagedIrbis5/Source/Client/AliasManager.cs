// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* AliasManager.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using AM;
using AM.IO;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Client
{
    /// <summary>
    /// Aliases for databases/servers.
    /// </summary>
    [DebuggerDisplay("Count={_aliases.Count}")]
    public sealed class AliasManager
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public AliasManager()
        {
            _aliases = new List<ConnectionAlias>();
        }

        #endregion

        #region Private members

        private readonly List<ConnectionAlias> _aliases;

        private ConnectionAlias? _GetAlias
            (
                string name
            )
        {
            foreach (var theAlias in _aliases)
            {
                if (theAlias.Name.SameString(name))
                {
                    return theAlias;
                }
            }

            return null;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Clear the table.
        /// </summary>
        public AliasManager Clear()
        {
            _aliases.Clear();

            return this;
        }

        /// <summary>
        /// Read file and create <see cref="AliasManager"/>.
        /// </summary>
        public static AliasManager FromPlainTextFile
            (
                string fileName
            )
        {
            using var reader = TextReaderUtility.OpenRead(fileName, IrbisEncoding.Ansi);
            var result = new AliasManager();

            while (true)
            {
                var line1 = reader.ReadLine();
                var line2 = reader.ReadLine();

                if (string.IsNullOrEmpty(line1)
                    || string.IsNullOrEmpty(line2))
                {
                    break;
                }
                var theAlias = new ConnectionAlias
                {
                    Name = line1,
                    Value = line2
                };
                result._aliases.Add(theAlias);
            }

            return result;
        }

        /// <summary>
        /// Get alias value if exists.
        /// </summary>
        public string? GetAliasValue(string name) => _GetAlias(name)?.Value;

        /// <summary>
        /// List aliases.
        /// </summary>
        public string[] ListAliases()
        {
            var result = _aliases
                .Select(alias => alias.Name)
                .NonNullItems()
                .ToArray();

            return result;
        }

        /// <summary>
        /// Save aliases to file.
        /// </summary>
        public void SaveToPlainTextFile
            (
                string fileName
            )
        {
            using var writer = TextWriterUtility.Create(fileName, IrbisEncoding.Ansi);
            foreach (var theAlias in _aliases)
            {
                writer.WriteLine(theAlias.Name);
                writer.WriteLine(theAlias.Value);
            }
        }

        /// <summary>
        /// Add new or modify existing alias.
        /// </summary>
        public AliasManager SetAlias
            (
                string name,
                string? value
            )
        {
            var theAlias = _GetAlias(name);
            if (ReferenceEquals(theAlias, null))
            {
                if (!string.IsNullOrEmpty(value))
                {
                    theAlias = new ConnectionAlias
                    {
                        Name = name,
                        Value = value
                    };
                    _aliases.Add(theAlias);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(value))
                {
                    _aliases.Remove(theAlias);
                }
                else
                {
                    theAlias.Value = value;
                }
            }

            return this;
        }

        #endregion
    }
}
