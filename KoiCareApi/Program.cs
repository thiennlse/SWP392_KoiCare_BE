using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Interface;
using Service;
using Service.Interface;
using Validation.Blog;
using Validation.Fish;
using Validation.Food;
using Validation.Member;
using Validation.Order;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region Add DBContext, SQL, Cloundinary
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.Configure<CloudinarySetting>(builder.Configuration.GetSection("CloudinarySetting"));
builder.Services.AddDbContext<KoiCareDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("KoiCareDB"), sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 10,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    }));
#endregion


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

#region CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000", "http://localhost:5000", "https://localhost:5001");
                          policy.AllowAnyMethod();
                          policy.AllowAnyHeader();
                      });
});
#endregion

#region Add Scope
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IFoodService, FoodService>();
builder.Services.AddScoped<IFoodRepository, FoodRepository>();
builder.Services.AddScoped<IFishService, FishService>();
builder.Services.AddScoped<IFishRepository, FishRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IPoolRepository, PoolRepository>();
builder.Services.AddScoped<IPoolService, PoolService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUploadImage,UploadImage>();
builder.Services.AddScoped<IWaterService, WaterService>();
builder.Services.AddScoped<IWaterRepository, WaterRepository>();

builder.Services.AddScoped<BlogValidation>();
builder.Services.AddScoped<FishValidation>();
builder.Services.AddScoped<FoodValidation>();
builder.Services.AddScoped<MemberValidation>();
builder.Services.AddScoped<OrderValidation>();
#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
