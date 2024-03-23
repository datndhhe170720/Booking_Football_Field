using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_2.Models;
using PagedList;

namespace ProjectPRN221.Controllers
{
    public class FootballFieldsController : Controller
    {
        private readonly TestProjectPRN221Context _context;

        public FootballFieldsController(TestProjectPRN221Context context)
        {
            _context = context;
        }

        // GET: FootballFields
        public async Task<IActionResult> Index()
        {
              return _context.FootballFields != null ? 
                          View(await _context.FootballFields.ToListAsync()) :
                          Problem("Entity set 'TestProjectPRN221Context.FootballFields'  is null.");
        }

        public async Task<IActionResult> Test(string? size, string? search, int? page)
        {
            // Lấy thông báo từ TempData nếu có
            var successMessage = TempData["SuccessMessage"] as string;

            // Truyền thông báo thành công vào view thông qua ViewBag hoặc ViewData
            ViewBag.SuccessMessage = successMessage;

            IQueryable<FootballField> query = _context.FootballFields;

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.FieldName.ToLower().Contains(search.ToLower()));
            }

            if (!string.IsNullOrEmpty(size))
            {
                query = query.Where(field => field.Size == size);
            }

            int pageSize = 3;
            int pageNumber = page ?? 1;

            // Tính toán số trang và dữ liệu cần hiển thị trên trang hiện tại
            var totalItems = await query.CountAsync();
            var footballFields = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            // Lấy danh sách các kích thước đầy đủ từ cơ sở dữ liệu
            var allSizes = await _context.FootballFields.Select(field => field.Size).Distinct().ToListAsync();

            // Gán danh sách các kích thước và dữ liệu phân trang vào ViewBag
            ViewBag.AllSizes = allSizes;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItemCount = totalItems;
            ViewBag.TotalPageCount = (int)Math.Ceiling((double)totalItems / pageSize);

            return View(footballFields);
        }

        //public async Task<IActionResult> Test(string? size, string? search ,int? page)
        //{
        //    // Lấy thông báo từ TempData nếu có
        //    var successMessage = TempData["SuccessMessage"] as string;

        //    // Truyền thông báo thành công vào view thông qua ViewBag hoặc ViewData
        //    ViewBag.SuccessMessage = successMessage;

        //    IQueryable<FootballField> query = _context.FootballFields;

        //    if (!string.IsNullOrEmpty(search))
        //    {
        //        query = query.Where(s => s.FieldName.ToLower().Contains(search.ToLower()));
        //    }

        //    if (!string.IsNullOrEmpty(size))
        //    {
        //        query = query.Where(field => field.Size == size);
        //    }

        //    var footballFields = await query.ToListAsync();

        //    // Lấy danh sách các kích thước đầy đủ từ cơ sở dữ liệu
        //    var allSizes = await _context.FootballFields.Select(field => field.Size).Distinct().ToListAsync();

        //    // Gán danh sách các kích thước vào ViewBag
        //    ViewBag.AllSizes = allSizes;


        //    if (page == null) page = 1;
        //    var pages = (from l in _context.FootballFields
        //                 select l).OrderBy(x => x.FieldId);
        //    int pageSize = 3;
        //    int pageNumber = (page ?? 1);
        //    return View(pages.ToPagedList(pageNumber, pageSize));


        //    return View(footballFields);
        //}

        // GET: FootballFields/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FootballFields == null)
            {
                return NotFound();
            }

            var footballField = await _context.FootballFields
                .FirstOrDefaultAsync(m => m.FieldId == id);
            if (footballField == null)
            {
                return NotFound();
            }

            return View(footballField);
        }

        // GET: FootballFields/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FootballFields/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FieldId,FieldName,Address,Size,Image,Price")] FootballField footballField)
        {
            if (ModelState.IsValid)
            {
                _context.Add(footballField);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(footballField);
        }

        // GET: FootballFields/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FootballFields == null)
            {
                return NotFound();
            }

            var footballField = await _context.FootballFields.FindAsync(id);
            if (footballField == null)
            {
                return NotFound();
            }
            return View(footballField);
        }

        // POST: FootballFields/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FieldId,FieldName,Address,Size,Image,Price")] FootballField footballField)
        {
            if (id != footballField.FieldId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(footballField);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FootballFieldExists(footballField.FieldId))
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
            return View(footballField);
        }

        // GET: FootballFields/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FootballFields == null)
            {
                return NotFound();
            }

            var footballField = await _context.FootballFields
                .FirstOrDefaultAsync(m => m.FieldId == id);
            if (footballField == null)
            {
                return NotFound();
            }

            return View(footballField);
        }

        // POST: FootballFields/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FootballFields == null)
            {
                return Problem("Entity set 'TestProjectPRN221Context.FootballFields'  is null.");
            }
            var footballField = await _context.FootballFields.FindAsync(id);
            if (footballField != null)
            {
                _context.FootballFields.Remove(footballField);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FootballFieldExists(int id)
        {
          return (_context.FootballFields?.Any(e => e.FieldId == id)).GetValueOrDefault();
        }
    }
}
