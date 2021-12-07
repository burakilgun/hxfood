using FluentValidation;
using FluentValidation.AspNetCore;
using HxFood.Api.Infrastructure.Filters;
using HxFood.Api.Models.Options;
using HxFood.Api.Models.Requests;
using HxFood.Api.Models.Requests.Category;
using HxFood.Api.Models.Requests.Product;
using HxFood.Api.Services.Abstract;
using HxFood.Api.Services.Concrete;
using HxFood.Api.Services.Mapping;
using HxFood.Api.Services.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(typeof(GlobalExceptionFilter));
    opt.Filters.Add(typeof(ApiValidationFilter));
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddMvcCore().AddFluentValidation();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = "localhost:6379";
});

builder.Services.AddAutoMapper(opt =>
{
    opt.AddProfile(typeof(GeneralMapping));
});

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));


builder.Services.AddScoped<IProductService, ProductManager>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();

builder.Services.AddTransient<IValidator<ProductAddRequest>, ProductAddRequestValidator>();
builder.Services.AddTransient<IValidator<ProductUpdateRequest>, ProductUpdateRequestValidator>();
builder.Services.AddTransient<IValidator<CategoryAddRequest>, CategoryAddRequestValidator>();
builder.Services.AddTransient<IValidator<CategoryUpdateRequest>, CategoryUpdateRequestValidator>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();