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
                    endereco.Complemento = enderecoApi.Complemento;
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
            if (ModelState.IsValid)
            {
                _context.Add(endereco);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(endereco);
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
                Complemento = enderecoApi.Complemento,
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
            public string Complemento { get; set; }
            public string Bairro { get; set; }
            public string Localidade { get; set; }
            public string UF { get; set; }
            
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
            var endereco = await _context.Enderecos.FindAsync(id);
            if (endereco != null)
            {
                _context.Enderecos.Remove(endereco);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnderecoExists(int id)
        {
            return _context.Enderecos.Any(e => e.IdEndereco == id);
        }
    }
}
