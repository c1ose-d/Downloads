using System.Text;
using System.Text.RegularExpressions;

namespace Downloads {
    internal class Program {
        private static string? RequestUri { get; set; }
        private static Regex Regex { get; } = new(@"/netcat_files/.*'");

        private static async Task Main(string[] args) {
            byte[] bytes;

            RequestUri = Console.ReadLine();

            using HttpClient httpClient = new();
            bytes = httpClient.GetByteArrayAsync(RequestUri).Result;
            string input = Encoding.UTF8.GetString(bytes);

            MatchCollection matchCollection = Regex.Matches(input);

            for (int i = 0; i < matchCollection.Count; i++) {
                HttpClient client1 = new();
                var response = await client1.GetAsync($"http://okrug-wyksa.ru{matchCollection[i].Value.Replace("'", "")}");
                string s = File.ReadAllText("Text.txt");
                using FileStream fs = new($"{s}\\{i}.{Regex.Split(matchCollection[i].Value, @"\.(?<val>.*?)'")[1]}", FileMode.CreateNew);
                await response.Content.CopyToAsync(fs);
            }
        }
    }
}