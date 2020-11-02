using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BitsOrchestra.Models;
using BitsOrchestra.Data;
using Microsoft.EntityFrameworkCore;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace BitsOrchestra.Controllers
{
    public class HomeController : Controller
    {
        ApplicationContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Records.ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<ActionResult> Upload(IFormFile upload)
        {
            if (upload != null)
            {
                string fileName = System.IO.Path.GetFileName(upload.FileName);
                _logger.LogInformation(fileName);
                using (var csvreader = new StreamReader(upload.OpenReadStream()))
                {

                    _logger.LogInformation("In using");
                    while (!csvreader.EndOfStream)
                    {
                        _logger.LogInformation("Inside stream");
                        var line = csvreader.ReadLine();
                        var values = line.Split(',');

                        foreach (var val in values)
                        {
                            _logger.LogInformation(val);
                        }


                        Record record;

                        try
                        {
                            string name = values[0];
                            DateTime date;
                            bool married = Boolean.Parse(values[2]);
                            string phone = values[3];
                            decimal salary = Decimal.Parse(values[4]);

                            if (!DateTime.TryParse(values[1], out date))
                            {
                                _logger.LogInformation("Wrong date format");
                            }

                            record = new Record { Name = name, DateOfBirth = date, Married = married, Phone = phone, Salary = salary };
                            _context.Add(record);
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogInformation("Exception on adding: " + ex.Message);
                        }

                    }
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return BadRequest("no id");
            }
            var record = await _context.Records.FindAsync(id);

            if (record == null)
            {
                return BadRequest("Error");
            }
            _context.Remove(record);
            _ = await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<ActionResult> Update(Record record)
        {
            if (record == null)
            {
                return BadRequest("Error");
            }
            _context.Update(record);
            _ = await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return BadRequest("no id");
            }
            var record = await _context.Records.FindAsync(id);
            return View(record);

        }


    }
}
