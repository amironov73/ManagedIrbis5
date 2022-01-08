// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* PftNode.cs -- абстрактный узел PFT-скрипта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Text;

#endregion

#nullable enable

namespace ManagedIrbis.PftLite;

/// <summary>
/// Абстрактный узел PFT-скрипта
/// </summary>
internal abstract class PftNode
{
    #region Special values

    /// <summary>
    /// Закрывающая скобка.
    /// </summary>
    internal static readonly PftNode CloseGroup = new ServiceNode ("close group");

    /// <summary>
    /// Запятая.
    /// </summary>
    internal static readonly PftNode Comma = new ServiceNode ("comma");

    /// <summary>
    /// Открывающая скобка.
    /// </summary>
    internal static readonly PftNode OpenGroup = new ServiceNode ("open group");

    #endregion

    #region Nested classes

    /// <summary>
    /// Сервисный узел вроде запятой или открывающей скобки.
    /// </summary>
    private sealed class ServiceNode
        : PftNode
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ServiceNode
            (
                string text
            )
        {
            _text = text;
        }

        #endregion

        #region Private members

        private readonly string _text;

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Execute"/>
        public override void Execute
            (
                PftContext context
            )
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return _text;
        }

        #endregion
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Исполнение действий, связанных с текущим узлом.
    /// </summary>
    public abstract void Execute
        (
            PftContext context
        );

    #endregion
}
