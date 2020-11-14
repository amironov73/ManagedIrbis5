// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IrbisPath.cs -- путь к файлам на сервере ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Sockets;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Путь к файлам на сервере ИРБИС64.
    /// </summary>
    public enum IrbisPath
    {
        /// <summary>
        /// Общесистемный путь
        /// </summary>
        System = 0,

        /// <summary>
        /// путь размещения сведений о базах данных сервера ИРБИС64
        /// </summary>
        Data = 1,

        /// <summary>
        /// путь на мастер-файл базы данных
        /// </summary>
        MasterFile = 2,

        /// <summary>
        /// путь на словарь базы данных
        /// </summary>
        InvertedFile = 3,

        /// <summary>
        /// путь на параметрию базы данных
        /// </summary>
        ParameterFile = 10,

        /// <summary>
        /// Полный текст
        /// </summary>
        FullText = 11,

        /// <summary>
        /// Внутренний ресурс
        /// </summary>
        InternalResource = 12
    }
}