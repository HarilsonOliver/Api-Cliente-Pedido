using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("v1/produtos")]
public class ProdutoController : ControllerBase
{
    private readonly ApiDbContext _context;

    public ProdutoController(ApiDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutos()
    {
        var produtos = await _context.Produtos
            .Select(p => new ProdutoDTO
            {
                Id = p.Id,
                Nome = p.Nome,
                Tipo = p.Tipo,
                Valor = p.Valor
            })
            .ToListAsync();

        return Ok(produtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProdutoDTO>> GetProduto(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);

        if (produto == null)
            return NotFound();

        var produtoDTO = new ProdutoDTO
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Tipo = produto.Tipo,
            Valor = produto.Valor
        };

        return Ok(produtoDTO);
    }

    [HttpPost]
    public async Task<ActionResult<ProdutoDTO>> CreateProduto(ProdutoCreateDTO produtoCreateDTO)
    {
        var produto = new Produto
        {
            Nome = produtoCreateDTO.Nome,
            Tipo = produtoCreateDTO.Tipo,
            Valor = produtoCreateDTO.Valor
        };

        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();

        var produtoDTO = new ProdutoDTO
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Tipo = produto.Tipo,
            Valor = produto.Valor
        };

        return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produtoDTO);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduto(int id, ProdutoCreateDTO produtoUpdateDTO)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto == null)
            return NotFound();

        produto.Nome = produtoUpdateDTO.Nome;
        produto.Tipo = produtoUpdateDTO.Tipo;
        produto.Valor = produtoUpdateDTO.Valor;

        _context.Entry(produto).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduto(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto == null)
            return NotFound();

        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
