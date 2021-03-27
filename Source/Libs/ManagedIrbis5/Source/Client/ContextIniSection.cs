// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ContextIniSection.cs -- CONTEXT-секция INI-файла для клиента
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

using AM;
using AM.IO;
using AM.Runtime;
using AM.Text;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Client
{
    /// <summary>
    /// CONTEXT-секция INI-файла для клиента.
    /// </summary>
    public sealed class ContextIniSection
        : AbstractIniSection
    {
        #region Constants

        /// <summary>
        /// Section name.
        /// </summary>
        public const string SectionName = "CONTEXT";

        #endregion

        #region Properties

        /// <summary>
        /// Database name.
        /// </summary>
        public string? Database
        {
            get => Section["DBN"];
            set => Section["DBN"] = value;
        }

        /// <summary>
        /// Display format description.
        /// </summary>
        public string? DisplayFormat
        {
            get => Section["PFT"];
            set => Section["PFT"] = value;
        }

        /// <summary>
        /// Current MFN.
        /// </summary>
        public int Mfn
        {
            get => Section.GetValue("CURMFN", 0);
            set => Section.SetValue("CURMFN", value);
        }

        /// <summary>
        /// Password.
        /// </summary>
        public string? Password
        {
            get => Section["UserPassword"] ?? Section["Password"];
            set => Section["UserPassword"] = value;
        }

        /// <summary>
        /// AsyncQuery.
        /// </summary>
        public string? Query
        {
            // TODO использовать UTF8

            get => Section["QUERY"];
            set => Section["QUERY"] = value;
        }

        /// <summary>
        /// Search prefix.
        /// </summary>
        public string? SearchPrefix
        {
            get => Section["PREFIX"];
            set => Section["PREFIX"] = value;
        }

        /// <summary>
        /// User name.
        /// </summary>
        public string? UserName
        {
            get => Section["UserName"];
            set => Section["UserName"] = value;
        }

        /// <summary>
        /// Worksheet code.
        /// </summary>
        public string? Worksheet
        {
            get => Section["WS"];
            set => Section["WS"] = value;
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ContextIniSection()
            : base (SectionName)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ContextIniSection
            (
                IniFile iniFile
            )
            : base(iniFile, SectionName)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ContextIniSection
            (
                IniFile.Section section
            )
            : base(section)
        {
        }

        #endregion
    }
}
