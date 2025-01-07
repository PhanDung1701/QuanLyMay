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
            // Xóa dòng Refresh và thay thế bằng Reload cho từng đối tượng
            foreach (var item in db.InvoiceDetails)
            {
                db.Entry(item).Reload();  // Làm mới đối tượng InvoiceDetail
            }

            var lst = (from item in db.InvoiceDetails
                       where item.invoiceId == invoiceId
                       select item).ToList();

            gv.DataSource = Support.ToDataTable<InvoiceDetail>(lst);
        }

        public static List<InvoiceDetail> GetDataGV(int invoiceId)
        {
            // Xóa dòng Refresh và thay thế bằng Reload cho từng đối tượng
            foreach (var item in db.InvoiceDetails)
            {
                db.Entry(item).Reload();  // Làm mới đối tượng InvoiceDetail
            }

            return (from item in db.InvoiceDetails
                    where item.invoiceId == invoiceId
                    select item).ToList();
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
            var model = db.InvoiceDetails.FirstOrDefault(x => x.id==id);
            if (model == null)
                return -1;
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
