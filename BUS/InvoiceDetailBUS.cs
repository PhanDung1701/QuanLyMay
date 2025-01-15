using DAO;
using DevExpress.XtraGrid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class InvoiceDetailBUS
    {
        private static ManagementShopClothesEntities1 db = new ManagementShopClothesEntities1();
        public static void GetDataGV(GridControl gv, int invoiceId)
        {
            // Lấy dữ liệu từ cơ sở dữ liệu với AsNoTracking để giảm tải bộ nhớ
            var lst = db.InvoiceDetails
                        .AsNoTracking()
                        .Where(item => item.invoiceId == invoiceId)
                        .ToList();

            gv.DataSource = Support.ToDataTable<InvoiceDetail>(lst);
        }

        public static List<InvoiceDetail> GetDataGV(int invoiceId)
        {
            // Truy vấn và trả về danh sách không theo dõi
            return db.InvoiceDetails
                     .AsNoTracking()
                     .Where(item => item.invoiceId == invoiceId)
                     .ToList();
        }

        public static int Insert(InvoiceDetail model)
        {
            var modelUpdate = db.InvoiceDetails.FirstOrDefault(x => x.invoiceId == model.invoiceId && x.productId == model.productId);
            if (modelUpdate != null)
            {
                model.id = modelUpdate.id;
                model.quantity += modelUpdate.quantity;
                Update(model);
                return 2;
            }
            else
            {
                db.InvoiceDetails.Add(model);
                db.SaveChanges();

                // Giảm quantity trong Product
                var product = db.Products.SingleOrDefault(x => x.id == model.productId);
                if (product != null)
                {
                    product.quantity -= model.quantity;
                    if (product.quantity < 0)
                        product.quantity = 0; // Đảm bảo không âm
                    db.SaveChanges();
                }

                return 1;
            }
        }

        public static int Update(InvoiceDetail model)
        {
            if (model.quantity == 0)
            {
                Delete(model.id);
                return 2;
            }
            else
            {

                var modelUpdate = db.InvoiceDetails.FirstOrDefault(x => x.invoiceId == model.invoiceId && x.productId == model.productId);
                if (modelUpdate == null)
                    return -1;
                modelUpdate.quantity = model.quantity;
                modelUpdate.price = model.price;
                db.SaveChanges();
                return 1;

            }
        }
        public static int Delete(int id)
        {
            var model = db.InvoiceDetails.FirstOrDefault(x => x.id == id);
            if (model == null)
                return -1;

            // Tăng quantity trong Product
            var product = db.Products.SingleOrDefault(x => x.id == model.productId);
            if (product != null)
            {
                product.quantity += model.quantity;
                db.SaveChanges();
            }

            db.InvoiceDetails.Remove(model);
            db.SaveChanges();
            return 1;
        }

        public static InvoiceDetail FindByID(int id)
        {
            return db.InvoiceDetails.SingleOrDefault(x => x.id==id);
        }
        public static bool IsProduct(int productId)
        {
            return db.InvoiceDetails.FirstOrDefault(x => x.productId == productId) != null;
        }
    }
}
