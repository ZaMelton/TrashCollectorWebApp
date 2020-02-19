using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrashCollectorV2.Contracts;
using TrashCollectorV2.Models;

namespace TrashCollectorV2.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IRepositoryWrapper _repo;

        public EmployeeController(IRepositoryWrapper repo)
        {
            _repo = repo;
        }

        // GET: Employee
        public ActionResult Index()
        {
            ViewModel employeeView = new ViewModel(); 
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (_repo.Employee.FindByCondition(e => e.IdentityUserId == userId).Any())
            {
                var employee = _repo.Employee.FindByCondition(e => e.IdentityUserId == userId).FirstOrDefault();
                employeeView.Employee = employee;
                var customers = _repo.Customer.FindByCondition(c => c.Address.ZipCode == employee.ZipCode);
                employeeView.CustomerList = customers;
                return View(employeeView);
            }
            else
            {
                return RedirectToAction("Create");
            }
        }

        // GET: Employee/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee)
        {
            try
            {
                Employee newEmployee = new Employee
                {
                    IdentityUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier),
                    Name = employee.Name,
                    ZipCode = employee.ZipCode
                };

                _repo.Employee.CreateEmployee(newEmployee);
                _repo.Save();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(employee);
            }
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Employee/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}