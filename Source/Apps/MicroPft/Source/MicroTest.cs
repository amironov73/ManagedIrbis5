// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* MicroTest.cs -- базовый класс для тестов MicroPft
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using ManagedIrbis;

#endregion

#nullable enable

namespace MicroPft;

/// <summary>
/// Базовый класс для тестов MicroPft
/// </summary>
internal class MicroTest
{
    #region Properties

    /// <summary>
    /// Количество успещно выполненных тестов.
    /// </summary>
    public int Success { get; set; }

    /// <summary>
    /// Общее количество запущенных тестов.
    /// </summary>
    public int Total { get; set; }

    #endregion
    
    #region Public methods

    /// <summary>
    /// Ожидание указанного исключения.
    /// </summary>
    public void RunForException<TException>
        (
            string format,
            Record record
        )
        where TException: Exception
    {
        Total++;

        try
        {
            var formatter = new MicroFormatter();
            Console.WriteLine (format.Trim());
            formatter.Parse (format);
            _ = formatter.Format (record);
            Console.WriteLine ($"\tEXPECTED {typeof (TException)}, got nothing");
        }
        catch (Exception exception)
        {
            if (exception.GetType() != typeof (TException))
            {
                Console.WriteLine ($"\tEXPECTED {typeof (TException)}, got {exception.GetType()}");
            }
            else
            {
                Console.WriteLine ($"\tGOT {typeof (TException)} as expected");
                Success++;
            }
        }

        Console.WriteLine();
    }

    /// <summary>
    /// Запуск, расформатирование и сравнение с ожидаемым результатом.
    /// </summary>
    public void RunForSuccess
        (
            string format,
            Record record,
            string expected
        )
    {
        Total++;

        try
        {
            var formatter = new MicroFormatter();
            formatter.Parse (format);
            Console.WriteLine (format.Trim());
            var actual = formatter.Format (record);
            if (string.CompareOrdinal (expected, actual) == 0)
            {
                Console.WriteLine("\tSUCCESS");
                Success++;
            }
            else
            {
                Console.WriteLine ($"\tERROR! Expected '{expected}', got '{actual}'");
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine ($"\tException: {exception.Message}");
        }

        Console.WriteLine ();
    }

    #endregion
}
