// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* BbkReference.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Text;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Systematization
{
    /// <summary>
    /// Отсылка.
    /// </summary>
    public sealed class BbkReference
    {
        #region Properties

        /// <summary>
        /// Условие отсылки.
        /// Подполе a.
        /// </summary>
        [SubField('a')]
        public string? Condition { get; set; }

        /// <summary>
        /// Отсылочный код.
        /// Подполе b.
        /// </summary>
        [SubField('b')]
        public string? Content { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the field.
        /// </summary>
        public static BbkReference Parse
            (
                Field field
            )
        {
            var result = new BbkReference
            {
                Condition = field.GetFirstSubFieldValue('a'),
                Content = field.GetFirstSubFieldValue('b')
            };

            return result;
        }

        #endregion
    }
}
