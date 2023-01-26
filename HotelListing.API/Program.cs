using HotelListing.API.Core.Configration;
using HotelListing.API.Core.IRepository;
using HotelListing.API.Core.Middleware;
using HotelListing.API.Core.Repository;
using HotelListing.API.Data;
using HotelListing.API.Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//config connection string db 
var connectionString = builder.Configuration.GetConnectionString("HotelListingDbConnection");
builder.Services.AddDbContext<HotelListingDbContext>(options =>
{
	options.UseSqlServer(connectionString);
});
//Add Identity User
builder.Services.AddIdentityCore<ApiUser>()
	.AddRoles<IdentityRole>()
	.AddTokenProvider<DataProtectorTokenProvider<ApiUser>>("HotelApiProviding")
	.AddEntityFrameworkStores<HotelListingDbContext>().AddDefaultTokenProviders();
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo { Title = "Hotel API", Version = "v1" });
	options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Description = "",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,
		Scheme = JwtBearerDefaults.AuthenticationScheme
	});
	options.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference=new OpenApiReference
				{
					Type=ReferenceType.SecurityScheme,
					Id=JwtBearerDefaults.AuthenticationScheme
				},
				Scheme="Oauth2",
				Name=JwtBearerDefaults.AuthenticationScheme,
				In=ParameterLocation.Header
			},
			new List<string>()
		}
	});
});

//cros config
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll",
		b => b.AllowAnyHeader()
		.AllowAnyOrigin()
		.AllowAnyMethod());
});
//config API versioning
builder.Services.AddApiVersioning(options =>
{
	options.AssumeDefaultVersionWhenUnspecified = true;
	options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
	options.ReportApiVersions = true;
	options.ApiVersionReader = ApiVersionReader.Combine(
		new QueryStringApiVersionReader("api-version"),
		new HeaderApiVersionReader("X-Version"),
		new MediaTypeApiVersionReader("ver")
		);
});
builder.Services.AddVersionedApiExplorer(options =>
{
	options.GroupNameFormat = "'v'VVV";
	options.SubstituteApiVersionInUrl = true;
});

//serilog config
builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));

//automapper config
builder.Services.AddAutoMapper(typeof(MapperConfig));

//repository injection
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IAuthManager, AuthManager>();

//Jwt configrations and authentication
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ClockSkew = TimeSpan.Zero,
		ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
		ValidAudience = builder.Configuration["JwtSettings:Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
	};
});

//config caching
builder.Services.AddResponseCaching(options =>
{
	options.MaximumBodySize = 1024;
	options.UseCaseSensitivePaths = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
//use middleware handle exception
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
//CROS config
app.UseCors("AllowAll");

//use Caching
app.UseResponseCaching();
app.Use(async (context, next) =>
{
	context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue()
	{
		Public = true,
		MaxAge = TimeSpan.FromSeconds(15),
	};
	context.Response.Headers[HeaderNames.Vary] = new string[] { "Accept-Encoding" };

	await next();
});

//use Authentication, and Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
