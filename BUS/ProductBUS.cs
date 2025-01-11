using DAO;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public static class ProductBUS
    {
        private static ManagementShopClothesEntities1 db = new ManagementShopClothesEntities1();

        public static void GetDataLk(RepositoryItemLookUpEdit lk, int supplierId = 0)
        {
            if (supplierId == 0)
                lk.DataSource = from item in db.Products select item;
            else
                lk.DataSource = from item in db.Products where item.supplierId == supplierId select item;
            lk.DisplayMember = "name";
            lk.ValueMember = "id";
        }

        public static void GetDataGV(GridControl gv)
        {
            var lst = (from item in db.Products select item).ToList();
            gv.DataSource = Support.ToDataTable<Product>(lst);
        }

        public static int Insert(Product model)
        {
            try
            {
                // Tính toán quantity và remate
                var category = db.Categories.SingleOrDefault(x => x.id == model.categoryId);
                if (category != null && category.quantity_unit.HasValue && model.requimate.HasValue)
                {
                    model.quantity = model.requimate / category.quantity_unit;
                    model.remate = model.requimate % category.quantity_unit;
                }

                db.Products.Add(model);
                db.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return -1;
            }
        }

        public static int Update(Product model)
        {
            var modelUpdate = db.Products.SingleOrDefault(x => x.id == model.id);
            try
            {
                if (modelUpdate == null)
                    return -1;

                // Tính toán quantity và remate
                var category = db.Categories.SingleOrDefault(x => x.id == model.categoryId);
                if (category != null && category.quantity_unit.HasValue && model.requimate.HasValue)
                {
                    model.quantity = model.requimate / category.quantity_unit;
                    model.remate = model.requimate % category.quantity_unit;
                }

                modelUpdate.name = model.name;
                modelUpdate.image = model.image;
                modelUpdate.price = model.price;
                modelUpdate.supplierId = model.supplierId;
                modelUpdate.sizeId = model.sizeId;
                modelUpdate.colorId = model.colorId;
                modelUpdate.materialId = model.materialId;
                modelUpdate.categoryId = model.categoryId;
                modelUpdate.requimate = model.requimate;
                modelUpdate.quantity = model.quantity;
                modelUpdate.remate = model.remate;

                db.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return -1;
            }
        }


        public static int Delete(int id)
        {
            try
            {
                var model = db.Products.SingleOrDefault(x => x.id == id);
                if (model == null)
                    return -1;

                db.Products.Remove(model);
                db.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return -1;
            }
        }

        public static Product FindById(int id)
        {
            return db.Products.SingleOrDefault(x => x.id == id);
        }
    }

}
