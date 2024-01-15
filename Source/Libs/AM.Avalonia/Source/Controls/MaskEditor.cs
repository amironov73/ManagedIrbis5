// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* MaskEditor.cs -- редактирование маски изображения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Reflection;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;

using JetBrains.Annotations;

#endregion

namespace AM.Avalonia.Controls;

/// <summary>
/// Редактирование маски изображения.
/// </summary>
[PublicAPI]
public sealed class MaskEditor
    : Control
{
    #region Private members

    private Bitmap? _image;
    private RenderTargetBitmap? _mask;
    private bool _circleVisible;
    private Point _circlePoint;
    private double _circleSize = 10;
    private bool _drawing;
    private bool _clearing;

    private Rect GetImageRect()
    {
        var image = _image.ThrowIfNull();
        var imageBounds = new Rect (image.Size);
        return AvaloniaUtility.Fit (Bounds, imageBounds);
    }

    private Rect GetMaskRect()
    {
        var mask = _mask.ThrowIfNull();
        var imaskBounds = new Rect (mask.Size);
        return AvaloniaUtility.Fit (Bounds, imaskBounds);
    }

    private DrawingContext CreateDrawingContextNotClear
        (
            RenderTargetBitmap bitmap
        )
    {
        var bitmapType = bitmap.GetType();
        var platformProperty = bitmapType.GetProperty
            (
                "PlatformImpl",
                BindingFlags.NonPublic|BindingFlags.Instance|BindingFlags.DeclaredOnly
            );
        var platform = platformProperty!.GetValue (bitmap)!;
        var platformType = platform.GetType();
        var itemProperty = platformType.GetProperty ("Item")!;
        var item = itemProperty.GetValue (platform)!;
        var itemType = item.GetType();
        var createMethod = itemType.GetMethod ("CreateDrawingContext")!;
        var temporary = createMethod.Invoke (item, null);
        var pdcType = typeof (Brush).Assembly.GetType ("Avalonia.Media.PlatformDrawingContext")!;
        var result = (DrawingContext) Activator.CreateInstance (pdcType, temporary, true)!;
        return result;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Установка изображения, для которого редактируется маска.
    /// </summary>
    public void SetImage
        (
            Bitmap image
        )
    {
        Sure.NotNull (image);

        _image = image;
        InvalidateVisual();
    }

    /// <summary>
    /// Установка изображения, для которого редактируется маска.
    /// </summary>
    public void SetMask
        (
            Bitmap image
        )
    {
        Sure.NotNull (image);

        var size = new PixelSize
            (
                (int) image.Size.Width,
                (int) image.Size.Height
            );
        _mask = new RenderTargetBitmap (size, image.Dpi);
        using (var maskContext = CreateDrawingContextNotClear (_mask))
        {
            maskContext.DrawImage (image, new Rect (_mask.Size));
        }

        InvalidateVisual();
    }

    private void _Draw
        (
            bool clear
        )
    {
        if (_image is null
            || _mask is null
            || !_drawing)
        {
            return;
        }

        var fittedImage = GetImageRect();
        Debug.WriteLine ($"Fitted image: {fittedImage}");
        var fittedMask = GetMaskRect();
        Debug.WriteLine ($"Fitted mask: {fittedMask}");

        using var drawingContext = CreateDrawingContextNotClear (_mask);
        // using var drawingContext = _mask.CreateDrawingContext();
        var circleCenter = new Point
            (
                fittedMask.X + _circlePoint.X /* * 2.2 */,
                fittedMask.Y + _circlePoint.Y /* * 2.5 */
            );
        Debug.WriteLine($"Circle center: {circleCenter}");
        var brush = clear
            ? Brushes.Transparent
            : Brushes.Red;

        drawingContext.DrawEllipse
            (
                brush,
                null,
                circleCenter,
                _circleSize,
                _circleSize
            );

        // _mask.Save ("mask.png", 100);
    }

    #endregion

    #region Control members

    /// <inheritdoc cref="OnPointerPressed"/>
    protected override void OnPointerPressed
        (
            PointerPressedEventArgs eventArgs
        )
    {
        base.OnPointerPressed (eventArgs);
        _drawing = true;
        var kind = eventArgs.GetCurrentPoint (null).Properties.PointerUpdateKind;
        _clearing = kind != PointerUpdateKind.LeftButtonPressed;
        _Draw (_clearing);
        InvalidateVisual();
    }

    /// <inheritdoc cref="Control.OnPointerReleased"/>
    protected override void OnPointerReleased
        (
            PointerReleasedEventArgs eventArgs
        )
    {
        base.OnPointerReleased (eventArgs);
        _drawing = false;
    }

    /// <inheritdoc cref="InputElement.OnPointerMoved"/>
    protected override void OnPointerMoved
        (
            PointerEventArgs eventArgs
        )
    {
        base.OnPointerMoved (eventArgs);

        if (_image is null)
        {
            return;
        }

        var cursorPosition = eventArgs.GetPosition (this);
        var imageRect = GetImageRect();
        cursorPosition = new Point
            (
                cursorPosition.X - imageRect.X,
                cursorPosition.Y - imageRect.Y
            );
        Debug.WriteLine ($"Cursor position: {cursorPosition}");
        _circleVisible = cursorPosition.X >= 0 && cursorPosition.X < imageRect.Width
            && cursorPosition.Y > 0 && cursorPosition.Y < imageRect.Height;
        if (_circleVisible)
        {
            _circlePoint = cursorPosition;
            // TODO скрыть курсор

            if (_drawing)
            {
                _Draw (_clearing);
            }
        }
        else
        {
            // TODO показать курсор
        }

        InvalidateVisual();
    }

    /// <inheritdoc cref="Visual.Render"/>
    public override void Render
        (
            DrawingContext context
        )
    {
        Sure.NotNull (context);

        context.FillRectangle
            (
                Brushes.Coral,
                Bounds
            );

        if (_image is not null)
        {
            var imageBounds = new Rect (_image.Size);
            var fittedImage = GetImageRect();
            context.DrawImage (_image, imageBounds, fittedImage);
            if (_mask is not null)
            {
                // var maskBounds = new Rect (_mask.Size);
                // var fittedMask = GetMaskRect();
                var fittedMask = new Rect (0, 0, fittedImage.Width * 2, fittedImage.Height * 2);
                context.DrawImage (_mask, imageBounds, fittedMask);
            }

            if (_circleVisible)
            {
                var circleCenter = new Point
                    (
                        fittedImage.X + _circlePoint.X,
                        fittedImage.Y + _circlePoint.Y
                    );
                context.DrawEllipse
                    (
                        Brushes.White,
                        new Pen (Brushes.Black),
                        circleCenter,
                        _circleSize,
                        _circleSize
                    );
            }
        }
    }

    #endregion
}
