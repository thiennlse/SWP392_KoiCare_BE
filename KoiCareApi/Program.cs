using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Repository;
using Repository.Interface;
using Service;
using Validation_Handler.BlogValidation;
using Validation_Handler.FishValidation;
using Validation_Handler.FoodValidation;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000");
                      });
});
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.Configure<CloudinarySetting>(builder.Configuration.GetSection("CloudinarySetting"));
builder.Services.AddDbContext<KoiCareDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("KoiCareDB")));
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.IgnoreNullValues = true;
    });
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IFoodService, FoodService>();
builder.Services.AddScoped<IFoodRepository, FoodRepository>();
builder.Services.AddScoped<IFishService, FishService>();
builder.Services.AddScoped<IFishRepository, FishRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IPoolService, PoolService>();
builder.Services.AddScoped<IPoolRepository, PoolRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IWaterService, WaterService>();
builder.Services.AddScoped<IWaterRepository, WaterRepository>();
builder.Services.AddScoped<IUploadImage,UploadImage>();
builder.Services.AddScoped<BlogValidation>();
builder.Services.AddScoped<FishValidation>();
builder.Services.AddScoped<FoodValidation>();

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
