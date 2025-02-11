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
    public static class EntrySlipBUS
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
            foreach (var item in db.EntrySlips)
            {
                db.Entry(item).Reload();
            }

            ClearCache(db);
            List<EntrySlip> lst;
            if (isPay)
                lst = (from item in db.EntrySlips select item).ToList();
            else
                lst = (from item in db.EntrySlips where item.isPay == false select item).ToList();

            gv.DataSource = Support.ToDataTable<EntrySlip>(lst);
        }


        public static int Insert(EntrySlip model)
        {
            try
            {
                db.EntrySlips.Add(model);
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
                var model = db.EntrySlips.FirstOrDefault(x => x.id == id);
                db.EntrySlips.Remove(model);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }
        public static int Update(int id, bool isPay)
        {
            try
            {
                var nk = db.EntrySlips.FirstOrDefault(x => x.id == id);
                nk.isPay = isPay;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }
        public static EntrySlip GetLast()
        {
            return db.EntrySlips.OrderByDescending(x => x.id).FirstOrDefault();
        }
        public static EntrySlip FindById(int id)
        {
            return db.EntrySlips.FirstOrDefault(x => x.id == id);
        }
        public static bool IsStaff(int staffId)
        {
            return db.EntrySlips.FirstOrDefault(x => x.staffId == staffId) != null;
        }
    }
}
