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
    public class TypeOfCustomerBUS
    {
        private static ManagementShopClothesEntities1 db = new ManagementShopClothesEntities1();

        public static void GetDataLk(RepositoryItemLookUpEdit lk)
        {
            lk.DataSource = from item in db.TypeOfCustomers select item;
            lk.DisplayMember = "name";
            lk.ValueMember = "id";
        }
        public static void GetDataLk(LookUpEdit lk)
        {
            lk.Properties.DataSource = from item in db.TypeOfCustomers select item;
            lk.Properties.DisplayMember = "name";
            lk.Properties.ValueMember = "id";
        }
        public static void GetDataGV(GridControl gv)
        {
            var lst = (from item in db.TypeOfCustomers select item).ToList();
            gv.DataSource = Support.ToDataTable<TypeOfCustomer>(lst);
        }
        public static int Insert(TypeOfCustomer model)
        {
            if (db.TypeOfCustomers.SingleOrDefault(x => x.name.ToLower().Equals(model.name.ToLower())) != null)
                return 0;
            try
            {
                db.TypeOfCustomers.Add(model);
                db.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {

                return -1;
            }

        }
        public static int Update(TypeOfCustomer model)
        {
            if (db.TypeOfCustomers.SingleOrDefault(x =>x.id!=model.id&& x.name.ToLower().Equals(model.name.ToLower())) != null)
                return 0;
            var modelUpdate = db.TypeOfCustomers.FirstOrDefault(x => x.id == model.id);
            try
            {
                if (modelUpdate == null)
                    return -1;
                modelUpdate.name = model.name;
                modelUpdate.discount = model.discount;
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
                var model = db.TypeOfCustomers.SingleOrDefault(x => x.id == id);
                if (model == null)
                    return -1;
                db.TypeOfCustomers.Remove(model);
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
