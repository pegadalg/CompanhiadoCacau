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
            // Carregar lista de clientes e produtos
            ViewBag.IdCliente = new SelectList(_context.Clientes, "ClienteId", "CPF");
            ViewBag.Produtos = _context.Produtos.ToList();

            // Criar um pedido novo
            var pedido = new Pedido
            {
                PedidoProdutos = new List<PedidoProduto>()
            };

            return View(pedido);
        }

        // POST: Pedido/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PedidoId,IdCliente,ValorTotal,Status,PedidoProdutos")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                // Calcular o valor total do pedido com base nos produtos
                decimal valorTotal = 0;
                foreach (var pedidoProduto in pedido.PedidoProdutos)
                {
                    var produto = await _context.Produtos.FindAsync(pedidoProduto.ProdutoId);
                    if (produto != null)
                    {
                        valorTotal += produto.Valor * pedidoProduto.Quantidade;
                    }
                }

                pedido.ValorTotal = valorTotal;

                // Adiciona o pedido no banco de dados
                _context.Add(pedido);
                await _context.SaveChangesAsync();

                // Atualizar os produtos (PedidoProduto) do pedido
                foreach (var pedidoProduto in pedido.PedidoProdutos)
                {
                    pedidoProduto.PedidoId = pedido.PedidoId;
                    _context.Add(pedidoProduto);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdCliente"] = new SelectList(_context.Clientes, "ClienteId", "CPF", pedido.IdCliente);
            ViewData["Produtos"] = new SelectList(_context.Produtos, "ProdutoId", "Nome");
            return View(pedido);
        }

        // GET: Pedido/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedidos
                .Include(p => p.PedidoProdutos) // Inclui os produtos do pedido
                .ThenInclude(pp => pp.Produto) // Inclui as informações do produto
                .FirstOrDefaultAsync(p => p.PedidoId == id);

            if (pedido == null)
            {
                return NotFound();
            }

            ViewData["IdCliente"] = new SelectList(_context.Clientes, "ClienteId", "CPF", pedido.IdCliente);
            return View(pedido);
        }

        // POST: Pedido/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PedidoId,IdCliente,ValorTotal,Status,PedidoProdutos")] Pedido pedido)
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

                    // Remove os produtos antigos
                    var produtosExistentes = _context.PedidoProdutos.Where(pp => pp.PedidoId == id).ToList();
                    _context.PedidoProdutos.RemoveRange(produtosExistentes);

                    // Adiciona os produtos atualizados (com as quantidades modificadas)
                    foreach (var pedidoProduto in pedido.PedidoProdutos)
                    {
                        pedidoProduto.PedidoId = pedido.PedidoId;
                        _context.Add(pedidoProduto);
                    }

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

            ViewData["IdCliente"] = new SelectList(_context.Clientes, "ClienteId", "CPF", pedido.IdCliente);
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
            var pedido = await _context.Pedidos
                .Include(p => p.PedidoProdutos) // Carregar os produtos relacionados ao pedido
                .FirstOrDefaultAsync(p => p.PedidoId == id);

            if (pedido != null)
            {
                // Remover os produtos do pedido
                _context.PedidoProdutos.RemoveRange(pedido.PedidoProdutos);

                // Remover o pedido
                _context.Pedidos.Remove(pedido);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedidos.Any(e => e.PedidoId == id);
        }
    }
}
