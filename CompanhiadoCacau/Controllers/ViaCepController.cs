using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CompanhiadoCacau.Models;
using System.Text.RegularExpressions;

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
            // Valida o formato do CEP
            if (!Regex.IsMatch(cep, @"^\d{5}-\d{3}$"))
            {
                return BadRequest("O formato do CEP deve ser 12345-678.");
            }

            try
            {
                var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var endereco = JsonConvert.DeserializeObject<Endereco>(json);

                    if (endereco == null || string.IsNullOrEmpty(endereco.CEP))
                    {
                        return NotFound(); // Retorna 404 se o endereço não for encontrado
                    }

                    return Ok(endereco);
                }

                return NotFound(); // Retorna 404 em caso de erro na API
            }
            catch (Exception ex)
            {
                // Retorna um erro genérico caso ocorra algum problema de conexão
                return StatusCode(500, $"Erro ao acessar a API do ViaCep: {ex.Message}");
            }
        }
    }
}
