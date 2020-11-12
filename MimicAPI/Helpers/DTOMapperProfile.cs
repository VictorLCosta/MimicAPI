using AutoMapper;
using MimicAPI.V1.Models;
using MimicAPI.V1.Models.DTO;

namespace MimicAPI.Helpers
{
    public class DTOMapperProfile : Profile
    {
        public DTOMapperProfile()
        {
            CreateMap<Word, DTOWord>();
            CreateMap<PaginationList<Word>, PaginationList<DTOWord>>();
        }
    }
}