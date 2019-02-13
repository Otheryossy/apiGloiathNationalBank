using System;
using System.Collections.Generic;
using GloiathNationalBank.Model;

namespace GloiathNationalBank.Helpers
{
    public class ConvertAmountHelper
    {

        public static RatesModel convertRate;

        public decimal ToEur(List<RatesModel> ratesJSON, List<string> rateToEur, decimal amount, string currency, List<string> verifyCurrency)
        {
            RatesModel rate;

            if (rateToEur.IndexOf(currency) != -1)
            {
                rate = GetRateValue(ratesJSON, currency, true);
                return RoundAmount(amount * Convert.ToDecimal(rate.Rate));
            }
            else
            {
                verifyCurrency.Add(currency);
                rate = GetRateValue(ratesJSON, currency, false, verifyCurrency);
                amount = RoundAmount(amount * Convert.ToDecimal(rate.Rate));
                return ToEur(ratesJSON, rateToEur, amount, rate.To, verifyCurrency);
            }
        }

        private RatesModel GetRateValue(List<RatesModel> ratesJSON, string currency, bool isEur, List<string> verifyCurrency = null)
        {

            foreach (RatesModel rate in ratesJSON)
            {
                if (rate.From == currency)
                {
                    if (isEur && rate.To == "EUR")
                    {
                        convertRate = rate;
                    }

                    if (!isEur && verifyCurrency.IndexOf(rate.To) == -1)
                    {
                        convertRate = rate;
                    }
                }
            }

            return convertRate;
        }

        public decimal RoundAmount(decimal value)
        {
            var roundAmount = Math.Round(value, 2, MidpointRounding.ToEven);
            return roundAmount;
        }
    }
}
