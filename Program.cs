using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

namespace selenium_sample
{
    class Program
    {
        static void Main(string[] args)
        {
            ChromeOptions optionsChr = new ChromeOptions();
            // optionsChr.AddArgument("--headless");
            var browser = new ChromeDriver("./drivers", optionsChr);
            // var browser = new InternetExplorerDriver("./drivers");

            browser.Manage().Window.Maximize();

            Console.WriteLine("Acessando Site Exemplo");
            browser.Navigate().GoToUrl(Directory.GetCurrentDirectory() + "/index.html");
            Thread.Sleep(1000);

            var dictionary = new Dictionary<string, string>();

            var email = browser.FindElement(By.Id("email"));
            dictionary.Add(email.GetAttribute("id"), email.GetAttribute("value"));

            var select = browser.FindElement(By.Id("single_select"));
            var selectElement = new SelectElement(select);
            var selectedOption = selectElement.SelectedOption.GetAttribute("value");
            dictionary.Add(select.GetAttribute("id"), selectedOption);

            var multiSelect = browser.FindElement(By.Id("multi_select"));
            var selectElements = new SelectElement(multiSelect);
            var selectedOptions = string.Join(",", selectElements.AllSelectedOptions.Select(x => x.GetAttribute("value")));
            dictionary.Add(multiSelect.GetAttribute("id"), selectedOptions);

            var checkboxes = browser.FindElements(By.Name("single-checkbox"));
            foreach (var checkbox in checkboxes)
                dictionary.Add(checkbox.GetAttribute("id"), checkbox.GetProperty("checked").ToString());

            var radioButtons = browser.FindElements(By.Name("single-radio"));
            dictionary.Add("single-radio", radioButtons.ToList().Where(x => x.GetProperty("checked") == "True").FirstOrDefault().GetAttribute("id"));

            var textArea = browser.FindElement(By.Id("exampleTextArea"));
            dictionary.Add(textArea.GetAttribute("id"), textArea.GetAttribute("value"));

            Console.WriteLine("Fim do Web Crawler");

            Console.WriteLine("Exibindo resultados");
            string json = JsonConvert.SerializeObject(dictionary, Formatting.Indented);
            var result = browser.FindElementById("result");
            result.SendKeys(json);
            Console.WriteLine(json);
        }
    }
}

