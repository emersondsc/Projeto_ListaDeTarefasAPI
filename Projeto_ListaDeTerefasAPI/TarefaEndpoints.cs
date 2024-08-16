using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Projeto_ListaDeTerefasAPI.Data;
using Projeto_ListaDeTerefasAPI.Modelos;
namespace Projeto_ListaDeTerefasAPI;

public static class TarefaEndpoints
{
    public static void MapTarefaEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/tarefa").WithTags(nameof(Tarefa));

        group.MapGet("/", async (Projeto_ListaDeTarefasAPIContext db) =>
        {
            return await db.Tarefa
                .Include(t => t.Pessoa) // Eager loading for Pessoa
                .ToListAsync();
        })
        .WithName("GetAllTarefas")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Tarefa>, NotFound>> 
            (int id, Projeto_ListaDeTarefasAPIContext db) =>
        {
            return await db.Tarefa.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Tarefa model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetTarefaById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Tarefa tarefa, Projeto_ListaDeTarefasAPIContext db) =>
        {
            var affected = await db.Tarefa
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    
                    .SetProperty(m => m.Nome, tarefa.Nome)
                    .SetProperty(m => m.Prazo, tarefa.Prazo)
                    .SetProperty(m => m.FInalizada, tarefa.FInalizada)
                    .SetProperty(m => m.Descricao, tarefa.Descricao)
                    .SetProperty(m => m.Pessoa, tarefa.Pessoa)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateTarefa")
        .WithOpenApi();
        

        group.MapPost("/", async (Tarefa tarefa, Projeto_ListaDeTarefasAPIContext db) =>
        {
             
            
            // Verifique se a pessoa já existe no contexto
            if (!await db.Pessoa.AnyAsync(p => p.Email == tarefa.Pessoa.Email))
            {
                db.Pessoa.Add(tarefa.Pessoa);
                await db.SaveChangesAsync();
                return Results.BadRequest("Pessoa não encontrada. Nova pessoa adiconada");
            }
            
            string email = tarefa.Pessoa.Email;
            var pessoa = await db.Pessoa.FirstOrDefaultAsync(p => p.Email == email);

            tarefa.Pessoa = pessoa;

            db.Tarefa.Add(tarefa);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/tarefa/{tarefa.Id}", tarefa);
        })
        .WithName("CreateTarefa")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, Projeto_ListaDeTarefasAPIContext db) =>
        {
            var affected = await db.Tarefa
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteTarefa")
        .WithOpenApi();
    }
}
