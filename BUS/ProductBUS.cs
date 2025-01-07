﻿using DAO;
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
    public static class  ProductBUS
    {

        private static ManagementShopClothesEntities1 db = new ManagementShopClothesEntities1();
        public static void GetDataLk(RepositoryItemLookUpEdit lk, int supplierId = 0)
        {
            ClearCache(db);
            if (supplierId == 0)
                lk.DataSource = from item in db.Products select item;
            else
                lk.DataSource = from item in db.Products where item.supplierId == supplierId select item;
            lk.DisplayMember = "name";
            lk.ValueMember = "id";
        }
        public static void ClearCache(this ManagementShopClothesEntities1 context)
        {
            const BindingFlags FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var method = context.GetType().GetMethod("ClearCache", FLAGS);
            method.Invoke(context, null);
        }
        public static void GetDataGV(GridControl gv)
        {
            ClearCache(db);
            var lst = (from item in db.Products select item).ToList();
            gv.DataSource = Support.ToDataTable<Product>(lst);
        }
        public static int Insert(Product model)
        {
            try
            {
                db.Products.Add(model);
                db.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {

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
                modelUpdate.name = model.name;
                modelUpdate.image = model.image;
                modelUpdate.price = model.price;
                modelUpdate.Supplier = db.Suppliers.SingleOrDefault(x => x.id == model.supplierId);
                modelUpdate.supplierId = model.supplierId;
                modelUpdate.Size = db.Sizes.SingleOrDefault(x => x.id == model.sizeId);
                modelUpdate.sizeId = model.sizeId;
                modelUpdate.Color = db.Colors.SingleOrDefault(x => x.id == model.colorId);
                modelUpdate.colorId = model.colorId;
                modelUpdate.Material = db.Materials.SingleOrDefault(x => x.id == model.materialId);
                modelUpdate.materialId = model.materialId;
                modelUpdate.Category = db.Categories.SingleOrDefault(x => x.id == model.categoryId);
                modelUpdate.categoryId = model.categoryId;
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
                var model = db.Products.SingleOrDefault(x => x.id == id);
                if (model == null)
                    return -1;
                db.Products.Remove(model);
                db.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {

                return -1;
            }

        }
        public static Product FindById(int id)
        {
            ClearCache(db);
            return db.Products.SingleOrDefault(x => x.id == id);
        }

    }
}
