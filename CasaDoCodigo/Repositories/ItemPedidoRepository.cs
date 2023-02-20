using CasaDoCodigo.Models;

namespace CasaDoCodigo.Repositories
{
    public class ItemPedidoRepository : BaseRepository<ItemPedido>, IItemPedidoRepository
    {
        public ItemPedidoRepository(ApplicationContext context) : base(context)
        {
        }

        public void UpdateQuantidade(ItemPedido itemPedido)
        {
            throw new System.NotImplementedException();
        }
    }
}
