using Libra.Models;
using Microsoft.EntityFrameworkCore;

namespace Libra.Services.DataBaseService
{
    public class DataBaseService: DbContext
    {
        public DataBaseService(DbContextOptions<DataBaseService> options):base (options)
        {
            
        }
    }
}
