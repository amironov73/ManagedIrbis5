// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PlainStatusBar.cs -- простая строка статуса
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Windows.Forms.General
{
    /// <summary>
    /// Простая строка статуса, реализованная целиком
    /// силами стандартных компонентов WinForms.
    /// </summary>
    public sealed class PlainStatusBar
        : IGeneralItemList
    {
        #region IGeneralItemList members

        /// <inheritdoc cref="IGeneralItemList.Count"/>
        public int Count => throw new NotImplementedException();

        /// <inheritdoc cref="IGeneralItemList.this[int]"/>
        public IGeneralItem this[int index] => throw new NotImplementedException();

        /// <inheritdoc cref="IGeneralItemList.this[string]"/>
        public IGeneralItem this[string id] => throw new NotImplementedException();

        /// <inheritdoc cref="IGeneralItemList.Add"/>
        public void Add(IGeneralItem item) => throw new NotImplementedException();

        /// <inheritdoc cref="IGeneralItemList.Clear"/>
        public void Clear() => throw new NotImplementedException();

        /// <inheritdoc cref="IGeneralItemList.CreateItem"/>
        public IGeneralItem CreateItem(string id, string caption) => throw new NotImplementedException();

        /// <inheritdoc cref="IGeneralItemList.Contains"/>
        public bool Contains(IGeneralItem item) => throw new NotImplementedException();

        /// <inheritdoc cref="IGeneralItemList.Remove"/>
        public void Remove(IGeneralItem item) => throw new NotImplementedException();

        #endregion

    } // class PlainStatusBar

} // namespace Am.Windows.Forms.General
