using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcTodoes.Models;

namespace TodoListWeb.Controllers
{
    public class TodoesController : Controller
    {
        private readonly MvcTodoesContext _context;

        public TodoesController(MvcTodoesContext context)
        {
            _context = context;
        }

        // GET: Todoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Todoes.ToListAsync());
        }

        // GET: Todoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoes = await _context.Todoes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todoes == null)
            {
                return NotFound();
            }

            return View(todoes);
        }

        // GET: Todoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Todoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Check")] Todoes todoes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(todoes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(todoes);
        }

        // GET: Todoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoes = await _context.Todoes.FindAsync(id);
            if (todoes == null)
            {
                return NotFound();
            }
            return View(todoes);
        }

        // POST: Todoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Check")] Todoes todoes)
        {
            if (id != todoes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(todoes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoesExists(todoes.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(todoes);
        }

        // GET: Todoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoes = await _context.Todoes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todoes == null)
            {
                return NotFound();
            }

            return View(todoes);
        }

        // POST: Todoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var todoes = await _context.Todoes.FindAsync(id);
            _context.Todoes.Remove(todoes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TodoesExists(int id)
        {
            return _context.Todoes.Any(e => e.Id == id);
        }
    }
}
