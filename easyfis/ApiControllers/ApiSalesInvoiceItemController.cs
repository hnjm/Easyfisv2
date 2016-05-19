﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiSalesInvoiceItemController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =======================
        // LIST Sales Invoice Item
        // =======================
        [Route("api/listSalesInvoiceItem")]
        public List<Models.TrnSalesInvoiceItem> Get()
        {
            var salesInvoiceItems = from d in db.TrnSalesInvoiceItems
                                    select new Models.TrnSalesInvoiceItem
                                    {
                                        Id = d.Id,
                                        SIId = d.SIId,
                                        SI = d.TrnSalesInvoice.SINumber,
                                        ItemId = d.ItemId,
                                        ItemCode = d.MstArticle.ManualArticleCode,
                                        Item = d.MstArticle.Article,
                                        ItemInventoryId = d.ItemInventoryId,
                                        ItemInventory = d.MstArticleInventory.InventoryCode,
                                        Particulars = d.Particulars,
                                        UnitId = d.UnitId,
                                        Unit = d.MstUnit.Unit,
                                        Quantity = d.Quantity,
                                        Price = d.Price,
                                        DiscountId = d.DiscountId,
                                        Discount = d.MstDiscount.Discount,
                                        DiscountRate = d.DiscountRate,
                                        DiscountAmount = d.DiscountAmount,
                                        NetPrice = d.NetPrice,
                                        Amount = d.Amount,
                                        VATId = d.VATId,
                                        VAT = d.MstTaxType.TaxType,
                                        VATPercentage = d.VATPercentage,
                                        VATAmount = d.VATAmount,
                                        BaseUnitId = d.BaseUnitId,
                                        BaseUnit = d.MstUnit1.Unit,
                                        BaseQuantity = d.BaseQuantity,
                                        BasePrice = d.BasePrice
                                    };
            return salesInvoiceItems.ToList();
        }

        // ================================
        // LIST Sales Invoice Item By SI Id
        // ================================
        [Route("api/listSalesInvoiceItemBySIId/{SIId}")]
        public List<Models.TrnSalesInvoiceItem> GetSalesInvoiceBySIId(String SIId)
        {
            var salesInvoiceItem_SIId = Convert.ToInt32(SIId);
            var salesInvoiceItems = from d in db.TrnSalesInvoiceItems
                                    where d.SIId == salesInvoiceItem_SIId
                                    select new Models.TrnSalesInvoiceItem
                                    {
                                        Id = d.Id,
                                        SIId = d.SIId,
                                        SI = d.TrnSalesInvoice.SINumber,
                                        ItemId = d.ItemId,
                                        ItemCode = d.MstArticle.ManualArticleCode,
                                        Item = d.MstArticle.Article,
                                        ItemInventoryId = d.ItemInventoryId,
                                        ItemInventory = d.MstArticleInventory.InventoryCode,
                                        Particulars = d.Particulars,
                                        UnitId = d.UnitId,
                                        Unit = d.MstUnit.Unit,
                                        Quantity = d.Quantity,
                                        Price = d.Price,
                                        DiscountId = d.DiscountId,
                                        Discount = d.MstDiscount.Discount,
                                        DiscountRate = d.DiscountRate,
                                        DiscountAmount = d.DiscountAmount,
                                        NetPrice = d.NetPrice,
                                        Amount = d.Amount,
                                        VATId = d.VATId,
                                        VAT = d.MstTaxType.TaxType,
                                        VATPercentage = d.VATPercentage,
                                        VATAmount = d.VATAmount,
                                        BaseUnitId = d.BaseUnitId,
                                        BaseUnit = d.MstUnit1.Unit,
                                        BaseQuantity = d.BaseQuantity,
                                        BasePrice = d.BasePrice
                                    };
            return salesInvoiceItems.ToList();
        }

        // ======================
        // ADD Sales Invoice Item
        // ======================
        [Route("api/addSalesInvoiceItem")]
        public int Post(Models.TrnSalesInvoiceItem saleItem)
        {
            try
            {
                Data.TrnSalesInvoiceItem newSaleItem = new Data.TrnSalesInvoiceItem();
                
                newSaleItem.SIId = saleItem.SIId;
                newSaleItem.ItemId = saleItem.ItemId;
                newSaleItem.ItemInventoryId = saleItem.ItemInventoryId;
                newSaleItem.Particulars = saleItem.Particulars;
                newSaleItem.UnitId = saleItem.UnitId;
                newSaleItem.Quantity = saleItem.Quantity;
                newSaleItem.Price = saleItem.Price;
                newSaleItem.DiscountId = saleItem.DiscountId;
                newSaleItem.DiscountRate = saleItem.DiscountRate;
                newSaleItem.DiscountAmount = saleItem.DiscountAmount;
                newSaleItem.NetPrice = saleItem.NetPrice;
                newSaleItem.Amount = saleItem.Amount;
                newSaleItem.VATId = saleItem.VATId;
                newSaleItem.VATPercentage = saleItem.VATPercentage;
                newSaleItem.VATAmount = saleItem.VATAmount;
                newSaleItem.BaseUnitId = saleItem.BaseUnitId;
                newSaleItem.BaseQuantity = saleItem.BaseQuantity;
                newSaleItem.BasePrice = saleItem.BasePrice;

                db.TrnSalesInvoiceItems.InsertOnSubmit(newSaleItem);
                db.SubmitChanges();

                var salesInvoces = from d in db.TrnSalesInvoices where d.Id == saleItem.SIId select d;
                if (salesInvoces.Any())
                {
                    var salesInvoiceItems = from d in db.TrnSalesInvoiceItems
                                            where d.SIId == saleItem.SIId
                                            select new Models.TrnSalesInvoiceItem
                                            {
                                                Id = d.Id,
                                                SIId = d.SIId,
                                                Amount = d.Amount,
                                                VATAmount = d.VATAmount
                                            };

                    Decimal amount;
                    if (!salesInvoiceItems.Any())
                    {
                        amount = 0;
                    }
                    else
                    {
                        amount = salesInvoiceItems.Sum(d => d.Amount + d.VATAmount);
                    }

                    var updateSales = salesInvoces.FirstOrDefault();
                    updateSales.Amount = amount;
                    db.SubmitChanges();
                }

                return newSaleItem.Id;

            }
            catch
            {
                return 0;
            }
        }

        // =========================
        // UPDATE Sales Invoice Item
        // =========================
        [Route("api/updateSalesInvoiceItem/{id}")]
        public HttpResponseMessage Put(String id, Models.TrnSalesInvoiceItem saleItem)
        {
            try
            {
                var saleItemId = Convert.ToInt32(id);
                var saleItems = from d in db.TrnSalesInvoiceItems where d.Id == saleItemId select d;

                if (saleItems.Any())
                {
                    var updateSalesInvoiceItem = saleItems.FirstOrDefault();

                    updateSalesInvoiceItem.SIId = saleItem.SIId;
                    updateSalesInvoiceItem.ItemId = saleItem.ItemId;
                    updateSalesInvoiceItem.ItemInventoryId = saleItem.ItemInventoryId;
                    updateSalesInvoiceItem.Particulars = saleItem.Particulars;
                    updateSalesInvoiceItem.UnitId = saleItem.UnitId;
                    updateSalesInvoiceItem.Quantity = saleItem.Quantity;
                    updateSalesInvoiceItem.Price = saleItem.Price;
                    updateSalesInvoiceItem.DiscountId = saleItem.DiscountId;
                    updateSalesInvoiceItem.DiscountRate = saleItem.DiscountRate;
                    updateSalesInvoiceItem.DiscountAmount = saleItem.DiscountAmount;
                    updateSalesInvoiceItem.NetPrice = saleItem.NetPrice;
                    updateSalesInvoiceItem.Amount = saleItem.Amount;
                    updateSalesInvoiceItem.VATId = saleItem.VATId;
                    updateSalesInvoiceItem.VATPercentage = saleItem.VATPercentage;
                    updateSalesInvoiceItem.VATAmount = saleItem.VATAmount;
                    updateSalesInvoiceItem.BaseUnitId = saleItem.BaseUnitId;
                    updateSalesInvoiceItem.BaseQuantity = saleItem.BaseQuantity;
                    updateSalesInvoiceItem.BasePrice = saleItem.BasePrice;

                    db.SubmitChanges();

                    var salesInvoces = from d in db.TrnSalesInvoices where d.Id == saleItem.SIId select d;
                    if (salesInvoces.Any())
                    {
                        var salesInvoiceItems = from d in db.TrnSalesInvoiceItems
                                                where d.SIId == saleItem.SIId
                                                select new Models.TrnSalesInvoiceItem
                                                {
                                                    Id = d.Id,
                                                    SIId = d.SIId,
                                                    Amount = d.Amount,
                                                    VATAmount = d.VATAmount
                                                };

                        Decimal amount;
                        if (!salesInvoiceItems.Any())
                        {
                            amount = 0;
                        }
                        else
                        {
                            amount = salesInvoiceItems.Sum(d => d.Amount + d.VATAmount);
                        }

                        var updateSales = salesInvoces.FirstOrDefault();
                        updateSales.Amount = amount;
                        db.SubmitChanges();
                    }

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // =========================
        // DELETE Sales Invoice Item
        // =========================
        [Route("api/deleteSalesInvoiceItem/{id}/{SIId}")]
        public HttpResponseMessage Delete(String id, String SIId)
        {
            try
            {
                var saleItemId = Convert.ToInt32(id);
                var saleItemSIId = Convert.ToInt32(SIId);
                var saleItems = from d in db.TrnSalesInvoiceItems where d.Id == saleItemId select d;

                if (saleItems.Any())
                {
                    db.TrnSalesInvoiceItems.DeleteOnSubmit(saleItems.First());
                    db.SubmitChanges();

                    var salesInvoces = from d in db.TrnSalesInvoices where d.Id == saleItemSIId select d;
                    if (salesInvoces.Any())
                    {
                        var salesInvoiceItems = from d in db.TrnSalesInvoiceItems
                                                where d.SIId == saleItemSIId
                                                select new Models.TrnSalesInvoiceItem
                                                {
                                                    Id = d.Id,
                                                    SIId = d.SIId,
                                                    Amount = d.Amount,
                                                    VATAmount = d.VATAmount
                                                };

                        Decimal amount;
                        if (!salesInvoiceItems.Any())
                        {
                            amount = 0;
                        }
                        else
                        {
                            amount = salesInvoiceItems.Sum(d => d.Amount + d.VATAmount);
                        }

                        var updateSales = salesInvoces.FirstOrDefault();
                        updateSales.Amount = amount;
                        db.SubmitChanges();
                    }

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}