using System;
using System.Collections.Generic;
using GloiathNationalBank.Model;
using GloiathNationalBank.Processes;
using Microsoft.AspNetCore.Mvc;

namespace GloiathNationalBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {

        SkuProcesses SkuProcesses = new SkuProcesses();
        // GET api/transaction   
        [HttpGet]
        public ActionResult<List<SkuListModel>> Get()
        {
            return SkuProcesses.GetListSku();
        }

        // GET api/transaction/nombreDelSku
        [HttpGet("{sku}")]
        public ActionResult<List<SkuTransactionModel>> Get(string sku)
        {
            return SkuProcesses.GetTransactionForSku(sku);
        }

    }
}
