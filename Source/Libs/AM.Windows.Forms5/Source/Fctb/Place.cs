// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

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
public struct Place : IEquatable<Place>
{
    public int iChar;
    public int iLine;

    public Place (int iChar, int iLine)
    {
        this.iChar = iChar;
        this.iLine = iLine;
    }

    public void Offset (int dx, int dy)
    {
        iChar += dx;
        iLine += dy;
    }

    public bool Equals (Place other)
    {
        return iChar == other.iChar && iLine == other.iLine;
    }

    public override bool Equals (object obj)
    {
        return (obj is Place) && Equals ((Place)obj);
    }

    public override int GetHashCode()
    {
        return iChar.GetHashCode() ^ iLine.GetHashCode();
    }

    public static bool operator != (Place p1, Place p2)
    {
        return !p1.Equals (p2);
    }

    public static bool operator == (Place p1, Place p2)
    {
        return p1.Equals (p2);
    }

    public static bool operator < (Place p1, Place p2)
    {
        if (p1.iLine < p2.iLine) return true;
        if (p1.iLine > p2.iLine) return false;
        if (p1.iChar < p2.iChar) return true;
        return false;
    }

    public static bool operator <= (Place p1, Place p2)
    {
        if (p1.Equals (p2)) return true;
        if (p1.iLine < p2.iLine) return true;
        if (p1.iLine > p2.iLine) return false;
        if (p1.iChar < p2.iChar) return true;
        return false;
    }

    public static bool operator > (Place p1, Place p2)
    {
        if (p1.iLine > p2.iLine) return true;
        if (p1.iLine < p2.iLine) return false;
        if (p1.iChar > p2.iChar) return true;
        return false;
    }

    public static bool operator >= (Place p1, Place p2)
    {
        if (p1.Equals (p2)) return true;
        if (p1.iLine > p2.iLine) return true;
        if (p1.iLine < p2.iLine) return false;
        if (p1.iChar > p2.iChar) return true;
        return false;
    }

    public static Place operator + (Place p1, Place p2)
    {
        return new Place (p1.iChar + p2.iChar, p1.iLine + p2.iLine);
    }

    public static Place Empty
    {
        get { return new Place(); }
    }

    public override string ToString()
    {
        return "(" + iChar + "," + iLine + ")";
    }
}
