using AppCustomer.Common;
using AppCustomer.DataAccess;
using AppCustomer.Repository;
using RepositoryCore;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppCustomer.Controllers
{
    public class CustomerController : Controller
    {
        private ICustomerRepository _iCustomerRepo;
        private ICountryRepository _iCountryRepo;
        public CustomerController(ICustomerRepository customerRepo,
            ICountryRepository countryRepo)
        {
            _iCustomerRepo = customerRepo;
            _iCountryRepo = countryRepo;
        }
        // GET: Customer
        public ActionResult Index()
        {
            var customerList = _iCustomerRepo.FindAllCustomer();
            if (Request.IsAjaxRequest())
            {
                return PartialView("_CustomerList", customerList);
            }
            return View(customerList);
        }
        

        // GET: Customer/Create
        public ActionResult Create()
        {
            FillViewBags();
            return PartialView("_Create", new Customer());
        }
        
        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            try
            {
                customer.City = null;    
                _iCustomerRepo.AddCustomer(customer);
                return ResponseMessages.GetCustomMessage(true, "Customer created successfully");
            }
            catch (UniqueKeyException uniqueKeyEx)
            {
                return ResponseMessages.GetGenericCreatedFailed("Customer", "CustomerId");
            }
            catch (DbEntityValidationException ex)
            {
                return ResponseMessages.GetGenericModalFailed();
            }
        }

        private void FillViewBags(int countryId=0, int stateId=0)
        {
            var enumValues = Enum.GetValues(typeof(GeneralUtil.CutomerTypes)).Cast<GeneralUtil.CutomerTypes>().ToList();
            List<KeyValuePair<string, int>> empTypes = new List<KeyValuePair<string, int>>();
            for (int i = 0; i < enumValues.Count; i++)
            {
                empTypes.Add(new KeyValuePair<string, int>(enumValues[i].ToString(), i + 1));
            }
            ViewBag.empTypes = new SelectList(empTypes, "Value", "Key");
            ViewBag.Countries = new SelectList(_iCountryRepo.GetAllCountries(), "CountryId", "CountryName");
            if (countryId == 0)
            {
                ViewBag.States = new SelectList(new List<State>(), "StateId", "StateName");
            }
            else
            {
                var states = _iStateRepo.GetStates(countryId);
                ViewBag.States = new SelectList((IEnumerable<object>)states, "StateId", "StateName");
            }

            if (stateId == 0)
            {
                ViewBag.Cities = new SelectList(new List<City>(), "CityId", "CityName");
            }
            else
            {
                var cities = _iCityRepo.GetCities(stateId);
                ViewBag.Cities = new SelectList((IEnumerable<object>)cities, "CityId", "CityName");
            }
            
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int id)
        {
            if (id != 0)
            {
                var customer = _iCustomerRepo.FindCustomer(id);
                FillViewBags(customer.City.State.CountryId, customer.City.StateId);
                return PartialView("_Edit", customer);
            }
            else
            {
                return ResponseMessages.GetCustomMessage(false, "Customer Not found");
            }            
        }

        // POST: Customer/Edit/5
        [HttpPost]
        public ActionResult Edit(Customer customer)
        {
            try
            {
                customer.City = null;
                _iCustomerRepo.EditCustomer(customer);
                return ResponseMessages.GetCustomMessage(true, "Customer Updated Successfully");
            }
            catch (UniqueKeyException uniqueKeyEx)
            {
                return ResponseMessages.GetGenericEditFailed("Customer", "CustomerId");
            }
            catch (DbEntityValidationException ex)
            {
                return ResponseMessages.GetGenericModalFailed();
            }
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                var customer = _iCustomerRepo.FindCustomer(id);
                return PartialView("_Delete", customer);
            }
            else
            {
                return ResponseMessages.GetCustomMessage(false, "Customer Not found");
            }
        }

        // POST: Customer/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _iCustomerRepo.DeleteCustomer(id);
                return ResponseMessages.GetCustomMessage(true, "Customer Deleted Successfully");
            }
            catch (ForeignKeyException foreignKeyEx)
            {
                return ResponseMessages.GetGenericDeleteFailed("Customer");
            }
        }

        public ActionResult Upload(int id)
        {
            TempData["CustId"] = id;
            return PartialView("_Upload");
        }
        [HttpPost]
        [ActionName("Upload")]
        public ActionResult UploadLogo(HttpPostedFileBase file)
        {
            
            var customerId = TempData["CustId"];
            TempData.Clear();
            if (file != null && customerId != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Images/Customers"), customerId + fileName);
                file.SaveAs(path);

                var cust = _iCustomerRepo.FindCustomer(Convert.ToInt32( customerId));
                cust.Logo = customerId + fileName;
                _iCustomerRepo.EditCustomer(cust);
                TempData.Add("Success", "Logo Uploaded");
            }
            TempData.Add("Failed", "File Not found. Try again");
            return RedirectToAction("Index");
        }
    }
}
