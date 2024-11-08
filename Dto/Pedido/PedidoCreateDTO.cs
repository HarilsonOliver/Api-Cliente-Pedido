public class PedidoCreateDTO
{
    public int ClienteId { get; set; }
    public string Status { get; set; }
    public List<PedidoProdutoCreateDTO> PedidoProdutos { get; set; }
}
