using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcTodoes.Models;

// To Iden and Authiriz
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace TodoListWeb.Controllers
{
    [Authorize]
    public class TodoesController : Controller
    {
        private readonly MvcTodoesContext _context;

        public TodoesController(MvcTodoesContext context)
        {
            _context = context;
        }

        private string getUserId(){
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                // the principal identity is a claims identity.
                // now we need to find the NameIdentifier claim
                var userIdClaim = claimsIdentity.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    return userIdClaim.Value;
                }
            }
            return null;
        }

        // GET: Todoes
        public async Task<IActionResult> Index(string searchString)
        {
            string _Iduser = getUserId();

            ViewData["CurrentFilter"] = searchString;
            var todoes = from s in _context.Todoes select s;

            todoes = todoes.Where(s => s.IdUser == _Iduser);

            // to filter todo list is not check
            todoes = todoes.Where(s => s.Check == false);

            if (!String.IsNullOrEmpty(searchString))
            {
                todoes = todoes.Where(s => s.Title.Contains(searchString));
            }

            return View(await todoes.ToListAsync());
        }

        // GET: Todoes/Done
        public async Task<IActionResult> Done(string searchString)
        {
            string _Iduser = getUserId();

            ViewData["CurrentFilter"] = searchString;
            var todoes = from s in _context.Todoes select s;

            todoes = todoes.Where(s => s.IdUser == _Iduser);

            // to filter todo list is check
            todoes = todoes.Where(s => s.Check == true);

            if (!String.IsNullOrEmpty(searchString))
            {
                todoes = todoes.Where(s => s.Title.Contains(searchString));
            }

            return View(await todoes.ToListAsync());
        }

        // GET: Todoes/ChangeStatus/?id
        public async Task<IActionResult> ChangeStatus(int? id,bool pageIndex)
        {
            // quarry data from id
            if (id == null)
            {
                return NotFound();
            }
            var todoes = await _context.Todoes.FindAsync(id);
            if (todoes == null)
            {
                return NotFound();
            }

            // Update value
            if (ModelState.IsValid)
            {
                try
                {
                    // To change Check value
                    todoes.Check = !todoes.Check ;
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
                if (pageIndex)
                {
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Done));
            }
            return NotFound();
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

        // POST: Todoes/Create/Title
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShotCreate(string Title)
        {
            string _Iduser = getUserId();

            if (Title == null){
                return NotFound();
            }
            Todoes todoes = new Todoes();
            todoes.Title = Title;
            todoes.IdUser = _Iduser;
            _context.Add(todoes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Create([Bind("Id,Title,Check,IdUser")] Todoes todoes)
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Check,IdUser")] Todoes todoes)
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
