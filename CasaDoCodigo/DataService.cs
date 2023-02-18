using Microsoft.EntityFrameworkCore;

namespace CasaDoCodigo
{
    class DataService : IDataService
    {
        private readonly ApplicationContext context;

        public DataService(ApplicationContext context)
        {
            this.context = context;
        }

        public void InicializarDB()
        {
            context.Database.Migrate();
        }
    }
}
