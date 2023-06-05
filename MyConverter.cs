using System;
using static System.Net.Mime.MediaTypeNames;


namespace Contracts
{
    public class MyConverter
    {
        public static string ConvertNumberToWords(long price)
        {
            if (price == 0L)
            {
                return "ноль";
            }

            if (price < 0)
            {
                return "минус " + ConvertNumberToWords(Math.Abs(price));
            }

            string text = "";
            if (price / 1000000000 > 0)
                if (price >= 2000000000)
                {
                    text += ConvertNumberToWords(price / 1000000000) + " миллиарда ";
                    price %= 1000000000;
                }   
                else
                {
                text += ConvertNumberToWords(price / 1000000000) + " миллиард ";
                price %= 1000000000;
                }
            
            if (price / 1000000 > 0)
                if(price >= 2000000)
                {
                    text += ConvertNumberToWords(price / 1000000) + " миллиона ";
                    price %= 1000000;
                }
                else
                {
                text += ConvertNumberToWords(price / 1000000) + " миллион ";
                price %= 1000000;
                }

            if (price / 1000 > 0)
                if (price >= 5000)
                {
                    text += ConvertNumberToWords(price / 1000) + " тысяч ";
                    price %= 1000;
                }
                else if (price>=2000)
                {
                    text += ConvertNumberToWords(price / 1000) + " тысячи ";
                    price %= 1000;
                }
                else
                {
                text += ConvertNumberToWords(price / 1000) + " тысяча ";
                price %= 1000;
                }


            if (price > 0)
            {
                string[] array = new string[20] { "ноль", "один", "два", "три", "четыре", "пять", "шесть", "семь", "восемь", "девять", "десять", "одиннадцать", "двенадцать", "тринадцать", "четырнадцать", "пятнадцать", "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать" };
                string[] array2 = new string[10] { "ноль", "десять", "двадцать", "тридцать", "сорок", "пятьдесят", "шестьдесят", "семьдесят", "восемьдесят", "девяносто" };
                string[] array3 = new string[10] { "сто", "двести", "триста", "четыреста", "пятьсот", "шестьсот", "семьсот", "восемьсот", "девятьсот", "тысяча" };

                if (price >= 100)
                {
                    text += array3[price / 100 - 1] + " ";
                    price %= 100;
                }

                if (price < 20)
                {
                    text += array[price];
                }
                else
                {
                    text += array2[price / 10];
                    if (price % 10 > 0)
                    {
                        text += " " + array[price % 10];
                    }
                }
            }

            return text;
        }
    }
}

