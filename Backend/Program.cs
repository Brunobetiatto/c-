var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // URL do Angular
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddDbContext<AppDbContext>();


var app = builder.Build();


app.UseCors("PermitirAngular");

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

app.MapPost("/usuarios/{id:int}", (int id, Usuario usuarioAtualizado, AppDbContext banco) => 
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

app.MapGet("/usuarios/{id:int}", (int id, AppDbContext banco) => 
{
    var usuario = banco.Usuarios.Find(id);
    if (usuario == null)
    {
        return Results.NotFound();
    } 
    return Results.Ok(usuario);
});

app.MapGet("/usuarios/abacate", (AppDbContext banco) => 
{
    var usuariosComAbacate = banco.Usuarios.Where(u => u.Abacate).ToList();
    return Results.Ok(usuariosComAbacate);
});

app.MapDelete("/usuarios/deleteAll", (AppDbContext banco) => 
{
    var todosUsuarios = banco.Usuarios.ToList();
    banco.Usuarios.RemoveRange(todosUsuarios);
    banco.SaveChanges();
    return Results.Ok("Todos os usuários foram deletados.");
});

app.Run();