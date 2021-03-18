// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* LocalProvider.cs -- провайдер, работающий с локальными файлами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Client
{
    /// <summary>
    /// Провайдер, работающий с локальными файлами.
    /// </summary>
    public class LocalProvider
        : IrbisProvider
    {
        #region IrbisProvider members

        public override string ReadFile(FileSpecification file) =>
            throw new NotImplementedException();

        public override int[] Search(string expression) =>
            throw new NotImplementedException();

        public override TermLink[] ExactSearchLinks(string term) =>
            throw new NotImplementedException();

        public override TermLink[] ExactSearchTrimLinks(string term, int i) =>
            throw new NotImplementedException();

        #endregion

    } // class LocalProvider

} // namespace ManagedIrbis.Client
