using DAO;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public static class ProductBUS
    {
        private static ManagementShopClothesEntities1 db = new ManagementShopClothesEntities1();
        public static void ClearCache(this ManagementShopClothesEntities1 context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            var entries = context.ChangeTracker.Entries().ToList();
            foreach (var entry in entries)
            {
                entry.State = EntityState.Detached;
            }
        }
        public static void GetDataLk(RepositoryItemLookUpEdit lk, int supplierId = 0)
        {
            var query = supplierId == 0
                        ? db.Products.AsQueryable()
                        : db.Products.Where(item => item.supplierId == supplierId);

            lk.DataSource = query.ToList();
            lk.DisplayMember = "name";
            lk.ValueMember = "id";
        }

        public static void GetDataGV(GridControl gv)
        {
            ClearCache(db);
            var products = db.Products.ToList();
            gv.DataSource = Support.ToDataTable<Product>(products);
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
                ClearCache(db);
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
                ClearCache(db);
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
