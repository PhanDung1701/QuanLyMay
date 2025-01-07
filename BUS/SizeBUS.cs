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
    public class SizeBUS
    {
        private static ManagementShopClothesEntities1 db = new ManagementShopClothesEntities1();

        public static void GetDataLk(RepositoryItemLookUpEdit lk)
        {
            lk.DataSource = from item in db.Sizes select item;
            lk.DisplayMember = "name";
            lk.ValueMember = "id";
        }
        public static void GetDataLk(LookUpEdit lk)
        {
            lk.Properties.DataSource = from item in db.Sizes select item;
            lk.Properties.DisplayMember = "name";
            lk.Properties.ValueMember = "id";
        }
        public static void GetDataGV(GridControl gv)
        {
            var lst = (from item in db.Sizes select item).ToList();
            gv.DataSource = Support.ToDataTable<Size>(lst);
        }
        public static int Insert(Size model)
        {
            if (db.Sizes.SingleOrDefault(x => x.name.ToLower().Equals(model.name.ToLower())) != null)
                return 0;
            try
            {
                db.Sizes.Add(model);
                db.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {

                return -1;
            }

        }
        public static int Update(Size model)
        {
            if (db.Sizes.SingleOrDefault(x =>x.id!=model.id&& x.name.ToLower().Equals(model.name.ToLower())) != null)
                return 0;
            var modelUpdate = db.Sizes.FirstOrDefault(x => x.id == model.id);
            try
            {
                if (modelUpdate == null)
                    return -1;
                modelUpdate.name = model.name;
                modelUpdate.note = model.note;
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
                var model = db.Sizes.SingleOrDefault(x => x.id == id);
                if (model == null)
                    return -1;
                db.Sizes.Remove(model);
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
