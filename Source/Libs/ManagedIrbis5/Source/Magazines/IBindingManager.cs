// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* IBindingManager.cs -- интерфейс менеджера подшивок
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

using AM;
using AM.Collections;
using AM.Text.Ranges;

using ManagedIrbis.Fields;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;
using ManagedIrbis.Records;

#endregion

#nullable enable

namespace ManagedIrbis.Magazines
{
    /// <summary>
    /// Интерфейс менеджера подшивок.
    /// </summary>
    public interface IBindingManager
        : IDisposable
    {

        /// <summary>
        /// Создание либо обновление подшивки по ее спецификации.
        /// </summary>
        void BindMagazines
            (
                BindingSpecification specification
            );

        /// <summary>
        /// Проверка номера журнала/газеты на возможность добавления в подшивку.
        /// </summary>
        bool CheckIssue
            (
                BindingSpecification specification,
                MagazineIssueInfo issue
            );

        /// <summary>
        /// Расшитие и удаление подшивки по ее индексу.
        /// </summary>
        public void UnbindMagazines
            (
                string bindingIndex
            );

    } // interface IBindingManager
}
