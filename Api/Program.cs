using System.IO.Abstractions;
using System.Text;
using Api.Middleware;
using Bussines.AutoMapper;
using Bussines.Filters.FactureFilters;
using Bussines.Filters.FactureFilters.Handlers;
using Bussines.Filters.FactureFilters.Interfaces;
using Bussines.Services;
using Bussines.Services.Interfaces;
using Data;
using Data.Models.AuthenticationModels;
using Data.Repositories;
using Data.Repositories.Interfaces;
using Interface.FiltersInterface.FactureFilterInterface;
using Interface.FiltersInterface.PagingFilterInterface;
using Interface.Logger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

builder.Services.AddDbContext<FacturesManagementContext>(options =>
options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Api")));

var tokenValidationParameters = new TokenValidationParameters()
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWT:Secret"] ?? throw new Exception("Secret Key is Null"))),

    ValidateIssuer = true,
    ValidIssuer = builder.Configuration["JWT:Issuer"] ?? throw new Exception("Issuer is Null"),

    ValidateAudience = true,
    ValidAudience = builder.Configuration["JWT:Audience"] ?? throw new Exception("Audience is Null"),

    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero,
};

builder.Services.AddSingleton(tokenValidationParameters);

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<FacturesManagementContext>()
    .AddDefaultTokenProviders();

// Add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = tokenValidationParameters;
});

builder.Services.AddScoped<IPagingOptionExtension, PagingOptionExtension>();
builder.Services.AddScoped<ISortingFactureExtension, SortingFactureExtension>();

builder.Services.AddScoped<IFacturePipelineGetHandler, FactureSortingHandler>();
builder.Services.AddScoped<IFacturePipelineGetHandler, FactureNameFilterHandler>();
builder.Services.AddScoped<IFacturePipelineGetHandler, CustomerNameFilterHandler>();
builder.Services.AddScoped<IFacturePipelineGetHandler, CreationDateFilterHandler>();
builder.Services.AddScoped<IFacturePipelineGetHandler, SaleDateFilterHandler>();
builder.Services.AddScoped<IFacturePipelineGetHandler, PaymentDateFilterHandler>();
builder.Services.AddScoped<IFacturePipelineGetHandler, FactureNumberFilterHandler>();
builder.Services.AddScoped<IFacturePipelineGetHandler, FacturePagingHandler>();

builder.Services.AddScoped<FactureSortingHandler>();
builder.Services.AddScoped<FactureNameFilterHandler>();
builder.Services.AddScoped<CustomerNameFilterHandler>();
builder.Services.AddScoped<CreationDateFilterHandler>();
builder.Services.AddScoped<SaleDateFilterHandler>();
builder.Services.AddScoped<PaymentDateFilterHandler>();
builder.Services.AddScoped<FactureNumberFilterHandler>();
builder.Services.AddScoped<FacturePagingHandler>();

builder.Services.AddScoped<IFactureFilterPipeline, FilterPipeline>(sp =>
{
    var pipeline = new FilterPipeline();

    var factureNameFilter = sp.GetRequiredService<FactureNameFilterHandler>();
    var customerNameFilter = sp.GetRequiredService<CustomerNameFilterHandler>();
    var creationDateFilter = sp.GetRequiredService<CreationDateFilterHandler>();
    var saleDateFilter = sp.GetRequiredService<SaleDateFilterHandler>();
    var paymentDateFilter = sp.GetRequiredService<PaymentDateFilterHandler>();
    var factureNumberFilter = sp.GetRequiredService<FactureNumberFilterHandler>();
    var facturePaging = sp.GetRequiredService<FacturePagingHandler>();
    var factureSorting = sp.GetRequiredService<FactureSortingHandler>();

    pipeline.AddHandler(factureSorting);
    pipeline.AddHandler(factureNameFilter);
    pipeline.AddHandler(customerNameFilter);
    pipeline.AddHandler(creationDateFilter);
    pipeline.AddHandler(saleDateFilter);
    pipeline.AddHandler(paymentDateFilter);
    pipeline.AddHandler(factureNumberFilter);
    pipeline.AddHandler(facturePaging);

    return pipeline;
});

builder.Services.AddSingleton<IFileSystem, FileSystem>();
builder.Services.AddSingleton<IFileLoggerService, FileLoggerService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IFactureRepository, FactureRepository>();
builder.Services.AddScoped<IFactureService, FactureService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPdfService, PdfService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
