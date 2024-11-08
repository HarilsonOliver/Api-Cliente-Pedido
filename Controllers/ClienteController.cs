using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("v1/clientes")]
public class ClienteController : ControllerBase
{
    private readonly ApiDbContext _context;

    public ClienteController(ApiDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetClientes()
    {
        var clientes = await _context.Clientes
            .Select(c => new ClienteDTO
            {
                Id = c.Id,
                Nome = c.Nome,
                Email = c.Email,
                NumeroContato = c.NumeroContato,
                DataNascimento = c.DataNascimento
            })
            .ToListAsync();

        return Ok(clientes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClienteDTO>> GetCliente(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente == null)
            return NotFound();

        var clienteDTO = new ClienteDTO
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Email = cliente.Email,
            NumeroContato = cliente.NumeroContato,
            DataNascimento = cliente.DataNascimento
        };

        return Ok(clienteDTO);
    }

    [HttpPost]
    public async Task<ActionResult<ClienteDTO>> CreateCliente(ClienteCreateDTO clienteCreateDTO)
    {
        var cliente = new Cliente
        {
            Nome = clienteCreateDTO.Nome,
            Email = clienteCreateDTO.Email,
            NumeroContato = clienteCreateDTO.NumeroContato,
            DataNascimento = clienteCreateDTO.DataNascimento
        };

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();

        var clienteDTO = new ClienteDTO
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Email = cliente.Email,
            NumeroContato = cliente.NumeroContato,
            DataNascimento = cliente.DataNascimento
        };

        return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, clienteDTO);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCliente(int id, ClienteCreateDTO clienteUpdateDTO)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null)
            return NotFound();

        cliente.Nome = clienteUpdateDTO.Nome;
        cliente.Email = clienteUpdateDTO.Email;
        cliente.NumeroContato = clienteUpdateDTO.NumeroContato;
        cliente.DataNascimento = clienteUpdateDTO.DataNascimento;

        _context.Entry(cliente).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCliente(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null)
            return NotFound();

        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
