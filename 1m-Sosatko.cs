using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Data;

namespace _1m_CryptoGraphs_Downloader
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            //UI
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Zadej prosím symbol (podporované zadání (ADAUSDT, ADA/USD) : ");
            Console.ForegroundColor = ConsoleColor.Green;
            string TradingPair = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Vyber burzu (FTX/BINANCE): ");
            Console.ForegroundColor = ConsoleColor.Green;
            string Exchange = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Zadej prosím počáteční datum (od kdy mam stáhnout data, formát např. 01.01.2021) : ");
            Console.ForegroundColor = ConsoleColor.Green;
            string StartDate = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Zadej prosím koncové datum (do kdy mam stáhnout data, formát např. 01.02.2021) : ");
            Console.ForegroundColor = ConsoleColor.Green;
            string StopDate = Console.ReadLine();



            //Dummy Variables
            //string TradingPair = "ADAUPUSDT";
            //string Exchange = "BINANCE";
            //string StartDate = "10.09.2021";
            //string StopDate = "22.09.2021";


            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine("Vybrané volby");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Symbol: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(TradingPair);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Burza: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(Exchange);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Od kdy: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(StartDate);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Do kdy: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(StopDate);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            

            TimeSpan StartT = DateTime.Parse(StartDate) - DateTime.UnixEpoch;
            int StartT_seconds = (int)StartT.TotalSeconds;
            double StartT_miliseconds = (double)StartT.TotalMilliseconds;
            TimeSpan StopT = DateTime.Parse(StopDate) - DateTime.UnixEpoch;
            int StopT_seconds = (int)StopT.TotalSeconds;
            double StopT_miliseconds = (double)StopT.TotalMilliseconds;

            //
            string EnvPath = $@"%USERPROFILE%\Documents\1m-Grafy\{TradingPair}_{StartDate}_{StopDate}.csv";
            string SavePath = (Environment.ExpandEnvironmentVariables(EnvPath)).Replace("/", "");
            //

            // Kontrola Prerekvizit.
            string EnvCheckPath = $@"%USERPROFILE%\Documents\1m-Grafy";
            string CheckPath = Environment.ExpandEnvironmentVariables(EnvCheckPath);

            if (Directory.Exists(CheckPath))
            {
                Console.WriteLine($"Adresář {CheckPath} existuje.");
            } else
            {
                Console.WriteLine($"Adresář {CheckPath} neexistuje. Vytvářím adresář.");
                Directory.CreateDirectory(CheckPath);
            }
            //

            // Poslední zastávka před exekucí.
            Console.WriteLine();
            Console.WriteLine("Pro pokračování stiskni ENTER");
            Console.ReadLine();

            if (Exchange == "BINANCE")
            {
                BINANCE(TradingPair, Exchange, StartT_miliseconds, StopT_miliseconds, SavePath);
            }

            if (Exchange == "FTX")
            {
                FTX(TradingPair, Exchange, StartT_seconds, StopT_seconds, SavePath);
            }

            // Exekuce provedena, konečná.
            Console.WriteLine("Stahování dokončeno, stiskni ENTER pro vypnutí programu.");
            Console.ReadLine();
        }

        private static void FTX(string TradingPair, string Exchange, int StartT, int StopT, string SavePath)
        {
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

                Console.WriteLine($"Zapisuji data, {StartT} az {StopT}");

                reader.Close();
                dataStream.Close();
                response.Close();
            }

        }
        private static void BINANCE(string TradingPair, string Exchange, double StartT, double StopT, string SavePath)
        {
            double StopT_Loop = StopT;

            //Sloužilo pro zpětnou kontrolu, ponechávám zde.
            //Console.WriteLine(StartT);
            //Console.WriteLine(StopT_Loop);

            while (StartT < StopT_Loop)
            {

                string URL = $"https://api.binance.com/api/v3/klines?symbol={TradingPair}&interval=1m&startTime={StartT}&limit=1000";

                WebRequest request = WebRequest.Create(URL);
                request.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();

                JsonTextReader readerJson = new JsonTextReader(new StringReader(responseFromServer));

                int i = 0;
                while (readerJson.Read())
                {
                    //string tokenType = readerJson.TokenType;
                    //string Value = readerJson.Value;
                    //Console.WriteLine("Token: {0}, Value: {1}", readerJson.TokenType, readerJson.Value);
                    //Console.WriteLine(readerJson.TokenType);
                    //Console.WriteLine(readerJson.Value);
                    string Condition = "String";

                    if (readerJson.Depth == 2)
                    {

                        bool compare = string.Equals(Condition, readerJson.TokenType.ToString());

                        //Console.WriteLine(compare);
                        if (compare)
                        {
                            //Console.Write($"{i} : ");
                            //Open price
                            if (i == 0)
                            {
                                //Console.Write("Open price : ");
                                //Console.WriteLine(readerJson.Value);
                            }

                            //High price
                            if (i == 1)
                            {
                                //Console.Write("High price : ");
                                //Console.WriteLine(readerJson.Value);
                            }

                            //Low price

                            if (i == 2)
                            {
                                //Console.Write("Low price : ");
                                //Console.WriteLine(readerJson.Value);
                            }

                            //Close price
                            if (i == 3)
                            {
                                //Console.Write("Close price : ");
                                //Console.WriteLine(readerJson.Value);

                                File.AppendAllText($@"{SavePath}", readerJson.Value + Environment.NewLine);
                            }

                            //Volume
                            if (i == 4)
                            {
                                //Console.Write("Volume : ");
                                //Console.WriteLine(readerJson.Value);
                            }

                            //Quoate Asset Volume
                            if (i == 5)
                            {
                                //Console.Write("Quote Asset Volume : ");
                                //Console.WriteLine(readerJson.Value);
                            }

                            //Taker buy base asset volume
                            if (i == 6)
                            {
                                //Console.Write("Taker buy base asset volume : ");
                                //Console.WriteLine(readerJson.Value);
                            }

                            //Taker buy quote asset volume
                            if (i == 7)
                            {
                                //Console.Write("Taker buy quote asset volume : ");
                                //Console.WriteLine(readerJson.Value);
                            }

                            //Ignore
                            if (i == 8)
                            {
                                //Console.Write("Ignore? : ");
                                //Console.WriteLine(readerJson.Value);
                            }

                            //Pricti i, ale kdyby jsi nuloval, nepricitej pred pruchodem, ale zaroven, vime ze i max 9, takze pricti
                            //A pak vynuluj.
                            i++;
                            

                            //Vynuluj Index
                            if (i == 9)
                            {
                                i = 0;
                            }
                        }
                        //Console.Write(readerJson.TokenType);
                        //Console.WriteLine(readerJson.Value);
                    }
                }

                Console.WriteLine($"Zapisuji data, {StartT} az {StopT}");

                reader.Close();
                dataStream.Close();
                response.Close();

                StartT += 60000 * 1000;
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

