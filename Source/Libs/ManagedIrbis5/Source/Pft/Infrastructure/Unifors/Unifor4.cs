// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* Unifor4.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.Text;
using ManagedIrbis.Infrastructure;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // &UNIFOR('4N,Format')  - ФОРМАТИРОВАНИЕ ПРЕДЫДУЩЕЙ КОПИИ ТЕКУЩЕЙ ЗАПИСИ:
    //
    // где:
    //
    // N - номер копии (в обратном порядке, т.е. если N=1 - это один шаг назад,
    // N=2 - два шага назад и т.д.). Может принимать значение * - это указывает
    // на последнюю копию.
    // Если N - пустое значение, то в случае повторяющейся группы в качестве
    // значения N берется НОМЕР ТЕКУЩЕГО ПОВТОРЕНИЯ, в противном случае
    // берется первая копия;
    // Format - формат; может задаваться непосредственно или в виде @имя_формата.
    //
    // Если не задается ни N ни Format, т.е. &unifor('4'), то возвращается
    // количество предыдущих копий.
    // Если запись не имеет предыдущих копий, то &unifor('4') возвращает 0,
    // а все остальные конструкции &unifor('4...') возвращают пустоту.
    //
    // Примеры:
    //
    // &unifor('41,@brief')
    // (...&unifor('4,v200^a')...)
    // &unifor('4*,(v910/)')
    //

    static class Unifor4
    {
        #region Public methods

        /// <summary>
        /// Format previous version of current record.
        /// </summary>
        public static void FormatPreviousVersion
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            var record = context.Record;
            if (!ReferenceEquals(record, null))
            {
                if (string.IsNullOrEmpty(expression))
                {
                    // ibatrak
                    // пустое выражение означает
                    // "количество предыдущих версий"

                    var output = (record.Version - 1).ToInvariantString();
                    context.WriteAndSetFlag(node, output);

                    return;
                }


                var mfn = record.Mfn;
                if (mfn != 0 && record.Version > 1)
                {
                    var version = context.Index;

                    var navigator = new TextNavigator(expression);
                    if (navigator.PeekChar() == '*')
                    {
                        version = 0;
                        navigator.ReadChar();
                    }
                    if (navigator.PeekChar() != ',')
                    {
                        var versionText = navigator.ReadInteger().ToString();
                        if (string.IsNullOrEmpty(versionText))
                        {
                            return;
                        }

                        version = -versionText.SafeToInt32();
                        navigator.ReadChar(); // eat the comma
                    }
                    else
                    {
                        navigator.ReadChar(); // eat the comma
                    }

                    var format = navigator.GetRemainingText().ToString();
                    if (string.IsNullOrEmpty(format))
                    {
                        return;
                    }

                    // ibatrak
                    // после вызова этого unifor
                    // в главном контексте сбрасываются флаги пост обработки
                    context.GetRootContext().PostProcessing = PftCleanup.None;

                    var parameters = new ReadRecordParameters
                    {
                        Database = context.Provider.Database,
                        Mfn = mfn,
                        Version = version
                    };
                    record = context.Provider.ReadRecord(parameters);

                    using (var guard = new PftContextGuard(context))
                    {
                        var nestedContext = guard.ChildContext;
                        nestedContext.Record = record;

                        // ibatrak
                        // формат вызывается в контексте без повторений
                        nestedContext.Reset();

                        // TODO some caching

                        var program = PftUtility.CompileProgram(format);
                        program.Execute(nestedContext);

                        var output = nestedContext.Text;
                        context.Write(node, output);
                    }
                }
            }
        }

        #endregion
    }
}
