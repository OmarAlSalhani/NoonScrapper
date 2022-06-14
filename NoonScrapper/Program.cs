using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NoonScrapper
{
    class Program
    {
        const string noon_name_node = "//div[@class='sc-d3293424-11 iOSKQc']";
        const string noon_price_node = "//div[@class='sc-ac248257-1 cfjJNJ']";
        static string noon_url(string search_text)
        {
            return $"https://www.noon.com/egypt-en/search/?q={search_text}";
        }
        static async Task<DataTable> LoadData(string url, string name_node, string price_node)
        {
            DataTable table = new DataTable();

            try
            {
                var names = new List<string>();
                var prices = new List<string>();
                table.Columns.Add("Product Name");
                table.Columns.Add("Product Price");
                var wc = new WebClient();
                wc.Encoding = Encoding.UTF8;
                string html = await wc.DownloadStringTaskAsync(url);
                var doc = new HtmlAgilityPack.HtmlDocument();
                html = WebUtility.HtmlDecode(html);
                doc.LoadHtml(html);
                foreach (var item in doc.DocumentNode.SelectNodes(name_node))
                {
                    names.Add(item.InnerText);
                }
                foreach (var item in doc.DocumentNode.SelectNodes(price_node))
                {
                    prices.Add(item.InnerText);
                }
                for (int i = 0; i < names.Count; i++)
                {
                    DataRow row = table.NewRow();
                    row[0] = names[i];
                    row[1] = prices[i];
                    table.Rows.Add(row);
                }

                return table;
            }
            catch (ArgumentOutOfRangeException)
            {
                return table;
            }
        }

        static async Task Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\t\t\t\t\t\tWelcome to noon scapper");
            Console.ResetColor();
            Console.Write("Enter Product name you want to search for : ");
            string search_text = Console.ReadLine();
            Console.WriteLine("Please Wait...");
            Console.WriteLine($"I`m Searching for {search_text} Now");
            DataTable table = await LoadData(noon_url(search_text), noon_name_node, noon_price_node);
            Console.WriteLine("Product Name\t\tPrice");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                string product_name = table.Rows[i][0].ToString();
                string product_price = table.Rows[i][1].ToString();
                Console.WriteLine($"{product_name}\t\t{product_price}");
            }
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine($"\n\nProducts Count :{ table.Rows.Count.ToString()}\n");
            Console.ResetColor();
        }
    }
}
