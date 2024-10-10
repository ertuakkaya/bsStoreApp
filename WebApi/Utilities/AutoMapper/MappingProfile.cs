using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace WebApi.Utilities.AutoMapper
{


    // AutoMapper 'ın kullanımı için MappingProfile sınıfını oluşturuyoruz.
    public class MappingProfile : Profile
    {

        // MappingProfile sınıfının constructor'ında map işlemlerini yapıyoruz.
        public MappingProfile()
        {
            // BookDtoForUpdate sınıfını Book sınıfına map ediyoruz.
            CreateMap<BookDtoForUpdate, Book>().ReverseMap();

            CreateMap<Book, BookDto>();

            CreateMap<BookDtoForInsertion, Book>();

            CreateMap<UserForRegistrationDto, User>();
        }
         



    }
}
