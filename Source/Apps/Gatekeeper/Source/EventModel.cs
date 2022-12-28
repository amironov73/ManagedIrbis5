// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* EventModel.cs -- модель данных для события посещения библиотеки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

#nullable enable

namespace Gatekeeper;

/// <summary>
/// Модель данных для события посещения библиотеки.
/// </summary>
internal sealed class EventModel
    : ReactiveObject
{
    #region Properties

    /// <summary>
    /// Номер читательского билета.
    /// </summary>
    [Reactive]
    public string? Ticket { get; set; }

    /// <summary>
    /// ФИО.
    /// </summary>
    [Reactive]
    public string? Name { get; set; }

    /// <summary>
    /// Действие -- вошел или вышел.
    /// </summary>
    [Reactive]
    public string? Action { get; set; }

    /// <summary>
    /// Дата и время события.
    /// </summary>
    [Reactive]
    public string? Moment { get; set; }

    #endregion

    #region Public methods

    public static EventModel[] GetTestEvents => new EventModel[]
    {
        new() { Moment = "28.12.2022 12:01:01", Ticket = "1234", Name = "Толстой, Лев Николаевич" },
        new() { Moment = "28.12.2022 12:02:01", Ticket = "1235", Name = "Абельдяев, Дмитрий Алексеевич" },
        new() { Moment = "28.12.2022 12:03:01", Ticket = "1236", Name = "Ашкинази, Михаил Осипович" },
        new() { Moment = "28.12.2022 12:04:01", Ticket = "1237", Name = "Бажин, Николай Федотович" },
        new() { Moment = "28.12.2022 12:05:01", Ticket = "1238", Name = "Брюсов, Валерий Яковлевич" },
        new() { Moment = "28.12.2022 12:06:01", Ticket = "1239", Name = "Булгарин, Фаддей Венедиктович" },
        new() { Moment = "28.12.2022 12:07:01", Ticket = "1240", Name = "Вагин, Всеволод Иванович" },
        new() { Moment = "28.12.2022 12:08:01", Ticket = "1241", Name = "Воскресенский, Аполлинарий Константинович" },
        new() { Moment = "28.12.2022 12:09:01", Ticket = "1242", Name = "Вышеславцев, Михаил Михайлович" },
        new() { Moment = "28.12.2022 12:10:01", Ticket = "1243", Name = "Габбе, Пётр Андреевич" },
        new() { Moment = "28.12.2022 12:11:01", Ticket = "1244", Name = "Гарин-Михайловский, Николай Георгиевич" },
        new() { Moment = "28.12.2022 12:12:01", Ticket = "1245", Name = "Гоголь, Николай Васильевич" },
        new() { Moment = "28.12.2022 12:13:01", Ticket = "1246", Name = "Гончаров, Иван Александрович" },
        new() { Moment = "28.12.2022 12:14:01", Ticket = "1247", Name = "Грибоедов, Александр Сергеевич" },
        new() { Moment = "28.12.2022 12:15:01", Ticket = "1248", Name = "Гурвич, Саул Исраэль" },
        new() { Moment = "28.12.2022 12:16:01", Ticket = "1249", Name = "Даль, Владимир Иванович" },
        new() { Moment = "28.12.2022 12:17:01", Ticket = "1250", Name = "Державин, Гавриил Романович" },
        new() { Moment = "28.12.2022 12:18:01", Ticket = "1251", Name = "Добролюбов, Николай Александрович" },
        new() { Moment = "28.12.2022 12:19:01", Ticket = "1252", Name = "Достоевский, Фёдор Михайлович" },
        new() { Moment = "28.12.2022 12:20:01", Ticket = "1253", Name = "Елагин, Николай Васильевич" },
        new() { Moment = "28.12.2022 12:21:01", Ticket = "1254", Name = "Ершов, Пётр Павлович" },
        new() { Moment = "28.12.2022 12:22:01", Ticket = "1255", Name = "Жадовская, Елизавета Александровна" },
    };

    #endregion


    #region Object members

    public override string ToString() => $"{Moment}: {Name}";

    #endregion
}
