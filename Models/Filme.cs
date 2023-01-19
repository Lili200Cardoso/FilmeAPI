using System.ComponentModel.DataAnnotations;

namespace FilmeAPI.Models;

public class Filme
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required(ErrorMessage = "O campo título é obrigatório.")]
    public string Titulo { get; set; }
    [Required(ErrorMessage = "O campo diretor é obrigatório.")]
    public string Diretor { get; set; }
    [MaxLength(30, ErrorMessage = "O Gênero não deve passar de 30 caracteres.")]
    public string Genero { get; set; }
    [Range(70,600,ErrorMessage = "A duração deve ter entre 70 e 600 minutos.")]
    public int Duracao { get; set; }
}
