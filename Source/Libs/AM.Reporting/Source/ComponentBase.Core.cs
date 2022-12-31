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

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting
{
    partial class ComponentBase
    {
        /// <summary>
        /// Draws the object.
        /// </summary>
        /// <param name="eventArgs">Paint event args.</param>
        /// <remarks>
        /// <para>This method is widely used in the AM.Reporting. It is called each time when the object needs to draw
        /// or print itself.</para>
        /// <para>In order to draw the object correctly, you should multiply the object's bounds by the <b>scale</b>
        /// parameter.</para>
        /// <para><b>cache</b> parameter is used to optimize the drawing speed. It holds all items such as
        /// pens, fonts, brushes, string formats that was used before. If the item with requested parameters
        /// exists in the cache, it will be returned (instead of create new item and then dispose it).</para>
        /// </remarks>
        public virtual void Draw (FRPaintEventArgs eventArgs)
        {
        }
    }
}
