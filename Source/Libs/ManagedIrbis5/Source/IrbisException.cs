// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IrbisException.cs -- исключения, специфичные для ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Базовое исключение для всех ситуаций,
    /// специфичных для ИРБИС64.
    /// </summary>
    [DebuggerDisplay("Code={" + nameof(ErrorCode) + "}, Message={" + nameof(Message) + "}")]
    public class IrbisException
        : ArsMagnaException
    {
        #region Properties

        /// <summary>
        /// Return (error) code.
        /// Less than zero means error.
        /// Zero or more means normal execution.
        /// </summary>
        public int ErrorCode { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Default constructor.
        /// </summary>
        public IrbisException()
        {
        }

        /// <summary>
        /// Constructor with return (error) code.
        /// </summary>
        public IrbisException
            (
                int returnCode
            )
            : base(GetErrorDescription(returnCode))
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
            : base(message)
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
            : base(message, innerException)
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
            return string.IsNullOrEmpty(exception.Message)
                ? GetErrorDescription(exception.ErrorCode)
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
            return string.Empty;
            /*

            var result = Resources.ErrorDescription_UnknownError;

            if (code > 0)
            {
                result = Resources.ErrorDescription_NoError;
            }
            else
            {
                switch (code)
                {
                    case 0:
                        result = Resources.ErrorDescription_NormalReturn;
                        break;

                    case -100:
                        result = Resources.ErrorDescription_GivenMfnOutsideTheDatabaseRange;
                        break;

                    case -101:
                        result = Resources.ErrorDescription_BadShelfSize;
                        break;

                    case -102:
                        result = Resources.ErrorDescription_BadShelfNumber;
                        break;

                    case -140:
                        result = Resources.ErrorDescription_MfnOutsideTheDatabaseRange;
                        break;

                    case -141:
                        result = Resources.ErrorDescription_ReadError;
                        break;

                    case -200:
                        result = Resources.ErrorDescription_FieldIsAbsent;
                        break;

                    case -201:
                        result = Resources.ErrorDescription_NoPreviousVersionOfTheRecord;
                        break;

                    case -202:
                        result = Resources.ErrorDescription_TermNotExist;
                        break;

                    case -203:
                        result = Resources.ErrorDescription_LastTerm;
                        break;

                    case -204:
                        result = Resources.ErrorDescription_FirstTerm;
                        break;

                    case -300:
                        result = Resources.ErrorDescription_DatabaseIsLocked;
                        break;

                    case -301:
                        result = Resources.ErrorDescription_DatabaseIsLocked;
                        break;

                    case -400:
                        result = Resources.ErrorDescription_MstError;
                        break;

                    case -401:
                        result = Resources.ErrorDescription_IfpError;
                        break;

                    case -402:
                        result = Resources.ErrorDescription_WriteError;
                        break;

                    case -403:
                        result = Resources.ErrorDescription_ActualizationError;
                        break;

                    case -600:
                        result = Resources.ErrorDescription_RecordIsLogicallyDeleted;
                        break;

                    case -601:
                        result = Resources.ErrorDescription_RecordIsPhysicallyDeleted;
                        break;

                    case -602:
                        result = Resources.ErrorDescription_RecordIsBlocked;
                        break;

                    case -603:
                        result = Resources.ErrorDescription_RecordIsLogicallyDeleted;
                        break;

                    case -605:
                        result = Resources.ErrorDescription_RecordIsPhysicallyDeleted;
                        break;

                    case -607:
                        result = Resources.ErrorDescription_AutoinGblError;
                        break;

                    case -608:
                        result = Resources.ErrorDescription_RecordVersionError;
                        break;

                    case -700:
                        result = Resources.ErrorDescription_BackupCreationError;
                        break;

                    case -701:
                        result = Resources.ErrorDescription_BackupRestoreError;
                        break;

                    case -702:
                        result = Resources.ErrorDescription_SortingError;
                        break;

                    case -703:
                        result = Resources.ErrorDescription_BadTerm;
                        break;

                    case -704:
                        result = Resources.ErrorDescription_DictionaryCreationError;
                        break;

                    case -705:
                        result = Resources.ErrorDescription_DictionaryLoadingError;
                        break;

                    case -800:
                        result = Resources.ErrorDescription_GlobalCorrectionParameterError;
                        break;

                    case -801:
                        result = Resources.ErrorDescription_ERR_GBL_REP;
                        break;

                    case -802:
                        result = Resources.ErrorDescription_ERR_GBL_MET;
                        break;

                    case -1111:
                        result = Resources.ErrorDescription_ServerExecutionError;
                        break;

                    case -2222:
                        result = Resources.ErrorDescription_ProtocolError;
                        break;

                    case -3333:
                        result = Resources.ErrorDescription_ClientNotRegistered;
                        break;

                    case -3334:
                        result = Resources.ErrorDescription_ClientNotInUse;
                        break;

                    case -3335:
                        result = Resources.ErrorDescription_BadClientIdentifier;
                        break;

                    case -3336:
                        result = Resources.ErrorDescription_WorkstationAccesDenied;
                        break;

                    case -3337:
                        result = Resources.ErrorDescription_ClientAlreadyRegistered;
                        break;

                    case -3338:
                        result = Resources.ErrorDescription_ClientNotAllowed;
                        break;

                    case -4444:
                        result = Resources.ErrorDescription_BadPassword;
                        break;

                    case -5555:
                        result = Resources.ErrorDescription_FileNotExist;
                        break;

                    case -6666:
                        result = Resources.ErrorDescription_ServerOverloaded;
                        break;

                    case -7777:
                        result = Resources.ErrorDescription_ProcessError;
                        break;

                    case -8888:
                        result = Resources.ErrorDescription_GeneralError;
                        break;
                }
            }

            return result;

            */
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="ArsMagnaException.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "ErrorCode: {2}{1}Description: {3}{1}{0}",
                    base.ToString(),
                    Environment.NewLine,
                    ErrorCode,
                    Message
                );
        }

        #endregion
    }
}