using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConcessionariaMVC.Services
{
    public class CepService
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<dynamic> GetCepInfoAsync(string cep)
        {
            if (string.IsNullOrWhiteSpace(cep)) return null;

            string url = $"https://viacep.com.br/ws/{cep}/json/";
            var response = await client.GetStringAsync(url);
            var cepInfo = JsonConvert.DeserializeObject<dynamic>(response);

            // Verifica se os campos estão presentes
            if (cepInfo == null ||
                string.IsNullOrWhiteSpace((string)cepInfo.logradouro) ||
                string.IsNullOrWhiteSpace((string)cepInfo.localidade) ||
                string.IsNullOrWhiteSpace((string)cepInfo.uf))
            {
                return null;
            }

            return cepInfo;
        }
    }
}
