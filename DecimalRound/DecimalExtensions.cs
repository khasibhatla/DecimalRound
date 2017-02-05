using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace DecimalRound
{
    /// <summary>
    /// A collection of extension methods for decimal related rounding operations 
    /// </summary>
    public static class DecimalExtensions
    {
        /// <summary>
        /// Performs an arithmetic symmetrical rounding to the IEnumerable decimal <paramref name="source"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static decimal CurrencySum(this IEnumerable<decimal> source)
        {
            return source.Sum(number => ArithmeticSymmetricalRound(number, GetCurrencyDecimalDigits()));
        }

        /// <summary>
        /// Performs an arithmetic symmetrical rounding to the IEnumerable decimal <paramref name="source"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static decimal CurrencySum(this IEnumerable<decimal?> source)
        {
            return source.Sum(number =>
            {
                Debug.Assert(number != null, $"number != null");
                return ArithmeticSymmetricalRound(number.Value, GetCurrencyDecimalDigits());
            });
        }

        /// <summary>
        /// Performs an arithmetic symmetrical rounding to the decimal <paramref name="source"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static decimal CurrencyRound(this decimal source)
        {
            return ArithmeticSymmetricalRound(source, GetCurrencyDecimalDigits());
        }

        /// <summary>
        /// Gets the number of decimal places to use in currency values. 
        /// </summary>
        /// <returns></returns>
        public static int GetCurrencyDecimalDigits()
        {
            var nfi = new CultureInfo(CultureInfo.CurrentCulture.Name, false).NumberFormat;
            return nfi.CurrencyDecimalDigits;
        }

        /// <summary>
        /// Performs an arithmetic symmetrical rounding to the decimal <paramref name="source"/> based on the number of <paramref name="decimals"/>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static decimal ArithmeticSymmetricalRound(decimal source, int decimals)
        {
            var factor = Convert.ToDecimal(Math.Pow(10, decimals));
            var sign = Math.Sign(source);
            return decimal.Truncate(source * factor + 0.5m * sign) / factor;
        }

        /// <summary>
        /// Performs an arithmetic symmetrical rounding to the decimal <paramref name="source"/> based on the number of <paramref name="decimals"/>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static decimal ArithmeticSymmetricalRound(decimal? source, int decimals)
        {
            var factor = Convert.ToDecimal(Math.Pow(10, decimals));

            if (!source.HasValue) return 0M;
            var sign = Math.Sign(source.Value);
            return decimal.Truncate(source.Value * factor + 0.5m * sign) / factor;
        }

        /// <summary>
        /// Performs a bankers rounding to the decimal <paramref name="source"/> based on the number of <paramref name="decimals"/>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static decimal BankersRound(decimal source, int decimals)
        {
            return decimal.Round(source, decimals);
        }

        /// <summary>
        /// Performs a rounding to the decimal <paramref name="source"/> based on the number of <paramref name="decimals"/> and away from zero
        /// </summary>
        /// <param name="source"></param>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static decimal MidpointRoundingAwayFromZero(decimal source, int decimals)
        {
            return decimal.Round(source, decimals, MidpointRounding.AwayFromZero);
        }
    }
}
