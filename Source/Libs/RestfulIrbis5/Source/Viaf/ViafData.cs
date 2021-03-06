﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ViafData.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using AM;
using AM.Json;

#endregion

#nullable enable

namespace RestfulIrbis.Viaf
{
    /// <summary>
    /// Данные о записи VIAF.
    /// </summary>
    public class ViafData
    {
        #region Properties

        /// <summary>
        /// VIAF id.
        /// </summary>
        public string? ViafId { get; set; }

        /// <summary>
        /// Name type.
        /// </summary>
        public string? NameType { get; set; }

        /// <summary>
        /// Sources.
        /// </summary>
        public ViafSource[]? Sources { get; set; }

        /// <summary>
        /// Main headings.
        /// </summary>
        public ViafMainHeading[]? MainHeadings { get; set; }

        /// <summary>
        /// Heading elements.
        /// </summary>
        public ViafHeadingElement[]? HeadingElements { get; set; }

        /// <summary>
        ///
        /// </summary>
        public object[]? X400 { get; set; }

        /// <summary>
        ///
        /// </summary>
        public object[]? X500 { get; set; }

        /// <summary>
        /// Links.
        /// </summary>
        public ViafLink[]? Links { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the object.
        /// </summary>
        public static ViafData Parse
            (
                // TODO: implement
                object obj
                // JObject obj
            )
        {
            /*
            var result = new ViafData
            {
                ViafId = obj["viafID"].NullableToString(),
                NameType = obj["nameType"].NullableToString(),
                Sources = ViafSource.Parse(obj.SelectArray("$.sources.source")),
                MainHeadings = ViafMainHeading.Parse(obj.SelectArray("$.mainHeadings.data")),
                HeadingElements = ViafHeadingElement.Parse(obj.SelectArray("$.mainHeadings.mainHeadingEl")),
                Links = ViafLink.Parse(obj.SelectArray("$.xLinks.xLink"))

            };

            return result;

            */

            throw new NotImplementedException();
        }

        #endregion
    }
}
