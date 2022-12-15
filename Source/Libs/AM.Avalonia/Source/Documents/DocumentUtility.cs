// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* DocumentUtility.cs -- полезные расширения для работы с документами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;

using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Media;

#endregion

#nullable enable

namespace AM.Avalonia.Documents;

/// <summary>
/// Полезные расширения для работы с документами.
/// </summary>
public static class DocumentUtility
{
    #region Public methods

    /// <summary>
    /// Добавление inline-элемента в коллекцию.
    /// </summary>
    public static void AddInline
        (
            this InlineCollection collection,
            object item
        )
    {
        Sure.NotNull (collection);
        Sure.NotNull (item);

        switch (item)
        {
            case Inline inline:
                collection.Add (inline);
                break;

            case string text:
                collection.Add (text);
                break;

            case Control control:
                collection.Add (control);
                break;

            default:
                throw new ApplicationException ($"Don't know how to handle type {item.GetType()}");
        }
    }

    /// <summary>
    /// Цвет фона.
    /// </summary>
    public static TElement Background<TElement>
        (
            this TElement element,
            IBrush brush
        )
        where TElement: TextElement
    {
        Sure.NotNull (element);
        Sure.NotNull (brush);

        element.Background = brush;

        return element;
    }

    /// <summary>
    /// Цвет фона.
    /// </summary>
    public static Run Background
        (
            this string text,
            IBrush brush
        )
    {
        Sure.NotNull (brush);

        return new Run (text) { Background = brush };
    }

    /// <summary>
    /// Базовая линия.
    /// </summary>
    public static TInline Baseline<TInline>
        (
            this TInline element,
            BaselineAlignment alignment
        )
        where TInline: Inline
    {
        Sure.NotNull (element);
        Sure.Defined (alignment);

        element.BaselineAlignment = alignment;

        return element;
    }

    /// <summary>
    /// Полужирный текст.
    /// </summary>
    public static TElement Bold<TElement>
        (
            this TElement element,
            FontWeight weight = FontWeight.Bold
        )
        where TElement: TextElement
    {
        Sure.NotNull (element);
        Sure.Defined (weight);

        element.FontWeight = weight;

        return element;
    }

    /// <summary>
    /// Полужирный текст.
    /// </summary>
    public static Run Bold
        (
            this string text,
            FontWeight weight = FontWeight.Bold
        )
    {
        Sure.Defined (weight);

        return new Run (text) { FontWeight = weight };
    }

    /// <summary>
    /// Гарнитура.
    /// </summary>
    public static TElement FontFamily<TElement>
        (
            this TElement element,
            string family
        )
        where TElement: TextElement
    {
        Sure.NotNull (element);
        Sure.NotNullNorEmpty (family);

        element.FontFamily = new FontFamily (family);

        return element;
    }

    /// <summary>
    /// Гарнитура.
    /// </summary>
    public static Run FontFamily
        (
            this string text,
            string family
        )
    {
        Sure.NotNullNorEmpty (family);

        return new Run (text) { FontFamily = new FontFamily (family) };
    }

    /// <summary>
    /// Гарнитура.
    /// </summary>
    public static TElement FontFamily<TElement>
        (
            this TElement element,
            FontFamily family
        )
        where TElement: TextElement
    {
        Sure.NotNull (element);
        Sure.NotNull (family);

        element.FontFamily = family;

        return element;
    }

    /// <summary>
    /// Гарнитура.
    /// </summary>
    public static Run FontFamily
        (
            this string text,
            FontFamily family
        )
    {
        Sure.NotNull (family);

        return new Run (text) { FontFamily = family };
    }

    /// <summary>
    /// Размер шрифта.
    /// </summary>
    public static TElement FontSize<TElement>
        (
            this TElement element,
            double fontSize
        )
        where TElement: TextElement
    {
        Sure.NotNull (element);
        Sure.Positive (fontSize);

        element.FontSize = fontSize;

        return element;
    }

    /// <summary>
    /// Размер шрифта.
    /// </summary>
    public static Run FontSize
        (
            this string text,
            double fontSize
        )
    {
        Sure.Positive (fontSize);

        return new Run (text) { FontSize = fontSize };
    }

    /// <summary>
    /// Цвет символов.
    /// </summary>
    public static TElement Foreground<TElement>
        (
            this TElement element,
            IBrush brush
        )
        where TElement: TextElement
    {
        Sure.NotNull (element);
        Sure.NotNull (brush);

        element.Foreground = brush;

        return element;
    }

    /// <summary>
    /// Цвет символов.
    /// </summary>
    public static Run Foreground
        (
            this string text,
            IBrush brush
        )
    {
        Sure.NotNull (brush);

        return new Run (text) { Foreground = brush };
    }

