using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrashCollectorV2.Contracts;
using TrashCollectorV2.Models;

namespace TrashCollectorV2.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IRepositoryWrapper _repo;

        public CustomerController(IRepositoryWrapper repo)
        {
            _repo = repo;
        }

        // GET: Customer
        public ActionResult Index()
        {
            ViewModel customerView = new ViewModel();

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (_repo.Customer.FindByCondition(e => e.IdentityUserId == userId).Any())
            {
                var customer = _repo.Customer.FindByCondition(e => e.IdentityUserId == userId).FirstOrDefault();
                customerView.Customer = customer;
                
                return View(customerView);
            }
            else
            {
                return RedirectToAction("Create");
            }
        }

        // GET: Customer/Details/5
        public ActionResult Details()
        {
            ViewModel customerView = new ViewModel();
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _repo.Customer.FindByCondition(c => c.IdentityUserId == userId).FirstOrDefault();
            customerView.Customer = customer;
            customerView.Address = _repo.Address.FindByCondition(a => a.Id == customer.AddressId).FirstOrDefault();
            customerView.Account = _repo.Account.FindByCondition(a => a.Id == customer.AccountId).FirstOrDefault();
            return View(customerView);
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            try
            {
                _repo.Address.Create(customer.Address);
                _repo.Save();

                Customer newCustomer = new Customer
                {
                    IdentityUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier),
                    Name = customer.Name,
                    AddressId = _repo.Address.FindByCondition(a => a.Equals(customer.Address)).FirstOrDefault().Id
                };

                _repo.Customer.CreateCustomer(newCustomer);
                _repo.Save();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Account/Create
        public ActionResult CreateAccount()
        {
            Account account = new Account();
            return View(account);
        }

        // POST: Account/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAccount(Account account)
        {
            try
            {
                _repo.Account.Create(account);
                _repo.Save();

                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var customer = _repo.Customer.FindByCondition(c => c.IdentityUserId == userId).FirstOrDefault();
                customer.AccountId = _repo.Account.FindByCondition(a => a.Equals(account)).FirstOrDefault().Id;
                _repo.Customer.Update(customer);
                _repo.Save();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Customer/Edit/5
        public ActionResult Edit()
        {
            ViewModel customerView = new ViewModel();
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _repo.Customer.FindByCondition(c => c.IdentityUserId == userId).FirstOrDefault();
            customerView.Customer = customer;
            customerView.Address = _repo.Address.FindByCondition(a => a.Id == customer.AddressId).FirstOrDefault();
            return View(customerView);
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ViewModel customerView)
        {
            try
            {
                _repo.Address.Create(customerView.Address);
                _repo.Save();

                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var customer = _repo.Customer.FindByCondition(c => c.IdentityUserId == userId).FirstOrDefault();
                Customer customerFromDb = _repo.Customer.FindByCondition(c => c.IdentityUserId == userId).FirstOrDefault();
                customerFromDb.Name = customerView.Customer.Name;
                _repo.Customer.Update(customerFromDb);
                _repo.Save();
                customerFromDb.AddressId = _repo.Address.FindByCondition(a => a.Equals(customerView.Address)).FirstOrDefault().Id;
                _repo.Customer.Update(customerFromDb);
                _repo.Save();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Customer/Edit/5
        public ActionResult EditAccount()
        {
            return View();
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAccount(Account account)
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

        // GET: Customer/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Customer/Delete/5
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