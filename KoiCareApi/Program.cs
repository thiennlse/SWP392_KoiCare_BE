using BusinessObject.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Npgsql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using Validation.Product;
using Azure.AI.OpenAI;
using OpenAI;

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
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    // Configure cookie options here, e.g., login path, etc.
    options.LoginPath = "/api/Auth/GoogleLogin"; // Set this to your actual login path if needed
})
.AddGoogle(googleOptions =>
{
    IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
    googleOptions.ClientId = googleAuthNSection["ClientId"];
    googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
    googleOptions.CallbackPath = new PathString("/api/Auth/external-login-callback");
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"], // Ensure this is set in your config
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // Ensure this is set in your config
    };
});


#endregion
// Add services to the container.
#region Add DBContext, SQL, Cloundinary
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.Configure<CloudinarySetting>(builder.Configuration.GetSection("CloudinarySetting"));
builder.Services.AddDbContext<KoiCareDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("KoiCareDB")));

#endregion

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

#region GenerativeAI
var generativeApiKey = configuration["GenerativeAI:ApiKey"] ?? throw new Exception("Cannot find Generative AI API Key");
#endregion

#region CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000", "https://koicare.vercel.app", "https://localhost:5001");
                          policy.AllowAnyMethod();
                          policy.AllowAnyHeader();
                      });
});
#endregion

#region Add Scope
builder.Services.Configure<CloudinarySetting>(builder.Configuration.GetSection("CloudinarySetting"));
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
builder.Services.AddScoped<IUploadImage, UploadImage>();
builder.Services.AddScoped<IWaterService, WaterService>();
builder.Services.AddScoped<IWaterRepository, WaterRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IGeminiService, GeminiService>();
builder.Services.AddScoped<ISubcriptionRepository,SubcriptionRepository>();
builder.Services.AddScoped<ISubcriptionService, SubcriptionService>();
builder.Services.AddScoped<BlogValidation>();
builder.Services.AddScoped<FishValidation>();
builder.Services.AddScoped<FoodValidation>();
builder.Services.AddScoped<MemberValidation>();
builder.Services.AddScoped<OrderValidation>();
builder.Services.AddScoped<ProductValidation>();
#endregion

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
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
app.UseAuthentication();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