    /// <summary>
    /// Курсивное начертание.
    /// </summary>
    public static TElement Italic<TElement>
        (
            this TElement element,
            FontStyle style = FontStyle.Italic
        )
        where TElement: TextElement
    {
        Sure.NotNull (element);
        Sure.Defined (style);

        element.FontStyle = style;

        return element;
    }

    /// <summary>
    /// Курсивное начертание.
    /// </summary>
    public static Run Italic
        (
            this string text,
            FontStyle style = FontStyle.Italic
        )
    {
        Sure.Defined (style);

        return new Run (text) { FontStyle = style };
    }

    /// <summary>
    /// Растягивание текста.
    /// </summary>
    public static TElement Stretch<TElement>
        (
            this TElement element,
            FontStretch stretch
        )
        where TElement: TextElement
    {
        Sure.NotNull (element);
        Sure.Defined (stretch);

        element.FontStretch = stretch;

        return element;
    }

    /// <summary>
    /// Растягивание текста.
    /// </summary>
    public static Run Stretch
        (
            this string text,
            FontStretch stretch
        )
    {
        Sure.Defined (stretch);

        return new Run (text) { FontStretch = stretch };
    }

    /// <summary>
    /// Подчеркивание.
    /// </summary>
    public static TInline Underline<TInline>
        (
            this TInline element
        )
        where TInline: Inline
    {
        Sure.NotNull (element);

        element.TextDecorations ??= new ();
        element.TextDecorations.Add
            (
                new TextDecoration
                {
                    Location = TextDecorationLocation.Underline,
                    Stroke = element.Foreground ?? Brushes.Black,
                    StrokeThicknessUnit = TextDecorationUnit.FontRecommended,
                    StrokeThickness = 1.0
                }
            );

        return element;
    }

    /// <summary>
    /// Подчеркивание.
    /// </summary>
    public static Run Underline
        (
            this string text
        )
    {
        return new Run (text) { TextDecorations = new ()
        {
            new()
            {
                Location = TextDecorationLocation.Underline,
                Stroke = Brushes.Black,
                StrokeThicknessUnit = TextDecorationUnit.FontRecommended,
                StrokeThickness = 1.0
            }
        }};
    }

    /// <summary>
    /// Пополнение фрагмента всякими элементами.
    /// </summary>
    public static TSpan With<TSpan>
        (
            this TSpan span,
            object item
        )
        where TSpan: Span
    {
        Sure.NotNull (span);
        Sure.NotNull (item);

        AddInline (span.Inlines, item);

        return span;
    }

    /// <summary>
    /// Пополнение фрагмента всякими элементами.
    /// </summary>
    public static TSpan With<TSpan>
        (
            this TSpan span,
            object item1,
            object item2
        )
        where TSpan: Span
    {
        Sure.NotNull (span);
        Sure.NotNull (item1);
        Sure.NotNull (item2);

        AddInline (span.Inlines, item1);
        AddInline (span.Inlines, item2);

        return span;
    }

    /// <summary>
    /// Пополнение фрагмента всякими элементами.
    /// </summary>
    public static TSpan With<TSpan>
        (
            this TSpan span,
            object item1,
            object item2,
            object item3
        )
        where TSpan: Span
    {
        Sure.NotNull (span);
        Sure.NotNull (item1);
        Sure.NotNull (item2);
        Sure.NotNull (item3);

        AddInline (span.Inlines, item1);
        AddInline (span.Inlines, item2);
        AddInline (span.Inlines, item3);

        return span;
    }

    /// <summary>
    /// Пополнение фрагмента всякими элементами.
    /// </summary>
    public static TSpan With<TSpan>
        (
            this TSpan span,
            params object[] items
        )
        where TSpan: Span
    {
        Sure.NotNull (span);
        Sure.NotNull (items);

        foreach (var item in items)
        {
            span.With (item);
        }

        return span;
    }

    /// <summary>
    /// Пополнение фрагмента всякими элементами.
    /// </summary>
    public static TSpan With<TSpan>
        (
            this TSpan span,
            IEnumerable items
        )
        where TSpan: Span
    {
        Sure.NotNull (span);
        Sure.NotNull (items);

        foreach (var item in items)
        {
            span.With (item);
        }

        return span;
    }

    /// <summary>
    /// Пополнение текстового блока всякими элементами.
    /// </summary>
    public static TBlock WithInlines<TBlock>
        (
            this TBlock textBlock,
            params object[] items
        )
        where TBlock: TextBlock
    {
        Sure.NotNull (textBlock);
        Sure.NotNull (items);

        textBlock.Inlines ??= new InlineCollection();
        foreach (var item in items)
        {
            AddInline (textBlock.Inlines, item);
        }

        return textBlock;
    }

    #endregion
}
