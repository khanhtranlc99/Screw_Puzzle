using System;
using System.Collections.Generic;
using System.IO;
using FileHelpers;
using FileHelpers.Events;
using UnityEngine;

namespace Joker2X.Localize.Scripts.Editor
{
    public class LocalizeParse
    {
        public void Parse(string data)
        {
            var engine = new FileHelperEngine<LanguageCSVFormat>();
            //engine.BeforeReadRecord += BeforeEvent;
            engine.AfterReadRecord += AfterRead;
            engine.HeaderText = engine.GetFileHeader();
            var languageList = engine.ReadStringAsList(data);
            
            for (int i = engine.Options.FieldCount - 1; i >= 0; i--)
            {
                var fieldsName = engine.Options.FieldsNames[i];
                if (string.CompareOrdinal("KEY_lang", fieldsName) != 0
                    //&& string.CompareOrdinal("English", fieldsName) != 0
                    )
                {
                    ExportFile(fieldsName, languageList);
                }
            }
        }

        private void ExportFile(string language, List<LanguageCSVFormat> languageList)
        {
            var engine = new FileHelperEngine<LanguageCSVFormat>();
            engine.AfterWriteRecord += AfterWrite;
            //engine.HeaderText = engine.GetFileHeader();
            for (int i = engine.Options.FieldCount - 1; i >= 0; i--)
            {
                var fieldsName = engine.Options.FieldsNames[i]; 
                if (string.CompareOrdinal("KEY_lang", fieldsName) != 0
                    //&& string.CompareOrdinal("English", fieldsName) != 0 
                    && string.CompareOrdinal(language, fieldsName) != 0)
                {
                    engine.Options.RemoveField(fieldsName);
                }
            }

            var outputFile = Path.Combine(Application.dataPath,
                string.Compare(language, "English", StringComparison.Ordinal) == 0
                    ? $"Resources/Localization.txt"
                    : $"Resources/Localization-{language}.txt");
            engine.HeaderText = $"KEY_lang,{language}";
            engine.WriteFile(outputFile, languageList);
        }

        private void AfterWrite(EngineBase engine, AfterWriteEventArgs<LanguageCSVFormat> args)
        {
            args.RecordLine = args.RecordLine.TrimEnd(',');
        }

        private void AfterRead(EngineBase engine, AfterReadEventArgs<LanguageCSVFormat> args)
        {
            //args.RecordLine = args.RecordLine.Replace("***---***", "\"");
            var arabic = args.Record.Arabic;
            var arabicFix =
                ArabicSupport.ArabicFixer.Fix(arabic, true, false).Replace('>', '(')
                             .Replace('<', ')')
                             .Replace('(', '<').Replace(')', '>');
            
            arabicFix = arabicFix.Replace("\n", @"\n")
                                 .Replace("\r", string.Empty);
            args.Record.Arabic = arabicFix;
            args.Record.FillEmptyField();
        }

    }

    [DelimitedRecord(",")]
    [IgnoreFirst]
    public class LanguageCSVFormat
    {
        [FieldQuoted('"', QuoteMode.OptionalForBoth)][FieldOrder(1)][FieldConverter(typeof(LanguageConverter))]
        public string KEY_lang;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)][FieldOrder(2)][FieldConverter(typeof(LanguageConverter))]
        public string English;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)][FieldOrder(3)][FieldConverter(typeof(LanguageConverter))]
        public string Vietnamese;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)][FieldOrder(4)][FieldConverter(typeof(LanguageConverter))]
        public string Russian;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)][FieldOrder(5)][FieldConverter(typeof(LanguageConverter))]
        public string Spanish;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)][FieldOrder(6)][FieldConverter(typeof(LanguageConverter))]
        public string French;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)][FieldOrder(7)][FieldConverter(typeof(LanguageConverter))]
        public string Japanese;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)][FieldOrder(8)][FieldConverter(typeof(LanguageConverter))]
        public string Korean;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)][FieldOrder(9)][FieldConverter(typeof(LanguageConverter))]
        public string Thai;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)][FieldOrder(10)]
        public string Arabic;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)][FieldOrder(11)][FieldConverter(typeof(LanguageConverter))]
        public string Chinese;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)][FieldOrder(12)][FieldConverter(typeof(LanguageConverter))]
        public string Portuguese;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)][FieldOrder(13)][FieldConverter(typeof(LanguageConverter))]
        public string German;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)][FieldOrder(14)][FieldConverter(typeof(LanguageConverter))]
        public string Italian;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)][FieldOrder(15)][FieldConverter(typeof(LanguageConverter))]
        public string Indonesian;
        [FieldQuoted('"', QuoteMode.OptionalForBoth)][FieldOrder(16)][FieldConverter(typeof(LanguageConverter))]
        public string Turkish;

        public void FillEmptyField()
        {
            if (string.IsNullOrEmpty(Vietnamese)) Vietnamese = English;
            if (string.IsNullOrEmpty(Russian)) Russian = English;
            if (string.IsNullOrEmpty(Spanish)) Spanish = English;
            if (string.IsNullOrEmpty(French)) French = English;
            if (string.IsNullOrEmpty(Japanese)) Japanese = English;
            if (string.IsNullOrEmpty(Korean)) Korean = English;
            if (string.IsNullOrEmpty(Thai)) Thai = English;
            if (string.IsNullOrEmpty(Arabic)) Arabic = English;
            if (string.IsNullOrEmpty(Chinese)) Chinese = English;
            if (string.IsNullOrEmpty(Portuguese)) Portuguese = English;
            if (string.IsNullOrEmpty(German)) German = English;
            if (string.IsNullOrEmpty(Italian)) Italian = English;
            if (string.IsNullOrEmpty(Indonesian)) Indonesian = English;
            if (string.IsNullOrEmpty(Turkish)) Turkish = English;
        }
    }
    
    

    public class LanguageConverter : ConverterBase
    {
        public override object StringToField(string from)
        {
            return from.Replace("\n", @"\n").Replace("\r", string.Empty);
        }
    }


}