using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FraudDetection.UI.Models;
using System.IO;

namespace FraudDetection.UI.Controllers
{
    public class FilesController : Controller
    {
        private readonly FrauddbContext _context;

        public FilesController(FrauddbContext context)
        {
            _context = context;
        }

        // GET: Files
        public async Task<IActionResult> Index()
        {
            return View(await _context.SourceFiles.Where(p => p.ParentId == null).ToListAsync());
        }

        // GET: Files/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sourceFile = await _context.SourceFiles.FirstOrDefaultAsync(m => m.Id == id);
            if (sourceFile == null)
            {
                return NotFound();
            }
            var images = await _context.SourceFiles.Where(m => m.ParentId == id).ToListAsync();
            var fileResult = new SourceFileDetailViewModel
            {
                Id = sourceFile.Id,
                FileName = sourceFile.FileName,
                FilePath = sourceFile.FilePath,
                CreateDate = sourceFile.CreateDate
            };
            if (images != null)
            {
                fileResult.Images = images.Select(s => new ImageResultViewModel { FileName = s.FileName, FilePath = s.FilePath, FileCaption = s.FileCaption });
            }

            return View(fileResult);
        }

        // GET: Files/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Files/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SourceFileViewModel sourceFile)
        {
            if (ModelState.IsValid)
            {
                var pathId = Guid.NewGuid().ToString();
                var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\dataset", pathId);
                Directory.CreateDirectory(path);
                var filePath = Path.Combine(path, sourceFile.SendFile.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await sourceFile.SendFile.CopyToAsync(stream);
                }
                var file = new SourceFile { FileName = sourceFile.SendFile.FileName, FilePath = pathId, CreateDate = DateTime.Now };
                _context.Add(file);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sourceFile);
        }

        // GET: Files/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sourceFile = await _context.SourceFiles.FindAsync(id);
            if (sourceFile == null)
            {
                return NotFound();
            }
            return View(sourceFile);
        }

        // POST: Files/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FileName,TotalRows,TotalColumns,CreateDate")] SourceFile sourceFile)
        {
            if (id != sourceFile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sourceFile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SourceFileExists(sourceFile.Id))
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
            return View(sourceFile);
        }

        // GET: Files/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sourceFile = await _context.SourceFiles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sourceFile == null)
            {
                return NotFound();
            }

            return View(sourceFile);
        }

        // POST: Files/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sourceFile = await _context.SourceFiles.FindAsync(id);
            _context.SourceFiles.Remove(sourceFile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SourceFileExists(int id)
        {
            return _context.SourceFiles.Any(e => e.Id == id);
        }

        // GET: Files/Result
        [Route("[controller]/[action]/{parentId:int}")]
        public IActionResult Result(int parentId)
        {
            return View(new ResultFileViewModel { ParentId = parentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddResult(ResultFileViewModel sourceFile)
        {
            if (ModelState.IsValid)
            {
                var pathId = _context.SourceFiles.Where(s => s.Id == sourceFile.ParentId).Select(r => r.FilePath).FirstOrDefault();
                var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\dataset", pathId);
                var filePath = Path.Combine(path, sourceFile.SendFile.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await sourceFile.SendFile.CopyToAsync(stream);
                }
                var file = new SourceFile
                {
                    FileName = sourceFile.SendFile.FileName,
                    FilePath = pathId,
                    CreateDate = DateTime.Now,
                    ParentId = sourceFile.ParentId,
                    FileCaption = sourceFile.FileCaption
                };
                _context.Add(file);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sourceFile);
        }
    }
}
