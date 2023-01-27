using System.Net.Http;
using System.Linq;
using System.Text.RegularExpressions;


namespace Crawler
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            if (args.Length == 0)
            {
                throw new ArgumentNullException("Podaj adres strony");
            }

            string websiteUrl = args[0];

            if (!Uri.IsWellFormedUriString(websiteUrl, UriKind.Absolute))
            {
                throw new ArgumentException("Podaj adres strony we właściwym formacie");
            }

            HttpClient httpClient = new HttpClient();

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(websiteUrl);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    Regex emailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);

                    MatchCollection emailMatches = emailRegex.Matches(result);

                    List<string> distinctEmailMatches = emailMatches
                        .Cast<Match>()
                        .Select(m => m.Value)
                        .Distinct()
                        .ToList();


                    if (emailMatches.Count > 0)
                    {
                        foreach (string emailMatch in distinctEmailMatches)
                        {
                            Console.WriteLine(emailMatch);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Nie znaleziono adresów email");
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Błąd w czasie pobierania strony");
            }
            finally
            {
                httpClient.Dispose();
            }

            
        }
    }
}