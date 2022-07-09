using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;
using UnityEngine.Events;
//using MathNet.Numerics.Distributions;
using static System.Math;
using StrOpe = StringOperationUtil.OptimizedStringOperation;
using System.Linq;

namespace IdleLibrary
{
    public class UsefulMethod : MonoBehaviour
    {
        public static string ShowStatsBreakdown(Multiplier multiplier, string title, string key, bool isHyphen = true
            , bool isIndent = true, bool isShowZero = false, bool isAddPercent = false, bool isTitle = true)
        {
            var _title = isTitle ? title : "";
            var text = "";
            var value = multiplier.GetMultipliersFromKey(key);
            var indent = isIndent ? "\\\\" : "";
            var hyphen = isHyphen ? "- " : "";
            if (value.added != 0)
            {        
                text += isAddPercent ?
                $"{hyphen} {_title} \\qquad +{tDigit(value.added * 100, 3)} \\% {indent}" : 
                $"{hyphen} {_title} \\qquad +{tDigit(value.added,3)} {indent}";
            }
            if (value.multiplied > 1.0)
            {

                text += $"{hyphen} {_title} \\qquad +{tDigit((value.multiplied - 1) * 100,3)} \\% {indent}";
            }else if(value.multiplied < 1.0)
            {
                text += $"{hyphen} {_title} \\qquad -{tDigit((value.multiplied - 1) * 100, 3)} \\% {indent}";

            }
            if (isShowZero && value.added == 0 && value.multiplied == 1.0)
            {
                return $"{hyphen} {_title} \\qquad +0 {indent}";
            }
            return text;
        }
        private static int GetPrecision(double value)
        {
            return (value - (int)value).ToString()
                .TrimEnd('0')
                .Replace("0.", string.Empty)
                .Replace("-", string.Empty)
                .Length;
        }
        public static StrOpe optStr { get => StrOpe.i; }
        /// <summary>
        /// 桁数が大きいものを，アルファベット表記に変える関数
        /// </summary>
        static string[] digit = new string[]
    {
       "", "K","M","B","T","Qa","Qi","Sx","Sp","Oc","No","Dc",
        "Ud","Dd","Td","Qad","Qid","Sxd","Spd","Ods","Nod","Vg","Uvg","Dvg","Tvg","Qavg","Qivg","Sxvg","Spvg","Ocvg","Novg",
       "Tg","Utg","Dtg","Ttg","Qatg","Qitg","Sxts","Sptg","Octg","Notg","Qag", "Uqag", "Dqag", "Tqag", "Qaqag", "Qiqag", "Sxqag", "Spqag", "Ocqag", "Noqag",
        "Qig", "UQig, DQig", "TQig", "QaQig", "QiQig", "SxQig", "SpQig", "OcQig", "NoQig", "Sxg", "USxg", "DSxg", "TSxg", "QaSxg", "QiSxg", "SxSxg", "SpSxg", "OcSxg", "NoSxg",
       "Spg", "USpg", "DSpg", "TSpg", "QaSpg", "QiSpg", "SxSpg", "SpSpg", "OcSpg", "NoSpg", "Ocg", "UOcg", "DOcg", "TOcg", "QaOcg", "QiOcg", "SxOcg", "SpOcg", "OcOcg", "NoOcg",
        "Nog", "UNog", "DNog", "TNog", "QaNog", "QiNog", "SxNog", "SpNog", "OcNog", "NoNog", "C", "Uc"
    };
        static StringBuilder digit_builder = new StringBuilder(100);
        static int log_1000_tdigit;
        static double head_value_tdigit;
        static string argument_ToString_tdigit = "";
        static bool isMinus_tdigit;


