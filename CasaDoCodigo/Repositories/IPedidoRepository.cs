using CasaDoCodigo.Models;

namespace CasaDoCodigo.Repositories
{
    public interface IPedidoRepository
    {
        void AddItem(string codigo);
        Pedido GetPedido();
        UpdateQuantidadeResponse UpdateQuantidade(ItemPedido itemPedido);
    }
}
