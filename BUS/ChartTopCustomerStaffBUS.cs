using DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
namespace BUS
{
    public static class ChartTopCustomerStaffBUS
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
        public static DataTable loadTopCustomerBuy(bool checkType, DateTime date)
        {
            ClearCache(db);
            return Support.ToDataTable(db.TopCustomerBuy(checkType, date).ToList());
        }
        public static DataTable loadTopStaffSell(bool checkType, DateTime date)
        {
            ClearCache(db);
            return Support.ToDataTable(db.TopStaffSell(checkType, date).ToList());
        }
    }
}
