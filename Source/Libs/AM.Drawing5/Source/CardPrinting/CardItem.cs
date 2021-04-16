// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* CardItem.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace AM.Drawing.CardPrinting
{
    /// <summary>
    /// Элемент читательской карточки: текст,
    /// штрих-код или картинка.
    /// </summary>
    public class CardItem
    {
        #region Properties

        [XmlElement("left")]
        [DisplayName("X")]
        public int Left { get; set; }

        [XmlElement("top")] [DisplayName("Y")] public int Top { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Собственно, здесь и происходит рисование
        /// элементов карточки.
        /// </summary>
        /// <param name="context">Контекст рисования.</param>
        public virtual void Draw(DrawingContext context)
        {
        }

        #endregion
    }
}
