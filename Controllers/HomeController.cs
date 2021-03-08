using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using OoditOpdracht.Models;

namespace OoditOpdracht.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

      
        public ActionResult Oodit()
        {
            ResultObject resultObject = new ResultObject();

            ViewBag.Message = "Uitwerking Oodit opdracht";

            return View(resultObject);
        }




        //POST:            
        [HttpPost]
        public ActionResult CalculateResults(string strInput)
        {
            ResultObject resultObject = new ResultObject();
           
            strInput = strInput.Trim();
            if (strInput.StartsWith("[") && strInput.EndsWith("]"))
            {
                //var itemsList = JsonConvert.DeserializeObject<List<string>>(strInput); //werkt alleen met integers??
                var itemsList = strInput.Substring(1, strInput.Length - 2).Split(',').ToList();

                var numbersList = new List<Int32>();
                foreach(string s in itemsList){
                    if (int.TryParse(s, out int number))
                    {
                        numbersList.Add(number);
                    }
                    else
                    {
                        resultObject.outputString = "error, input contains non-numeric data.";
                        return View("~/Views/Home/Oodit.cshtml", resultObject);
                    }
                }

                List<int> resultList = numbersList.GroupBy(x => x)
                                            .Select(g => new { Text = g.Key, Count = g.Count() })                                           
                                            .Where(p=>p.Count>=3)
                                            .OrderByDescending(q=>q.Text)
                                            .Select(r=>r.Text).ToList();

                //resultObject.outputString = "[";
                //resultObject.outputString += string.Join(",", resultList);
                //resultObject.outputString += "]";
                resultObject.outputString = JsonConvert.SerializeObject(resultList);               

                return View("~/Views/Home/Oodit.cshtml", resultObject);
            }
            else
            {
                resultObject.outputString = "error, inputstring not in correct format.";
                return View("~/Views/Home/Oodit.cshtml", resultObject);               
            }


        }


    }
}