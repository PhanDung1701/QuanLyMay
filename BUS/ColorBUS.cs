using DAO;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace BUS
{
    public class ColorBUS
    {
        private static ManagementShopClothesEntities1 db = new ManagementShopClothesEntities1();

        public static void GetDataLk(RepositoryItemLookUpEdit lk)
        {
            lk.DataSource = db.Colors.ToList();
            lk.DisplayMember = "name";
            lk.ValueMember = "id";
        }

        public static void GetDataGV(GridControl gv)
        {
            var lst = db.Colors.ToList();
            gv.DataSource = Support.ToDataTable<Color>(lst);
        }

        public static int Insert(Color model)
        {
            if (db.Colors.Any(x => x.name.ToLower() == model.name.ToLower()))
                return 0;
            try
            {
                db.Colors.Add(model);
                db.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public static int Update(Color model)
        {
            if (db.Colors.Any(x => x.id != model.id && x.name.ToLower() == model.name.ToLower()))
                return 0;
            try
            {
                var modelUpdate = db.Colors.Find(model.id);
                if (modelUpdate == null)
                    return -1;

                modelUpdate.name = model.name;
                modelUpdate.note = model.note;
                db.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public static int Delete(int id)
        {
            try
            {
                var model = db.Colors.Find(id);
                if (model == null)
                    return -1;

                db.Colors.Remove(model);
                db.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}