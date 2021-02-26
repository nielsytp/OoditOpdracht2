using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OoditOpdracht.Models;

namespace OoditOpdracht.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {

            ResultObject resultObject = new ResultObject();


            ViewBag.Message = "Uitwerking Oodit opdracht";

            return View(resultObject);
        }




        //POST:    
        //this method is called when the user has selected any event-checkboxes and clicks on the send button
        //The selected events are in the CheckedProductIds array, we put these in an array and send that to the server.
        //a task will then analyse the buyers and create a file. The file results will be shown on the client when the task finished
        [HttpPost]
        public ActionResult CalculateResults(string strInput)
        {
            ResultObject resultObject = new ResultObject();

            strInput = strInput.Trim();
            if (strInput.StartsWith("[") && strInput.EndsWith("]"))
            {
                string innerstring = strInput.Substring(1, strInput.Length - 2);

                var itemsArray = innerstring.Split(',');
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
                        return View("~/Views/Home/contact.cshtml", resultObject);
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

                resultObject.outputString = "[";
                foreach (int i in validNumbersSorted)
                {
                    resultObject.outputString = resultObject.outputString + i + ",";
                }
                //remove last ",";
                if (resultObject.outputString.Length > 1)
                {
                    resultObject.outputString = resultObject.outputString.Substring(0, resultObject.outputString.Length - 1);
                } 
                resultObject.outputString = resultObject.outputString + "]";

                return View("~/Views/Home/contact.cshtml", resultObject);
            }
            else
            {
                resultObject.outputString = "error, inputstring not in correct format.";
                return View("~/Views/Home/contact.cshtml", resultObject);               
            }


        }


    }
}