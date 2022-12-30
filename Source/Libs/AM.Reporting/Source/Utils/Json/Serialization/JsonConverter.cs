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

using System.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace AM.Reporting.Utils.Json.Serialization
{
    public static partial class JsonConverter
    {
        public static T Deserialize<T> (string json)
        {
            return JsonDeserializer.Deserialize<T> (json);
        }

        public static string Serialize<T> (T instance)
        {
            return JsonSerializer.Serialize (instance);
        }

        public static byte[] SerializeToBytes<T> (T instance)
        {
            var strContent = Serialize (instance);
            var bytes = Encoding.UTF8.GetBytes (strContent);
            return bytes;
        }
    }
}
