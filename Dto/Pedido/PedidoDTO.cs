public class PedidoDTO
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string Status { get; set; }
    public List<PedidoProdutoDTO> PedidoProdutos { get; set; }
}

