using k8s.KubeConfigModels;
using Microsoft.EntityFrameworkCore;

namespace Libra.Services.DataBaseService
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base (options)
        {
            
        }
    }
}
