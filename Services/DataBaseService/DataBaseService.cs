using LitHub.Models;
using Microsoft.EntityFrameworkCore;

namespace LitHub.Services.DataBaseService
{
    public class DataBaseService: DbContext
    {
        public DataBaseService(DbContextOptions<DataBaseService> options):base (options)
        {
            
        }
    }
}