        public static bool IsScentific;
        public static int EffectiveDigit;
        public static bool isEffect;
        public static string tDigit(double value, int decimal_point = 0, bool removeZero = true, string argument_toString = null, bool toShowSign = false, bool toRoundDown = false)
        {
            var point = decimal_point;
            var isZero = removeZero;
            if (isEffect)
            {
                point = EffectiveDigit;
                isZero = false;
            }
            if (IsScentific)
            {
                if(value <= 1000)
                {
                    switch (point)
                    {
                        case 1: return value.ToString("F1"); 
                        case 2: return value.ToString("F2");
                        case 3: return value.ToString("F3");
                        case 4: return value.ToString("F4");
                        case 5: return value.ToString("F5");
                        case 6: return value.ToString("F6");
                        default: return value.ToString("F0"); 
                    }
                }
                return value.ToString($"0.00e00");
            }
            //ToStringとして使う場合そのまま返す
            if (argument_toString != null) { return value.ToString(argument_toString); }
            if (double.IsInfinity(value))
            {
                return value.ToString();
            }
            //マイナスの処理
            isMinus_tdigit = (value < 0);
            if (isMinus_tdigit) { value = -value; }
            //共通の処理
            log_1000_tdigit = value >= 1000 ? (int)Math.Log(value, 1000) : 0;
            head_value_tdigit = value / Math.Pow(1000, log_1000_tdigit);
            digit_builder.Clear();

            argument_ToString_tdigit = "";
            if (value >= 1000)
            {
                if (head_value_tdigit >= 100) { argument_ToString_tdigit = "F0"; }
                else if (head_value_tdigit >= 10) { argument_ToString_tdigit = "F1"; }
                else if (head_value_tdigit >= 1) { argument_ToString_tdigit = "F2"; }
            }
            else
            {
                //decimal_point = Math.Min(3,GetPrecision(head_value_tdigit));
                if (toRoundDown) head_value_tdigit = RoundDown(head_value_tdigit, decimal_point); //切り捨ての処理
                switch (point)
                {
                    case 1: argument_ToString_tdigit = "F1"; break;
                    case 2: argument_ToString_tdigit = "F2"; break;
                    case 3: argument_ToString_tdigit = "F3"; break;
                    case 4: argument_ToString_tdigit = "F4"; break;
                    case 5: argument_ToString_tdigit = "F5"; break;
                    case 6: argument_ToString_tdigit = "F6"; break;
                    default: argument_ToString_tdigit = "F0"; break;
                }
            }
            if (isZero) digit_builder.Append(RemoveZero(head_value_tdigit.ToString(argument_ToString_tdigit)));
            else digit_builder.Append(head_value_tdigit.ToString(argument_ToString_tdigit));
            digit_builder.Append(digit[log_1000_tdigit]);
            //マイナスの処理
            if (isMinus_tdigit) digit_builder.Insert(0, "-");
            else if (toShowSign) digit_builder.Insert(0, "+");
            return digit_builder.ToString();
        }
        public static string percent(double d, int num = 2)
        {
            if (d == 0)
                return 0 + "%";
            return tDigit(d * 100, num) + "%";
        }

        //不要な0を取り消す関数
        static System.Text.RegularExpressions.Regex pattern1 = new System.Text.RegularExpressions.Regex(@"(\.\d+?)0+\b");
        static System.Text.RegularExpressions.Regex pattern2 = new System.Text.RegularExpressions.Regex(@"\.0*$");
        public static string RemoveZero(string number)
        {
            //末尾の0を消す関数
            string strcut = pattern1.Replace(number, "$1");                                                //0を消す
            strcut = pattern2.Replace(strcut, "");                                                         //.0,,,で終わっていたらそれを消す
                                                                                                           //string strcut = System.Text.RegularExpressions.Regex.Replace(number, @"(\.\d+?)0+\b", "$1"); //0を消す
                                                                                                           //strcut = System.Text.RegularExpressions.Regex.Replace(strcut, @"\.0*$", "");                 //.0,,,で終わっていたらそれを消す
            return strcut;
        }

        static int factor_roundDown;
        public static double RoundDown(double number, int digit)
        {
            switch (digit)
            {
                case 0: factor_roundDown = 1; break;
                case 1: factor_roundDown = 10; break;
                case 2: factor_roundDown = 100; break;
                case 3: factor_roundDown = 1000; break;
                case 4: factor_roundDown = 10000; break;
                case 5: factor_roundDown = 100000; break;
                default: factor_roundDown = (int)Math.Pow(10, digit); break;
            }
            return FastFloor(number * factor_roundDown) / factor_roundDown;
        }
        public static double FastFloor(double num)
        {
            if ((num >= long.MinValue) && (num <= long.MaxValue))
            {
                return (double)(long)num;
            }
            else
            {
                return Math.Floor(num);
            }
        }


       　//windowを出すコルーチン
        public static IEnumerator bigAndBig(GameObject targetUI)
        {
            targetUI.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
            targetUI.SetActive(true);
            for (float i = 0.5f; i <= 1.0f; i += 0.1f)
            {
                targetUI.GetComponent<RectTransform>().localScale = new Vector3(i, i, i);
                yield return new WaitForSeconds(0.01f);
            }
            targetUI.GetComponent<RectTransform>().localScale = Vector3.one;
        }

