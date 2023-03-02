// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* CounterPart.cs -- счетчик
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

#nullable enable

namespace NamerCommon;

/// <summary>
/// Счетчик.
/// </summary>
[PublicAPI]
public sealed class CounterPart
    : NamePart
{
    #region Properties

    /// <inheritdoc cref="Designation"/>
    public override string Designation => "counter";

    /// <inheritdoc cref="NamePart.Title"/>
    public override string Title => "Счетчик";

    /// <summary>
    /// Начальное значение счетчика.
    /// </summary>
    public int InitialValue { get; set; }

    /// <summary>
    /// Значение счетчика.
    /// </summary>
    public int CurrentValue { get; set; }

    /// <summary>
    /// Ширина.
    /// </summary>
    public int Width { get; set; }

    #endregion

    #region NamePart members

    /// <inheritdoc cref="NamePart.Parse"/>
    public override NamePart Parse 
        (
            string text
        )
    {
        return new CounterPart();
    }

    /// <inheritdoc cref="NamePart.Render"/>
    public override string Render 
        (
            NamingContext context,
            FileInfo fileInfo
        )
    {
        var value = ++CurrentValue;

        return value.ToString();
    }

    /// <inheritdoc cref="NamePart.Reset"/>
    public override void Reset()
    {
        base.Reset();
        CurrentValue = InitialValue;
    }

    #endregion
}
