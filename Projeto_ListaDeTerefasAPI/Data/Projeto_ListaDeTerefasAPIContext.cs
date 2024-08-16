using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Projeto_ListaDeTerefasAPI.Modelos;

namespace Projeto_ListaDeTerefasAPI.Data
{
    public class Projeto_ListaDeTarefasAPIContext : DbContext
    {
        public Projeto_ListaDeTarefasAPIContext(DbContextOptions<Projeto_ListaDeTarefasAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Projeto_ListaDeTerefasAPI.Modelos.Tarefa> Tarefa { get; set; } = default!;
        public DbSet<Projeto_ListaDeTerefasAPI.Modelos.Pessoa> Pessoa { get; set; } = default!;
    }


}
