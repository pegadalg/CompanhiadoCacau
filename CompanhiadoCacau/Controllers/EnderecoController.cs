using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CompanhiadoCacau.Data;
using CompanhiadoCacau.Models;

namespace CompanhiadoCacau.Controllers
{
    public class EnderecoController : Controller
    {
        private readonly CiadoCacauContext _context;
        private readonly HttpClient _httpClient;

        public EnderecoController(CiadoCacauContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }
        // Método para buscar endereço pelo CEP
        [HttpPost]
        public async Task<IActionResult> BuscarEnderecoPorCep(string cep)
        {
            if (string.IsNullOrEmpty(cep))
            {
                return BadRequest("CEP não pode ser vazio.");
            }

            // Remove caracteres não numéricos
            cep = cep.Replace("-", "").Trim();

            // Chama a API ViaCep
            var enderecoApi = await _httpClient.GetFromJsonAsync<ViaCepResponse>($"https://viacep.com.br/ws/{cep}/json/");

            if (enderecoApi == null || enderecoApi.Logradouro == null)
            {
                return NotFound("Endereço não encontrado.");
            }

            // Mapeia os dados recebidos para o modelo Endereco
            var endereco = new Endereco
            {
                CEP = enderecoApi.CEP,
                Logradouro = enderecoApi.Logradouro,
                Bairro = enderecoApi.Bairro,
                Localidade = enderecoApi.Localidade,
                UF = enderecoApi.UF
                // Não vamos preencher o número porque isso deve ser feito pelo usuário
            };

            return Json(endereco); // Retorna o endereço como JSON
        }

        // Classe para mapear a resposta da API ViaCep
        public class ViaCepResponse
        {
            public string CEP { get; set; }
            public string Logradouro { get; set; }
            public string Bairro { get; set; }
            public string Localidade { get; set; }
            public string UF { get; set; }

        }

        // GET: Endereco
        public async Task<IActionResult> Index()
        {
            return View(await _context.Enderecos.ToListAsync());
        }

        // GET: Endereco/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var endereco = await _context.Enderecos
                .FirstOrDefaultAsync(m => m.IdEndereco == id);
            if (endereco == null)
            {
                return NotFound();
            }

            return View(endereco);
        }

        // GET: Endereco/Create
        public async Task<IActionResult> Create(string cep)
        {
            var endereco = new Endereco();
            if (!string.IsNullOrEmpty(cep))
            {
                // Chama a API ViaCep
                var enderecoApi = await _httpClient.GetFromJsonAsync<ViaCepResponse>($"https://viacep.com.br/ws/{cep}/json/");

                if (enderecoApi != null && enderecoApi.Logradouro != null)
                {
                    endereco.CEP = enderecoApi.CEP;
                    endereco.Logradouro = enderecoApi.Logradouro;
                    endereco.Bairro = enderecoApi.Bairro;
                    endereco.Localidade = enderecoApi.Localidade;
                    endereco.UF = enderecoApi.UF;
                }
            }

            return View(endereco);
        }

        // POST: Endereco/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEndereco,CEP,Logradouro,Complemento,Bairro,Localidade,UF,Numero")] Endereco endereco)
        {
            // Verifica se o campo Complemento está vazio e, em caso afirmativo, define como null
            if (string.IsNullOrEmpty(endereco.Complemento))
            {
                endereco.Complemento = null;
            }

            // Exibe erros do ModelState para debugar se o ModelState não estiver válido
            if (!ModelState.IsValid)
            {
                // Imprime os erros do ModelState no console para depuração
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage); // Exibe o erro no console
                }

                // Retorna a view com os erros, preservando os dados preenchidos no formulário
                return View(endereco);
            }

            try
            {
                // Adiciona o novo endereço ao contexto
                _context.Add(endereco);

                // Salva as mudanças no banco de dados de forma assíncrona
                await _context.SaveChangesAsync();

                // Redireciona para a página de índice (ou para a listagem de endereços, por exemplo)
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Se ocorrer algum erro, registra e exibe o erro no console para depurar
                Console.WriteLine("Erro ao salvar o endereço: " + ex.Message);

                // Adiciona um erro genérico de falha de salvamento
                ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar os dados. Tente novamente.");

                // Retorna a view com os dados preenchidos e o erro
                return View(endereco);
            }
        }




        // GET: Endereco/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var endereco = await _context.Enderecos.FindAsync(id);
            if (endereco == null)
            {
                return NotFound();
            }
            return View(endereco);
        }

        // POST: Endereco/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEndereco,CEP,Logradouro,Complemento,Bairro,Localidade,UF,Numero")] Endereco endereco)
        {
            if (id != endereco.IdEndereco)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(endereco);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnderecoExists(endereco.IdEndereco))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(endereco);
        }

        // GET: Endereco/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var endereco = await _context.Enderecos
                .FirstOrDefaultAsync(m => m.IdEndereco == id);
            if (endereco == null)
            {
                return NotFound();
            }

            return View(endereco);
        }

        // POST: Endereco/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Carrega o endereço pelo id
            var endereco = await _context.Enderecos.FindAsync(id);

            if (endereco == null)
            {
                return NotFound();
            }

            // Verificar se o endereço está associado a algum cliente
            var clienteAssociado = await _context.Clientes
                .AnyAsync(c => c.EnderecoId == id);

            if (clienteAssociado) // Se existe um cliente com este endereço, bloqueia a exclusão
            {
                // Adiciona uma mensagem de erro que será exibida na View
                TempData["ErrorMessage"] = "Não é possível excluir o endereço porque ele está associado a um cliente.";
                return RedirectToAction(nameof(Index)); // Redireciona para a lista de endereços
            }

            // Caso contrário, exclui o endereço normalmente
            _context.Enderecos.Remove(endereco);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Redireciona após a exclusão
        }



        private bool EnderecoExists(int id)
        {
            return _context.Enderecos.Any(e => e.IdEndereco == id);
        }
    }
}
