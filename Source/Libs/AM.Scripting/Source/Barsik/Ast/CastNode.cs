// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* ChangeTypeNode.cs -- преобразование типа
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Преобразование типа.
/// </summary>
internal sealed class CastNode
    : AtomNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="typeNode">Требуемый тип,
    /// к которому должен быть преобразован операнд.</param>
    /// <param name="operandNode">Операнд.</param>
    public CastNode
        (
            AtomNode typeNode,
            AtomNode operandNode
        )
    {
        Sure.NotNull (typeNode);
        Sure.NotNull (operandNode);

        _typeNode = typeNode;
        _operandNode = operandNode;
    }

    #endregion

    #region Private members

    private readonly AtomNode _typeNode;
    private readonly AtomNode _operandNode;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute
        (
            Context context
        )
    {
        var targetType = context.FindType (_typeNode);
        if (targetType is null)
        {
            context.Error.WriteLine ($"Type {_typeNode} not found");
            return null;
        }

        var operandValue = _operandNode.Compute (context);
        if (operandValue is null)
        {
            // ReSharper disable HeapView.BoxingAllocation
            return Type.GetTypeCode (targetType) switch
            {
                TypeCode.Boolean => false,
                TypeCode.Byte => (byte) 0,
                TypeCode.Char => '\0',
                TypeCode.Decimal => 0.0m,
                TypeCode.Double => 0.0,
                TypeCode.Int16 => (short) 0,
                TypeCode.Int32 => 0,
                TypeCode.Int64 => 0L,
                TypeCode.Single => 0.0f,
                TypeCode.DateTime => DateTime.MinValue,
                TypeCode.SByte => (sbyte) 0,
                TypeCode.UInt16 => (ushort) 0,
                TypeCode.UInt32 => (uint) 0,
                TypeCode.UInt64 => (ulong) 0,
                _ => null
            };
            // ReSharper restore HeapView.BoxingAllocation
        }

        if (targetType == typeof (string) && operandValue is IFormattable formattable)
        {
            return formattable.ToString (null, CultureInfo.InvariantCulture);
        }

        if (targetType == typeof (bool))
        {
            return BarsikUtility.ToBoolean (operandValue);
        }

        if (targetType == typeof (short))
        {
            if (operandValue is string text)
            {
                return short.Parse (text, NumberStyles.Any, CultureInfo.InvariantCulture);
            }
        }

        if (targetType == typeof (ushort))
        {
            if (operandValue is string text)
            {
                return ushort.Parse (text, NumberStyles.Any, CultureInfo.InvariantCulture);
            }
        }

        if (targetType == typeof (int))
        {
            if (operandValue is string text)
            {
                return text.SafeToInt32();

                // return int.Parse (text, NumberStyles.Any, CultureInfo.InvariantCulture);
            }
        }

        if (targetType == typeof (uint))
        {
            if (operandValue is string text)
            {
                return uint.Parse (text, NumberStyles.Any, CultureInfo.InvariantCulture);
            }
        }

        if (targetType == typeof (long))
        {
            if (operandValue is string text)
            {
                return text.SafeToInt64();

                // return long.Parse (text, NumberStyles.Any, CultureInfo.InvariantCulture);
            }
        }

        if (targetType == typeof (ulong))
        {
            if (operandValue is string text)
            {
                return ulong.Parse (text, NumberStyles.Any, CultureInfo.InvariantCulture);
            }
        }

        if (targetType == typeof (float))
        {
            if (operandValue is string text)
            {
                return float.Parse (text, NumberStyles.Any, CultureInfo.InvariantCulture);
            }
        }

        if (targetType == typeof (double))
        {
            if (operandValue is string text)
            {
                return double.Parse (text, NumberStyles.Any, CultureInfo.InvariantCulture);
            }
        }

        if (targetType == typeof (decimal))
        {
            if (operandValue is string text)
            {
                return decimal.Parse (text, NumberStyles.Any, CultureInfo.InvariantCulture);
            }
        }

        return Convert.ChangeType (operandValue, targetType);
    }

    #endregion
}
