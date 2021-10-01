// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* RuleSet.cs -- набор правил
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

using AM;

using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.Quality
{
    /// <summary>
    /// Набор правил.
    /// </summary>
    public sealed class RuleSet
    {
        #region Properties

        /// <summary>
        /// Правила, входящие в набор.
        /// </summary>
        [JsonPropertyName("rules")]
        public QualityRule[]? Rules { get; set; }

        #endregion

        #region Private members

        private static readonly Dictionary<string,Type> _registeredRules = new ();

        #endregion

        #region Public methods

        /// <summary>
        /// Проверка одной записи.
        /// </summary>
        public RecordReport CheckRecord
            (
                RuleContext context
            )
        {
            var record = context.Record.ThrowIfNull("context.Record");
            var result = new RecordReport
            {
                Description = context.Connection.ThrowIfNull("context.Connection").FormatRecord
                    (
                        context.BriefFormat.ThrowIfNull("context.BriefFormat"),
                        record.Mfn
                    ),
                Index = record.FM(903),
                Mfn = record.Mfn
            };
            RuleUtility.RenumberFields (record);

            result.Quality = 1000;
            var bonus = 0;

            if (Rules is not null)
            {
                foreach (var rule in Rules)
                {
                    var oneReport = rule.CheckRecord(context);
                    result.Defects.AddRange(oneReport.Defects);
                    result.Quality -= oneReport.Damage;
                    bonus += oneReport.Bonus;
                }
            }

            if (result.Quality >= 900)
            {
                result.Quality += bonus;
            }

            return result;

        } // method CheckRecord

        /// <summary>
        /// Получение правила по его имени.
        /// </summary>
        public static QualityRule? GetRule
            (
                string name
            )
        {
            if (!_registeredRules.TryGetValue
                (
                    name,
                    out var ruleType
                ))
            {
                return null;
            }

            ruleType = ruleType.ThrowIfNull("ruleType");

            var result = (QualityRule?) Activator.CreateInstance
                (
                    ruleType
                );

            return result;

        } // method GetRule

        /// <summary>
        /// Load set of rules from the specified file.
        /// </summary>
        public static RuleSet LoadJson
            (
                string fileName
            )
        {
            /*

            string text = File.ReadAllText(fileName);
            JObject obj = JObject.Parse(text);

            RuleSet result = new RuleSet();
            List<QualityRule> rules = new List<QualityRule>();

            foreach (JToken o in obj["rules"])
            {
                string name = o.ToString();
                QualityRule rule = GetRule(name);
                if (rule != null)
                {
                    rules.Add(rule);
                }
            }

            result.Rules = rules.ToArray();

            return result;

            */

            throw new NotImplementedException();

        } // method LoadJson

        /// <summary>
        /// Регистрация все правил из указанной сборки.
        /// </summary>
        public static void RegisterAssembly
            (
                Assembly assembly
            )
        {
            var types = assembly
                .GetTypes()
                .Where(t => t.IsPublic)
                .Where(t => !t.IsAbstract)
                .Where(t => t.IsSubclassOf(typeof(QualityRule)))
                .ToArray();
            foreach (var ruleType in types)
            {
                RegisterRule(ruleType);
            }

        } // method RegisterAssembly

        /// <summary>
        /// Регистрация встроенных правил.
        /// </summary>
        public static void RegisterBuiltinRules () =>
            RegisterAssembly(Assembly.GetExecutingAssembly());

        /// <summary>
        /// Регистрация правила по его типу.
        /// </summary>
        public static void RegisterRule (Type ruleType) => _registeredRules.Add
            (
                ruleType.Name,
                ruleType
            );

        /// <summary>
        /// Отменяет регистрацию правила с указанным именем.
        /// </summary>
        public static void UnregisterRule
            (
                string name
            )
        {
            if (_registeredRules.ContainsKey(name))
            {
                _registeredRules.Remove(name);
            }

        } // method UnregisterRule

        #endregion

    } // class RuleSet

} // namespace ManagedIrbis.Quality
