﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* FstIndexMethod.cs -- метод индексирования (перечисление)
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis.Fst
{
    //
    // Метод индексирования определяет специфическую
    // обработку данных, созданных форматом.
    //
    // Имеется девять методов индексирования.
    // Эти методы широко применяются в системе ИРБИС
    // для определения принадлежности терминов к определенным
    // элементам описания. Именно на основе этих методов
    // создается модель словарей по различным элементам
    // данных ("Авторы", "Заглавие" и т.д.).
    // При этом при показе словарей соответствующие
    // префиксы опускаются.
    //
    // Метод индексирования 0 - формируется элемент из каждой
    // строки, созданной форматом.
    // Этот метод обычно используется для индексирования
    // в целом всего поля или подполя. Однако заметим,
    // что система в данном случае строит элементы
    // все же из строк, а не из полей.
    // В качестве выходного результата форматирования выступает
    // строка символов, в которой нет никакого указания
    // на ее принадлежность (или принадлежность части строки)
    // тому или иному полю или подполю. В связи с этим, надо
    // следить за правильностью формулировки формата,
    // чтобы он порождал правильные данные, особенно
    // когда производится индексация повторяющихся полей
    // и/или более чем одного поля.
    // Другими словами, при использовании данного метода
    // формат выборки данных должен быть таким,
    // чтобы он порождал точно одну строку для каждого
    // индексируемого элемента.
    //
    // Метод индексирования 1 - создается элемент из каждого
    // подполя или строки, созданных форматом
    // Так как в этом случае система будет производить
    // поиск кодов разделителей подполей в строке,
    // созданной форматом, то для обеспечения правильной работы
    // метода в формате должен быть указан режим проверки mpl
    // (или вообще не указан никакой режим, так как режим проверки
    // выбирается по умолчанию), который обеспечивает сохранность
    // разделителей подполей в выходном результате формата
    // (напомним, что режимы заголовка и данных заменяют разделители
    // подполей на знаки пунктуации). Заметим, что метод
    // индексирования 1 включает в себя метод индексирования 0.
    //
    // Метод индексирования 2 - Создается элемент из каждого
    // термина или фразы, заключенных в угловые скобки (<...>)
    // Любой текст, расположенный вне скобок, не индексируется.
    // Заметим, что данный метод требует, чтобы в формате
    // указывался режим проверки, так как любой другой режим
    // удаляет угловые скобки. Например, текст
    // <Отчет> по использованию <информатики> и <программирования> в <средней школе>
    // приведет к порождению следующих элементов:
    // отчет
    // информатики
    // программирования
    // средней школе
    //
    // Метод индексирования 3 - cоздается элемент из каждого
    // термина или фразы, заключенных в косые черты (/.../)
    // Во всем остальном он работает точно так же, как и метод индексирования 2.
    // Например, текст
    // /Отчет/ по использованию /информатики/ и /программирования/ в /средней школе/
    // приведет к порождению следующих элементов:
    // отчет
    // информатики
    // программирования
    // средней школе
    //
    // Метод индексирования 4 - cоздается элемент из каждого
    // слова в тексте, созданном форматом
    // Словом является непрерывная последовательность алфавитных символов.
    // Алфавитные символы определяются с помощью системной
    // таблицы ISISACW.TAB.
    // При использовании этого метода можно предотвратить индексацию
    // по некоторым незначащим словам, определив их в специальном файле,
    // получившем название файла стоп-слов (файл с расширением
    // STW в директории БД).
    // При использовании данного метода для индексации поля,
    // содержащего разделители подполей, в формате выборки данных
    // необходимо указать режимы заголовка или данных (mhl или mdl)
    // с тем, чтобы замена разделителей подполей произошла
    // до индексации, так как в противном случае буква разделителя
    // подполей будет рассматриваться как составная часть слова.
    //
    // Методы индексирования 5, 6, 7, 8 - присоединяют
    // к индексируемым терминам префиксы
    // Методы индексирования 5, 6, 7 и 8 аналогичны соответственно
    // методам 1, 2, 3, 4 за исключением того, что они дополнительно
    // предоставляют возможность присоединять к индексируемым
    // терминам префиксы. Присоединяемый префикс определяется
    //в формате выборки данных в виде безусловного литерала
    // и имеет следующий вид:
    // 'dp...pd', [format]
    // где:
    // d
    // выбранный по усмотрению пользователя ограничитель
    // (который не попадает в текст префикса;
    // p..p
    // собственно префикс.
    // Например, строка ТВП
    // 1  8  '/К=/',v200^a
    // приведет к индексированию каждого слова подполя А поля 200
    // с предварительным присоединением к каждому термину префикса "К=".
    //

    /// <summary>
    /// Метод индексирования.
    /// </summary>
    public enum FstIndexMethod
    {
        /// <summary>
        ///
        /// </summary>
        Method0 = 0,

        /// <summary>
        ///
        /// </summary>
        Method1 = 1,

        /// <summary>
        ///
        /// </summary>
        Method2 = 2,

        /// <summary>
        ///
        /// </summary>
        Method3 = 3,

        /// <summary>
        ///
        /// </summary>
        Method4 = 4,

        /// <summary>
        ///
        /// </summary>
        Method5 = 5,

        /// <summary>
        ///
        /// </summary>
        Method6 = 6,

        /// <summary>
        ///
        /// </summary>
        Method7 = 7,

        /// <summary>
        ///
        /// </summary>
        Method8 = 8

    } // enum FstIndexMethod

} // namespace ManagedIrbis.Fst
