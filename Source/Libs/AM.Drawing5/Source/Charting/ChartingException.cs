// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* ChartingException.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.Serialization;

#endregion

#nullable enable

namespace AM.Drawing.Charting;

/// <summary>
/// An exception thrown by Charting.
/// A child class of <see cref="ApplicationException"/>.
/// </summary>
public class ChartingException
    : ApplicationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChartingException"/>
    /// class with serialized data.
    /// </summary>
    /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/>
    /// instance that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"/>
    /// instance that contains contextual information about the source or destination.</param>
    protected ChartingException
        (
            SerializationInfo info,
            StreamingContext context
        )
        : base (info, context)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Exception"/> class with a specified
    /// error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.
    /// If the innerException parameter is not a null reference, the current exception is raised
    /// in a catch block that handles the inner exception.</param>
    public ChartingException
        (
            string message,
            Exception innerException
        )
        : base (message, innerException)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Exception"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public ChartingException
        (
            string message
        )
        : base (message)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Exception"/> class.
    /// </summary>
    public ChartingException()
    {
        // пустое тело конструктора
    }
}
