// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ObjectStringFlector.cs -- извлекает значения свойств
 *  Ars Magna project, http://arsmagna.ru
 */

#nullable enable

using System;

namespace AM.Reflection;

/// <summary>
/// Извлекает значения свойств из объекта.
/// </summary>
public static class ObjectStringFlector
{
    #region Private members

    private static object? GetValueByIndex
        (
            object source,
            string path,
            int startIndex
        )
    {
        var sourceType = source.GetType();
        var boundaryEndIndex = path.IndexOf (']', startIndex);
        var indexString = path.Substring
            (
                startIndex + 1,
                boundaryEndIndex - startIndex - 1
            );

        if (sourceType.IsArray)
        {
            return ((Array) source).GetValue (indexString.SafeToInt32());
        }

        if (sourceType == typeof (string))
        {
            return ((string) source)[indexString.SafeToInt32()];
        }

        var index = indexString.IndexOfAny (_stringBoundaryChars) == 0
            ? new object[] { indexString.Trim (_stringBoundaryChars) }
            : new object[] { indexString.SafeToInt32() };

        var propertyInfo = sourceType.GetProperty ("Item");
        if (propertyInfo == null)
        {
            throw new Exception();
        }

        return propertyInfo.GetValue (source, index);
    }

    private static readonly char[] _boundaryStartChars = { '.', '[' };

    private static readonly char[] _stringBoundaryChars = { '\'', '"' };

    #endregion

    #region Public methods

    /// <summary>
    /// Получение значения свойства объекта.
    /// </summary>
    public static object? GetValue
        (
            object source,
            string path,
            int startIndex = 0
        )
    {
        Sure.NotNull (source);
        Sure.NotNullNorEmpty (path);

        if (path[startIndex] == '[')
        {
            var indexedProperty = GetValueByIndex (source, path, startIndex);
            if (indexedProperty is null)
            {
                return null;
            }

            var closingBracketIndex = path.IndexOf (']', startIndex);

            if (closingBracketIndex < path.Length - 1)
            {
                return GetValue (indexedProperty, path, closingBracketIndex + 1);
            }

            return indexedProperty;
        }

        if (path[startIndex] == '.')
        {
            startIndex++;
        }

        var sourceType = source.GetType();
        var boundaryIndex = path.IndexOfAny (_boundaryStartChars, startIndex);
        var currentPropertyName = boundaryIndex > -1
            ? path.Substring (startIndex, boundaryIndex - startIndex)
            : path.Substring (startIndex);

        var propertyInfo = sourceType.GetProperty (currentPropertyName);
        if (propertyInfo == null)
        {
            throw new Exception();
        }

        var currentProperty = propertyInfo.GetValue (source, null);
        if (currentProperty is null)
        {
            return null;
        }

        if (currentPropertyName.Length + startIndex == path.Length)
        {
            return currentProperty;
        }

        return GetValue (currentProperty, path, boundaryIndex);
    }

    #endregion
}
