// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* AbstractIniSection.cs -- абстрактная секция INI-файла для клиента
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
    /// Абстрактная секция INI-файла для клиента.
    /// </summary>
    public abstract class AbstractIniSection
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// INI file section.
        /// </summary>
        public IniFile.Section Section { get; protected set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        protected AbstractIniSection
            (
                string sectionName
            )
        {
            Sure.NotNull (sectionName);

            _ourIniFile = new IniFile();
            Section = _ourIniFile.GetOrCreateSection (sectionName);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected AbstractIniSection
            (
                IniFile iniFile,
                string sectionName
            )
        {
            Sure.NotNull (iniFile);
            Sure.NotNull (sectionName);

            _ourIniFile = null;
            Section = iniFile.GetOrCreateSection (sectionName);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected AbstractIniSection
            (
                IniFile.Section section
            )
        {
            Sure.NotNull (section);

            _ourIniFile = null;
            Section = section;
        }

        #endregion

        #region Private members

        private readonly IniFile? _ourIniFile;

        #endregion

        #region Public methods

        /// <summary>
        /// Очистка секции.
        /// </summary>
        public void Clear()
        {
            Section.Clear();
        }

        /// <summary>
        /// Получение булевого значения.
        /// </summary>
        public bool GetBoolean
            (
                string name,
                string defaultValue
            )
        {
            Sure.NotNullNorEmpty (name, nameof (name));
            Sure.NotNullNorEmpty (defaultValue, nameof (defaultValue));

            return Utility.ToBoolean
                (
                    Section.GetValue (name, defaultValue)
                        .ThrowIfNull()
                );
        }

        /// <summary>
        /// Установка булевого значения.
        /// </summary>
        public void SetBoolean
            (
                string name,
                bool value
            )
        {
            Sure.NotNullNorEmpty (name, nameof (name));

            Section.SetValue
                (
                    name,
                    value ? "1" : "0"
                );
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            _ourIniFile?.Dispose();
        }

        #endregion

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Section.ToString();
        }
    }
}
