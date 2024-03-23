using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221_2.Models;

namespace ProjectPRN221_2.Controllers
{
    public class BookingsController : Controller
    {
        private readonly TestProjectPRN221Context _context;

        public BookingsController(TestProjectPRN221Context context)
        {
            _context = context;
        }

        // GET: Bookings
        //public async Task<IActionResult> Index(string status)
        //{
        //    IQueryable<Booking> query = _context.Bookings;
        //    if (!string.IsNullOrEmpty(status))
        //    {
        //        query = query.Where(b => b.Status == status);
        //    }
        //    var statusBooking = await query.Include(b => b.Field).Include(b => b.TimeSlot).ToListAsync();

        //    var allStatus = await _context.Bookings.Select(b => b.Status).Distinct().ToListAsync();
        //    ViewBag.AllStatus = allStatus;

        //    return View(statusBooking);

        //}
        public async Task<IActionResult> Index(string status, int? page)
        {
            IQueryable<Booking> query = _context.Bookings;
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(b => b.Status == status);
            }
            var statusBooking = await query.Include(b => b.Field).Include(b => b.TimeSlot).ToListAsync();
            var allStatus = await _context.Bookings.Select(b => b.Status).Distinct().ToListAsync();
            ViewBag.AllStatus = allStatus;
            ViewBag.statusBooking = statusBooking;

            int pageNumber = page ?? 1; // Số trang mặc định nếu không được cung cấp
            int pageSize = 10; // Số lượng mục trên mỗi trang

            var bookings = await query.Include(b => b.Field)
                                      .Include(b => b.TimeSlot)
                                      .OrderBy(b => b.BookingId)
                                      .ToListAsync();

            var pagedBookings = bookings.Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();

            return View(pagedBookings);
        }


        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Field)
                .Include(b => b.TimeSlot)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            //ViewData["FieldId"] = new SelectList(_context.FootballFields, "FieldId", "FieldId");
            //ViewData["TimeSlotId"] = new SelectList(_context.TimeSlots, "TimeSlotId", "TimeSlotId");

            ViewData["FieldName"] = new SelectList(_context.FootballFields, "FieldName", "FieldName");
            ViewData["Slot"] = new SelectList(_context.TimeSlots, "Slot", "Slot");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,CustomerName,Date,Status,CustomerPhone,Message")] Booking booking)
        {
            string fieldName = Request.Form["FieldName"];
            string SlotName = Request.Form["Slot"];


            var field = _context.FootballFields.FirstOrDefault(f => f.FieldName.Equals(fieldName));
            var slot = _context.TimeSlots.FirstOrDefault(f => f.Slot.Equals(SlotName));
            if (ModelState.IsValid)
            {

                // Kiểm tra xem có trùng lặp không
                var existingBooking = await _context.Bookings.FirstOrDefaultAsync(b => b.FieldId == field.FieldId && b.TimeSlotId == slot.TimeSlotId && b.Date == booking.Date);
                if (existingBooking != null)
                {
                    // Nếu đã tồn tại một booking với field, time slot và ngày giống nhau
                    // Kiểm tra status của booking hiện tại
                    if (existingBooking.Status == "1")
                    {
                        // Nếu status của booking hiện tại là 1, không cho phép tạo thêm booking mới
                        ModelState.AddModelError("", "You cannot register for this football field because someone else has already registered for that football field.");
                        ViewData["FieldName"] = new SelectList(_context.FootballFields, "FieldName", "FieldName");
                        ViewData["Slot"] = new SelectList(_context.TimeSlots, "Slot", "Slot");
                        return View(booking);
                    }
                    // Nếu status của booking hiện tại không phải là 1, tiếp tục tạo booking mới
                }
                booking.TimeSlotId = slot.TimeSlotId;
                booking.FieldId = field.FieldId;
                _context.Add(booking);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Your soccer field order has been created and is awaiting approval. We will contact you via the following phone number";
                return RedirectToAction("test", "FootballFields");
            }
        

            ViewData["FieldName"] = new SelectList(_context.FootballFields, "FieldName", "FieldName");
            ViewData["Slot"] = new SelectList(_context.TimeSlots, "Slot", "Slot");

            return View(booking);

        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["FieldId"] = new SelectList(_context.FootballFields, "FieldId", "FieldId", booking.FieldId);
            ViewData["TimeSlotId"] = new SelectList(_context.TimeSlots, "TimeSlotId", "TimeSlotId", booking.TimeSlotId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,CustomerName,FieldId,TimeSlotId,Date,Status,CustomerPhone,Message")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
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
            ViewData["FieldId"] = new SelectList(_context.FootballFields, "FieldId", "FieldId", booking.FieldId);
            ViewData["TimeSlotId"] = new SelectList(_context.TimeSlots, "TimeSlotId", "TimeSlotId", booking.TimeSlotId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Field)
                .Include(b => b.TimeSlot)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bookings == null)
            {
                return Problem("Entity set 'TestProjectPRN221Context.Bookings'  is null.");
            }
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
          return (_context.Bookings?.Any(e => e.BookingId == id)).GetValueOrDefault();
        }


        [HttpPost]
        public IActionResult UpdateStatus(int bookingId, string newStatus)
        {
            var booking = _context.Bookings.Find(bookingId);
            if (booking == null)
            {
                return NotFound();
            }

            booking.Status = newStatus;
            _context.SaveChanges();

            return Ok();
        }

    }
}
