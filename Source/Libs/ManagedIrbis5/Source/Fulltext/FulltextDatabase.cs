﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* FulltextDatabase.cs -- встроенная база данных TEXT
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using ManagedIrbis.Direct;

#endregion

#nullable enable

namespace ManagedIrbis.Fulltext
{
    /// <summary>
    /// Встроенная база данных TEXT.
    /// </summary>
    public class FulltextDatabase
        : IDisposable
    {
        #region Constants

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        public const string DatabaseName = "TEXT";

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FulltextDatabase
            (
                string fileName
            )
        {
            throw new NotImplementedException();
            // _direct = new DirectAccess64(fileName, DirectAccessMode.ReadOnly);
        }

        #endregion

        #region Private members

        // private readonly DirectAccess64 _direct;

        #endregion

        #region Public methods

        /// <summary>
        /// Get pages for main record.
        /// </summary>
        public FulltextRecord[] GetPages
            (
                string mainGuid
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get specified page for main record.
        /// </summary>
        public FulltextRecord? GetPage
            (
                string mainGuid,
                int pageNumber
            )
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            // _direct.Dispose();
        }

        #endregion
    }
}
