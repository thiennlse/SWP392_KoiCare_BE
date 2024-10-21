using BusinessObject.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Net.payOS;
using Repository;
using Repository.Interface;
using Service;
using Service.Interface;
using System.Text;
using Validation.Blog;
using Validation.Fish;
using Validation.Food;
using Validation.Member;
using Validation.Order;

var builder = WebApplication.CreateBuilder(args);

// Add configuration settings for PayOS

IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
#region PayOS
PayOS payOS = new PayOS(configuration["Environment:PAYOS_CLIENT_ID"] ?? throw new Exception("can not find Environment"),
                        configuration["Environment:PAYOS_API_KEY"] ?? throw new Exception("can not find Environment"),
                        configuration["Environment:PAYOS_CHECKSUM_KEY"] ?? throw new Exception("can not find Environment")
                        );
#endregion

#region JWT
var jwtIssuer = builder.Configuration.GetSection("JWT:Issuer").Get<string>();
var jwtAudience = builder.Configuration.GetSection("JWT:Audience").Get<string>();
var jwtKey = builder.Configuration.GetSection("JWT:Key").Get<string>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = jwtIssuer,
         ValidAudience = jwtIssuer,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
     };
 });
#endregion
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
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.Configure<CloudinarySetting>(builder.Configuration.GetSection("CloudinarySetting"));
builder.Services.AddDbContext<KoiCareDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("KoiCareDB")));
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.IgnoreNullValues = true;
    });
builder.Services.AddSingleton(payOS);// add singleton
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
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<BlogValidation>();
builder.Services.AddScoped<FishValidation>();
builder.Services.AddScoped<FoodValidation>();
builder.Services.AddScoped<MemberValidation>();
builder.Services.AddScoped<OrderValidation>();
#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Please enter your token with this format: ''Bearer YOUR_TOKEN''",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Name = "Bearer",
                In = ParameterLocation.Header,
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
