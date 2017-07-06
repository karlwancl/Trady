using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trady.Importer;
using System.Linq;

namespace Trady.Test
{
    [TestClass]
    public class ImporterTest
    {
        public ImporterTest()
        {
            // Test culture info
            CultureInfo.CurrentCulture = new CultureInfo("nl-nl");
        }

        [TestMethod]
        public void ImportByGoogleFinance()
        {
            var importer = new GoogleFinanceImporter();
            var candle = importer.ImportAsync("NASDAQ/AAPL", new DateTime(2017, 1, 3), new DateTime(2017, 1, 3)).Result.First();
			Assert.AreEqual(candle.Open, 115.8m);
			Assert.AreEqual(candle.High, 116.33m);
			Assert.AreEqual(candle.Low, 114.76m);
			Assert.AreEqual(candle.Close, 116.15m);
			Assert.AreEqual(candle.Volume, 28_781_865);
        }

        [TestMethod]
        public void ImportByGoogleFinance()
        {
            var importer = new GoogleFinanceImporter();
            var candle = importer.ImportAsync("NASDAQ/AAPL", new DateTime(2017, 1, 3), new DateTime(2017, 1, 3)).Result.First();
			Assert.AreEqual(candle.Open, 115.8m);
			Assert.AreEqual(candle.High, 116.33m);
			Assert.AreEqual(candle.Low, 114.76m);
			Assert.AreEqual(candle.Close, 116.15m);
			Assert.AreEqual(candle.Volume, 28_781_865);
        }

        [TestMethod]
        public void ImportByQuandlYahoo()
        {
			// Test account api key
			const string ApiKey = "Sys3z7hfYmzjiXPxwfQJ";

            var importer = new QuandlWikiImporter(ApiKey);
            var candle = importer.ImportAsync("AAPL", new DateTime(2017, 1, 3), new DateTime(2017, 1, 3)).Result.First();
			Assert.AreEqual(candle.Open, 115.8m);
			Assert.AreEqual(candle.High, 116.33m);
			Assert.AreEqual(candle.Low, 114.76m);
			Assert.AreEqual(candle.Close, 116.15m);
			Assert.AreEqual(candle.Volume, 28_781_865);
        }

        [TestMethod]
        public void ImportByYahoo()
        {
			var importer = new YahooFinanceImporter();
			var candle = importer.ImportAsync("^GSPC", new DateTime(2017, 1, 3), new DateTime(2017, 1, 4)).Result.First();  // Endtime stock history exclusive
			Assert.AreEqual(candle.Open, 2251.570068m);
			Assert.AreEqual(candle.High, 2263.879883m);
			Assert.AreEqual(candle.Low, 2245.129883m);
			Assert.AreEqual(candle.Close, 2257.830078m);
			Assert.AreEqual(candle.Volume, 3_770_530_000);
		}

        //[TestMethod]
        //public void CultureNotBeingSetCauseYahooImporterToFail()
        //{
        //    CultureInfo.CurrentCulture = new CultureInfo("en-us");
        //    var importer = new YahooFinanceImporter();
        //    var candle = importer.ImportAsync("^GSPC", new DateTime(2017, 1, 3), new DateTime(2017, 1, 4)).Result.First();  // Endtime stock history exclusive
        //    Assert.AreEqual(candle.Open, 2251.570068m);

        //    /// Argentina's culture. Just to give a culture that use , and . in the opposite way from  United States 
        //    /// and 3.000 means three thousands.
        //    CultureInfo.CurrentCulture = new CultureInfo("es-ar");
        //    importer = new YahooFinanceImporter();
        //    candle = importer.ImportAsync("^GSPC", new DateTime(2017, 1, 3), new DateTime(2017, 1, 4)).Result.First();  // Endtime stock history exclusive
        //    /// Actual value is 2251.570068m but if you don't set the culture to the right one the call to yahoo brings
        //    /// wrong numbers.
        //    Assert.AreEqual(candle.Open, 2251570068m);
        //}

        [TestMethod]
        public void ImportByStooq()
        {
            var importer = new StooqImporter();
            var candle = importer.ImportAsync("^SPX", new DateTime(2017, 1, 3), new DateTime(2017, 1, 3)).Result.First();   // Endtime stock history inclusive
			Assert.AreEqual(candle.Open, 2251.57m);
			Assert.AreEqual(candle.High, 2263.88m);
			Assert.AreEqual(candle.Low, 2245.13m);
			Assert.AreEqual(candle.Close, 2257.83m);
			Assert.AreEqual(candle.Volume, 644_640_832);
		}

        [TestMethod]
        public void ImportFromCsv()
        {
            var importer = new CsvImporter("fb.csv", new CultureInfo("en-US"));
            var candles = importer.ImportAsync("FB").Result;
            Assert.AreEqual(candles.Count(), 1216);
            var firstCandle = candles.First();
            Assert.AreEqual(firstCandle.DateTime, new DateTime(2012,5,18));
        }
    }
}
