using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Projeto_ListaDeTerefasAPI.Data;
using Projeto_ListaDeTerefasAPI;
using Projeto_ListaDeTarefasAPI;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Projeto_ListaDeTarefasAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Projeto_ListaDeTerefasAPIContext") ?? throw new InvalidOperationException("Connection string 'Projeto_ListaDeTerefasAPIContext' not found.")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapTarefaEndpoints();

app.MapPessoaEndpoints();


app.Run();

