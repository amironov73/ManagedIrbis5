// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* BarsikList.cs -- список объектов для Барсика
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;

#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Список объектов для Барсика
    /// </summary>
    public sealed class BarsikList
        : List<dynamic?>
    {
        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            var output = new StringWriter();
            BarsikUtility.PrintSequence (output, this);

            return output.ToString();
        }

        #endregion
    }
}
