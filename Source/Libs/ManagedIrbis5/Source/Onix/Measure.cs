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

/* Measure.cs -- сведения о физическом размере и весе экземпляра
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
    /// Сведения о физическом размере и весе экземпляра издания содержат:
    /// </para>
    /// <list type="bullet">
    /// <item>код, обозначающий физическую величину;</item>
    /// <item>цифровое значение физической величины;</item>
    /// <item>единицу измерения.</item>
    /// </list>
    /// </summary>
    public sealed class Measure
    {
        #region Properties

        /// <summary>
        /// Код, обозначающий физическую величину.
        /// См. <see cref="ManagedIrbis.Onix.MeasureType"/>.
        /// </summary>
        [ShortTag ("x315")]
        [XmlElement ("MeasureType")]
        [DisplayName ("Код, обозначающий физическую величину")]
        public string? Type { get; set; }

        /// <summary>
        /// Цифровое обозначение физической величины записывают арабскими цифрами.
        /// </summary>
        [ShortTag ("c094")]
        [XmlElement ("Measurement")]
        [DisplayName ("Цифровое обозначение физической величины")]
        public string? Measurement { get; set; }

        /// <summary>
        /// Единица измерения физической величины.
        /// См. <see cref="ManagedIrbis.Onix.MeasureUnitCode"/>.
        /// </summary>
        [ShortTag ("c095")]
        [XmlElement ("MeasureUnitCode")]
        [DisplayName ("Единица измерения")]
        public string? Unit { get; set; }

        #endregion
    }
}
