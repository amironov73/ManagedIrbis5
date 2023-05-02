// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* PftNode.cs -- абстрактный узел PFT-скрипта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM;
using AM.Runtime.Mere;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace MicroPft.Ast;

/// <summary>
/// Абстрактный узел PFT-скрипта
/// </summary>
internal abstract class PftNode
    : IMereSerializable
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
        /// <exception cref="NotImplementedException">
        /// Метод обязательно должен быть переопределен
        /// в потомке.
        /// </exception>
        public override void Execute
            (
                PftContext context
            )
        {
            // метод обязательно должен быть переопределен в потомке
            Magna.Logger.LogError ("The method must be overridden in the child");
            throw new ApplicationException ("The method must be overridden in the child");
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

    #region IMereSerializable members

    /// <inheritdoc cref="IMereSerializable.MereSerialize"/>
    /// <exception cref="NotImplementedException">
    /// Метод обязательно должен быть переопределен
    /// в потомке.
    /// </exception>
    public virtual void MereSerialize
        (
            BinaryWriter writer
        )
    {
        // метод обязательно должен быть переопределен в потомке
        Magna.Logger.LogError ("The method must be overridden in the child");
        throw new ApplicationException ("The method must be overridden in the child");
    }

    /// <inheritdoc cref="IMereSerializable.MereDeserialize"/>
    /// <exception cref="NotImplementedException">
    /// Метод обязательно должен быть переопределен
    /// в потомке.
    /// </exception>
    public virtual void MereDeserialize
        (
            BinaryReader reader
        )
    {
        // метод обязательно должен быть переопределен в потомке
        Magna.Logger.LogError ("The method must be overridden in the child");
        throw new ApplicationException ("The method must be overridden in the child");
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
