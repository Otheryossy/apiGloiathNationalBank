using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using GloiathNationalBank.Model;
using Newtonsoft.Json;

namespace GloiathNationalBank.Services
{
    public class ApiService
    {
        public List<RatesModel> GetRates()
        {

            List<RatesModel> rates;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://quiet-stone-2094.herokuapp.com/rates.json");
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                rates = JsonConvert.DeserializeObject<List<RatesModel>>(json);
            }

            return rates;

        }

        public List<TransactionModel> GetTransactions()
        {

            List<TransactionModel> transaction;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://quiet-stone-2094.herokuapp.com/transactions.json");
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                transaction = JsonConvert.DeserializeObject<List<TransactionModel>>(json);
            }

            return transaction;

        }
    }
}
