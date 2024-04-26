﻿using AutoMapper;
using AutoMapper.Execution;
using Microsoft.Extensions.Configuration;
using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.Theproducts.Dots
{
    public class ProductUrlResolver : IValueResolver<Product, ProductDetailsDto, string>
    {
        private readonly IConfiguration _configuration;

        public ProductUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(Product source, ProductDetailsDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ImageUrl))
                return $"{_configuration["BaseUrl"]} {source.ImageUrl}";

            return null;
        }
    }
}