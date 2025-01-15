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
        public static void ClearCache(this ManagementShopClothesEntities1 context)
        {
            // Kiểm tra nếu context hợp lệ
            if (context == null) throw new ArgumentNullException(nameof(context));

            // Xóa tất cả các thực thể đang được theo dõi bởi DbContext
            var entries = context.ChangeTracker.Entries().ToList();
            foreach (var entry in entries)
            {
                entry.State = EntityState.Detached; // Ngắt theo dõi
            }
        }
        // Đơn hàng phiếu nhập trong tháng hiện tại
        public static DataTable loadInvoiceAndEntrySlipMonthNow()
        {
            ClearCache(db);
            DataTable tb = new DataTable();
            tb.Columns.Add("name");
            tb.Columns.Add("quantity");
            DataRow dr = tb.NewRow();
            dr[0] = "Hoá đơn";
            dr[1] = db.Invoices.Count(x => x.createDate.Value.Month == DateTime.Now.Month && x.createDate.Value.Year == DateTime.Now.Year);
            tb.Rows.Add(dr);
            dr = tb.NewRow();
            dr[0] = "Phiếu nhập";
            dr[1] = db.EntrySlips.Count(x => x.createDate.Value.Month == DateTime.Now.Month && x.createDate.Value.Year == DateTime.Now.Year);
            tb.Rows.Add(dr);

            return tb;
        }

        //top sản phẩm bán chạy nhất
        public static DataTable loadTopSelling()
        {
            ClearCache(db);
            return Support.ToDataTable(db.TopSelling(30).OrderByDescending(x => x.quantity).ThenBy(x => x.name).ToList());
        }
        //load doanh thu năm hiện tại
        public static DataTable loadStatisticalYear()
        {
            ClearCache(db);
            DataTable tb = new DataTable();
            tb.Columns.Add("name");
            tb.Columns.Add("money");
            DataRow dr = tb.NewRow();
            dr[0] = "Tiền thu";
            dr[1] = db.Invoices.Where(x => x.createDate.Value.Year == DateTime.Now.Year).ToList().Sum(x => x.total);
            tb.Rows.Add(dr);
            dr = tb.NewRow();
            dr[0] = "Tiền chi";
            dr[1] = db.EntrySlips.Where(x => x.createDate.Value.Year == DateTime.Now.Year).ToList().Sum(x => x.total);
            tb.Rows.Add(dr);

            return tb;
        }
        //sản phẩm sắp hết hàng <=5
        public static DataTable loadProductNotStock()
        {
            ClearCache(db);
            DataTable tb = new DataTable();
            tb.Columns.Add("name");
            tb.Columns.Add("quantity");
            var list = db.Products.ToList().Where(x => x.quantity <= 5);
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
