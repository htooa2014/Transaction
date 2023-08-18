using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using System.Xml;
using Transaction.Models;
using Transaction.Service;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Transaction.Controllers
{
    public class TransactionsController : Controller
    {
        ITransactionService _transactionService = null;
        List<Transactions> _transactions= new List<Transactions>();

        string[] validFileTypes = { ".xls", ".xlsx", ".csv", ".xml" };


        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public IActionResult Index(List<Transactions> transactions = null)
        {
            transactions = transactions == null ? new List<Transactions>() : transactions;
            return View(transactions);
        }

        [HttpPost]
        public IActionResult Index(IFormFile file, [FromServices] IHostingEnvironment hostingEnvironment)
        {
            string fileName = $"{hostingEnvironment.WebRootPath}\\files\\{file.FileName}";
            string extension = System.IO.Path.GetExtension(fileName).ToLower();
            using (FileStream fileStream = System.IO.File.Create(fileName))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }

            var transactions = new List<Transactions>();
            if (validFileTypes.Contains(extension))
            {
                transactions = this.GetTransactionList(file.FileName, extension);
                SaveTransactions(transactions);
                transactions=GetAllTransactions();
            }
            else
            {
                ViewBag.Error = "Please Upload Files in .xls, .xlsx or .csv format";
            }



            return Index(transactions);
        }

        private List<Transactions> GetTransactionList(string fName, string extension)
        {
            List<Transactions> transactions = new List<Transactions>();

            var fileName = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + fName;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            if (extension == ".csv" || extension.Trim() == ".xls" || extension.Trim() == ".xlsx")
            {
                using (var stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read))
                {
                    if (extension == ".csv")
                    {
                        using (var reader = ExcelReaderFactory.CreateCsvReader(stream))
                        {

                            while (reader.Read())
                            {
                                transactions.Add(new Transactions()
                                {
                                    TransactionId = reader.GetValue(0).ToString(),
                                    Amount = Convert.ToDecimal(reader.GetValue(1).ToString()),
                                    CurrencyCode = reader.GetValue(2).ToString(),
                                    TransactionDate = Convert.ToDateTime(reader.GetValue(3).ToString()),
                                    Status = reader.GetValue(4).ToString(),
                                });
                            }

                        }
                    }
                    else if (extension.Trim() == ".xls" || extension.Trim() == ".xlsx")
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            while (reader.Read())
                            {
                                transactions.Add(new Transactions()
                                {
                                    TransactionId = reader.GetValue(0).ToString(),
                                    Amount = Convert.ToDecimal(reader.GetValue(1).ToString()),
                                    CurrencyCode = reader.GetValue(2).ToString(),
                                    TransactionDate = Convert.ToDateTime(reader.GetValue(3).ToString()),
                                    Status = reader.GetValue(4).ToString(),

                                });
                            }
                        }
                    }
                   
                }
            }
            else if (extension.Trim() == ".xml")
            {
                Transactions trans;
                XmlDocument doc = new XmlDocument();
                doc.Load(string.Concat(fileName));

                //Loop through the selected Nodes.
                foreach (XmlNode node in doc.SelectNodes("/Transactions/Transaction"))
                {
                    //Fetch the Node values and assign it to Model.
                    

                    trans = new Transactions();


                    trans.TransactionId = node.Attributes["id"].InnerText;
                    trans.TransactionDate = Convert.ToDateTime(node["TransactionDate"].InnerText);
                    
                        XmlNodeList paymentDetails = node.SelectNodes("PaymentDetails");

                    foreach (XmlNode paymentNode in paymentDetails)
                    {
                        trans.Amount = Convert.ToDecimal(paymentNode["Amount"].InnerText);
                        trans.CurrencyCode = paymentNode["CurrencyCode"].InnerText;
                    }
                    trans.Status = node["Status"].InnerText;
                    
                    transactions.Add(trans);
                }
            }

            return transactions;
        }


        public void SaveTransactions(List<Transactions> transactions)
        {
            _transactions = _transactionService.SaveTransactions(transactions); 
          
        }
        public List<Transactions>  GetAllTransactions()
        {
            List<Transactions> t= new List<Transactions>();
            t = _transactionService.GetTransactions();

            return t;

        }

    }
}
