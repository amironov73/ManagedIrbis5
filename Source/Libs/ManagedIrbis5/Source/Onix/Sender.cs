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

/* Sender.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
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
    /// <para>Сведения об отправителе сообщения ONIX содержат:</para>
    /// <list type="bullet">
    /// <item>идентификационный номер отправителя;</item>
    /// <item>сведения о контактном лице отправителя;</item>
    /// <item>имя (наименование) отправителя;</item>
    /// <item>адрес контактной электронной почты отправителя.</item>
    /// </list>
    /// </summary>
    public sealed class Sender
    {
        #region Properties

        /// <summary>
        /// Идентификационным номером отправителя является его
        /// идентификационный номер налогоплательщика (ИНН).
        /// </summary>
        [XmlElement ("SenderIdentifier")]
        public string? Id { get; set; }

        /// <summary>
        /// Имя (наименование) отправителя приводится в форме, указанной
        /// в его уставных документах (юридическое имя). Имя должно
        /// содержать не более 50 символов.
        /// </summary>
        [XmlElement ("SenderName")]
        public string? SenderName { get; set; }

        /// <summary>
        /// Сведения о контактном лице отправителя могут содержать
        /// его имя, название подразделения, телефон. Полноту сведений
        /// определяет отправитель сообщения. Сведения о контактном
        /// лице должны включать не более 300 символов.
        /// </summary>
        [XmlElement ("ContactName")]
        public string? ContactName { get; set; }

        /// <summary>
        /// Адрес контактной электронной почты отправителя должен содержать
        /// не более 100 символов.
        /// </summary>
        [XmlElement ("EmailAddress")]
        public string? EmailAddress { get; set; }

        #endregion
    }
}
