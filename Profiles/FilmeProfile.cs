using AutoMapper;
using FilmeAPI.Data.Dtos;
using FilmeAPI.Models;

namespace FilmeAPI.Profiles;

public class FilmeProfile : Profile
{
    public FilmeProfile()
    {
        CreateMap<CreateFilmeDto, Filme>().ReverseMap();
        CreateMap<Filme, ReadFilmeDto>().ReverseMap(); 
        CreateMap<UpdateFilmeDto, Filme>().ReverseMap(); 
    }
}
