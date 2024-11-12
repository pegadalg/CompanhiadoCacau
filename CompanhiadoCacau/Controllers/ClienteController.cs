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
    public class ClienteController : Controller
    {
        private readonly CiadoCacauContext _context;

        public ClienteController(CiadoCacauContext context)
        {
            _context = context;
        }

        // GET: Cliente
        public async Task<IActionResult> Index()
        {
            var ciadoCacauContext = _context.Clientes.Include(c => c.Endereco);
            return View(await ciadoCacauContext.ToListAsync());
        }

        // GET: Cliente/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.Endereco)
                .FirstOrDefaultAsync(m => m.ClienteId == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Cliente/Create
        public IActionResult Create()
        {
            ViewData["EnderecoId"] = new SelectList(_context.Enderecos, "IdEndereco", "CEP");
            return View();
        }

        // POST: Cliente/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClienteId,Nome,DataNascimento,CPF,Email,Telefone,EnderecoId")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                // Verifica se já existe algum cliente com o mesmo CPF
                bool cpfExistente = await _context.Clientes
                    .AnyAsync(c => c.CPF == cliente.CPF);

                if (cpfExistente)
                {
                    ModelState.AddModelError("CPF", "CPF já cadastrado");
                    // Recarrega a lista de Endereços para o formulário
                    ViewData["EnderecoId"] = new SelectList(_context.Enderecos, "IdEndereco", "CEP", cliente.EnderecoId);
                    return View(cliente);
                }

                // Se o CPF não existir, salva o novo cliente
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Se a model não for válida, recarrega os dados
            ViewData["EnderecoId"] = new SelectList(_context.Enderecos, "IdEndereco", "CEP", cliente.EnderecoId);
            return View(cliente);
        }


        // GET: Cliente/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            ViewData["EnderecoId"] = new SelectList(_context.Enderecos, "IdEndereco", "CEP", cliente.EnderecoId);
            return View(cliente);
        }

        // POST: Cliente/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClienteId,Nome,DataNascimento,CPF,Email,Telefone,EnderecoId")] Cliente cliente)
        {
            if (id != cliente.ClienteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.ClienteId))
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
            ViewData["EnderecoId"] = new SelectList(_context.Enderecos, "IdEndereco", "CEP", cliente.EnderecoId);
            return View(cliente);
        }

        // GET: Cliente/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.Endereco)
                .FirstOrDefaultAsync(m => m.ClienteId == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Carregar o cliente com os pedidos associados
            var cliente = await _context.Clientes
                .Include(c => c.PedidosCliente) // Incluir pedidos para verificar se há algum
                .FirstOrDefaultAsync(c => c.ClienteId == id);

            if (cliente == null)
            {
                return NotFound();
            }

            // Verificar se o cliente tem pedidos associados
            if (cliente.PedidosCliente.Any()) // Se tiver pedidos, não permite a exclusão
            {
                // Adiciona uma mensagem de erro que será exibida na View
                TempData["ErrorMessage"] = "Não é possível excluir o cliente porque ele possui pedidos.";
                return RedirectToAction(nameof(Index)); // Redireciona para a lista de clientes
            }

            // Caso contrário, exclui o cliente normalmente
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Redireciona após a exclusão
        }
        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.ClienteId == id);
        }
    }
}
