using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcBach.Data;
using MvcBach.Models;
using MvcBach.Models.Process;
using OfficeOpenXml;
using X.PagedList;

namespace MvcBach.Controllers
{
    public class HeThongPhanPhoiController : Controller
    {
        private readonly ApplicationDbContext _context;

        private ExcelProcess _excelProcess = new ExcelProcess();

        public HeThongPhanPhoiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HeThongPhanPhoi
        public async Task<IActionResult> Index (int? page, int? PageSize)
        {
            ViewBag.PageSize= new List<SelectListItem>()
            {
        
            new SelectListItem() { Value="3", Text="3" }, 
            new SelectListItem() { Value="5", Text="5" }, 
            new SelectListItem() { Value="10",Text="10" }, 
            new SelectListItem() { Value="15", Text="15" }, 
            new SelectListItem() { Value="25", Text="25" },
            new SelectListItem() { Value="50" ,Text="50" },
            };
            int pagesize = (PageSize ?? 3);
            ViewBag.psize = pagesize;
            var model = _context.HeThongPhanPhoi.ToList().ToPagedList (page ?? 1, pagesize); 
            return View(model);
        }
         // Shearch : HTPP
        public async Task<IActionResult> Shearch()
        {
            return View(await _context.HeThongPhanPhoi.ToListAsync());

        }
        [HttpPost]
         public async Task<IActionResult> Shearch( string searchTen)
        {
            
            return View(await _context.HeThongPhanPhoi.Where(m => m.MaHTPP.Contains(searchTen) || m.TenHTPP.Contains(searchTen)).ToListAsync());
        
        }

        // GET: HeThongPhanPhoi/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var heThongPhanPhoi = await _context.HeThongPhanPhoi
                .FirstOrDefaultAsync(m => m.MaHTPP == id);
            if (heThongPhanPhoi == null)
            {
                return NotFound();
            }

            return View(heThongPhanPhoi);
        }

        // GET: HeThongPhanPhoi/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HeThongPhanPhoi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaHTPP,TenHTPP")] HeThongPhanPhoi heThongPhanPhoi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(heThongPhanPhoi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(heThongPhanPhoi);
        }

        // GET: HeThongPhanPhoi/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var heThongPhanPhoi = await _context.HeThongPhanPhoi.FindAsync(id);
            if (heThongPhanPhoi == null)
            {
                return NotFound();
            }
            return View(heThongPhanPhoi);
        }

        // POST: HeThongPhanPhoi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaHTPP,TenHTPP")] HeThongPhanPhoi heThongPhanPhoi)
        {
            if (id != heThongPhanPhoi.MaHTPP)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(heThongPhanPhoi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HeThongPhanPhoiExists(heThongPhanPhoi.MaHTPP))
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
            return View(heThongPhanPhoi);
        }

        // GET: HeThongPhanPhoi/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var heThongPhanPhoi = await _context.HeThongPhanPhoi
                .FirstOrDefaultAsync(m => m.MaHTPP == id);
            if (heThongPhanPhoi == null)
            {
                return NotFound();
            }

            return View(heThongPhanPhoi);
        }

        // POST: HeThongPhanPhoi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var heThongPhanPhoi = await _context.HeThongPhanPhoi.FindAsync(id);
            if (heThongPhanPhoi != null)
            {
                _context.HeThongPhanPhoi.Remove(heThongPhanPhoi);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HeThongPhanPhoiExists(string id)
        {
            return _context.HeThongPhanPhoi.Any(e => e.MaHTPP == id);
        }
  

        // Upload EXcel
        public async Task<IActionResult> Upload()
        {
        return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file!=null)
            {
                string fileExtension= Path.GetExtension (file.FileName);
                if (fileExtension != ".xls" && fileExtension != ".xlsx")
                {
                ModelState.AddModelError("", "Please choose excel file to upload!");

                } else
                {

                var fileName = DateTime.Now.ToShortTimeString() + fileExtension;
                var filePath = Path.Combine (Directory.GetCurrentDirectory() + "/Uploads/Excels", fileName); 
                var fileLocation = new FileInfo(filePath).ToString();
                using (var stream = new FileStream(filePath, FileMode.Create))
                {

                    await file.CopyToAsync(stream);

                    var dt = _excelProcess.ExcelToDataTable(fileLocation);
                    //using for loop to read data from dt
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                    //create new Person object
                    var ps = new HeThongPhanPhoi();
                    //set value to attributes
                    ps.MaHTPP = dt.Rows[i][1].ToString(); 
                    ps.TenHTPP = dt.Rows[i][2].ToString(); 
                    
                    //add object to context 
                    _context.Add(ps);
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                    }
                }
            }
            return View();

         }


        public IActionResult Download()
        {
        //Name the file when downloading
        var fileName = "HTPP" + ".xlsx";
        using (ExcelPackage excelPackage = new ExcelPackage())
        {
        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");
        //add some text to cell A1
        worksheet.Cells["A1"].Value = "MaHTPP"; 
        worksheet.Cells["B1"].Value = "TenHTPP";
       
        //get all Person
        var HTList = _context.HeThongPhanPhoi.ToList(); 
        //fill data to worksheet
        worksheet.Cells["A2"].LoadFromCollection (HTList);
        var stream = new MemoryStream (excelPackage.GetAsByteArray()); //download file
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        }



    }
}       