using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;



namespace LibraryManagementSystem.Controllers
{
    [Authorize]// Wymaga zalogowania
    public class LoansController : Controller
    {
        private readonly LibraryDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public LoansController(LibraryDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Loans
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User); // Pobierz ID zalogowanego użytkownika
            var loans = await _context.Loans
                .Include(l => l.Book)
                .Where(l => l.UserId == userId)
                .ToListAsync();
            return View(loans);
        }

        // GET: Loans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans
                .Include(l => l.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        // GET: Loans/Create
        public async Task<IActionResult> Create(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null || book.AvailableCopies <= 0)
            {
                return NotFound("Książka niedostępna!");
            }

            var loan = new Loan
            {
                BookId = bookId,
                LoanDate = System.DateTime.Now,
                ReturnDate = System.DateTime.Now.AddDays(14), // Domyślny okres wypożyczenia: 14 dni
                Status = "Wypożyczona"
            };

            return View(loan);
        }

        // POST: Loans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,LoanDate,ReturnDate,Status")] Loan loan) //Create(Loan loan)
        {
            var userId = _userManager.GetUserId(User); // Pobierz ID zalogowanego użytkownika
            loan.UserId = userId;

            var book = await _context.Books.FindAsync(loan.BookId);
            if (book == null || book.AvailableCopies <= 0)
            {
                return BadRequest("Brak dostępnych egzemplarzy tej książki.");
            }

            // Zmniejsz liczbę dostępnych egzemplarzy
            book.AvailableCopies -= 1;

            loan.Book = book;
            loan.Status = "Wypożyczona";

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Books"); //return RedirectToAction(nameof(Index));
        }

        // GET: Loans/Extend/5 (Przedłużenie wypożyczenia)
        public async Task<IActionResult> Extend(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan == null || loan.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            loan.ReturnDate = loan.ReturnDate.AddDays(7); // Przedłużenie o 7 dni
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Loans/Return/5 (Zwrot książki)
        public async Task<IActionResult> Return(int id)
        {
            var loan = await _context.Loans.Include(l => l.Book).FirstOrDefaultAsync(l => l.Id == id);
            if (loan == null || loan.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            // Zwiększ liczbę dostępnych egzemplarzy książki
            loan.Book.AvailableCopies += 1;
            loan.ReturnDate = System.DateTime.Now;

            loan.Status = "Zwrócona";
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Loans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Author", loan.BookId);
            return View(loan);
        }

        // POST: Loans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookId,UserId,LoanDate,ReturnDate,Status")] Loan loan)
        {
            if (id != loan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanExists(loan.Id))
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
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Author", loan.BookId);
            return View(loan);
        }

        // GET: Loans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans
                .Include(l => l.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        // POST: Loans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan != null)
            {
                _context.Loans.Remove(loan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoanExists(int id)
        {
            return _context.Loans.Any(e => e.Id == id);
        }
    }
}
