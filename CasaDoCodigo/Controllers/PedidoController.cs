﻿using CasaDoCodigo.Models;
using CasaDoCodigo.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Serialization;

namespace CasaDoCodigo.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IProdutoRepository produtoRepository;
        private readonly IPedidoRepository pedidoRepository;

        public PedidoController(
            IProdutoRepository produtoRepository, 
            IPedidoRepository pedidoRepository)
        {
            this.produtoRepository = produtoRepository;
            this.pedidoRepository = pedidoRepository;
        }

        public IActionResult Cadastro()
        {
            return View();
        }

        public IActionResult Carrinho(string codigo)
        {
            if (!string.IsNullOrEmpty(codigo))
            {
                pedidoRepository.AddItem(codigo);
            }
            Pedido pedido = pedidoRepository.GetPedido();
            return View(pedido.Itens);
        }

        public IActionResult Carrossel()
        {
            return View(produtoRepository.GetProdutos());
        }

        public IActionResult Resumo()
        {
            Pedido pedido = pedidoRepository.GetPedido();
            return View(pedido);
        }

        [HttpPost]
        public void UpdateQuantidade([FromBody] ItemPedidoViewModel itemPedido)
        {

        }
    }

    public class ItemPedidoViewModel
    {
        public ItemPedidoViewModel(int id, int quantidade)
        {
            Id = id;
            Quantidade = quantidade;
        }

        public int Id { get; set; }
        public int Quantidade { get; set; }
    }
}
