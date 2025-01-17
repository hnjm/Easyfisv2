﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.POSIntegrationApiControllers
{
    public class POSIntegrationApiStockTransferController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ===============================================
        // GET Stock Transfer (POS Integration) - Stock In
        // ===============================================
        [HttpGet, Route("api/get/POSIntegration/stockTransferItems/IN/{stockTransferDate}/{toBranchCode}")]
        public List<POSIntegrationEntities.POSIntegrationTrnStockTransfer> GetStockTransferItemsINPOSIntegration(String stockTransferDate, String toBranchCode, POSIntegrationEntities.POSIntegrationTrnSalesInvoice POSIntegrationTrnSalesInvoiceObject)
        {
            var stockTransfer = from d in db.TrnStockTransfers.OrderByDescending(d => d.Id)
                                where d.STDate == Convert.ToDateTime(stockTransferDate)
                                && d.MstBranch1.BranchCode.Equals(toBranchCode)
                                && d.IsLocked == true
                                select new POSIntegrationEntities.POSIntegrationTrnStockTransfer
                                {
                                    BranchCode = d.MstBranch.BranchCode,
                                    Branch = d.MstBranch.Branch,
                                    STNumber = d.STNumber,
                                    STDate = d.STDate.ToShortDateString(),
                                    ToBranchCode = d.MstBranch1.BranchCode,
                                    ToBranch = d.MstBranch1.Branch,
                                    Article = d.MstArticle.Article,
                                    Particulars = d.Particulars,
                                    ManualSTNumber = d.ManualSTNumber,
                                    PreparedBy = d.MstUser3.FullName,
                                    CheckedBy = d.MstUser1.FullName,
                                    ApprovedBy = d.MstUser.FullName,
                                    IsLocked = d.IsLocked,
                                    CreatedBy = d.MstUser2.FullName,
                                    CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                    UpdatedBy = d.MstUser4.FullName,
                                    UpdatedDateTime = d.UpdatedDateTime.ToShortDateString(),
                                    listPOSIntegrationTrnStockTransferItem = db.TrnStockTransferItems.Select(i => new POSIntegrationEntities.POSIntegrationTrnStockTransferItem
                                    {
                                        STId = i.STId,
                                        ItemCode = i.MstArticle.ManualArticleCode,
                                        Item = i.MstArticle.Article,
                                        InventoryCode = i.MstArticleInventory.InventoryCode,
                                        Particulars = i.Particulars,
                                        Unit = i.MstUnit.Unit,
                                        Quantity = i.Quantity,
                                        Cost = i.Cost,
                                        Amount = i.Amount,
                                        BaseUnit = i.MstUnit1.Unit,
                                        BaseQuantity = i.BaseQuantity,
                                        BaseCost = i.BaseCost
                                    }).Where(i => i.STId == d.Id).ToList(),
                                };

            return stockTransfer.ToList();
        }

        // ================================================
        // GET Stock Transfer (POS Integration) - Stock Out
        // ================================================
        [HttpGet, Route("api/get/POSIntegration/stockTransferItems/OT/{stockTransferDate}/{fromBranchCode}")]
        public List<POSIntegrationEntities.POSIntegrationTrnStockTransfer> GetStockTransferItemsOTPOSIntegration(String stockTransferDate, String fromBranchCode, POSIntegrationEntities.POSIntegrationTrnSalesInvoice POSIntegrationTrnSalesInvoiceObject)
        {
            var stockTransfer = from d in db.TrnStockTransfers.OrderByDescending(d => d.Id)
                                where d.STDate == Convert.ToDateTime(stockTransferDate)
                                && d.MstBranch.BranchCode.Equals(fromBranchCode)
                                && d.IsLocked == true
                                select new POSIntegrationEntities.POSIntegrationTrnStockTransfer
                                {
                                    BranchCode = d.MstBranch.BranchCode,
                                    Branch = d.MstBranch.Branch,
                                    STNumber = d.STNumber,
                                    STDate = d.STDate.ToShortDateString(),
                                    ToBranchCode = d.MstBranch1.BranchCode,
                                    ToBranch = d.MstBranch1.Branch,
                                    Article = d.MstArticle.Article,
                                    Particulars = d.Particulars,
                                    ManualSTNumber = d.ManualSTNumber,
                                    PreparedBy = d.MstUser3.FullName,
                                    CheckedBy = d.MstUser1.FullName,
                                    ApprovedBy = d.MstUser.FullName,
                                    IsLocked = d.IsLocked,
                                    CreatedBy = d.MstUser2.FullName,
                                    CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                    UpdatedBy = d.MstUser4.FullName,
                                    UpdatedDateTime = d.UpdatedDateTime.ToShortDateString(),
                                    listPOSIntegrationTrnStockTransferItem = db.TrnStockTransferItems.Select(i => new POSIntegrationEntities.POSIntegrationTrnStockTransferItem
                                    {
                                        STId = i.STId,
                                        ItemCode = i.MstArticle.ManualArticleCode,
                                        Item = i.MstArticle.Article,
                                        InventoryCode = i.MstArticleInventory.InventoryCode,
                                        Particulars = i.Particulars,
                                        Unit = i.MstUnit.Unit,
                                        Quantity = i.Quantity,
                                        Cost = i.Cost,
                                        Amount = i.Amount,
                                        BaseUnit = i.MstUnit1.Unit,
                                        BaseQuantity = i.BaseQuantity,
                                        BaseCost = i.BaseCost
                                    }).Where(i => i.STId == d.Id).ToList(),
                                };

            return stockTransfer.ToList();
        }
    }
}
