// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* IndexDictionary.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Compiler
{
    /// <summary>
    ///
    /// </summary>
    internal sealed class IndexDictionary
        : Dictionary<string, IndexInfo>
    {
        #region Properties

        public int LastId { get; set; }

        #endregion

        #region Public methods

        public IndexInfo Create
            (
                IndexSpecification index
            )
        {
            var text = index.ToText();
            var result = new IndexInfo
                (
                    index,
                    ++LastId
                );
            Add(text, result);

            return result;
        } // method Create

        public IndexInfo? Get
            (
                IndexSpecification specification
            )
        {
            var text = specification.ToText();
            TryGetValue(text, out var result);

            return result;
        } // method Get

        #endregion

    } // class IndexDictionary

} // namespace ManagedIrbis.Pft.Infrastructure.Compiler
