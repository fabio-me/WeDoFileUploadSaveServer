using Microsoft.EntityFrameworkCore;
using WeDoFileUploadSaveServer.Repositories.Models;

namespace WeDoFileUploadSaveServer.Repositories.Contexts
{
    public class DbContextMariaDB : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // String de conexão
                var connectionString = "server=localhost;user=root;password=123;database=wedofileuploadsaveserver";
                var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));

                // Configurar o provedor do MySQL
                optionsBuilder.UseMySql(connectionString, serverVersion)
                    //.LogTo(Console.WriteLine, LogLevel.Information) // Log para depuração
                    //.EnableSensitiveDataLogging() // Log de dados sensíveis (apenas para desenvolvimento)
                    .EnableDetailedErrors(); // Erros detalhados (apenas para desenvolvimento)
            }
        }

        public DbSet<FileDb> FileDb { get; set; }




    }
}