        //windowを消すコルーチン
        public static IEnumerator smallAndSmall(GameObject targetUI)
        {
            targetUI.GetComponent<RectTransform>().localScale = Vector3.one;
            for (float i = 1.0f; i >= 0.5f; i -= 0.1f)
            {
                targetUI.GetComponent<RectTransform>().localScale = new Vector3(i, i, i);
                yield return new WaitForSeconds(0.01f);
            }
            targetUI.SetActive(false);
            targetUI.GetComponent<RectTransform>().localScale = Vector3.one;
        }

        //windowを非アクティブにせず、消すコルーチン
        public static IEnumerator smallAndSmallActive(GameObject targetUI)
        {
            targetUI.GetComponent<RectTransform>().localScale = Vector3.one;
            for (float i = 1.0f; i >= 0.5f; i -= 0.1f)
            {
                targetUI.GetComponent<RectTransform>().localScale = new Vector3(i, i, i);
                yield return new WaitForSeconds(0.01f);
            }
            //targetUI.SetActive(false);
            targetUI.GetComponent<RectTransform>().localScale = Vector3.zero;
        }


        //the difference of Datetime to float
        public static float DeltaTimeFloat(DateTime DT)
        {
            if (DT == null)
            {
                return 0;
            }

            float ans = ((float)DateTime.Now.Subtract(DT).Seconds
                + DateTime.Now.Subtract(DT).Minutes * 60
                + DateTime.Now.Subtract(DT).Hours * 3600
                + DateTime.Now.Subtract(DT).Days * 86400);

            if (ans >= 0)
            {
                return ans;
            }
            else
            {
                return 0;
            }
        }

        public static string DoubleTimeToDate(double Time, bool CoronMode = false)
        {
            int Day = 0;
            int Hour = 0;
            int Minute = 0;
            int Second = 0;
            string DayString = "";
            string HourString = "";
            string MinuteString = "";
            string SecondString = "";

            Day = (int)Math.Floor(Time / 86400);
            Hour = (int)Math.Floor((Time % 86400) / 3600);
            Minute = (int)Math.Floor((Time % 3600) / 60);
            Second = (int)Math.Floor((Time % 60));

            if (CoronMode)
            {
                Hour = (int)Math.Floor(Time / 3600);
                Minute = (int)Math.Floor((Time % 3600) / 60);
                Second = (int)Math.Floor((Time % 60));
                if (Hour > 0) { HourString = Hour.ToString("D2") + ":"; }
                MinuteString = Minute.ToString("D2") + ":";
                SecondString = Second.ToString("D2");
            }
            else
            {
                Day = (int)Math.Floor(Time / 86400);
                Hour = (int)Math.Floor((Time % 86400) / 3600);
                Minute = (int)Math.Floor((Time % 3600) / 60);
                Second = (int)Math.Floor((Time % 60));
                if (Day > 0) { DayString = Day.ToString() + "d"; }
                if (Hour > 0) { HourString = Hour.ToString() + "h"; }
                if (Minute > 0) { MinuteString = Minute.ToString() + "m"; }
                if (Second > 0) { SecondString = Second.ToString() + "s"; }
            }

            return DayString + HourString + MinuteString + SecondString;
        }

        public static float Pdf_Ed(double Ramda, double X)
        {
            return 1.0f - (float)Math.Pow(Math.E, (-1 * Ramda * X));
        }

        public static double ZC(double number)//ZC stands for "Zero Check";
        {
            return Math.Max(0.000000001, number);
        }

        public static double Domain(double Value, double Max, double Min)
        {
            double tempDouble = 0;
            tempDouble = Value <= Min ? Min : Value;
            tempDouble = Value >= Max ? Max : Value;
            return tempDouble;
        }

        public static List<Component> FindObjectsOfInterface<T>() where T : class
        {
            var list = new List<Component>();
            foreach (var n in FindObjectsOfType<Component>())
            {
                var Interface = n as T;
                if (Interface != null)
                {
                    list.Add(n);
                }
            }
            return list;
        }

        /// <summary>
        /// 配列のサイズが第二引数のものと違ったら合わせる関数
        /// </summary>
        public static void InitializeArrayWithNew<Type>(ref Type[] Obj, int Length) where Type : new()
        {
            if (Obj == null || Obj[0] == null) {
                Obj = new Type[Length];
                Obj = Enumerable.Range(0, Length).Select(_ => new Type()).ToArray();
            }
            if (Obj.Length != Length)
            {
                Array.Resize(ref Obj, Length);
            }
        }

        public static void InitializeArray<Type>(ref Type[] Obj, int Length) 
        {
            if (Obj == null || Obj.Length == 0 || Obj[0] == null)
            {
                Obj = new Type[Length];
            }
            if (Obj.Length != Length)
            {
                Array.Resize(ref Obj, Length);
            }
        }
    }
}