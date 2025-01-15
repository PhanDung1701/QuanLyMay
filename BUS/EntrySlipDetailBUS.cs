using DAO;
using DevExpress.XtraGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class EntrySlipDetailBUS
    {
        private static ManagementShopClothesEntities1 db = new ManagementShopClothesEntities1();

        public static void GetDataGV(GridControl gv, int entrySlipId)
        {
            // Xóa dòng này vì không có Refresh trong Entity Framework
            // db.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, db.EntrySlipDetails);

            // Làm mới tất cả đối tượng EntrySlipDetails từ cơ sở dữ liệu
            foreach (var item in db.EntrySlipDetails)
            {
                db.Entry(item).Reload();  // Làm mới từng đối tượng
            }

            var lst = (from item in db.EntrySlipDetails
                       where item.entrySlipId == entrySlipId
                       select item).ToList();
            gv.DataSource = Support.ToDataTable<EntrySlipDetail>(lst);
        }

        public static List<EntrySlipDetail> GetDataGV(int entrySlipId)
        {

            foreach (var item in db.EntrySlipDetails)
            {
                db.Entry(item).Reload();  // Làm mới từng đối tượng
            }

            return (from item in db.EntrySlipDetails
                    where item.entrySlipId == entrySlipId
                    select item).ToList();
        }

        public static int Insert(EntrySlipDetail model)
        {
            var modelUpdate = db.EntrySlipDetails.FirstOrDefault(x => x.entrySlipId == model.entrySlipId && x.productId == model.productId);

            if (modelUpdate != null)
            {
                model.quantity += modelUpdate.quantity;
                model.id = modelUpdate.id;
                Update(model);
                return 2;
            }
            else
            {
                db.EntrySlipDetails.Add(model);
                db.SaveChanges();

                // Tăng quantity trong Product
                var product = db.Products.SingleOrDefault(x => x.id == model.productId);
                if (product != null)
                {
                    product.quantity += model.quantity;
                    db.SaveChanges();
                }

                return 1;
            }
        }


        public static int Update(EntrySlipDetail model)
        {
            if (model.quantity == 0)
            {
                Delete(model.id);
                return 2;
            }
            else
            {
                var modelUpdate = db.EntrySlipDetails.FirstOrDefault(x => x.id == model.id);
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
            var model = db.EntrySlipDetails.FirstOrDefault(x => x.id == id);
            if (model == null)
                return -1;

            // Giảm quantity trong Product
            var product = db.Products.SingleOrDefault(x => x.id == model.productId);
            if (product != null)
            {
                product.quantity -= model.quantity;
                if (product.quantity < 0)
                    product.quantity = 0; // Đảm bảo không âm
            }

            db.EntrySlipDetails.Remove(model);
            db.SaveChanges();
            return 1;
        }


        public static bool IsProduct(int productId)
        {
            return db.EntrySlipDetails.FirstOrDefault(x => x.productId == productId) != null;
        }
    }
}