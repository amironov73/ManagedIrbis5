// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* BookSellingIndex.cs -- комплексный книготорговый индекс-шифр
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Systematization;

//
// https://ru.wikipedia.org/wiki/Комплексный_книготорговый_индекс_шифр
//
// Комплексный книготорговый индекс-шифр — стандартный элемент выходных
// сведений, предназначенный для классификации книги в книготорговой
// сети (книжных магазинах и т. д.). Он применялся в выходных сведениях
// изданий СССР и России до вступления в силу ГОСТ 7.4-95.
//
// Комплексный книготорговый индекс-шифр имел структуру
//
//      B - C
// A ----------- H
//    D(E) - G
//
// где:
//
// A — первая буква авторского знака.
// B — десятизначный код «Единой классификации литературы для книгоиздания
//     в СССР». Две первые цифры — это раздел тематической классификации,
//     следующие две — подраздел, далее — рубрика, затем — подрубрика,
//     и последние две цифры — еще более мелкое деление. Соответствующие
//     коды опубликованы в «Единой классификации…» По ГОСТ 7.4—86 у книг,
//     брошюр и нотных изданий десятизначный код располагался в левом нижнем
//     углу отворота титульного листа, а в однолистных изданиях — в левом
//     нижнем углу поля листа.
// C — номер издания среди выпущенных данным издательством в данном
//     отчётном году.
// D — код издательства по «Единой классификации литературы для книгоиздания
//     в СССР».
// E — код ведомства, которому подчинено издательство.
// G — год выпуска издания.
// H — документ, в котором было объявлено о выходе данного издания,
//     либо «без объявл.».
//

/// <summary>
/// Комплексный книготорговый индекс-шифр.
/// </summary>
public class BookSellingIndex
{
    #region Properties

    /// <summary>
    /// Авторский знак.
    /// </summary>
    public string? AuthorSign { get; set; }

    /// <summary>
    /// Код по "Единой классификации литературы для книгоиздания
    /// в СССР".
    /// </summary>
    public string? Classification { get; set; }

    /// <summary>
    /// Номер издания среди выпущенных данным издательство
    /// в данном отчетном году.
    /// </summary>
    public string? Number { get; set; }

    /// <summary>
    /// Код издательства по "Единой классификации литературы
    /// для книгоиздания в СССР".
    /// </summary>
    public string? Publisher { get; set; }

    /// <summary>
    /// Код ведомства, которому подчинено издательство.
    /// </summary>
    public string? Department { get; set; }

    /// <summary>
    /// Год выпуска издания.
    /// </summary>
    public string? Year { get; set; }

    /// <summary>
    /// Документ, в котором было объявлено о выходе данного
    /// издания, либо "без объявл.".
    /// </summary>
    public string? Announcement { get; set; }

    #endregion
}
