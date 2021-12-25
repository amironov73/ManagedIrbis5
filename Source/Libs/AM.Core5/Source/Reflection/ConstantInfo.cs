// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ConstantInfo.cs -- информация о константе
 *  Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Reflection;

/// <summary>
/// Типизированная информация о константе.
/// </summary>
public sealed class ConstantInfo<T>
{
    #region Properties

    /// <summary>
    /// Имя константы.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Значение константы.
    /// </summary>
    public T Value { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ConstantInfo
        (
            string name,
            T value
        )
    {
        Name = name;
        Value = value;
    }

    #endregion
}
