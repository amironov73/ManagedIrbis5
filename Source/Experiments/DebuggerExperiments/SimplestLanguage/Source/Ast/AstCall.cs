// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* AstCall.cs -- вызов процедуры
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

#endregion

#nullable enable

namespace SimplestLanguage;

/// <summary>
/// Вызов процедуры.
/// </summary>
public sealed class AstCall
    : AstNode
{
    #region Properties

    /// <summary>
    /// Имя процедуры.
    /// </summary>
    public string ProcedureName { get; }

    /// <summary>
    /// Аргументы.
    /// </summary>
    public IReadOnlyList<AstValue> Arguments { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AstCall
        (
            string procedureName,
            IReadOnlyList<AstValue> arguments
        )
    {
        ProcedureName = procedureName;
        Arguments = arguments;
    }

    #endregion

    #region AstNode members

    /// <inheritdoc cref="AstNode.Execute"/>
    public override void Execute
        (
            LanguageContext context
        )
    {
        throw new NotImplementedException();
    }

    #endregion
}
