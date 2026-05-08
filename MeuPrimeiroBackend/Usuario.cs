public class Usuario
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public required string Email { get; set; }

    public required bool Abacate { get; set; } // Se o usuário gosta de abacate ou não
    
    // A regra de ouro do EF Core: apenas as propriedades que estão aqui 
    // vão virar colunas na sua tabela. Colunas que foram removidas da 
    // estrutura do seu banco (como uma coluna de "interesses", por exemplo) 
    // simplesmente ficam de fora dessa classe.
}