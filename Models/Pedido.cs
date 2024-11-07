public class Pedido
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string Status { get; set; }
    public Cliente Cliente { get; set; }
    public List<PedidoProduto> PedidoProdutos { get; set; }
}