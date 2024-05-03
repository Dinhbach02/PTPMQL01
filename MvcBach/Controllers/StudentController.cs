using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using MvcBach.Data;
using MvcBach.Models;
using MvcBach.Models.Process;

namespace MvcBach.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private ExcelProcess _excelProcess = new ExcelProcess();
         public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _context.Student.ToListAsync();
            return View(model);
        }
        

        public async Task<IActionResult> Shearch()
        {
            return View(await _context.Student.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Shearch( string searchTen)
        {
            
            return View(await _context.Student.Where(m => m.FullName.Contains(searchTen)).ToListAsync());
        
        }


       
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentID,FullName,Age")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }
       

       
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: HeThongPhanPhoi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("StudentID,FullName,Age")] Student student)
        {
            if (id != student.StudentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentID))
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
            return View(student);
        }
        // GET: HeThongPhanPhoi/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.StudentID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: HeThongPhanPhoi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var student = await _context.Student.FindAsync(id);
            if (student != null)
            {
                _context.Student.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool StudentExists(string studentID)
        {
            throw new NotImplementedException();
        }



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
                    var ps = new Student();
                    //set value to attributes
                    ps.StudentID = dt.Rows[i][0].ToString(); 
                    ps.FullName = dt.Rows[i][1].ToString(); 
                    ps.Age = dt.Rows[i][2].ToString(); 
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
        var fileName = "YourFileName" + ".xlsx";
        using (ExcelPackage excelPackage = new ExcelPackage())
        {
        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");
        //add some text to cell A1
        worksheet.Cells["A1"].Value = "StudentID"; 
        worksheet.Cells["B1"].Value = "FullName";
        worksheet.Cells["C1"].Value = "Age"; 
        //get all Person
        var studentList = _context.Student.ToList(); 
        //fill data to worksheet
        worksheet.Cells["A2"].LoadFromCollection (studentList);
        var stream = new MemoryStream (excelPackage.GetAsByteArray()); //download file
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        }

    } 
}