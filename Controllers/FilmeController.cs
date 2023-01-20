using AutoMapper;
using FilmeAPI.Data;
using FilmeAPI.Data.Dtos;
using FilmeAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmeAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    private FilmeContext _context;
    private IMapper _mapper;

    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="filmeDto">Objeto com os campos necessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public  IActionResult AdicionaFilme([FromBody]CreateFilmeDto filmeDto)
    {
        Filme filme = _mapper.Map<Filme>(filmeDto);
 
        _context.Filmes.Add(filme);
        _context.SaveChanges();
        //Qual é a ação que criou esse recurso?RecuperarFilmsPorId(), Objecto gerado, Objecto tratado...
        return CreatedAtAction(nameof(RecuperaFilmesPorId), new { Id = filme.Id }, filme);
    }

    /// <summary>
    /// Lista todos os filmes adicionados no banco de dados
    /// </summary>
    /// <param name="skip">variável que recebe o indice de inicio da páginação</param>
    /// <param name="take">variável que recebe a quantidade de filmes por página</param>
    /// <returns>IEnumerable</returns>
    /// <response code="200">Caso a listagem seja feita com sucesso</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IEnumerable<ReadFilmeDto> RecuperaFilmes([FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        //Skip e take para páginação do conteúdo...skip= apartir do id...take=quantidade de elementos;
        return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take));
    }

    /// <summary>
    /// Lista um filme do banco de dados
    /// </summary>
    /// <param name="id">Identificador único</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso a listagem seja feita com sucesso</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult RecuperaFilmesPorId(int id)
    {
        Filme filme =  _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if(filme != null)
        {
            ReadFilmeDto filmeDto = _mapper.Map<ReadFilmeDto>(filme);
            return Ok(filmeDto);
        }
        return NotFound();
    }

    /// <summary>
    /// Atualiza um filme do banco de dados
    /// </summary>
    /// <param name="id">Identificdor único</param>
    /// <param name="filmeDto">Objeto com os campos necessários para atualização de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso a atualização seja feita com sucesso</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult AtualizaFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if(filme == null)
        {
            return NotFound();

        }
        _mapper.Map(filmeDto, filme);
        _context.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Atualiza parcialmente um filme do banco de dados
    /// </summary>
    /// <param name="id">Identificador único</param>
    /// <param name="patch">Objeto com os campos necessários para a atualização parcial e um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso a atualização seja feita com sucesso</response>
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult AtualizaFilmeParcial(int id, JsonPatchDocument<UpdateFilmeDto> patch)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();

        var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);
        //Aplicando as mudanças solicitadas pelo patch:
        patch.ApplyTo(filmeParaAtualizar, ModelState);
        //Como não temos como utilizar as validaçóes do DataAnotaccion, devemos verificar na mão se
        //o mesmo é válido...
        //Se o filme não for valido:
        if(!TryValidateModel(filmeParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }
        //se for valido:
        _mapper.Map(filmeParaAtualizar, filme);
        _context.SaveChanges();
        return NoContent();
    }


    /// <summary>
    /// Remove um filme do banco de dados
    /// </summary>
    /// <param name="id">Identificador único</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso a remoção seja feita com sucesso</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult DeletaFilme(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();       
       
        _context.Remove(filme);
        _context.SaveChanges();
        return NoContent();

    }
}
