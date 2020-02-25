using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using TrashCollectorV2.Contracts;
using TrashCollectorV2.Models;
using Customer = TrashCollectorV2.Models.Customer;
using Account = TrashCollectorV2.Models.Account;

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

        // GET: Customer/BalanceDetails
        public ActionResult BalanceDetails()
        {
            ViewModel customerView = new ViewModel();
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _repo.Customer.FindByCondition(c => c.IdentityUserId == userId).FirstOrDefault();
            var account = _repo.Account.FindByCondition(a => a.Id == customer.AccountId).FirstOrDefault();
            ViewBag.StripePublishKey = Api_Key.PAYMENT_KEY;
            return View(account);
        }

        public ActionResult Charge(string stripeEmail, string stripeToken, int accountId)
        {
            try
            {
                var customers = new CustomerService();
                var charges = new ChargeService();

                var account = _repo.Account.FindByCondition(a => a.Id == accountId).FirstOrDefault();
                var customer = customers.Create(new CustomerCreateOptions
                {
                    Email = stripeEmail,
                    Source = stripeToken
                });

                var charge = charges.Create(new ChargeCreateOptions
                {
                    Amount = (long)account.Balance,//charge in cents
                    Description = "Sample Charge",
                    Currency = "usd",
                    Customer = customer.Id
                });

                account.Balance = 0;
                _repo.Account.Update(account);
                _repo.Save();

                return View("BalanceDetails", account);
            }
            catch (Exception)
            {
                return View("Index");
            }
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
                //this will set the next pickup day so that you can use this for the employee when they pickup customeres trash
                //when the employee picks up trash, the next pickup will be set to the next week
                DateTime nextPickup = DateTime.Now;
                while (!nextPickup.DayOfWeek.Equals(account.PickupDay))
                {
                    nextPickup = nextPickup.AddDays(1);
                }
                account.NextPickupDate = nextPickup;
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
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var customerFromDb = _repo.Customer.FindByCondition(c => c.IdentityUserId == userId).FirstOrDefault();
                customerFromDb.Name = customerView.Customer.Name;
                _repo.Customer.Update(customerFromDb);
                _repo.Save();

                var addressFromDb = _repo.Address.GetAddress(customerFromDb.AddressId ?? default);
                addressFromDb.StreetAddress = customerView.Address.StreetAddress;
                addressFromDb.City = customerView.Address.City;
                addressFromDb.State = customerView.Address.State;
                addressFromDb.ZipCode = customerView.Address.ZipCode;
                _repo.Address.Update(addressFromDb);
                _repo.Save();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Account/Edit/5
        public ActionResult EditAccount()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _repo.Customer.FindByCondition(c => c.IdentityUserId == userId).FirstOrDefault();
            var account = _repo.Account.GetAccount(customer.AccountId ?? default);
            return View(account);
        }

        // POST: Account/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAccount(Account accountFromForm)
        {
            try
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var customer = _repo.Customer.FindByCondition(c => c.IdentityUserId == userId).FirstOrDefault();
                var accountFromDb = _repo.Account.GetAccount(customer.AccountId ?? default);

                accountFromDb.PickupDay = accountFromForm.PickupDay;
                DateTime nextPickup = DateTime.Now;
                while (!nextPickup.DayOfWeek.Equals(accountFromForm.PickupDay))
                {
                    nextPickup = nextPickup.AddDays(1);
                }
                accountFromDb.NextPickupDate = nextPickup;

                accountFromDb.OneTimePickup = accountFromForm.OneTimePickup;
                accountFromDb.StartSuspend = accountFromForm.StartSuspend;
                accountFromDb.EndSuspend = accountFromForm.EndSuspend;
                _repo.Account.Update(accountFromDb);
                _repo.Save();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Account/Edit/5
        public ActionResult EditWeeklyPickup()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _repo.Customer.FindByCondition(c => c.IdentityUserId == userId).FirstOrDefault();
            var account = _repo.Account.GetAccount(customer.AccountId ?? default);
            return View(account);
        }

        // POST: Account/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditWeeklyPickup(Account accountFromForm)
        {
            try
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var customer = _repo.Customer.FindByCondition(c => c.IdentityUserId == userId).FirstOrDefault();
                var accountFromDb = _repo.Account.GetAccount(customer.AccountId ?? default);
                accountFromDb.PickupDay = accountFromForm.PickupDay;
                DateTime nextPickup = DateTime.Now;
                while (!nextPickup.DayOfWeek.Equals(accountFromForm.PickupDay))
                {
                    nextPickup = nextPickup.AddDays(1);
                }
                accountFromDb.NextPickupDate = nextPickup;
                _repo.Account.Update(accountFromDb);
                _repo.Save();

                //customer.AccountId = accountFromDb.Id;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Account/Edit/5
        public ActionResult EditOneTimePickup()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _repo.Customer.FindByCondition(c => c.IdentityUserId == userId).FirstOrDefault();
            var account = _repo.Account.GetAccount(customer.AccountId ?? default);
            return View(account);
        }

        // POST: Account/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOneTimePickup(Account accountFromForm)
        {
            try
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var customer = _repo.Customer.FindByCondition(c => c.IdentityUserId == userId).FirstOrDefault();
                var accountFromDb = _repo.Account.GetAccount(customer.AccountId ?? default);
                accountFromDb.OneTimePickup = accountFromForm.OneTimePickup;
                _repo.Account.Update(accountFromDb);
                _repo.Save();

                //customer.AccountId = accountFromDb.Id;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Account/Edit/5
        public ActionResult EditSuspendDates()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = _repo.Customer.FindByCondition(c => c.IdentityUserId == userId).FirstOrDefault();
            var account = _repo.Account.GetAccount(customer.AccountId ?? default);
            return View(account);
        }

        // POST: Account/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSuspendDates(Account accountFromForm)
        {
            try
            {
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var customer = _repo.Customer.FindByCondition(c => c.IdentityUserId == userId).FirstOrDefault();
                var accountFromDb = _repo.Account.GetAccount(customer.AccountId ?? default);
                accountFromDb.StartSuspend = accountFromForm.StartSuspend;
                accountFromDb.EndSuspend = accountFromForm.EndSuspend;
                _repo.Account.Update(accountFromDb);
                _repo.Save();

                //customer.AccountId = accountFromDb.Id;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}