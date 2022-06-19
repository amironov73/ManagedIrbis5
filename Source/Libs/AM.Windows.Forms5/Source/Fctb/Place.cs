// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* Place.cs -- положение символа в тексте
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Положение символа в тексте, состоящее из номера строки и номера колонки.
/// </summary>
public struct Place
    : IEquatable<Place>
{
    #region Fields

    /// <summary>
    /// Номер колонки.
    /// </summary>
    public int Column;

    /// <summary>
    /// Номер строки.
    /// </summary>
    public int Line;

    #endregion

    #region Properties

    /// <summary>
    /// Пустая позиция.
    /// </summary>
    public static Place Empty => new ();

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Place (int column, int line)
    {
        Column = column;
        Line = line;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Смещение на заданную величину.
    /// </summary>
    public void Offset
        (
            int dx,
            int dy
        )
    {
        Column += dx;
        Line += dy;
    }

    #endregion

    #region IEquatable members

    /// <inheritdoc cref="IEquatable{T}.Equals(T?)"/>
    public bool Equals
        (
            Place other
        )
    {
        return Column == other.Column && Line == other.Line;
    }

    #endregion

    #region Operators

    /// <summary>
    /// Оператор сравнения: неравенство.
    /// </summary>
    public static bool operator !=
        (
            Place place1,
            Place place2
        )
    {
        return !place1.Equals (place2);
    }

    /// <summary>
    /// Оператор сравнения: равенство.
    /// </summary>
    public static bool operator ==
        (
            Place place1,
            Place place2
        )
    {
        return place1.Equals (place2);
    }

    /// <summary>
    /// Оператор сравнения: меньше.
    /// </summary>
    public static bool operator <
        (
            Place p1,
            Place p2
        )
    {
        if (p1.Line < p2.Line) return true;
        if (p1.Line > p2.Line) return false;
        if (p1.Column < p2.Column) return true;
        return false;
    }

    /// <summary>
    /// Оператор сравнения: меньше или равно.
    /// </summary>
    public static bool operator <=
        (
            Place p1,
            Place p2
        )
    {
        if (p1.Equals (p2)) return true;
        if (p1.Line < p2.Line) return true;
        if (p1.Line > p2.Line) return false;
        if (p1.Column < p2.Column) return true;
        return false;
    }

    /// <summary>
    /// Оператор сравнения: больше.
    /// </summary>
    public static bool operator >
        (
            Place p1,
            Place p2
        )
    {
        if (p1.Line > p2.Line) return true;
        if (p1.Line < p2.Line) return false;
        if (p1.Column > p2.Column) return true;
        return false;
    }

    /// <summary>
    /// Оператор сравнения: больше или равно.
    /// </summary>
    public static bool operator >=
        (
            Place p1,
            Place p2
        )
    {
        if (p1.Equals (p2)) return true;
        if (p1.Line > p2.Line) return true;
        if (p1.Line < p2.Line) return false;
        if (p1.Column > p2.Column) return true;
        return false;
    }

    /// <summary>
    /// Оператор сравнения.
    /// </summary>
    public static Place operator +
        (
            Place p1,
            Place p2
        )
    {
        return new Place (p1.Column + p2.Column, p1.Line + p2.Line);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="ValueType.Equals(object?)"/>
    public override bool Equals (object? obj)
    {
        return (obj is Place place) && Equals (place);
    }

    /// <inheritdoc cref="ValueType.GetHashCode"/>
    public override int GetHashCode()
    {
        return HashCode.Combine (Column, Line);
    }

    /// <inheritdoc cref="ValueType.ToString"/>
    public override string ToString()
    {
        return "(" + Column + "," + Line + ")";
    }

    #endregion
}
