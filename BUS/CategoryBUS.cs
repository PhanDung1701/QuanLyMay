using DAO;  
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BUS
{
    public class CategoryBUS
    {
        private static ManagementShopClothesEntities1 db = new ManagementShopClothesEntities1();

        public static void GetDataLk(RepositoryItemLookUpEdit lk)
        {
            lk.DataSource = db.Categories.ToList();
            lk.DisplayMember = "name";
            lk.ValueMember = "id";
        }

        public static void GetDataGV(GridControl gv)
        {
            var lst = db.Categories.ToList();
            gv.DataSource = Support.ToDataTable<Category>(lst);
        }

        public static int Insert(Category model)
        {
            if (db.Categories.Any(x => x.name.ToLower() == model.name.ToLower()))
                return 0;
            try
            {
                db.Categories.Add(new Category
                {
                    name = model.name,
                    quantity_unit = model.quantity_unit // Thêm trường mới
                });
                db.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return -1;
            }
        }


        public static int Update(Category model)
        {
            if (db.Categories.Any(x => x.id != model.id && x.name.ToLower() == model.name.ToLower()))
                return 0;

            try
            {
                var modelUpdate = db.Categories.Find(model.id);
                if (modelUpdate == null)
                    return -1;

                modelUpdate.name = model.name;
                modelUpdate.quantity_unit = model.quantity_unit; // Thêm trường mới
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
                var model = db.Categories.Find(id);
                if (model == null)
                    return -1;

                db.Categories.Remove(model);
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