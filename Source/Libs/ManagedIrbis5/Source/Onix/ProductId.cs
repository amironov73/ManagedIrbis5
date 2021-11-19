// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* ProductId.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Onix
{
    /// <summary>
    /// <para>
    /// Сведения о международном стандартном номере издания или его эквиваленте содержат:
    /// </para>
    /// <list type="bullet">
    /// <item>код вида международного стандартного номера издания или его эквивалента;</item>
    /// <item>цифровую часть международного стандартного номера издания или его эквивалента.</item>
    /// </list>
    /// </summary>
    [XmlRoot ("ProdictIdentifier")]
    public sealed class ProductId
    {
        #region Properties

        /// <summary>
        /// Код вида международного стандартного номера издания или его эквивалента.
        /// См. <see cref="ManagedIrbis.Onix.ProductIdType"/>.
        /// </summary>
        [ShortTag ("b221")]
        [XmlElement ("ProductIDType")]
        [DisplayName ("Код вида стандарта")]
        public int ProductIdType { get; set; }

        /// <summary>
        /// Цифровая часть международного стандартного номера издания
        /// или его эквивалента приводится без дефисов и пробелов.
        /// </summary>
        [ShortTag ("b244")]
        [XmlElement ("IDValue")]
        [DisplayName ("Цифровая часть стандартного номера")]
        public string? IdValue { get; set; }

        #endregion
    }
}
