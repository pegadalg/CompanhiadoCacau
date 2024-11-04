using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CompanhiadoCacau.Models;

namespace CompanhiadoCacau.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViaCepController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public ViaCepController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: api/viacep/cep/{cep}
        [HttpGet("cep/{cep}")]
        public async Task<IActionResult> GetEnderecoPorCep(string cep)
        {
            var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var endereco = JsonConvert.DeserializeObject<Endereco>(json);

                return Ok(endereco);
            }

            return NotFound(); // Retorna 404 se o endereço não for encontrado
        }
    }
}
