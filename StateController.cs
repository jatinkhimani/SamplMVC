using AppCustomer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace AppCustomer.Controllers
{
    public class StateController : Controller
    {
        private IStateRepository _iStateRepo;
        public StateController(IStateRepository stateRepo)
        {
            _iStateRepo = stateRepo;
        }
        // GET: State
        public ActionResult GetStatesByCountryId(int id)
        {
            var states=_iStateRepo.GetStates(id);
            return Json(new { States = states }, JsonRequestBehavior.AllowGet);
        }
    }
}