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
    public class MaterialBUS
    {
        private static ManagementShopClothesEntities1 db = new ManagementShopClothesEntities1();

        public static void GetDataLk(RepositoryItemLookUpEdit lk)
        {
            lk.DataSource = from item in db.Materials select item;
            lk.DisplayMember = "name";
            lk.ValueMember = "id";
        }
        public static void GetDataLk(LookUpEdit lk)
        {
            lk.Properties.DataSource = from item in db.Materials select item;
            lk.Properties.DisplayMember = "name";
            lk.Properties.ValueMember = "id";
        }
        public static void GetDataGV(GridControl gv)
        {
            var lst = (from item in db.Materials select item).ToList();
            gv.DataSource = Support.ToDataTable<Material>(lst);
        }
        public static int Insert(Material model)
        {
            if (db.Materials.SingleOrDefault(x => x.name.ToLower().Equals(model.name.ToLower())) != null)
                return 0;
            try
            {
                db.Materials.Add(model);
                db.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {

                return -1;
            }

        }
        public static int Update(Material model)
        {
            if (db.Materials.SingleOrDefault(x =>x.id!=model.id&& x.name.ToLower().Equals(model.name.ToLower())) != null)
                return 0;
            var modelUpdate = db.Materials.FirstOrDefault(x => x.id == model.id);
            try
            {
                if (modelUpdate == null)
                    return -1;
                modelUpdate.name = model.name;
                modelUpdate.note = model.note;
                modelUpdate.origin = model.origin;
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
                var model = db.Materials.SingleOrDefault(x => x.id == id);
                if (model == null)
                    return -1;
                db.Materials.Remove(model);
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
