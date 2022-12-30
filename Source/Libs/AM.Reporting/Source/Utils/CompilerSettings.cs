// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

#endregion

#nullable enable

namespace AM.Reporting.Utils
{
    /// <summary>
    /// Specifies the behaviour of compiler when exception is thrown.
    /// </summary>
    public enum CompilerExceptionBehaviour
    {
        /// <summary>
        /// Default behaviour. Throw exception.
        /// </summary>
        Default,

        /// <summary>
        /// Show exception message and replace incorrect expression by <b>Placeholder</b>.
        /// </summary>
        ShowExceptionMessage,

        /// <summary>
        /// Replace expression with exception message. Don't show any messages.
        /// </summary>
        ReplaceExpressionWithExceptionMessage,

        /// <summary>
        /// Replace exception with <b>Placeholder</b> value. Don't show any messages.
        /// </summary>
        ReplaceExpressionWithPlaceholder
    }

    /// <summary>
    /// Contains compiler settings.
    /// </summary>
    public class CompilerSettings
    {
        #region Fields

        private string placeholder;
        private CompilerExceptionBehaviour exceptionBehaviour;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or set the string that will be used for replacing incorrect expressions.
        /// </summary>
        public string Placeholder
        {
            get { return placeholder; }
            set { placeholder = value; }
        }

        /// <summary>
        /// Gets or sets the behaviour of compiler when exception is thrown.
        /// </summary>
        public CompilerExceptionBehaviour ExceptionBehaviour
        {
            get { return exceptionBehaviour; }
            set { exceptionBehaviour = value; }
        }

        /// <summary>
        /// Get or sets number of recompiles
        /// </summary>
        /// <remarks>
        /// Report compiler can try to fix compilation errors and recompile your report again. This property sets the number of such attempts.
        /// </remarks>
        public int RecompileCount { get; set; } = 1;

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompilerSettings"/> class.
        /// </summary>
        public CompilerSettings()
        {
            placeholder = "";
            exceptionBehaviour = CompilerExceptionBehaviour.Default;
        }

        #endregion Constructors
    }
}
