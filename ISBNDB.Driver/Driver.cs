using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ISBNDB
{
    public class Driver
    {
        public string ApiKey { get; private set; }
        private HttpClient _httpClient = new HttpClient();

        private string RootUrl 
        {
            get
            {
                return string.Format( "http://isbndb.com/api/v2/json/{0}/", ApiKey );
            }
        }

        public Driver( string apiKey )
        {
            ApiKey = apiKey;
        }

        public async Task<Book> GetByISBNAsync( string isbn )
        {
            var response = await _httpClient.GetAsync( RootUrl + "book/" + isbn );
            response.EnsureSuccessStatusCode();

            string contentString = await response.Content.ReadAsStringAsync();
            var bookResponse = JsonConvert.DeserializeObject<BookResponse>( contentString );
            return bookResponse.Data.FirstOrDefault();
        }
    }
}
