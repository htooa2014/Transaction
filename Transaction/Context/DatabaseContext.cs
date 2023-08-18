using Microsoft.EntityFrameworkCore;
using Transaction.Models;

namespace Transaction.Context
{
    public class DatabaseContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var connectionString = string.Format(@"Data Source=ACEBI3059\SQL2019;Initial Catalog=Transactions;User ID=sa;Password=sasa;Integrated Security=True;");
            options.UseSqlServer(connectionString);

        }

        public DbSet<Transactions> transactions { get; set; }
    }
}
