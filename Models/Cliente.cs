public class Cliente
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string NumeroContato { get; set; }
    public DateTime DataNascimento { get; set; }
    public List<Pedido> Pedidos { get; set; } = new List<Pedido>();
}

