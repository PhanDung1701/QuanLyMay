﻿using DAO;
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
    public static class InvoiceBUS
    {
        private static ManagementShopClothesEntities1 db = new ManagementShopClothesEntities1();
        public static void ClearCache(this ManagementShopClothesEntities1 context)
        {
            // Kiểm tra nếu context hợp lệ
            if (context == null) throw new ArgumentNullException(nameof(context));

            // Xóa tất cả các thực thể đang được theo dõi bởi DbContext
            var entries = context.ChangeTracker.Entries().ToList();
            foreach (var entry in entries)
            {
                entry.State = EntityState.Detached; // Ngắt theo dõi
            }
        }
        public static void GetDataGV(GridControl gv, bool isPay = true)
        {
            foreach (var item in db.Invoices)
            {
                db.Entry(item).Reload();
            }

            ClearCache(db);
            List<Invoice> lst;
            if (isPay)
                lst = (from item in db.Invoices select item).ToList();
            else
                lst = (from item in db.Invoices where item.isPay == false select item).ToList();

            gv.DataSource = Support.ToDataTable<Invoice>(lst);
        }

        public static int Insert(Invoice model)
        {
            try
            {
                db.Invoices.Add(model);
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
                var model = db.Invoices.SingleOrDefault(x => x.id == id);
                db.Invoices.Remove(model);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }
        public static int Update(int id,bool isPay)
        {
            try
            {
                var model = db.Invoices.FirstOrDefault(x => x.id == id);
                model.isPay = isPay;                
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }
        public static Invoice GetLast()
        {
            return db.Invoices.OrderByDescending(x => x.id).FirstOrDefault();
        }
        public static Invoice FindById(int id)
        {
            return db.Invoices.FirstOrDefault(x => x.id == id);
        }
        public static bool IsStaff(int staffId)
        {
            return db.Invoices.FirstOrDefault(x => x.staffId == staffId) != null;
        }
        public static bool IsCustomer(int customerId)
        {
            return db.Invoices.FirstOrDefault(x => x.customerId == customerId) != null;
        }
    }
}
