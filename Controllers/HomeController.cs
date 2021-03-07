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
                var itemsArray = JsonConvert.DeserializeObject<string[]>(strInput);
                
                var dicCounter = new Dictionary<int, int>();
                for (int i = 0; i< itemsArray.Length; i++)
                {
                    int value;
                    if (int.TryParse(itemsArray[i], out value))
                    {
                        if (dicCounter.ContainsKey(value))
                        {
                            dicCounter[value]++;
                        }
                        else
                        {
                            dicCounter.Add(value, 1);
                        }
                    }
                    else
                    {
                        //one of the items is no integer. Return error
                        resultObject.outputString = "error, inputstring not in correct format.";
                        return View("~/Views/Home/Oodit.cshtml", resultObject);
                    }
                }

                //now get the strings where there are more than 3 occurrences from:
                List<int> validNumbers = new List<int>();
                foreach(var kvPair in dicCounter)
                {
                    if (kvPair.Value >= 3)
                    {
                        validNumbers.Add(kvPair.Key);
                    }
                }

                //sort the numbers decending
                List<int> validNumbersSorted = validNumbers.OrderByDescending(x => x).ToList();

                //resultObject.outputString = JsonConvert.SerializeObject(validNumbersSorted);
                resultObject.outputString = "[";
                resultObject.outputString += string.Join(",", validNumbersSorted);
                resultObject.outputString += "]";

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