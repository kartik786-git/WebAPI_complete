﻿using AutoMapper;
using WebAPI.Entity;
using WebAPI.VeiwModel;

namespace WebAPI.MapperProfile
{
    public class YourMappingProfile : Profile
    {

        public YourMappingProfile()
        {
            //CreateMap<enity, Dto>();
            CreateMap<Product, ProductRequest>().ReverseMap();
            CreateMap<Order, OrderRequest>().ReverseMap();

            CreateMap<Blog,BlogPostReqeust>().ReverseMap();
            CreateMap<Post, PostRequest>().ReverseMap();
            CreateMap<Post,PostResponse>().ReverseMap();
        }
    }
}
