// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* DisplayIniSection.cs -- DISPLAY-секция INI-файла для клиента
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
    /// DISPLAY-секция INI-файла для клиента.
    /// </summary>
    /// <remarks>
    /// Находится в серверном INI-файле irbisc.ini.
    /// </remarks>
    public sealed class DisplayIniSection
        : AbstractIniSection
    {
        #region Constants

        /// <summary>
        /// Section name.
        /// </summary>
        public const string SectionName = "Display";

        #endregion

        #region Properties

        /// <summary>
        /// Размер порции для показа кратких описаний.
        /// </summary>
        public int MaxBriefPortion
        {
            get => Section.GetValue("MaxBriefPortion", 6);
            set => Section.SetValue("MaxBriefPortion", value);
        }

        /// <summary>
        /// Максимальное количество отмеченных документов.
        /// </summary>
        public int MaxMarked
        {
            get => Section.GetValue("MaxMarked", 100);
            set => Section.SetValue("MaxMarked", value);
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DisplayIniSection()
            : base(SectionName)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public DisplayIniSection
            (
                IniFile iniFile
            )
            : base(iniFile, SectionName)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public DisplayIniSection
            (
                IniFile.Section section
            )
            : base(section)
        {
        }

        #endregion
    }
}
