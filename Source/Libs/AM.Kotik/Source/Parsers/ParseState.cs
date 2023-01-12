// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UseNameofExpression

/* ParseState.cs -- хранит состояние в процессе разбора
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Хранит состояние в процессе разбора.
/// </summary>
public sealed class ParseState
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    internal ParseState()
    {
        // пустое тело конструктора
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Возвращает общее количество обработанных токенов.
    /// Проще говоря, текущее абсолютное смещение от начала
    /// входного потока.
    /// </summary>
    public int Location => throw new NotImplementedException();

    /// <summary>
    /// Есть текущий токен или уже достигнут конец входного потока?
    /// </summary>
    public bool HasCurrent => throw new NotImplementedException();

    /// <summary>
    /// Текущий токен.
    /// </summary>
    public Token Current => throw new NotImplementedException();

    /// <summary>
    /// Продвижение вперед во входном потоке на указанное количество токенов.
    /// </summary>
    public void Advance
        (
            int count = 1
        )
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Заглядывание вперед на указанное количество токенов.
    /// </summary>
    public ReadOnlySpan<Token> LookAhead
        (
            int count
        )
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Запоминание текущей позиции в потоке.
    /// </summary>
    public void PushBookmark()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Забывание текущей позиции в потоке.
    /// </summary>
    public void PopBookmark()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Возврат к ранее запомненной позиции в потоке.
    /// </summary>
    public void Rewind()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Вывод строки отладочного текста.
    /// </summary>
    public void Trace
        (
            string line
        )
    {
        line.NotUsed();

        // TODO реализовать
    }

    #endregion
}
