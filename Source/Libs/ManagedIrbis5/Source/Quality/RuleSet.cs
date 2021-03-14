// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* RuleSet.cs -- набор правил
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

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
        public QualityRule[] Rules { get; set; }

        #endregion

        #region Private members

        private static readonly Dictionary<string,Type> _registeredRules
            = new ();

        #endregion

        #region Public methods

        /// <summary>
        /// Merge two reports.
        /// </summary>
        public static RecordReport MergeReport
            (
                RecordReport first,
                RecordReport second
            )
        {
            RecordReport result = new RecordReport
            {
                Defects = new DefectList (first.Defects.Concat(second.Defects)),
                Description = first.Description,
                Quality = first.Quality + second.Quality - 1000,
                Mfn = first.Mfn,
                Index = first.Index
            };

            return result;
        }


        /// <summary>
        /// Проверка одной записи
        /// </summary>
        public RecordReport CheckRecord
            (
                RuleContext context
            )
        {
            RecordReport result = new RecordReport
            {
                Description = context.Connection.FormatRecord
                    (
                        context.BriefFormat,
                        context.Record.Mfn
                    ),
                Index = context.Record.FM(903),
                Mfn = context.Record.Mfn
            };
            RuleUtility.RenumberFields
                (
                    context.Record
                );

            result.Quality = 1000;
            int bonus = 0;

            foreach (QualityRule rule in Rules)
            {
                RuleReport oneReport = rule.CheckRecord(context);
                result.Defects.AddRange(oneReport.Defects);
                result.Quality -= oneReport.Damage;
                bonus += oneReport.Bonus;
            }

            if (result.Quality >= 900)
            {
                result.Quality += bonus;
            }

            return result;
        }

        /// <summary>
        /// Получаем правило по его имени.
        /// </summary>
        public static QualityRule? GetRule
            (
                string name
            )
        {
            Type ruleType;
            if (!_registeredRules.TryGetValue
                (
                    name,
                    out ruleType)
                )
            {
                return null;
            }

            QualityRule result = (QualityRule)Activator.CreateInstance
                (
                    ruleType
                );

            return result;
        }

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

            return new RuleSet();
        }

        /// <summary>
        /// Регистрируем все правила из указанной сборки.
        /// </summary>
        public static void RegisterAssembly
            (
                Assembly assembly
            )
        {
            Type[] types = assembly
                .GetTypes()
                .Where(t => t.IsPublic)
                .Where(t => !t.IsAbstract)
                .Where(t => t.IsSubclassOf(typeof(QualityRule)))
                .ToArray();
            foreach (Type ruleType in types)
            {
                RegisterRule(ruleType);
            }
        }

        /// <summary>
        /// Регистрация встроенных правил.
        /// </summary>
        public static void RegisterBuiltinRules ()
        {
            RegisterAssembly(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// Register rule from type.
        /// </summary>
        public static void RegisterRule
            (
                Type ruleType
            )
        {
            string ruleName = ruleType.Name;

            _registeredRules.Add
                (
                    ruleName,
                    ruleType
                );
        }

        /// <summary>
        /// Отменяем регистрацию правила с указанным именем.
        /// </summary>
        /// <param name="name"></param>
        public static void UnregisterRule
            (
                string name
            )
        {
            if (_registeredRules.ContainsKey(name))
            {
                _registeredRules.Remove(name);
            }
        }

        #endregion

    } // class RuleSet

} // namespace ManagedIrbis.Quality
