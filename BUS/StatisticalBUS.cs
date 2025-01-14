using DAO;
using DevExpress.XtraGrid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BUS
{
    public static class StatisticalBUS
    {
        private static ManagementShopClothesEntities1 db = new ManagementShopClothesEntities1();

        public static double TotalEntrySlip(DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            return db.EntrySlips
                     .Where(x => x.isPay == true && x.createDate >= dateTimeFrom && x.createDate <= dateTimeTo)
                     .Sum(x => x.total.GetValueOrDefault());
        }

        public static double TotalInvoice(DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            return db.Invoices
                     .Where(x => x.isPay == true && x.createDate >= dateTimeFrom && x.createDate <= dateTimeTo)
                     .Sum(x => x.total.GetValueOrDefault());
        }

        public static DataTable loadDetailStatistical(GridControl gc, DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            DataTable tb = new DataTable();
            tb.Columns.Add("date");
            tb.Columns.Add("invoice");
            tb.Columns.Add("entrySlip");
            tb.Columns.Add("profit");

            var invoices = db.Invoices
                             .Where(x => x.isPay == true && x.createDate >= dateTimeFrom && x.createDate <= dateTimeTo)
                             .Select(x => new { x.createDate, x.total })
                             .ToList();

            var entrySlips = db.EntrySlips
                               .Where(x => x.isPay == true && x.createDate >= dateTimeFrom && x.createDate <= dateTimeTo)
                               .Select(x => new { x.createDate, x.total })
                               .ToList();

            for (DateTime date = dateTimeFrom.Date; date <= dateTimeTo.Date; date = date.AddDays(1))
            {
                var dailyInvoices = invoices
                    .Where(x => x.createDate.Value.Date == date)
                    .Sum(x => x.total.GetValueOrDefault());

                var dailyEntrySlips = entrySlips
                    .Where(x => x.createDate.Value.Date == date)
                    .Sum(x => x.total.GetValueOrDefault());

                if (dailyInvoices > 0 || dailyEntrySlips > 0)
                {
                    DataRow dr = tb.NewRow();
                    dr["date"] = date.ToShortDateString();
                    dr["invoice"] = Support.convertVND(dailyInvoices); 
                    dr["entrySlip"] = Support.convertVND(dailyEntrySlips);
                    dr["profit"] = Support.convertVND(dailyInvoices - dailyEntrySlips);
                    tb.Rows.Add(dr);
                }
            }

            gc.DataSource = tb;
            return tb;
        }

    }
}
