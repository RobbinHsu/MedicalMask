using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MedicalMask
{
    public class MaskService
    {
        private HttpClient _client;

        public MaskService(IHttpClientFactory client)
        {
            _client = client.CreateClient();
            _client.BaseAddress = new Uri("https://quality.data.gov.tw/");
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.Add("User-Agent", "QueryMask Sample");
        }

        public async Task<IEnumerable<MaskInfo>> GetMaskInfo()
        {
            var response = await _client.GetAsync("dq_download_json.php?nid=116285&md5_url=2150b333756e64325bdbc4a5fd45fad1");
            response.EnsureSuccessStatusCode();
            await using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<IEnumerable<MaskInfo>>(responseStream);
        }
    }
}
