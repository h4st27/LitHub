using k8s.KubeConfigModels;
using Microsoft.EntityFrameworkCore;

namespace LitHub.Services.DataBaseService
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base (options)
        {
            
        }
    }
}
