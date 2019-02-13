using System;
using System.Collections.Generic;
using System.Linq;
using GloiathNationalBank.Helpers;
using GloiathNationalBank.Model;
using GloiathNationalBank.Services;

namespace GloiathNationalBank.Processes
{
    public class SkuProcesses
    {


        public static List<RatesModel> ratesJSON = new List<RatesModel>();
        public static List<TransactionModel> listTransactionsJSON = new List<TransactionModel>();
        public static List<string> rateToEur = new List<string>();
        public List<SkuTransactionModel> allTransactions = new List<SkuTransactionModel>();
        public List<SkuListModel> listSku = new List<SkuListModel>();
        public decimal transactionSum;


        internal List<SkuListModel> GetListSku()
        {

            try {

                var api = new ApiService();
                listTransactionsJSON = api.GetTransactions();
                 ratesJSON = api.GetRates();

                var groupSku = listTransactionsJSON.GroupBy(x => x.Sku).Select(g => new { name = g.Key });

                foreach (var sku in groupSku)
                {
                    listSku.Add(new SkuListModel()
                    {
                        Name = sku.name
                    });
                }

            } catch (Exception ex) {

                listSku = null;
                throw ex;
            }

            return listSku;
        }


        internal List<SkuTransactionModel> GetTransactionForSku(string sku)
        {
            try
            {
                FilterRateToEur();
                var ConvertHelper = new ConvertAmountHelper();
                var transactionSku = listTransactionsJSON.Where(x => x.Sku == sku).ToList();

                foreach (var element in transactionSku)
                {
                    var amountInt = Convert.ToDecimal(element.Amount);

                    if (element.Currency == "EUR")
                    {
                        allTransactions.Add(new SkuTransactionModel
                        {
                            Name = element.Sku,
                            Amount = ConvertHelper.RoundAmount(amountInt),
                            Currency = "EUR"
                        });

                        transactionSum += ConvertHelper.RoundAmount(amountInt);
                    }

                    if (element.Currency != "EUR")
                    {
                        decimal amountConvert = ConvertHelper.ToEur(ratesJSON, rateToEur, amountInt, element.Currency, new List<string>());

                        allTransactions.Add(new SkuTransactionModel
                        {
                            Name = element.Sku,
                            Amount = amountConvert,
                            Currency = "EUR"
                        });

                        transactionSum += amountConvert;
                    }
                }

                allTransactions.Add(new SkuTransactionModel
                {
                    Sum = transactionSum
                });

            } catch (Exception ex) {
                allTransactions = null;
                throw ex;
            }

            return allTransactions;
        }

        private void FilterRateToEur()
        {
            foreach (RatesModel rate in ratesJSON)
            {
                if (rate.To == "EUR")
                {
                    rateToEur.Add(rate.From);
                }
            }
        }

    }
}
