// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global

/* FieldValue.cs -- валидация и нормализация значений полей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

using AM;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis;

/// <summary>
/// Валидация и нормализация значений полей.
/// </summary>
public static class FieldValue
{
    #region Properties

    /// <summary>
    /// Бросать исключения при валидации?
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static bool ThrowOnVerify { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Проверка значения поля на валидность.
    /// </summary>
    public static bool IsValidValue
        (
            string? value
        )
    {
        if (!string.IsNullOrEmpty (value))
        {
            foreach (var c in value)
            {
                // в значении поля должны отстуствовать
                // разделители и управляющие символы
                if (c is SubField.Delimiter or < ' ')
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Нормализация значения поля.
    /// Удаляет начальные и конечные пробелы.
    /// Не для всех полей это правильно в общем случае.
    /// </summary>
    public static string? Normalize
        (
            string? value
        )
    {
        if (string.IsNullOrEmpty (value))
        {
            return value;
        }

        var result = value.Trim();

        return result;
    }

    /// <summary>
    /// Проверка значения подполя на валидность.
    /// Может выбросить исключение, если
    /// <see cref="ThrowOnVerify"/> установлено в <c>true</c>.
    /// </summary>
    public static bool Verify
        (
            string? value
        )
    {
        return Verify (value, ThrowOnVerify);
    }

    /// <summary>
    /// Проверка значения поля на валидность.
    /// Может выбросить исключение, если
    /// <paramref name="throwOnError"/> установлено в <c>true</c>.
    /// </summary>
    public static bool Verify
        (
            string? value,
            bool throwOnError
        )
    {
        var result = IsValidValue (value);

        if (!result)
        {
            Magna.Logger.LogError
                (
                    nameof (FieldValue) + "::" + nameof (Verify)
                    + ": bad field value={Value}",
                    value.ToVisibleString()
                );

            if (throwOnError)
            {
                throw new VerificationException
                    (
                        nameof (FieldValue) + "::" + nameof (Verify)
                        + ": bad field value="
                        + value.ToVisibleString()
                    );
            }
        }

        return result;
    }

    #endregion
}
