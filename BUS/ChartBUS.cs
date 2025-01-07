using DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.Entity;

namespace BUS
{
    public static class ChartBUS
    {
        private static ManagementShopClothesEntities1 db = new ManagementShopClothesEntities1();

        // Đơn hàng phiếu nhập trong tháng hiện tại
        public static DataTable loadInvoiceAndEntrySlipMonthNow()
        {
            DataTable tb = new DataTable();
            tb.Columns.Add("name");
            tb.Columns.Add("quantity");

            DataRow dr = tb.NewRow();
            dr[0] = "Hoá đơn";
            dr[1] = db.Invoices
                .Where(x => x.createDate.Value.Month == DateTime.Now.Month && x.createDate.Value.Year == DateTime.Now.Year)
                .SelectMany(x => x.InvoiceDetails) // Lấy tất cả InvoiceDetails
                .Count();
            tb.Rows.Add(dr);

            dr = tb.NewRow();
            dr[0] = "Phiếu nhập";
            dr[1] = db.EntrySlips
                .Where(x => x.createDate.Value.Month == DateTime.Now.Month && x.createDate.Value.Year == DateTime.Now.Year)
                .SelectMany(x => x.EntrySlipDetails) // Lấy tất cả EntrySlipDetails
                .Count();
            tb.Rows.Add(dr);

            return tb;
        }

        // Top sản phẩm bán chạy nhất
        public static DataTable loadTopSelling()
        {
            // Giả sử TopSelling là một Stored Procedure
            return Support.ToDataTable(db.TopSelling(30)
                                       .OrderByDescending(x => x.quantity)
                                       .ThenBy(x => x.name)
                                       .ToList());
        }

        // Load doanh thu năm hiện tại
        public static DataTable loadStatisticalYear()
        {
            DataTable tb = new DataTable();
            tb.Columns.Add("name");
            tb.Columns.Add("money");

            DataRow dr = tb.NewRow();
            dr[0] = "Tiền thu";
            dr[1] = db.Invoices
                     .Where(x => x.createDate.Value.Year == DateTime.Now.Year)
                     .SelectMany(x => x.InvoiceDetails) // Lấy tất cả InvoiceDetails
                     .Sum(x => x.price.HasValue && x.quantity.HasValue ? x.price.Value * x.quantity.Value : 0); // Tính tổng tiền
            tb.Rows.Add(dr);

            dr = tb.NewRow();
            dr[0] = "Tiền chi";
            dr[1] = db.EntrySlips
                     .Where(x => x.createDate.Value.Year == DateTime.Now.Year)
                     .SelectMany(x => x.EntrySlipDetails)
                     .Sum(x => x.price.HasValue && x.quantity.HasValue ? x.price.Value * x.quantity.Value : 0);
            tb.Rows.Add(dr);

            return tb;
        }

        // Sản phẩm sắp hết hàng <=5
        public static DataTable loadProductNotStock()
        {
            DataTable tb = new DataTable();
            tb.Columns.Add("name");
            tb.Columns.Add("quantity");

            var list = db.Products
                        .Where(x => x.quantity <= 5)
                        .Select(x => new { x.name, x.quantity })
                        .ToList();

            foreach (var item in list)
            {
                DataRow dr = tb.NewRow();
                dr[0] = item.name;
                dr[1] = item.quantity;
                tb.Rows.Add(dr);
            }

            return tb;
        }
    }
}
