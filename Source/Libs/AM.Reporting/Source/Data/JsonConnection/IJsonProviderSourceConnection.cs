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

#nullable enable

namespace FastReport.Data.JsonConnection
{
    /// <summary>
    /// Provider for getting a json object fron connection source
    /// </summary>
    public interface IJsonProviderSourceConnection
    {
        /// <summary>
        /// Returns JsonBase object from connection source specific by tableDataSource
        /// </summary>
        /// <param name="tableDataSource"></param>
        /// <returns></returns>
        JsonBase GetJson(TableDataSource tableDataSource);
    }
}