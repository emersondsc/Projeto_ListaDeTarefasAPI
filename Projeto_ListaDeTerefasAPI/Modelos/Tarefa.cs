namespace Projeto_ListaDeTerefasAPI.Modelos
{
    public record Tarefa (int Id, string Nome, DateOnly Prazo, bool FInalizada, string? Descricao)
    { 
        public Pessoa? Pessoa { get; set; }
    };
}
