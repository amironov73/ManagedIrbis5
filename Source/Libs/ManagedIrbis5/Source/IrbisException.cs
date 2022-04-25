// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IrbisException.cs -- исключения, специфичные для ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis;

/// <summary>
/// Базовое исключение для всех ситуаций,
/// специфичных для ИРБИС64.
/// </summary>
public class IrbisException
    : ArsMagnaException
{
    #region Properties

    /// <summary>
    /// Return (error) code.
    /// Less than zero means error.
    /// Zero or more means normal execution.
    /// </summary>
    public int ErrorCode { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public IrbisException()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Constructor with return (error) code.
    /// </summary>
    public IrbisException
        (
            int returnCode
        )
        : base (GetErrorDescription (returnCode))
    {
        ErrorCode = returnCode;
    }

    /// <summary>
    /// Constructor with error message.
    /// </summary>
    public IrbisException
        (
            string message
        )
        : base (message)
    {
    }

    /// <summary>
    /// Constructor with error message and inner exception.
    /// </summary>
    public IrbisException
        (
            string message,
            Exception innerException
        )
        : base (message, innerException)
    {
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Get text description of the error.
    /// </summary>
    public static string GetErrorDescription
        (
            IrbisException exception
        )
    {
        return string.IsNullOrEmpty (exception.Message)
            ? GetErrorDescription (exception.ErrorCode)
            : exception.Message;
    }

    /// <summary>
    /// Get text description ot the error.
    /// </summary>
    public static string GetErrorDescription
        (
            int code
        )
    {
        var result = "Unknown error";

        if (code > 0)
        {
            result = "No error";
        }
        else
        {
            switch (code)
            {
                case 0:
                    result = "Normal return";
                    break;

                case -100:
                    result = "Given MFN is outside the database range";
                    break;

                case -101:
                    result = "Bad shelf size";
                    break;

                case -102:
                    result = "Bad shelf number";
                    break;

                case -140:
                    result = "MFN outside the database range";
                    break;

                case -141:
                    result = "Read error";
                    break;

                case -200:
                    result = "The field is absent";
                    break;

                case -201:
                    result = "No previous version of the record";
                    break;

                case -202:
                    result = "Term doesn't exist";
                    break;

                case -203:
                    result = "Last term in the list";
                    break;

                case -204:
                    result = "First term in the list";
                    break;

                case -300:
                    result = "Database is exclusively locked";
                    break;

                case -301:
                    result = "Database is exclusively locked";
                    break;

                case -400:
                    result = "Master file error";
                    break;

                case -401:
                    result = "Index file error";
                    break;

                case -402:
                    result = "Write error";
                    break;

                case -403:
                    result = "Error during record actualization";
                    break;

                case -600:
                    result = "The record is logically deleted";
                    break;

                case -601:
                    result = "The record is physically deleted";
                    break;

                case -602:
                    result = "The record is blocked";
                    break;

                case -603:
                    result = "The record is logically deleted";
                    break;

                case -605:
                    result = "The record is physically deleted";
                    break;

                case -607:
                    result = "Error during autoin.gbl processing";
                    break;

                case -608:
                    result = "Record version mismatch";
                    break;

                case -700:
                    result = "Backup creation error";
                    break;

                case -701:
                    result = "Backup restore error";
                    break;

                case -702:
                    result = "Error during sorting";
                    break;

                case -703:
                    result = "Bad term";
                    break;

                case -704:
                    result = "Dictionary creation error";
                    break;

                case -705:
                    result = "Dictionary loading error";
                    break;

                case -800:
                    result = "Global correction parameter error";
                    break;

                case -801:
                    result = "Global correction: bad rep tag";
                    break;

                case -802:
                    result = "Global correction: bad tag";
                    break;

                case -1111:
                    result = "Server execution error";
                    break;

                case -2222:
                    result = "Protocol error";
                    break;

                case -3333:
                    result = "Client not registered";
                    break;

                case -3334:
                    result = "Client doesn't in use";
                    break;

                case -3335:
                    result = "Bad client identifier";
                    break;

                case -3336:
                    result = "Workstation access denied";
                    break;

                case -3337:
                    result = "The client is already registered";
                    break;

                case -3338:
                    result = "Client not allowed";
                    break;

                case -4444:
                    result = "Bad password";
                    break;

                case -5555:
                    result = "File not exist";
                    break;

                case -6666:
                    result = "Server is overloaded";
                    break;

                case -7777:
                    result = "Administrator thread error";
                    break;

                case -8888:
                    result = "General error";
                    break;
            }
        }

        return result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString() => string.Format
        (
            "ErrorCode: {2}{1}Description: {3}{1}{0}",
            base.ToString(),
            Environment.NewLine,
            ErrorCode,
            Message
        );

    #endregion
}
