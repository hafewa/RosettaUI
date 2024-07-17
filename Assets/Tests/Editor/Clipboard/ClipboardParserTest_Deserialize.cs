﻿using System;
using System.Globalization;
using NUnit.Framework;
using UnityEngine;

namespace RosettaUI.Test
{
    // ReSharper disable once InconsistentNaming
    public class ClipboardParserTest_Deserialize
    {
        [TestCaseSource(nameof(BoolSource))]
        public void MatchUnityEditorMethod_Bool(string text) => TestMatch(text, EditorClipBoardParser.ParseBool);

        [TestCaseSource(nameof(IntSource))]
        public void MatchUnityEditorMethod_Int(string text) => TestMatch(text, EditorClipBoardParser.ParseInteger);

        [TestCaseSource(nameof(UIntSource))]
        public void MatchUnityEditorMethod_UInt(string text) => TestMatch(text, EditorClipBoardParser.ParseUint);

        [TestCaseSource(nameof(FloatSource))]
        public void MatchUnityEditorMethod_Float(string text) => TestMatch(text, EditorClipBoardParser.ParseFloat);

        [TestCaseSource(nameof(EnumSource))]
        public void MatchUnityEditorMethod_Enum(string text) => TestMatch(text, EditorClipBoardParser.ParseEnum<EnumForTest>);

        [TestCaseSource(nameof(Vector2Source))]
        public void MatchUnityEditorMethod_Vector2(string text) => TestMatch(text, EditorClipBoardParser.ParseVector2);

        [TestCaseSource(nameof(GradientSource))]
        public void MatchUnityEditorMethod_Gradient(string text) =>
            TestMatch(text, EditorClipBoardParser.ParseGradient);


        private static string[] BoolSource => new[]
        {
            "True", "False", "true", "false", "", null, "expect parse fail"
        };

        private static string[] IntSource => new[]
        {
            "0", "1", "10", "-1", "-10", int.MinValue.ToString(), int.MaxValue.ToString(), null, "expect parse fail"
        };

        private static string[] UIntSource => new[]
        {
            "0", "1", "10", "-1", "-10", uint.MinValue.ToString(), uint.MaxValue.ToString(), null, "expect parse fail"
        };

        private static string[] FloatSource => new[]
        {
            "0", "0.1", "1.0", "10", "-0.1", "-1", "-10",
            float.MinValue.ToString(CultureInfo.InvariantCulture),
            float.MaxValue.ToString(CultureInfo.InvariantCulture),
            null, "expect parse fail"
        };

        private static string[] EnumSource => new[]
        {
            "", "_",
            "one", "One",
            "_two", "Two",
            "three_", "Three",
            "fourthItem", "FourthItem",
            "FifthItem", "fifthItem",
            "Sixth_Item", "SixthItem",
            "SEVEN", "Seven",
            null,
            "expect parse fail"
        };

        private static string[] Vector2Source => new[]
        {
            "Vector2(0,0)", "Vector2(1,1)", "Vector2(0,1)", "Vector2(1,0)",
            "Vector2(0.1,0.1)", "Vector2(-0.1,-0.1)", 
            "Vector2(NaN,Nan)", "Vector2(Infinity,Infinity)", "Vector2(-Infinity, -Infinity)",
            "Vector2(0,)", "Vector2(,0)", "Vector2(,)", "Vector2()", "Vector2(,)", "Vector2(,0)", "Vector2(0,)",
            "Vector2(0,0,0)", "Vector2(0,0,0,0)", "Vector2(0,0,0,0,0)", "Vector2(0,0,0,0,0,0)",
            "Vector2 (0,0)", "Vector2( 0,0)", "Vector2(0 ,0)", "Vector2(0, 0)", "Vector2(0,0 )", "Vector2(0,0) ",
            "Vector3(0,0)", "Vector4(0,0)",
            null, "expect parse fail"
        };
        
        private static string[] GradientSource => new[]
            { EditorClipBoardParser.WriteGradient(new Gradient() { mode = GradientMode.Fixed }) };



        public void TestMatch<T>(string text, Func<string, (bool, T)> expectedFunc)
        {
            var (expectedSuccess, expectedValue) = expectedFunc(text);
            var (testSuccess, testValue) = ClipboardParser.Deserialize<T>(text);
            Assert.AreEqual(expectedSuccess, testSuccess);

            if (!expectedSuccess) return;
            if (typeof(T).IsValueType)
            {
                Assert.AreEqual(expectedValue, testValue);
            }
            else
            {
                Assert.AreEqual(JsonUtility.ToJson(expectedValue), JsonUtility.ToJson(testValue));
            }
        }
    }
}