// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* CmdOption.cs -- опция командной строки, хранящая свое значение
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace AM.CommandLine;

/// <summary>
/// Опция командной строки, хранящая свое значение.
/// </summary>
[PublicAPI]
public sealed class CmdOption
{
    #region Properties

    /// <summary>
    /// Имя переключателя.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Значение переключателя.
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// Признак использования.
    /// </summary>
    public bool Used { get; set; }

    #endregion

    /// <summary>
    /// Интерпретация значения как логического.
    /// </summary>
    public bool AsBoolean() => !string.IsNullOrWhiteSpace(Value) && Utility.ToBoolean (Value);

    /// <summary>
    /// Интерпретация значения как целого числа (возможно, со знаком).
    /// </summary>
    public int AsInt32() => Value.SafeToInt32 ();

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Name}={Value}";

    #endregion
}
