using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    // Isso diz que teremos uma tabela chamada "Usuarios"
    public DbSet<Usuario> Usuarios { get; set; } 

    // Aqui configuramos onde o banco vai ser salvo
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=meubanco.db");
    }
}