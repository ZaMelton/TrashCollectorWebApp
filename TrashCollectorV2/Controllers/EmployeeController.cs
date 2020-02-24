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
                var customers = _repo.Customer.GetCustomersIncludeAll();
                customers = customers.Where(c => c.Address.ZipCode == employee.ZipCode).ToList();
                customers = CheckSuspendedCustomers(customers);
                employeeView.CustomerList = GetTodaysCustomers(customers);
                return View(employeeView);
            }
            else
            {
                return RedirectToAction("Create");
            }
        }

        public List<Customer> GetTodaysCustomers(List<Customer> customers)
        {
            customers = customers.Where(c => c.Account.NextPickupDate.Date == DateTime.Today 
                                        || c.Account.OneTimePickup.Date == DateTime.Today).ToList();
            return customers;
        }

        public List<Customer> CheckSuspendedCustomers(List<Customer> customers)
        {
            foreach(var customer in customers)
            {
                if (DateTime.Today >= customer.Account.StartSuspend && DateTime.Today < customer.Account.EndSuspend)
                {
                    customer.Account.IsSuspended = true;
                    Account accountFromDb = _repo.Account.FindByCondition(a => a.Id == customer.AccountId).FirstOrDefault();
                    accountFromDb.IsSuspended = true;
                    _repo.Account.Update(accountFromDb);
                    _repo.Save();
                }
                else
                {
                    Account accountFromDb = _repo.Account.FindByCondition(a => a.Id == customer.AccountId).FirstOrDefault();
                    accountFromDb.IsSuspended = false;
                    _repo.Account.Update(accountFromDb);
                    _repo.Save();
                }
            }
            return customers;
        }

        public ActionResult ConfirmPickup(int accountId)
        {
            Account accountFromDb = _repo.Account.FindByCondition(a => a.Id == accountId).FirstOrDefault();
            if(accountFromDb.NextPickupDate.Date == DateTime.Today)
            {
                accountFromDb.NextPickupDate = accountFromDb.NextPickupDate.AddDays(7);
            }
            if (accountFromDb.OneTimePickup.Date == DateTime.Today)
            {
                accountFromDb.OneTimePickup = accountFromDb.OneTimePickup.AddYears(1);
            }
            accountFromDb.Balance += 10000;
            _repo.Account.Update(accountFromDb);
            _repo.Save();
            return RedirectToAction("Index");
        }

        public ActionResult FilterByDay()
        {
            ViewModel viewModel = new ViewModel();
            var customers = _repo.Customer.GetCustomersIncludeAll();
            viewModel.CustomerList = customers;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FilterByDay(ViewModel viewModel)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employee = _repo.Employee.FindByCondition(e => e.IdentityUserId == userId).FirstOrDefault();
            var customers = _repo.Customer.GetCustomersIncludeAll();
            customers = customers.Where(c => c.Account.PickupDay == viewModel.FilterDay 
                                        && c.Address.ZipCode == employee.ZipCode 
                                        && !c.Account.IsSuspended).ToList();
            viewModel.CustomerList = customers;
            return View(viewModel);
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

        public ActionResult CustomerDetails(int customerId)
        {
            var customer = _repo.Customer.GetCustomer(customerId);
            customer.Address = _repo.Address.FindByCondition(a => a.Id == customer.AddressId).FirstOrDefault();
            return View(customer);
        }
    }
}