﻿using CasaDoCodigo.Models;
using CasaDoCodigo.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CasaDoCodigo.Repositories
{
    public class PedidoRepository : BaseRepository<Pedido>, IPedidoRepository
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IItemPedidoRepository itemPedidoRepository;
        private readonly ICadastroRepository cadastroRepository;

        public PedidoRepository(
            ApplicationContext context, 
            IHttpContextAccessor contextAccessor,
            IItemPedidoRepository itemPedidoRepository,
            ICadastroRepository cadastroRepository) : base(context)
        {
            this.contextAccessor = contextAccessor;
            this.itemPedidoRepository = itemPedidoRepository;
            this.cadastroRepository = cadastroRepository;
        }

        public void AddItem(string codigo)
        {
            var produto = context.Set<Produto>()
                .Where(p => p.Codigo == codigo)
                .SingleOrDefault();

            if (produto == null)
            {
                throw new ArgumentException("Produto não encontrado");
            }

            var pedido = GetPedido();

            var itemPedido = context.Set<ItemPedido>()
                .Where(i => i.Produto.Codigo == codigo
                    && i.Pedido.Id == pedido.Id)
                .SingleOrDefault();

            if (itemPedido == null)
            {
                itemPedido = new ItemPedido(pedido, produto, 1, produto.Preco);
                context.Set<ItemPedido>().Add(itemPedido);
                context.SaveChanges();
            }

        }

        public Pedido GetPedido()
        {
            var pedidoId = GetPedidoId();
            var pedido = dbSet
                .Include(p => p.Itens)
                .ThenInclude(p => p.Produto)
                .Include(p => p.Cadastro)
                .Where(p => p.Id == pedidoId)
                .SingleOrDefault();

            if (pedido == null)
            {
                pedido = new Pedido();
                dbSet.Add(pedido);
                context.SaveChanges();
                SetPedidoId(pedido.Id);
            }

            return pedido;
        }

        private int? GetPedidoId()
        {
            return contextAccessor.HttpContext.Session.GetInt32("pedidoId");
        }

        private void SetPedidoId(int pedidoId)
        {
            contextAccessor.HttpContext.Session.SetInt32("pedidoId", pedidoId);
        }

        public UpdateQuantidadeResponse UpdateQuantidade(ItemPedido itemPedido)
        {
            var itemPedidoDB = itemPedidoRepository.GetItemPedido(itemPedido.Id);

            if (itemPedidoDB != null)
            {
                itemPedidoDB.AtualizarQuantidade(itemPedido.Quantidade);

                if (itemPedidoDB.Quantidade == 0)
                {
                    itemPedidoRepository.RemoveItemPedido(itemPedidoDB.Id);
                }

                context.SaveChanges();

                var carrinhoViewModel = new CarrinhoViewModel(GetPedido().Itens); 
                return new UpdateQuantidadeResponse(itemPedidoDB, carrinhoViewModel);
            }

            throw new ArgumentException("ItemPedido não encontrado.");
        }

        public Pedido UpdateCadastro(Cadastro cadastro)
        {
            var pedido = GetPedido();
            cadastroRepository.Update(pedido.Cadastro.Id, cadastro);
            return pedido;
        }
    }
}
