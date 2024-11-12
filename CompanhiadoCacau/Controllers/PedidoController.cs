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
    public class PedidoController : Controller
    {
        private readonly CiadoCacauContext _context;

        public PedidoController(CiadoCacauContext context)
        {
            _context = context;
        }

        // GET: Pedido
        public async Task<IActionResult> Index()
        {
            var ciadoCacauContext = _context.Pedidos.Include(p => p.Cliente);
            return View(await ciadoCacauContext.ToListAsync());
        }

        // GET: Pedido/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(m => m.PedidoId == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // GET: Pedido/Create
        public IActionResult Create()
        {
            // Carregar a lista de produtos disponíveis para o pedido
            ViewBag.Produtos = new SelectList(_context.Produtos, "ProdutoId", "Nome");

            // Carregar a lista de clientes
            ViewBag.IdCliente = new SelectList(_context.Clientes, "ClienteId", "Nome");

            return View();
        }
        // POST: Pedido/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pedido pedido, int[] Produtos, int[] Quantidades)
        {
            if (ModelState.IsValid)
            {
                // Criar o pedido
                pedido.Status = StatusPedido.Pendente; // Status padrão
                pedido.DataPedido = DateOnly.FromDateTime(DateTime.Now); // Data do pedido
                _context.Add(pedido);
                await _context.SaveChangesAsync(); // Salva o pedido para obter o PedidoId

                // Associar os produtos ao pedido
                if (Produtos != null && Quantidades != null && Produtos.Length == Quantidades.Length)
                {
                    for (int i = 0; i < Produtos.Length; i++)
                    {
                        var pedidoProduto = new PedidoProduto
                        {
                            PedidoId = pedido.PedidoId,
                            ProdutoId = Produtos[i],
                            Quantidade = Quantidades[i]
                        };

                        _context.Add(pedidoProduto);
                    }

                    await _context.SaveChangesAsync(); // Salva os registros de PedidoProduto
                }

                return RedirectToAction(nameof(Index));
            }

            // Recarrega os dados caso ocorra erro de validação
            ViewBag.Produtos = new SelectList(_context.Produtos, "ProdutoId", "Nome");
            ViewBag.IdCliente = new SelectList(_context.Clientes, "ClienteId", "Nome");
            return View(pedido);
        }

        // GET: Pedido/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }

            // Passando a lista de clientes para o ViewData
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "ClienteId", "CPF", pedido.IdCliente);

            // Passando os valores do enum StatusPedido para o ViewBag
            ViewBag.Status = Enum.GetValues(typeof(StatusPedido))
                                 .Cast<StatusPedido>()
                                 .Select(e => new SelectListItem
                                 {
                                     Text = e.ToString(),  // Nome do status
                                     Value = ((int)e).ToString()  // Valor numérico associado ao enum
                                 }).ToList();

            return View(pedido);
        }

        // POST: Pedido/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PedidoId,IdCliente,Status,DataPedido,ResponsavelAtendimento")] Pedido pedido)
        {
            if (id != pedido.PedidoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.PedidoId))
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

            // Recarregar os dados caso ocorra algum erro
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "ClienteId", "CPF", pedido.IdCliente);

            // Recarregar a lista de Status para o dropdown
            ViewData["Status"] = Enum.GetValues(typeof(StatusPedido))
                                     .Cast<StatusPedido>()
                                     .Select(e => new SelectListItem
                                     {
                                         Text = e.ToString(),
                                         Value = ((int)e).ToString()
                                     }).ToList();

            return View(pedido);
        }

        // GET: Pedido/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(m => m.PedidoId == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // POST: Pedido/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Carregar o pedido com os produtos associados
            var pedido = await _context.Pedidos
                .Include(p => p.PedidoProdutos) // Incluir produtos para verificar se há algum associado
                .FirstOrDefaultAsync(p => p.PedidoId == id);

            if (pedido == null)
            {
                return NotFound();
            }

            // Verificar se o pedido tem produtos associados
            if (pedido.PedidoProdutos.Any()) // Se tiver produtos, não permite a exclusão
            {
                // Adiciona uma mensagem de erro que será exibida na View
                TempData["ErrorMessage"] = "Não é possível excluir o pedido porque ele possui produtos associados.";
                return RedirectToAction(nameof(Index)); // Redireciona para a lista de pedidos
            }

            // Caso contrário, exclui o pedido normalmente
            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Redireciona após a exclusão
        }


        private bool PedidoExists(int id)
        {
            return _context.Pedidos.Any(e => e.PedidoId == id);
        }
    }
}
