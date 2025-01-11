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
    public partial class uc_product : DevExpress.XtraEditors.XtraUserControl
    {
        private frmMain frm;
        private ImageCollection images = new ImageCollection(); //{ ImageSize=new Size(20, 20) };
        private OpenFileDialog open;
        public uc_product(frmMain frm)
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
            ProductBUS.GetDataGV(gcProduct);
            CategoryBUS.GetDataGV(gcCategory);
            ColorBUS.GetDataLk(lkColor);
            CategoryBUS.GetDataLk(lkCategory);
            SizeBUS.GetDataLk(lkSize);
            SupplierBUS.GetDataLk(lkSupplier);
            MaterialBUS.GetDataLk(lkMaterial);
            gvProduct.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
            gvCategory.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;

            gvProduct.IndicatorWidth = 50;
            gvCategory.IndicatorWidth = 50;
        }

        #region quần áo, loại quần áo
        //xoá 
        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 0)
            {
                DataRow dr = gvProduct.GetFocusedDataRow();
                if (dr != null)
                {
                    if (XtraMessageBox.Show("Bạn có muốn xoá quần áo " + dr["name"].ToString() + " ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        int i = ProductBUS.Delete(int.Parse(dr["id"].ToString()));
                        if (i == 1)
                        {
                            XtraMessageBox.Show("Xoá thành công", "Thông báo");
                            ProductBUS.GetDataGV(gcProduct);
                        }
                        else
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }
            }
            else
            {
                DataRow dr = gvCategory.GetFocusedDataRow();
                if (dr != null)
                {
                    if (XtraMessageBox.Show("Bạn có muốn xoá loại quần áo " + dr["name"].ToString() + " ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        int i = CategoryBUS.Delete(int.Parse(dr["id"].ToString()));
                        if (i == 1)
                        {
                            XtraMessageBox.Show("Xoá thành công", "Thông báo");
                            CategoryBUS.GetDataGV(gcCategory);
                        }
                        else
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        //xuất ra file excel quần áo, loại quần áo
        private void btnExcel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "Excel Files (*.xlsx)|*.xls";
            sf.Title = "Xuất ra file excel";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                string str = "quần áo";
                if (xtraTabControl1.SelectedTabPageIndex == 0)
                    gvProduct.ExportToXls(sf.FileName);
                else 
                {
                    gvCategory.ExportToXls(sf.FileName);
                    str = "loại quần áo";
                }
                
                XtraMessageBox.Show("Xuất file excel " + str + " thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
        //xuất ra file word quần áo, loại quần áo
        private void btnWord_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "Word Files (*.docx)|*.docx";
            sf.Title = "Xuất ra file word";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                string str = "quần áo";
                if (xtraTabControl1.SelectedTabPageIndex == 0)
                    gvProduct.ExportToDocx(sf.FileName);
                else
                {
                    gvCategory.ExportToDocx(sf.FileName);
                    str = "loại quần áo";
                }
                XtraMessageBox.Show("Xuất file word " + str + " thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
        //xuất ra file Pdf quần áo, loại quần áo
        private void btnPdf_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "Pdf Files (*.pdf)|*.pdf";
            sf.Title = "Xuất ra file pdf";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                string str = "quần áo";
                if (xtraTabControl1.SelectedTabPageIndex == 0)
                    gvProduct.ExportToPdf(sf.FileName);
                else if (xtraTabControl1.SelectedTabPageIndex == 1)
                {
                    gvCategory.ExportToPdf(sf.FileName);
                    str = "loại quần áo";
                }
               
                XtraMessageBox.Show("Xuất file pdf " + str + " thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
        #endregion
        #region quần áo
        //phím delete
        private void gcProduct_ProcessGridKey(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && gvProduct.State != GridState.Editing)
            {
                DataRow dr = gvProduct.GetFocusedDataRow();
                if (dr != null)
                {
                    if (XtraMessageBox.Show("Bạn có muốn xoá quần áo " + dr["name"].ToString() + " ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        int id = int.Parse(dr["id"].ToString());

                        int i = ProductBUS.Delete(id);
                        if (i == 1)
                        {
                            XtraMessageBox.Show("Xoá thành công", "Thông báo");
                            ProductBUS.GetDataGV(gcProduct);
                        }
                        else
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        //thay đổi hình ảnh sách
        private void imageProduct_Click(object sender, EventArgs e)
        {
            open = new OpenFileDialog();
            if (open.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(open.FileName);
                if (!File.Exists("../../Images/" + open.SafeFileName))
                {
                    pictureBox1.Image.Save("../../Images/" + open.SafeFileName);
                }
                try
                {
                    DataRow dr = gvProduct.GetFocusedDataRow();
                    var model = new Product
                    {
                        id = int.Parse(dr["id"].ToString().Trim()),
                        name = dr["name"].ToString().Trim(),
                        image = open.SafeFileName,
                        price = double.Parse(dr["price"].ToString().Trim()),
                        categoryId = int.Parse(dr["categoryId"].ToString().Trim()),
                        colorId = int.Parse(dr["colorId"].ToString().Trim()),
                        supplierId = int.Parse(dr["supplierId"].ToString().Trim()),
                        sizeId = int.Parse(dr["sizeId"].ToString().Trim()),
                        materialId = int.Parse(dr["materialId"].ToString().Trim()),
                    };
                    int i = ProductBUS.Update(model);
                    if (i == 1)
                        ProductBUS.GetDataGV(gcProduct);
                    else
                        XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    open = null;
                }
                catch (Exception)
                {

                }
            }
        }
        //load hình ảnh
        private void gvProduct_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if (gvProduct.GetDataRow(e.RowHandle) != null && gvProduct.GetDataRow(e.RowHandle)["image"] != null)
            {
                string imageName = gvProduct.GetDataRow(e.RowHandle)["image"].ToString();
                if (!string.IsNullOrEmpty(imageName))
                {
                    try
                    {
                        Image img = Image.FromFile("../../Images/" + imageName);
                        images.Images.Clear();
                        images.Images.Add(img);
                    }
                    catch (Exception ex)
                    {
                        Image img = Image.FromFile("../../Images/loadImg.png");
                        images.Images.Clear();
                        images.Images.Add(img);
                    }

                    imageProduct.Images = images;
                }
            }
        }
        //thêm sửa
        private void gvProduct_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            string sErr = "";
            bool bVali = true;

            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrWhiteSpace(gvProduct.GetRowCellValue(e.RowHandle, "name")?.ToString()))
            {
                bVali = false;
                sErr = "Vui lòng điền tên sản phẩm.\n";
            }
            if (string.IsNullOrWhiteSpace(gvProduct.GetRowCellValue(e.RowHandle, "categoryId")?.ToString()))
            {
                bVali = false;
                sErr += "Vui lòng chọn thể loại.\n";
            }
            if (string.IsNullOrWhiteSpace(gvProduct.GetRowCellValue(e.RowHandle, "colorId")?.ToString()))
            {
                bVali = false;
                sErr += "Vui lòng chọn màu sắc.\n";
            }
            if (string.IsNullOrWhiteSpace(gvProduct.GetRowCellValue(e.RowHandle, "sizeId")?.ToString()))
            {
                bVali = false;
                sErr += "Vui lòng chọn kích cỡ.\n";
            }
            if (string.IsNullOrWhiteSpace(gvProduct.GetRowCellValue(e.RowHandle, "materialId")?.ToString()))
            {
                bVali = false;
                sErr += "Vui lòng chọn chất liệu.\n";
            }
            if (string.IsNullOrWhiteSpace(gvProduct.GetRowCellValue(e.RowHandle, "supplierId")?.ToString()))
            {
                bVali = false;
                sErr += "Vui lòng chọn nhà cung cấp.\n";
            }
            if (string.IsNullOrWhiteSpace(gvProduct.GetRowCellValue(e.RowHandle, "price")?.ToString()))
            {
                bVali = false;
                sErr += "Vui lòng điền giá sản phẩm.\n";
            }
            if (string.IsNullOrWhiteSpace(gvProduct.GetRowCellValue(e.RowHandle, "requimate")?.ToString()))
            {
                bVali = false;
                sErr += "Vui lòng nhập số lượng yêu cầu (requimate).\n";
            }

            // Xử lý khi tất cả các trường hợp hợp lệ
            if (bVali)
            {
                try
                {
                    // Lấy giá trị requimate và categoryId
                    int requimate = int.Parse(gvProduct.GetRowCellValue(e.RowHandle, "requimate").ToString().Trim());
                    int categoryId = int.Parse(gvProduct.GetRowCellValue(e.RowHandle, "categoryId").ToString().Trim());

                    // Lấy thông tin category để tính toán quantity và remate
                    var category = CategoryBUS.FindById(categoryId);
                    int quantity = 0;
                    int remate = 0;

                    if (category != null && category.quantity_unit.HasValue)
                    {
                        int quantityUnit = category.quantity_unit.Value;
                        quantity = requimate / quantityUnit; // Tính quantity
                        remate = requimate % quantityUnit;  // Tính remate
                    }
                    else
                    {
                        throw new Exception("Không tìm thấy thông tin đơn vị tính (quantity_unit) cho thể loại này.");
                    }

                    // Thêm mới
                    if (e.RowHandle < 0)
                    {
                        var model = new Product
                        {
                            name = gvProduct.GetRowCellValue(e.RowHandle, "name").ToString().Trim(),
                            image = open == null || open.SafeFileName == null ? gvProduct.GetRowCellValue(e.RowHandle, "image").ToString() : open.SafeFileName,
                            price = double.Parse(gvProduct.GetRowCellValue(e.RowHandle, "price").ToString().Trim()),
                            categoryId = categoryId,
                            colorId = int.Parse(gvProduct.GetRowCellValue(e.RowHandle, "colorId").ToString().Trim()),
                            sizeId = int.Parse(gvProduct.GetRowCellValue(e.RowHandle, "sizeId").ToString().Trim()),
                            materialId = int.Parse(gvProduct.GetRowCellValue(e.RowHandle, "materialId").ToString().Trim()),
                            supplierId = int.Parse(gvProduct.GetRowCellValue(e.RowHandle, "supplierId").ToString().Trim()),
                            requimate = requimate,
                            quantity = quantity,
                            remate = remate
                        };
                        int i = ProductBUS.Insert(model);
                        open = null;

                        if (i == 1)
                            XtraMessageBox.Show("Thêm thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        else
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    // Sửa
                    else
                    {
                        var model = new Product
                        {
                            id = int.Parse(gvProduct.GetRowCellValue(e.RowHandle, "id").ToString().Trim()),
                            name = gvProduct.GetRowCellValue(e.RowHandle, "name").ToString().Trim(),
                            image = open == null || open.SafeFileName == null ? gvProduct.GetRowCellValue(e.RowHandle, "image").ToString() : open.SafeFileName,
                            price = double.Parse(gvProduct.GetRowCellValue(e.RowHandle, "price").ToString().Trim()),
                            categoryId = categoryId,
                            colorId = int.Parse(gvProduct.GetRowCellValue(e.RowHandle, "colorId").ToString().Trim()),
                            sizeId = int.Parse(gvProduct.GetRowCellValue(e.RowHandle, "sizeId").ToString().Trim()),
                            materialId = int.Parse(gvProduct.GetRowCellValue(e.RowHandle, "materialId").ToString().Trim()),
                            supplierId = int.Parse(gvProduct.GetRowCellValue(e.RowHandle, "supplierId").ToString().Trim()),
                            requimate = requimate,
                            quantity = quantity,
                            remate = remate
                        };
                        int i = ProductBUS.Update(model);
                        open = null;

                        if (i == -1)
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    // Cập nhật lại GridControl
                    ProductBUS.GetDataGV(gcProduct);
                }
                catch (Exception ex)
                {
                    e.Valid = false;
                    XtraMessageBox.Show($"Lỗi: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                e.Valid = false;
                XtraMessageBox.Show(sErr, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //đánh số thứ tự bảng sách
        private void gvProduct_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (!e.Info.IsRowIndicator || e.RowHandle < 0)
                return;
            e.Info.DisplayText = (e.RowHandle + 1) + "";
        }
        //ngăn không cho chuyển dòng khi sai dữ liệu sách
        private void gvProduct_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }
        #endregion
        #region loại quần áo
        //phím delete xoá loại quần áo
        private void gcCategory_ProcessGridKey(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && gvCategory.State != GridState.Editing)
            {
                DataRow dr = gvCategory.GetFocusedDataRow();
                if (dr != null)
                {
                    if (XtraMessageBox.Show("Bạn có muốn xoá loại quần áo " + dr["name"].ToString() + " ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        int id = int.Parse(dr["id"].ToString());

                        int i = CategoryBUS.Delete(id);
                        if (i == 1)
                        {
                            XtraMessageBox.Show("Xoá thành công", "Thông báo");
                            CategoryBUS.GetDataGV(gcCategory);
                        }
                        else
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        //đánh số thứ tự
        private void gvCategory_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (!e.Info.IsRowIndicator || e.RowHandle < 0)
                return;
            e.Info.DisplayText = (e.RowHandle + 1) + "";
        }
        //ngăn không cho chuyển dòng khi sai dữ liệu
        private void gvCategory_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }
        //thêm sửa
        private void gvCategory_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            string sErr = "";
            bool bVali = true;

            if (gvCategory.GetRowCellValue(e.RowHandle, "name").ToString().Trim() == "")
            {
                bVali = false;
                sErr += "Vui lòng điền tên loại quần áo.\n";
            }

            if (string.IsNullOrWhiteSpace(gvCategory.GetRowCellValue(e.RowHandle, "quantity_unit")?.ToString()) ||
                !int.TryParse(gvCategory.GetRowCellValue(e.RowHandle, "quantity_unit")?.ToString(), out _))
            {
                bVali = false;
                sErr += "Vui lòng nhập số lượng (quantity_unit) hợp lệ.\n";
            }

            if (bVali)
            {
                // Thêm mới
                if (e.RowHandle < 0)
                {
                    try
                    {
                        var model = new Category
                        {
                            name = gvCategory.GetRowCellValue(e.RowHandle, "name").ToString().Trim(),
                            quantity_unit = int.Parse(gvCategory.GetRowCellValue(e.RowHandle, "quantity_unit").ToString().Trim()) // Thêm trường mới
                        };
                        int i = CategoryBUS.Insert(model);
                        if (i == 1)
                            XtraMessageBox.Show("Thêm thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        else if (i == 0)
                            XtraMessageBox.Show("Trùng tên loại quần áo.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        else
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show($"Lỗi: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    CategoryBUS.GetDataGV(gcCategory);
                }
                // Sửa
                else
                {
                    try
                    {
                        var model = new Category
                        {
                            id = int.Parse(gvCategory.GetRowCellValue(e.RowHandle, "id").ToString().Trim()),
                            name = gvCategory.GetRowCellValue(e.RowHandle, "name").ToString().Trim(),
                            quantity_unit = int.Parse(gvCategory.GetRowCellValue(e.RowHandle, "quantity_unit").ToString().Trim()) // Thêm trường mới
                        };
                        int i = CategoryBUS.Update(model);
                        if (i == 1)
                            XtraMessageBox.Show("Cập nhật thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        else if (i == 0)
                            XtraMessageBox.Show("Trùng tên loại quần áo.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        else
                            XtraMessageBox.Show("Có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show($"Lỗi: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    CategoryBUS.GetDataGV(gcCategory);
                }
            }
            else
            {
                e.Valid = false;
                XtraMessageBox.Show(sErr, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion


    }
}
