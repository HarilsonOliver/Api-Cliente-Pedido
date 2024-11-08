using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("v1/pedidos")]
public class PedidoController : ControllerBase
{
    private readonly ApiDbContext _context;

    public PedidoController(ApiDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PedidoDTO>>> GetPedidos()
    {
        var pedidos = await _context.Pedidos
            .Include(p => p.PedidoProdutos)
            .Select(p => new PedidoDTO
            {
                Id = p.Id,
                ClienteId = p.ClienteId,
                Status = p.Status,
                PedidoProdutos = p.PedidoProdutos.Select(pp => new PedidoProdutoDTO
                {
                    PedidoId = pp.PedidoId,
                    ProdutoId = pp.ProdutoId,
                    Quantidade = pp.Quantidade
                }).ToList()
            })
            .ToListAsync();

        return Ok(pedidos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PedidoDTO>> GetPedido(int id)
    {
        var pedido = await _context.Pedidos
            .Include(p => p.PedidoProdutos)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pedido == null)
            return NotFound();

        var pedidoDTO = new PedidoDTO
        {
            Id = pedido.Id,
            ClienteId = pedido.ClienteId,
            Status = pedido.Status,
            PedidoProdutos = pedido.PedidoProdutos.Select(pp => new PedidoProdutoDTO
            {
                PedidoId = pp.PedidoId,
                ProdutoId = pp.ProdutoId,
                Quantidade = pp.Quantidade
            }).ToList()
        };

        return Ok(pedidoDTO);
    }

    [HttpPost]
    public async Task<ActionResult<PedidoDTO>> CreatePedido(PedidoCreateDTO pedidoCreateDTO)
    {
        var pedido = new Pedido
        {
            ClienteId = pedidoCreateDTO.ClienteId,
            Status = pedidoCreateDTO.Status,
            PedidoProdutos = pedidoCreateDTO.PedidoProdutos.Select(pp => new PedidoProduto
            {
                ProdutoId = pp.ProdutoId,
                Quantidade = pp.Quantidade
            }).ToList()
        };

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        var pedidoDTO = new PedidoDTO
        {
            Id = pedido.Id,
            ClienteId = pedido.ClienteId,
            Status = pedido.Status,
            PedidoProdutos = pedido.PedidoProdutos.Select(pp => new PedidoProdutoDTO
            {
                PedidoId = pp.PedidoId,
                ProdutoId = pp.ProdutoId,
                Quantidade = pp.Quantidade
            }).ToList()
        };

        return CreatedAtAction(nameof(GetPedido), new { id = pedido.Id }, pedidoDTO);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePedido(int id, PedidoCreateDTO pedidoUpdateDTO)
    {
        var pedido = await _context.Pedidos.Include(p => p.PedidoProdutos).FirstOrDefaultAsync(p => p.Id == id);
        if (pedido == null)
            return NotFound();

        pedido.ClienteId = pedidoUpdateDTO.ClienteId;
        pedido.Status = pedidoUpdateDTO.Status;
        pedido.PedidoProdutos = pedidoUpdateDTO.PedidoProdutos.Select(pp => new PedidoProduto
        {
            ProdutoId = pp.ProdutoId,
            Quantidade = pp.Quantidade
        }).ToList();

        _context.Entry(pedido).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePedido(int id)
    {
        var pedido = await _context.Pedidos.FindAsync(id);
        if (pedido == null)
            return NotFound();

        _context.Pedidos.Remove(pedido);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
