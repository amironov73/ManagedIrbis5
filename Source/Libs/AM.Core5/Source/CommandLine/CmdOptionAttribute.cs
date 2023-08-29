
// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* CmdOptionAttribute.cs -- атрибут, задающий соответствие имени опции и свойства
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace AM.CommandLine;

/// <summary>
/// Атрибут, задающий соответствие имени опции и свойства класса.
/// </summary>
[PublicAPI]
[AttributeUsage (AttributeTargets.Property|AttributeTargets.Field)]
public sealed class CmdOptionAttribute
    : Attribute
{
    #region Properties

    /// <summary>
    /// Имя опции, значение которой должно быть присвоено
    /// свойству, помеченному данным атрибутом.
    /// </summary>
    public string Name { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public CmdOptionAttribute
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        Name = name;
    }

    #endregion
}
