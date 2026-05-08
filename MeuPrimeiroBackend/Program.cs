
var builder = WebApplication.CreateBuilder(args);

// Avisamos o projeto que vamos usar o banco de dados
builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

// ROTA PARA BUSCAR: Vai no banco e traz todos os usuários
app.MapGet("/usuarios", (AppDbContext banco) => 
{
    return banco.Usuarios.ToList(); 
});

// ROTA PARA SALVAR: Recebe um novo usuário e salva no banco SQL
app.MapPost("/usuarios", (Usuario novoUsuario, AppDbContext banco) => 
{
    banco.Usuarios.Add(novoUsuario); // Adiciona na memória
    banco.SaveChanges(); // Dispara o comando SQL de INSERT de verdade
    
    return Results.Ok($"Usuário {novoUsuario.Nome} salvo com sucesso!");
    
});

app.MapPost("/usuarios/{id}", (int id, Usuario usuarioAtualizado, AppDbContext banco) => 
{
    var usuario = banco.Usuarios.Find(id);
    if (usuario == null)
    {
        return Results.NotFound();
    }

    usuario.Nome = usuarioAtualizado.Nome;
    usuario.Email = usuarioAtualizado.Email;
    usuario.Abacate = usuarioAtualizado.Abacate;

    banco.SaveChanges();
    return Results.Ok($"Usuário {usuario.Nome} atualizado com sucesso!");
});

app.Run();