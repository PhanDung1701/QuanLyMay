using DAO;
using DevExpress.XtraGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public static class EntrySlipBUS
    {
        private static ManagementShopClothesEntities1 db = new ManagementShopClothesEntities1();

        public static void GetDataGV(GridControl gv, bool isPay = true)
        {
            IQueryable<EntrySlip> query;

            if (isPay)
            {
                query = db.EntrySlips.Where(x => x.isPay == true);
            }
            else
            {
                query = db.EntrySlips.Where(x => x.isPay == false);
            }

            var lst = query.ToList(); 
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
