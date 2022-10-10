// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IVerifiable.cs -- интерфейс объектов, поддерживающих верификацию
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

#endregion

#nullable enable

namespace AM
{
    /// <summary>
    /// Интерфейс объектов, поддерживающих верификацию.
    /// </summary>
    public interface IVerifiable
    {
        /// <summary>
        /// Проверка состояния объекта.
        /// </summary>
        /// <param name="throwOnError">Бросать
        /// <see cref="VerificationException"/> при обнаружении ошибки.
        /// </param>
        bool Verify
            (
                bool throwOnError
            );
    }
}
