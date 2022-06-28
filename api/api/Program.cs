using api.DataModels;
using api.Repositories;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<StudentAdminContext>(opts =>
opts.UseSqlServer(builder.Configuration.GetConnectionString("StudentsConn")));

builder.Services.AddScoped<IStudentRepository,StudentRepository>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddCors((opts) => {
    opts.AddPolicy("angularApp", (crosPolicyBuilder) =>
    {
        crosPolicyBuilder.WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .WithMethods("GET","POST","PUT","DELETE")
        .WithExposedHeaders("*");
    });
});

builder.Services.AddScoped<IImageRepository, LocalStorageImageRepository>();
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Resources")),
    RequestPath = "/Resources"
});

app.UseCors("angularApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
