using DAO;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class SupplierBUS
    {
        private static ManagementShopClothesEntities1 db = new ManagementShopClothesEntities1();

        public static void GetDataLk(RepositoryItemLookUpEdit lk)
        {
            lk.DataSource = db.Suppliers.ToList();
            lk.DisplayMember = "name";
            lk.ValueMember = "id";
        }
        public static void GetDataLk(LookUpEdit lk)
        {
            lk.Properties.DataSource = db.Suppliers.ToList(); // Entity Framework lấy dữ liệu
            lk.Properties.DisplayMember = "name"; // Trường hiển thị
            lk.Properties.ValueMember = "id"; // Trường giá trị
        }
        public static void GetDataGV(GridControl gv)
        {
            var lst = db.Suppliers.ToList();
            gv.DataSource = Support.ToDataTable<Supplier>(lst);
        }
        public static int Insert(Supplier model)
        {
            if (db.Suppliers.SingleOrDefault(x => x.name.ToLower().Equals(model.name.ToLower())) != null)
                return 0;
            try
            {
                db.Suppliers.Add(model);
                db.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {

                return -1;
            }

        }
        public static int Update(Supplier model)
        {
            if (db.Suppliers.SingleOrDefault(x =>x.id!=model.id&& x.name.ToLower().Equals(model.name.ToLower())) != null)
                return 0;
            var modelUpdate = db.Suppliers.FirstOrDefault(x => x.id == model.id);
            try
            {
                if (modelUpdate == null)
                    return -1;
                modelUpdate.name = model.name;
                modelUpdate.address = model.address;
                modelUpdate.phone = model.phone;
                db.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {

                return -1;
            }

        }
        public static int Delete(int id)
        {
            try
            {
                var model = db.Suppliers.SingleOrDefault(x => x.id == id);
                if (model == null)
                    return -1;
                db.Suppliers.Remove(model);
                db.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {

                return -1;
            }

        }
    }
}
