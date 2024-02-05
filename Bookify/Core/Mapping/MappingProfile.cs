﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookify.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Category
            CreateMap<Category, CategoryViewModel>();
            CreateMap<CategoryFormViewModel, Category>().ReverseMap();
			CreateMap<Category, SelectListItem>()
                .ForMember(dest=>dest.Value,opt=>opt.MapFrom(src=>src.id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));



			//Author
			CreateMap<Author, AuthorViewModel>();
            CreateMap<AuthorFormViewModel, Author>().ReverseMap();
            CreateMap<Author, SelectListItem>()
				.ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.id))
				.ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));
			//Book
			CreateMap<BookFormViewModel, Book>().ReverseMap()
                .ForMember(dest => dest.Catgegories, opt => opt.Ignore());
			CreateMap<Book, BookViewModel>()
				.ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author!.Name))
            	.ForMember(dest => dest.Catgegories, opt => opt.MapFrom(src => src.Catgegories!.Select(x=>x.Category!.Name).ToList()));


		}
	}
}
