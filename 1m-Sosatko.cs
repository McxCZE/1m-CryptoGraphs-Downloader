using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace _1m_CryptoGraphs_Downloader
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Console.WriteLine(DateTime.UnixEpoch);

            Console.Write("Zadej prosím symbol (podporované zadání (ADAUSDT, ADA/USD) : ");
            string TradingPair = Console.ReadLine();
            Console.WriteLine("Vybraná burza FTX, další přibydou : ");
            //string Exchange = Console.ReadLine();
            Console.Write("Zadej prosím počáteční datum (od kdy mam stáhnout data, formát např. 01.01.2021) : ");
            string StartDate = Console.ReadLine();
            Console.Write("Zadej prosím koncové datum (do kdy mam stáhnout data, formát např. 01.02.2021) : ");
            string StopDate = Console.ReadLine();

            //Dummy Variables
            //string TradingPair = "ADA-PERP";
            string Exchange = "FTX";
            //string StartDate = "10.09.2021";
            //string StopDate = "22.09.2021";

            Console.WriteLine("-------------");

            Console.WriteLine("Vybrané volby");
            Console.Write("Symbol: ");
            Console.WriteLine(TradingPair);
            Console.Write("Burza: ");
            Console.WriteLine(Exchange);
            Console.Write("Od kdy: ");
            Console.WriteLine(StartDate);
            Console.Write("Do kdy: ");
            Console.WriteLine(StopDate);

            TimeSpan StartT = DateTime.Parse(StartDate) - DateTime.UnixEpoch;
            int StartT_seconds = (int)StartT.TotalSeconds;
            TimeSpan StopT = DateTime.Parse(StopDate) - DateTime.UnixEpoch;
            int StopT_seconds = (int)StopT.TotalSeconds;

            //
            string EnvPath = $@"%USERPROFILE%\Documents\1m-Grafy\{TradingPair}_{StartDate}_{StopDate}.csv";
            string SavePath = (Environment.ExpandEnvironmentVariables(EnvPath)).Replace("/", "");
            //

            FTX(TradingPair, Exchange, StartT_seconds, StopT_seconds, SavePath);

            Console.WriteLine("Stahování dokončeno, stiskni enter pro vypnutí programu.");
            Console.ReadLine();
        }

        private static void FTX(string TradingPair, string Exchange, int StartT, int StopT, string SavePath)
        {
            //Console.WriteLine(TradingPair);
            //Console.WriteLine(Exchange);
            //Console.WriteLine(StartT);
            //Console.WriteLine(StopT);
            //Console.WriteLine(SavePath);

            int StopT_Loop = StopT;

            while (StartT < StopT_Loop)
            {
                StartT += 90000;
                StopT = StartT + 90000;

                string URL = $"https://ftx.com/api/markets/{TradingPair}/candles?resolution=60&start_time={StartT}&end_time={StopT}";

                WebRequest request = WebRequest.Create(URL);
                request.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();

                //Console.WriteLine(responseFromServer);

                FTX_Response FTX_Response = JsonConvert.DeserializeObject<FTX_Response>(responseFromServer);

                //Console.WriteLine(FTX_Response.result.Count);

                int i = 0;
                while (FTX_Response.result.Count > i)
                {
                    //Console.WriteLine(FTX_Response.result[i].close);

                    string closePrice = (FTX_Response.result[i].close).ToString("G").Replace(",",".");

                    File.AppendAllText($@"{SavePath}", closePrice + Environment.NewLine);
                    i++;
                }

                Console.WriteLine($"Zapisuji do : {SavePath}");

                reader.Close();
                dataStream.Close();
                response.Close();
            }

        }

        class FTX_result
        {
            public double close { get; set; }
            public double high { get; set; }
            public double low { get; set; }
            public double open { get; set; }
            public string startTime { get; set; }
            public double volume { get; set; }
        }
        class FTX_Response
        {
            public bool success { get; set; }
            public List<FTX_result> result { get; set; }
        }
    }
}

