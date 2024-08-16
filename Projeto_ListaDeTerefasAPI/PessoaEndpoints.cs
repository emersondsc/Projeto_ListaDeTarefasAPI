using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Projeto_ListaDeTerefasAPI.Data;
using Projeto_ListaDeTerefasAPI.Modelos;
namespace Projeto_ListaDeTarefasAPI;

public static class PessoaEndpoints
{
    public static void MapPessoaEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Pessoa").WithTags(nameof(Pessoa));

        group.MapGet("/", async (Projeto_ListaDeTarefasAPIContext db) =>
        {
            return await db.Pessoa.ToListAsync();
        })
        .WithName("GetAllPessoas")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Pessoa>, NotFound>> (int id, Projeto_ListaDeTarefasAPIContext db) =>
        {
            return await db.Pessoa.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Pessoa model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetPessoaById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Pessoa pessoa, Projeto_ListaDeTarefasAPIContext db) =>
        {
            var affected = await db.Pessoa
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    
                    .SetProperty(m => m.Nome, pessoa.Nome)
                    .SetProperty(m => m.Email, pessoa.Email)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdatePessoa")
        .WithOpenApi();

        group.MapPost("/", async (Pessoa pessoa, Projeto_ListaDeTarefasAPIContext db) =>
        {
            db.Pessoa.Add(pessoa);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Pessoa/{pessoa.Id}",pessoa);
        })
        .WithName("CreatePessoa")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, Projeto_ListaDeTarefasAPIContext db) =>
        {
            //para deletar uma pessoa é necessário deletar as tarefas dela antes
            var affectedTarefas = await db.Tarefa
               .Where(model => model.Pessoa.Id == id)
               .ExecuteDeleteAsync();

            var affectedPessoa = await db.Pessoa
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affectedPessoa == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeletePessoa")
        .WithOpenApi();
    }
}
