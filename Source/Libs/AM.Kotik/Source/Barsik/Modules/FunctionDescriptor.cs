// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* FunctionDescriptor.cs -- описатель функции
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

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
    public Func<Context, dynamic?[], dynamic?> CallPoint { get; }

    /// <summary>
    /// Нужно ли вычислять значения аргументов перед передачей в функцию?
    /// </summary>
    public bool ComputeArguments { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public FunctionDescriptor
        (
            string name,
            Func<Context, dynamic?[], dynamic?> callPoint,
            bool compute = true,
            string? description = null
        )
    {
        Sure.NotNullNorEmpty (name);
        Sure.NotNull (callPoint);

        Name = name;
        Description = description;
        ComputeArguments = compute;
        CallPoint = callPoint;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() =>
        Utility.JoinNonEmpty (": ",  "func " + Name, Description);

    #endregion
}
