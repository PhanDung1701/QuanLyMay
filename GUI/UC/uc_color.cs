using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BUS;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using System.IO;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors.ViewInfo;
using System.Reflection;
using DevExpress.Utils.Menu;
using GUI.FRM;
using DAO;

namespace GUI.UC
{
    public partial class uc_color : DevExpress.XtraEditors.XtraUserControl
    {
        private frmMain frm;
        private ImageCollection images = new ImageCollection(); //{ ImageSize=new Size(20, 20) };
        private OpenFileDialog open;
        public uc_color(frmMain frm)
        {
            InitializeComponent();
            this.frm = frm;

        }

        private void btnDong_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frm._close();
        }

        private void uc_staff_Load(object sender, EventArgs e)
        {
            ColorBUS.GetDataGV(gcColor);
            gvColor.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
            gvColor.IndicatorWidth = 50;
        }

        #region màu sắc
        //phím delete
        private void gcColor_ProcessGridKey(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && gvColor.State != GridState.Editing)
            {
                DataRow dr = gvColor.GetFocusedDataRow();
                if (dr != null)
                {
                    if (XtraMessageBox.Show("Bạn có muốn xoá màu sắc " + dr["name"].ToString() + " ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        int id = int.Parse(dr["id"].ToString());

                        int i = ColorBUS.Delete(id);
                        if (i == 1)
                        {
                            XtraMessageBox.Show("Xoá thành công", "Thông báo");
                            ColorBUS.GetDataGV(gcColor);
                        }
                        else
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        //đánh số thứ tự
        private void gvColor_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (!e.Info.IsRowIndicator || e.RowHandle < 0)
                return;
            e.Info.DisplayText = (e.RowHandle + 1) + "";
        }
        //ngăn không cho chuyển dòng khi sai dữ liệu
        private void gvColor_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }
        //thêm sửa
        private void gvColor_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            string sErr = "";
            bool bVali = true;
            if (gvColor.GetRowCellValue(e.RowHandle, "name").ToString().Trim() == "")
            {
                bVali = false;
                sErr = "Vui lòng điền tên màu sắc.\n";
            }
            if (bVali)
            {
                //thêm mới
                if (e.RowHandle < 0)
                {
                    try
                    {
                        var model = new DAO.Color
                        {
                            name = gvColor.GetRowCellValue(e.RowHandle, "name").ToString().Trim(),
                            note = gvColor.GetRowCellValue(e.RowHandle, "note").ToString().Trim(),
                        };
                        int i = ColorBUS.Insert(model);
                        open = null;
                        if (i == 1)
                            XtraMessageBox.Show("Thêm thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        else
                        if (i == 0)
                            XtraMessageBox.Show("Trùng tên màu sắc.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        else
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    catch (Exception ex)
                    {

                    }
                    ColorBUS.GetDataGV(gcColor);
                }
                //sửa 
                else
                {
                    int i = -1;
                    try
                    {
                        var model = new DAO.Color
                        {
                            id = int.Parse(gvColor.GetRowCellValue(e.RowHandle, "id").ToString().Trim()),
                            name = gvColor.GetRowCellValue(e.RowHandle, "name").ToString().Trim(),
                            note = gvColor.GetRowCellValue(e.RowHandle, "note").ToString().Trim(),
                        };
                        i = ColorBUS.Update(model);
                        open = null;
                    }
                    catch (Exception)
                    {

                    }
                    if (i == -1)
                        XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        if (i == 0)
                        XtraMessageBox.Show("Trùng tên màu sắc.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    ColorBUS.GetDataGV(gcColor);
                }
            }
            else
            {

                e.Valid = false;

                XtraMessageBox.Show(sErr, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //xoá 
        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataRow dr = gvColor.GetFocusedDataRow();
            if (dr != null)
            {
                if (XtraMessageBox.Show("Bạn có muốn xoá màu sắc " + dr["name"].ToString() + " ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int i = ColorBUS.Delete(int.Parse(dr["id"].ToString()));
                    if (i == 1)
                    {
                        XtraMessageBox.Show("Xoá thành công", "Thông báo");
                        ColorBUS.GetDataGV(gcColor);
                    }
                    else
                        XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }
        //xuất ra file excel 
        private void btnExcel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "Excel Files (*.xlsx)|*.xls";
            sf.Title = "Xuất ra file excel";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                gvColor.ExportToXls(sf.FileName);
                string str = "màu sắc";
                XtraMessageBox.Show("Xuất file excel " + str + " thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
        //xuất ra file word
        private void btnWord_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "Word Files (*.docx)|*.docx";
            sf.Title = "Xuất ra file word";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                gvColor.ExportToXls(sf.FileName);
                string str = "màu sắc";
                XtraMessageBox.Show("Xuất file word " + str + " thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
        //xuất ra file Pdf
        private void btnPdf_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "Pdf Files (*.pdf)|*.pdf";
            sf.Title = "Xuất ra file pdf";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                gvColor.ExportToXls(sf.FileName);
                string str = "màu sắc";
                XtraMessageBox.Show("Xuất file pdf " + str + " thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
        #endregion


    }
}
