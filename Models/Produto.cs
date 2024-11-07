public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Tipo { get; set; }
    public decimal Valor { get; set; }
    public List<PedidoProduto> PedidoProdutos { get; set; } = new List<PedidoProduto>();  // Inicializando como lista vazia
}
