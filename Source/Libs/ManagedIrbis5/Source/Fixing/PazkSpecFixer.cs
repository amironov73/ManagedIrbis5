// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable UnusedType.Global

/* PazkSpecFixer.cs -- преобразует запись из рабочего листа PAZK в SPEC
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using ManagedIrbis.Records;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Fixing;

/// <summary>
/// Преобразует библиографическую запись из рабочего листа PAZK
/// в SPEC, если это необходимо.
/// </summary>
public sealed class PazkSpecFixer
{
    #region Properties

    /// <summary>
    /// Хост - для получения различных сервисов.
    /// </summary>
    public IHost Host { get; }

    /// <summary>
    /// Логгер.
    /// </summary>
    public ILogger Logger { get; }

    /// <summary>
    /// Конфигурация записей.
    /// </summary>
    public RecordConfiguration Configuration { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PazkSpecFixer
        (
            IHost host,
            RecordConfiguration? configuration = null
        )
    {
        Sure.NotNull (host);

        Host = host;
        Logger = LoggingUtility.GetLogger (host, typeof (PazkSpecFixer));
        Configuration = configuration ?? RecordConfiguration.GetDefault();
    }

    #endregion

    #region Private members

    /// <summary>
    /// Изменяем код подполя, если оно есть.
    /// </summary>
    private static void ChangeSubFieldCode
        (
            Field field,
            char fromCode,
            char toCode
        )
    {
        var subfield = field.GetSubField (fromCode);
        if (subfield is not null)
        {
            subfield.Code = toCode;
        }
    }

    private static void MoveSubField
        (
            Field recipient,
            char toCode,
            Field? donor,
            char fromCode,
            bool removeOriginal
        )
    {
        if (donor is null)
        {
            return;
        }

        var subfield = donor.GetSubField (fromCode);
        if (subfield is not null)
        {
            recipient.SetSubFieldValue (toCode, subfield.Value);
            if (removeOriginal)
            {
                donor.RemoveSubField (fromCode);
            }
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Исправляем указанную запись.
    /// </summary>
    /// <param name="provider">Синхронный провайдер (в данном случае
    /// не используется).</param>
    /// <param name="record">Библиографическая запись, подлежащая
    /// исправлению.</param>
    /// <returns>Признак успешного выполнения операции.</returns>
    public bool FixRecord
        (
            ISyncProvider provider,
            Record record
        )
    {
        Sure.NotNull (record);
        provider.NotUsed();

        var mfn = record.Mfn;

        // рабочий лист
        var worksheet = Configuration.GetWorksheet (record);
        if (!worksheet.SameString (Constants.Pazk))
        {
            // не тот рабочий лист
            Logger.LogInformation ("MFN={Mfn}: wrong worksheet '{Worksheet}'", mfn, worksheet);
            return true;
        }

        // основные данные общей части сводного описания тома многотомного издания
        if (record.HaveField (461))
        {
            // уже есть общая часть
            Logger.LogInformation ("MFN={Mfn}: already have field 461", mfn);
            return true;
        }

        // выпуск, часть (станет заглавием тома)
        var title = record.GetField (923);
        if (title is null)
        {
            // нет поля с выпуском/частью
            Logger.LogInformation ("MFN={Mfn}: have not field 923", mfn);
            return true;
        }

        // заглавие (станет общей частью)
        var common = record.GetField (200);
        if (common is null)
        {
            // нет поля с заглавием
            Logger.LogInformation ("MFN={Mfn}: have not field 200", mfn);
            return true;
        }

        // выходные данные
        var imprint = record.GetField (210);

        record.SetValue (Configuration.WorksheetTag, Constants.Spec);
        record.SetValue (900, 'a', "a"); // текст
        record.SetValue (900, 'b', "03"); // многотомник

        // превращаем заглавие в общую часть
        common.Tag = 461;
        ChangeSubFieldCode (common, 'a', 'c'); // общее заглавие
        MoveSubField (common, 'h', imprint, 'd', false); // год издания
        MoveSubField (common, 'g', imprint, 'c', true); // издательство
        MoveSubField (common, 'd', imprint, 'a', true); // город

        // превращаем выпуск в заглавие тома
        title.Tag = 200;
        ChangeSubFieldCode (title, 'h', 'v'); // номер тома
        ChangeSubFieldCode (title, 'i', 'a'); // собственно заглавие тома

        // переносим первого автора в общую часть
        var firstAuthor = record.GetField (700);
        if (firstAuthor is not null)
        {
            firstAuthor.Tag = 961;
            firstAuthor.SetSubFieldValue ('z', "ДА");
        }

        // переносим прочих авторов в общую часть
        foreach (var author in record.EnumerateField (701))
        {
            author.Tag = 961;
        }

        foreach (var author in record.EnumerateField (702))
        {
            author.Tag = 961;
        }

        // аналогично коллективные авторы
        var firstCollective = record.GetField (710);
        if (firstCollective is not null)
        {
            firstCollective.Tag = 962;
            firstCollective.SetSubFieldValue ('z', "ДА");
        }

        foreach (var collective in record.EnumerateField (711))
        {
            collective.Tag = 962;
        }

        foreach (var collective in record.EnumerateField (972))
        {
            collective.Tag = 962;
        }

        Logger.LogInformation ("MFN={Mfn}: success", mfn);

        return true;
    }

    #endregion
}
