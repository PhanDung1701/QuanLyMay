﻿using DAO;
using DevExpress.XtraGrid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class InventoryBUS
    {
        private static ManagementShopClothesEntities1 db = new ManagementShopClothesEntities1();
       
        public static int QuantityEntrySlip(DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            var lstImport = db.EntrySlips.Where(x => x.isPay == true && x.createDate.Value.CompareTo(dateTimeFrom) >= 0 && x.createDate.Value.CompareTo(dateTimeTo) <= 0).ToList();
            int sum = 0;
            foreach (var import in lstImport)
                sum += db.EntrySlipDetails.Where(x => x.entrySlipId == import.id).Sum(x => x.quantity) ?? 0;
            return sum;
        }

        public static int QuantityInvoice(DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            var lstOrder = db.Invoices.Where(x => x.isPay == true && x.createDate.Value.CompareTo(dateTimeFrom) >= 0 && x.createDate.Value.CompareTo(dateTimeTo) <= 0).ToList();
            int sum = 0;
            foreach (var order in lstOrder)
            {
                sum += db.InvoiceDetails.Where(x => x.invoiceId == order.id).Sum(x => x.quantity) ?? 0;
            }
            return sum;
        }
        public static DataTable loadDetailInventory(GridControl gc, DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            DataTable tb = new DataTable();
            List<ItemInventory> lstItemInventory = new List<ItemInventory>();
            tb.Columns.Add("date");
            tb.Columns.Add("product");
            tb.Columns.Add("entrySlip");
            tb.Columns.Add("invoice");
            var lstOrder = db.Invoices.Where(x => x.isPay == true && x.createDate.Value.CompareTo(dateTimeFrom) >= 0 && x.createDate.Value.CompareTo(dateTimeTo) <= 0).ToList();
            var lstImport = db.EntrySlips.Where(x => x.isPay == true && x.createDate.Value.CompareTo(dateTimeFrom) >= 0 && x.createDate.Value.CompareTo(dateTimeTo) <= 0).ToList();
            for (DateTime date = dateTimeFrom; date.CompareTo(dateTimeTo) <= 0; date = date.AddDays(1))
            {
                var lstOrderTemp = lstOrder.Where(x => DateTime.Parse(x.createDate.Value.ToShortDateString()).CompareTo(DateTime.Parse(date.ToShortDateString())) == 0).ToList();
                var lstImportTemp = lstImport.Where(x => DateTime.Parse(x.createDate.Value.ToShortDateString()).CompareTo(DateTime.Parse(date.ToShortDateString())) == 0).ToList();
                if (lstOrderTemp.Count != 0 || lstImportTemp.Count != 0)
                {
                    loadProductOfDay(lstItemInventory, date, lstImportTemp, lstOrderTemp);
                    loadTotalEntrySlipAndInvoice(lstItemInventory, lstImportTemp, lstOrderTemp);
                }

            }
            //thêm datarow vào datatable
            foreach (var item in lstItemInventory)
            {
                DataRow dr = tb.NewRow();
                dr[0] = item.Date;
                dr[1] = ProductBUS.FindById(item.ProductId).name;
                dr[2] = item.QuantityEntrySlip;
                dr[3] = item.QuantityInvoice;
                tb.Rows.Add(dr);
            }
            gc.DataSource = tb;
            return tb;
        }

        private static void loadTotalEntrySlipAndInvoice(List<ItemInventory> lstItemInventory, List<EntrySlip> lstImportTemp, List<Invoice> lstOrderTemp)
        {
            // Tính tổng số lượng nhập của 1 linh kiện trong 1 ngày
            foreach (var import in lstImportTemp)
            {
                var entrySlipDetails = db.EntrySlipDetails.Where(x => x.entrySlipId == import.id).ToList();
                foreach (var importDetail in entrySlipDetails)
                {
                    var inventoryItem = lstItemInventory.FirstOrDefault(x => x.ProductId == importDetail.productId);
                    if (inventoryItem != null)
                    {
                        int quantityEntrySlip = db.EntrySlipDetails.Where(x => x.entrySlipId == import.id && x.productId == inventoryItem.ProductId).Sum(x => x.quantity) ?? 0;
                        inventoryItem.QuantityEntrySlip += quantityEntrySlip;
                    }
                }
            }

            // Tính tổng số lượng bán của 1 linh kiện trong 1 ngày
            foreach (var order in lstOrderTemp)
            {
                var invoiceDetails = db.InvoiceDetails.Where(x => x.invoiceId == order.id).ToList();
                foreach (var orderDetail in invoiceDetails)
                {
                    var inventoryItem = lstItemInventory.FirstOrDefault(x => x.ProductId == orderDetail.productId);
                    if (inventoryItem != null)
                    {
                        int quantityInvoice = db.InvoiceDetails.Where(x => x.invoiceId == order.id && x.productId == inventoryItem.ProductId).Sum(x => x.quantity) ?? 0;
                        inventoryItem.QuantityInvoice += quantityInvoice;
                    }
                }
            }
        }

        private static void loadProductOfDay(List<ItemInventory> lstItemInventory, DateTime date, List<EntrySlip> lstImportTemp, List<Invoice> lstOrderTemp)
        {

            //thêm tất cả linh kiện trong danh sách nhập kho theo ngày
            foreach (var import in lstImportTemp)
            {
                var list = db.EntrySlipDetails.Where(x => x.entrySlipId == import.id).ToList();
                foreach (var importDetail in list)
                    if (lstItemInventory.FirstOrDefault(x => x.ProductId == importDetail.productId) == null)
                        lstItemInventory.Add(new ItemInventory() { Date = date.ToShortDateString(), ProductId = importDetail.productId.Value, QuantityEntrySlip = 0, QuantityInvoice = 0 });
            }
            //thêm tất cả linh kiện trong danh sách hoá đơn theo ngày
            foreach (var order in lstOrderTemp)
            {
                var list = db.InvoiceDetails.Where(x => x.invoiceId == order.id).ToList();
                foreach (var orderDetail in list)
                    if (lstItemInventory.FirstOrDefault(x => x.ProductId == orderDetail.productId) == null)
                        lstItemInventory.Add(new ItemInventory() { Date = date.ToShortDateString(), ProductId = orderDetail.productId.Value, QuantityInvoice = 0, QuantityEntrySlip = 0 });
            }
        }
    }
}
