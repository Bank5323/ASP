using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcTodoes.Models;

    public class MvcTodoesContext : DbContext
    {
        public MvcTodoesContext (DbContextOptions<MvcTodoesContext> options)
            : base(options)
        {
        }

        public DbSet<MvcTodoes.Models.Todoes> Todoes { get; set; }
    }
