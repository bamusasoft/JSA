using System;
using System.Text;
using Microsoft.VisualBasic;

namespace Jsa.ViewsModel.Helpers
{
    public static class SayNumber
    {
        private static string zero = "صفر";
        private static string one = "واحد";
        private static string two = "إثنان";
        private static string Three = "ثلاثة";
        private static string four = "أربعة";
        private static string five = "خمسة";
        private static string six = "ستة";
        private static string seven = "سبعة";
        private static string eight = "ثمانية";
        private static string nine = "تسعة";
        private static string ten = "عشر";
        private static string twenty = "عشرون";
        private static string thirty = "ثلاثون";
        private static string fourty = "أربعون";
        private static string fifty = "خمسون";
        private static string sixty = "ستون";
        private static string seventy = "سبعون";
        private static string eighty = "ثمانون";
        private static string ninty = "تسعون";
        private static string hundred = "مائة";
        private static string twoHundreds = "مئتان";
        private static string Thousend = "الف";
        private static string twoThousends = "الفان";
        private static string thousends = "آلاف";
        private static string milion = "مليون";
        private static string twoMilions = "مليونان";
        private static string milions = "ملايين";
        private static string billion = "مليار";
        private static string twoBillions = "ملياران";
        private static string fagat = " فقط ";
        private static string lagair = " لاغير ";
        private static string r = " ريـال ";
        private static string h = " هللة ";


        /// <summary>
        /// Convert the specified number to words
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToWords(decimal number)
        {
            string result = "";
            //Extract fraction in case of there's any
            string fraction = (number - Decimal.Truncate(number)).ToString();

            int decim = int.Parse(fraction);

            long digit = (int) number;

            /*
             * We can't use double as type for number argument due to the lost of precision in fraction
            //إستخرج الرقم الصحيح أولاً
            //long digit = (long)number;
            //int decim = 0;
            ////إستخرج الكسر في العدد بطرح العدد الصحيح من كامل الرقم
            //double temp = number - digit;
            //if (temp > 0)
            //{
            //    //Convert the fraction to string, so we can substract the two digit in the fraction that we interest in.
            //    string t = temp.ToString();
            //    //Substract the two digit of fraction we interest in 
            //    string q = t.Substring(2, 2);
            //    //Parse the fraction to int.
            //   decim  = int.Parse(q);
            //}
             */
            string rial = NumberInWords(digit);
            string halala = NumberInWords(decim);
            if (rial != "" && decim >= 0)
            {
                result = fagat + rial + r + " و " + halala + h + lagair;
            }
            if (rial == "" && decim != 0)
            {
                result = fagat + halala + h + lagair;
            }
            if (rial == "" && decim == 0)
            {
                result = zero;
            }
            if (rial != "" && decim == 0)
            {
                result = fagat + rial + r + lagair;
            }
            return result;
        }

        private static string NumberInWords(long number)
        {
            var c = Strings.Format(number, "000000000000");

            var numberInWords = new StringBuilder();
            string letter1 = "";
            int c1 = int.Parse(Strings.Mid(c, 12, 1));
            switch (c1)
            {
                case 1:
                    letter1 = one;
                    break;
                case 2:
                    letter1 = two;
                    break;
                case 3:
                    letter1 = Three;
                    break;
                case 4:
                    letter1 = four;
                    break;
                case 5:
                    letter1 = five;
                    break;
                case 6:
                    letter1 = six;
                    break;
                case 7:
                    letter1 = seven;
                    break;
                case 8:
                    letter1 = eight;
                    break;
                case 9:
                    letter1 = nine;
                    break;
            }

            string letter2 = "";
            int c2 = int.Parse(Strings.Mid(c, 11, 1));
            switch (c2)
            {
                case 1:
                    letter2 = ten;
                    break;
                case 2:
                    letter2 = twenty;
                    break;
                case 3:
                    letter2 = thirty;
                    break;
                case 4:
                    letter2 = fourty;
                    break;
                case 5:
                    letter2 = fifty;
                    break;
                case 6:
                    letter2 = sixty;
                    break;
                case 7:
                    letter2 = seventy;
                    break;
                case 8:
                    letter2 = eighty;
                    break;
                case 9:
                    letter2 = ninty;
                    break;
            }
            if (letter1 != "" && c2 > 1)
            {
                letter2 = letter1 + " و " + letter2;
            }
            if (letter2 == "")
            {
                letter2 = letter1;
            }
            if (c1 == 0 && c2 == 1)
            {
                letter2 = letter2 + "ة";
            }
            if (c1 == 1 && c2 == 1)
            {
                letter2 = "إحدى عشر";
            }
            if (c1 == 2 && c2 == 1)
            {
                letter2 = "إثني عشر";
            }
            if (c1 > 2 && c2 == 1)
            {
                letter2 = letter1 + " " + letter2;
            }

            int c3 = int.Parse(Strings.Mid(c, 10, 1));
            string letter3 = "";
            if (c3 == 1)
            {
                letter3 = hundred;
            }
            if (c3 == 2)
            {
                letter3 = twoHundreds;
            }
            if (c3 > 2)
            {
                letter3 = Strings.Left(NumberInWords(c3), Strings.Len(NumberInWords(c3)) - 1) + hundred;
            }
            if (letter3 != "" && letter2 != "")
            {
                letter3 = letter3 + " و " + letter2;
            }
            if (letter3 == "")
            {
                letter3 = letter2;
            }

            int c4 = int.Parse(Strings.Mid(c, 7, 3));

            string letter4 = "";
            if (c4 == 1)
            {
                letter4 = Thousend;
            }
            else if (c4 == 2)
            {
                letter4 = twoThousends;
            }
            else if (c4 >= 3 && c4 <= 10)
            {
                letter4 = NumberInWords(c4) + " " + thousends;
            }
            else if (c4 > 10)
            {
                letter4 = NumberInWords(c4) + " " + Thousend;
            }

            if (letter4 != "" && letter3 != "")
            {
                letter4 = letter4 + " و " + letter3;
            }
            if (letter4 == "")
            {
                letter4 = letter3;
            }

            int c5 = int.Parse(Strings.Mid(c, 4, 3));

            string letter5 = "";
            if (c5 == 1)
            {
                letter5 = milion;
            }
            else if (c5 == 2)
            {
                letter5 = twoMilions;
            }
            else if (c5 >= 3 && c5 <= 10)
            {
                letter5 = NumberInWords(c5) + " " + milions;
            }
            else if (c5 > 10)
            {
                letter5 = NumberInWords(c5) + " " + milion;
            }
            if (letter5 != "" && letter4 != "")
            {
                letter5 = letter5 + " و " + letter4;
            }
            if (letter5 == "")
            {
                letter5 = letter4;
            }

            string letter6 = "";
            int c6 = int.Parse(Strings.Mid(c, 1, 3));

            if (c6 == 1)
            {
                letter6 = billion;
            }
            else if (c6 == 2)
            {
                letter6 = twoBillions;
            }
            else if (c6 > 2)
            {
                letter6 = NumberInWords(c6) + " " + billion;
            }
            if (letter6 != "" && letter5 != "")
            {
                letter6 = letter6 + " و " + letter5;
            }
            if (letter6 == "")
            {
                letter6 = letter5;
            }
            return letter6;
        }
    }
}