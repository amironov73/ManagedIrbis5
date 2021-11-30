// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* FunctionDescriptor.cs -- описатель функции
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Описатель функции.
    /// </summary>
    public sealed class FunctionDescriptor
    {
        #region Properties

        /// <summary>
        /// Имя функции.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Описание в произвольной форме.
        /// </summary>
        public string? Description { get; }

        /// <summary>
        /// Точка вызова.
        /// </summary>
        public Func<dynamic?[],dynamic?> CallPoint { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public FunctionDescriptor
            (
                string name,
                Func<dynamic?[], dynamic?> callPoint,
                string? description = null
            )
        {
            Name = name;
            Description = description;
            CallPoint = callPoint;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return Name;
        }

        #endregion

    }
}
