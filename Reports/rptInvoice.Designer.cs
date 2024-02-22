
namespace Pharmacy.Reports
{
    partial class rptInvoice
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(rptInvoice));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.detailTable = new DevExpress.XtraReports.UI.XRTable();
            this.detailTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.productName = new DevExpress.XtraReports.UI.XRTableCell();
            this.quantity = new DevExpress.XtraReports.UI.XRTableCell();
            this.unitPrice = new DevExpress.XtraReports.UI.XRTableCell();
            this.lineTotal = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.vendorLogo2 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.thankYouLabel = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.GroupHeader2 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.invoiceInfoTable = new DevExpress.XtraReports.UI.XRTable();
            this.invoiceInfoTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.invoiceLabel = new DevExpress.XtraReports.UI.XRTableCell();
            this.invoiceInfoTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.totalCaption2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.invoiceDateCaption = new DevExpress.XtraReports.UI.XRTableCell();
            this.invoiceNumberCaption = new DevExpress.XtraReports.UI.XRTableCell();
            this.invoiceInfoTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.total2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.invoiceDate = new DevExpress.XtraReports.UI.XRTableCell();
            this.invoiceNumber = new DevExpress.XtraReports.UI.XRTableCell();
            this.vendorTable = new DevExpress.XtraReports.UI.XRTable();
            this.vendorNameRow = new DevExpress.XtraReports.UI.XRTableRow();
            this.vendorName = new DevExpress.XtraReports.UI.XRTableCell();
            this.vendorPhoneRow = new DevExpress.XtraReports.UI.XRTableRow();
            this.vendorPhone = new DevExpress.XtraReports.UI.XRTableCell();
            this.vendorEmailRow = new DevExpress.XtraReports.UI.XRTableRow();
            this.vendorEmail = new DevExpress.XtraReports.UI.XRTableCell();
            this.vendorLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.totalTable = new DevExpress.XtraReports.UI.XRTable();
            this.totalRow = new DevExpress.XtraReports.UI.XRTableRow();
            this.totalCaption = new DevExpress.XtraReports.UI.XRTableCell();
            this.total = new DevExpress.XtraReports.UI.XRTableCell();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.headerTable = new DevExpress.XtraReports.UI.XRTable();
            this.headerTableRow = new DevExpress.XtraReports.UI.XRTableRow();
            this.productDesctiptionCaption = new DevExpress.XtraReports.UI.XRTableCell();
            this.quantityCaption = new DevExpress.XtraReports.UI.XRTableCell();
            this.unitPriceCaption = new DevExpress.XtraReports.UI.XRTableCell();
            this.lineTotalCaption = new DevExpress.XtraReports.UI.XRTableCell();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            this.baseControlStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            ((System.ComponentModel.ISupportInitialize)(this.detailTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.invoiceInfoTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vendorTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.totalTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.headerTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.detailTable});
            this.Detail.HeightF = 71.00001F;
            this.Detail.KeepTogether = true;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.StyleName = "baseControlStyle";
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // detailTable
            // 
            this.detailTable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(224)))), ((int)(((byte)(220)))));
            this.detailTable.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(203)))), ((int)(((byte)(200)))));
            this.detailTable.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.detailTable.LocationFloat = new DevExpress.Utils.PointFloat(6.357829E-06F, 0F);
            this.detailTable.Name = "detailTable";
            this.detailTable.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.detailTableRow1});
            this.detailTable.SizeF = new System.Drawing.SizeF(849.998F, 71.00001F);
            this.detailTable.StylePriority.UseBackColor = false;
            this.detailTable.StylePriority.UseBorderColor = false;
            this.detailTable.StylePriority.UseBorders = false;
            this.detailTable.StylePriority.UseFont = false;
            // 
            // detailTableRow1
            // 
            this.detailTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.productName,
            this.quantity,
            this.unitPrice,
            this.lineTotal});
            this.detailTableRow1.Name = "detailTableRow1";
            this.detailTableRow1.Weight = 21.20787324129633D;
            // 
            // productName
            // 
            this.productName.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.productName.Name = "productName";
            this.productName.Padding = new DevExpress.XtraPrinting.PaddingInfo(20, 0, 10, 0, 100F);
            this.productName.StylePriority.UseBackColor = false;
            this.productName.StylePriority.UseBorders = false;
            this.productName.StylePriority.UseFont = false;
            this.productName.StylePriority.UsePadding = false;
            this.productName.StylePriority.UseTextAlignment = false;
            this.productName.Text = "[ItemTitle]";
            this.productName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.productName.Weight = 1.1251511729064141D;
            // 
            // quantity
            // 
            this.quantity.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.quantity.Name = "quantity";
            this.quantity.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 5, 0, 100F);
            this.quantity.RowSpan = 2;
            this.quantity.StylePriority.UseBackColor = false;
            this.quantity.StylePriority.UseBorders = false;
            this.quantity.StylePriority.UseFont = false;
            this.quantity.StylePriority.UsePadding = false;
            this.quantity.StylePriority.UseTextAlignment = false;
            this.quantity.Text = "[Qty]";
            this.quantity.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.quantity.Weight = 0.28331515764335091D;
            // 
            // unitPrice
            // 
            this.unitPrice.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.unitPrice.Name = "unitPrice";
            this.unitPrice.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 5, 0, 100F);
            this.unitPrice.RowSpan = 2;
            this.unitPrice.StylePriority.UseBackColor = false;
            this.unitPrice.StylePriority.UseBorders = false;
            this.unitPrice.StylePriority.UseFont = false;
            this.unitPrice.StylePriority.UsePadding = false;
            this.unitPrice.StylePriority.UseTextAlignment = false;
            this.unitPrice.Text = "[ItemPrice]";
            this.unitPrice.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.unitPrice.TextFormatString = "{0:$0.00}";
            this.unitPrice.Weight = 0.33330614645352324D;
            // 
            // lineTotal
            // 
            this.lineTotal.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.lineTotal.Name = "lineTotal";
            this.lineTotal.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 5, 0, 100F);
            this.lineTotal.RowSpan = 2;
            this.lineTotal.StylePriority.UseBackColor = false;
            this.lineTotal.StylePriority.UseBorders = false;
            this.lineTotal.StylePriority.UseFont = false;
            this.lineTotal.StylePriority.UseForeColor = false;
            this.lineTotal.StylePriority.UsePadding = false;
            this.lineTotal.StylePriority.UseTextAlignment = false;
            this.lineTotal.Text = "[ItemTotal]";
            this.lineTotal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.lineTotal.TextFormatString = "{0:$0.00}";
            this.lineTotal.Weight = 0.30136512989217096D;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 39F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.StylePriority.UseBackColor = false;
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.vendorLogo2,
            this.thankYouLabel,
            this.xrLine1});
            this.BottomMargin.HeightF = 119F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // vendorLogo2
            // 
            this.vendorLogo2.ImageAlignment = DevExpress.XtraPrinting.ImageAlignment.BottomLeft;
            this.vendorLogo2.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("vendorLogo2.ImageSource"));
            this.vendorLogo2.LocationFloat = new DevExpress.Utils.PointFloat(10.00187F, 13.99994F);
            this.vendorLogo2.Name = "vendorLogo2";
            this.vendorLogo2.SizeF = new System.Drawing.SizeF(150.8333F, 47F);
            this.vendorLogo2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.vendorLogo2.StylePriority.UseBorders = false;
            this.vendorLogo2.StylePriority.UsePadding = false;
            // 
            // thankYouLabel
            // 
            this.thankYouLabel.CanGrow = false;
            this.thankYouLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.thankYouLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(183)))), ((int)(((byte)(222)))));
            this.thankYouLabel.LocationFloat = new DevExpress.Utils.PointFloat(429.9998F, 0F);
            this.thankYouLabel.Multiline = true;
            this.thankYouLabel.Name = "thankYouLabel";
            this.thankYouLabel.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.thankYouLabel.SizeF = new System.Drawing.SizeF(420.0002F, 60.99996F);
            this.thankYouLabel.StylePriority.UseFont = false;
            this.thankYouLabel.StylePriority.UseForeColor = false;
            this.thankYouLabel.StylePriority.UseTextAlignment = false;
            this.thankYouLabel.Text = "THANK YOU FOR Ordering\r\n";
            this.thankYouLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
            // 
            // xrLine1
            // 
            this.xrLine1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(183)))), ((int)(((byte)(222)))));
            this.xrLine1.LineWidth = 2F;
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 67.84851F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(849.998F, 10.84723F);
            this.xrLine1.StylePriority.UseForeColor = false;
            // 
            // GroupHeader2
            // 
            this.GroupHeader2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1,
            this.invoiceInfoTable,
            this.vendorTable,
            this.vendorLogo});
            this.GroupHeader2.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("InvoiceNumber", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader2.GroupUnion = DevExpress.XtraReports.UI.GroupUnion.WithFirstDetail;
            this.GroupHeader2.HeightF = 320F;
            this.GroupHeader2.Level = 1;
            this.GroupHeader2.Name = "GroupHeader2";
            this.GroupHeader2.StyleName = "baseControlStyle";
            this.GroupHeader2.StylePriority.UseBackColor = false;
            // 
            // xrTable1
            // 
            this.xrTable1.BorderColor = System.Drawing.Color.Gray;
            this.xrTable1.Font = new System.Drawing.Font("Segoe UI", 8.75F);
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(10.00361F, 155F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable1.SizeF = new System.Drawing.SizeF(168.0001F, 20F);
            this.xrTable1.StylePriority.UseBorderColor = false;
            this.xrTable1.StylePriority.UseFont = false;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell2});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 0.7999999929574797D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.CanShrink = true;
            this.xrTableCell2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CustomerAddress]")});
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseBorders = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell2.Weight = 3D;
            // 
            // invoiceInfoTable
            // 
            this.invoiceInfoTable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(224)))), ((int)(((byte)(220)))));
            this.invoiceInfoTable.LocationFloat = new DevExpress.Utils.PointFloat(339.9998F, 187.08F);
            this.invoiceInfoTable.Name = "invoiceInfoTable";
            this.invoiceInfoTable.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.invoiceInfoTableRow1,
            this.invoiceInfoTableRow2,
            this.invoiceInfoTableRow3});
            this.invoiceInfoTable.SizeF = new System.Drawing.SizeF(510.0002F, 122.92F);
            this.invoiceInfoTable.StylePriority.UseBackColor = false;
            // 
            // invoiceInfoTableRow1
            // 
            this.invoiceInfoTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.invoiceLabel});
            this.invoiceInfoTableRow1.Name = "invoiceInfoTableRow1";
            this.invoiceInfoTableRow1.Weight = 0.79166666666666663D;
            // 
            // invoiceLabel
            // 
            this.invoiceLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(183)))), ((int)(((byte)(222)))));
            this.invoiceLabel.Font = new System.Drawing.Font("Segoe UI", 30F, System.Drawing.FontStyle.Bold);
            this.invoiceLabel.ForeColor = System.Drawing.Color.White;
            this.invoiceLabel.Multiline = true;
            this.invoiceLabel.Name = "invoiceLabel";
            this.invoiceLabel.StylePriority.UseBackColor = false;
            this.invoiceLabel.StylePriority.UseFont = false;
            this.invoiceLabel.StylePriority.UseForeColor = false;
            this.invoiceLabel.StylePriority.UseTextAlignment = false;
            this.invoiceLabel.Text = "Invoice Details\r\n";
            this.invoiceLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.invoiceLabel.Weight = 2.9597637771210623D;
            // 
            // invoiceInfoTableRow2
            // 
            this.invoiceInfoTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.totalCaption2,
            this.invoiceDateCaption,
            this.invoiceNumberCaption});
            this.invoiceInfoTableRow2.Name = "invoiceInfoTableRow2";
            this.invoiceInfoTableRow2.Weight = 0.4305545043945313D;
            // 
            // totalCaption2
            // 
            this.totalCaption2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(203)))), ((int)(((byte)(200)))));
            this.totalCaption2.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.totalCaption2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.totalCaption2.ForeColor = System.Drawing.Color.Gray;
            this.totalCaption2.Name = "totalCaption2";
            this.totalCaption2.StylePriority.UseBackColor = false;
            this.totalCaption2.StylePriority.UseBorderColor = false;
            this.totalCaption2.StylePriority.UseBorders = false;
            this.totalCaption2.StylePriority.UseFont = false;
            this.totalCaption2.StylePriority.UseForeColor = false;
            this.totalCaption2.StylePriority.UseTextAlignment = false;
            this.totalCaption2.Text = "TOTAL DUE";
            this.totalCaption2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.totalCaption2.Weight = 0.98658793062184347D;
            // 
            // invoiceDateCaption
            // 
            this.invoiceDateCaption.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.invoiceDateCaption.ForeColor = System.Drawing.Color.Gray;
            this.invoiceDateCaption.Name = "invoiceDateCaption";
            this.invoiceDateCaption.StylePriority.UseBackColor = false;
            this.invoiceDateCaption.StylePriority.UseFont = false;
            this.invoiceDateCaption.StylePriority.UseForeColor = false;
            this.invoiceDateCaption.StylePriority.UseTextAlignment = false;
            this.invoiceDateCaption.Text = "INVOICE DATE";
            this.invoiceDateCaption.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.invoiceDateCaption.Weight = 0.98658792324960953D;
            // 
            // invoiceNumberCaption
            // 
            this.invoiceNumberCaption.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(203)))), ((int)(((byte)(200)))));
            this.invoiceNumberCaption.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.invoiceNumberCaption.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.invoiceNumberCaption.ForeColor = System.Drawing.Color.Gray;
            this.invoiceNumberCaption.Name = "invoiceNumberCaption";
            this.invoiceNumberCaption.StylePriority.UseBackColor = false;
            this.invoiceNumberCaption.StylePriority.UseBorderColor = false;
            this.invoiceNumberCaption.StylePriority.UseBorders = false;
            this.invoiceNumberCaption.StylePriority.UseFont = false;
            this.invoiceNumberCaption.StylePriority.UseForeColor = false;
            this.invoiceNumberCaption.StylePriority.UseTextAlignment = false;
            this.invoiceNumberCaption.Text = "INVOICE NO";
            this.invoiceNumberCaption.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.invoiceNumberCaption.Weight = 0.98658792324960942D;
            // 
            // invoiceInfoTableRow3
            // 
            this.invoiceInfoTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.total2,
            this.invoiceDate,
            this.invoiceNumber});
            this.invoiceInfoTableRow3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.invoiceInfoTableRow3.Name = "invoiceInfoTableRow3";
            this.invoiceInfoTableRow3.StylePriority.UseFont = false;
            this.invoiceInfoTableRow3.Weight = 0.41666778564453133D;
            // 
            // total2
            // 
            this.total2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(203)))), ((int)(((byte)(200)))));
            this.total2.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.total2.Name = "total2";
            this.total2.StylePriority.UseBackColor = false;
            this.total2.StylePriority.UseBorderColor = false;
            this.total2.StylePriority.UseBorders = false;
            this.total2.StylePriority.UseFont = false;
            this.total2.StylePriority.UseTextAlignment = false;
            this.total2.Text = "[Total]";
            this.total2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.total2.TextFormatString = "{0:$0.00}";
            this.total2.Weight = 0.98658793062184347D;
            // 
            // invoiceDate
            // 
            this.invoiceDate.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[invoicedate]")});
            this.invoiceDate.Name = "invoiceDate";
            this.invoiceDate.StylePriority.UseBackColor = false;
            this.invoiceDate.StylePriority.UseFont = false;
            this.invoiceDate.StylePriority.UseTextAlignment = false;
            this.invoiceDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.invoiceDate.TextFormatString = "{0:dd/MM/yyyy}";
            this.invoiceDate.Weight = 0.98658792324960953D;
            // 
            // invoiceNumber
            // 
            this.invoiceNumber.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(203)))), ((int)(((byte)(200)))));
            this.invoiceNumber.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.invoiceNumber.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[OrderSerial]")});
            this.invoiceNumber.Name = "invoiceNumber";
            this.invoiceNumber.StylePriority.UseBackColor = false;
            this.invoiceNumber.StylePriority.UseBorderColor = false;
            this.invoiceNumber.StylePriority.UseBorders = false;
            this.invoiceNumber.StylePriority.UseFont = false;
            this.invoiceNumber.StylePriority.UseTextAlignment = false;
            this.invoiceNumber.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.invoiceNumber.Weight = 0.98658792324960942D;
            // 
            // vendorTable
            // 
            this.vendorTable.BorderColor = System.Drawing.Color.Gray;
            this.vendorTable.Font = new System.Drawing.Font("Segoe UI", 8.75F);
            this.vendorTable.LocationFloat = new DevExpress.Utils.PointFloat(10F, 90F);
            this.vendorTable.Name = "vendorTable";
            this.vendorTable.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.vendorNameRow,
            this.vendorPhoneRow,
            this.vendorEmailRow});
            this.vendorTable.SizeF = new System.Drawing.SizeF(168.0001F, 65F);
            this.vendorTable.StylePriority.UseBorderColor = false;
            this.vendorTable.StylePriority.UseFont = false;
            // 
            // vendorNameRow
            // 
            this.vendorNameRow.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.vendorName});
            this.vendorNameRow.Name = "vendorNameRow";
            this.vendorNameRow.Weight = 0.999999988262511D;
            // 
            // vendorName
            // 
            this.vendorName.CanShrink = true;
            this.vendorName.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[customername]")});
            this.vendorName.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.vendorName.Name = "vendorName";
            this.vendorName.StylePriority.UseFont = false;
            this.vendorName.StylePriority.UseTextAlignment = false;
            this.vendorName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.vendorName.Weight = 3D;
            // 
            // vendorPhoneRow
            // 
            this.vendorPhoneRow.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.vendorPhone});
            this.vendorPhoneRow.Name = "vendorPhoneRow";
            this.vendorPhoneRow.Weight = 0.7999999929574797D;
            // 
            // vendorPhone
            // 
            this.vendorPhone.CanShrink = true;
            this.vendorPhone.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CustomerPhone]")});
            this.vendorPhone.Name = "vendorPhone";
            this.vendorPhone.StylePriority.UseBorders = false;
            this.vendorPhone.StylePriority.UseTextAlignment = false;
            this.vendorPhone.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.vendorPhone.Weight = 3D;
            // 
            // vendorEmailRow
            // 
            this.vendorEmailRow.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.vendorEmail});
            this.vendorEmailRow.Name = "vendorEmailRow";
            this.vendorEmailRow.Weight = 0.79999999295747959D;
            // 
            // vendorEmail
            // 
            this.vendorEmail.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CustomerEmail]")});
            this.vendorEmail.Name = "vendorEmail";
            this.vendorEmail.StylePriority.UseTextAlignment = false;
            this.vendorEmail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.vendorEmail.Weight = 3D;
            // 
            // vendorLogo
            // 
            this.vendorLogo.ImageAlignment = DevExpress.XtraPrinting.ImageAlignment.MiddleCenter;
            this.vendorLogo.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("vendorLogo.ImageSource"));
            this.vendorLogo.LocationFloat = new DevExpress.Utils.PointFloat(10.00361F, 10F);
            this.vendorLogo.Name = "vendorLogo";
            this.vendorLogo.SizeF = new System.Drawing.SizeF(167.9965F, 80F);
            this.vendorLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.vendorLogo.StylePriority.UseBorders = false;
            this.vendorLogo.StylePriority.UsePadding = false;
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.totalTable});
            this.GroupFooter1.GroupUnion = DevExpress.XtraReports.UI.GroupFooterUnion.WithLastDetail;
            this.GroupFooter1.HeightF = 119.3326F;
            this.GroupFooter1.KeepTogether = true;
            this.GroupFooter1.Name = "GroupFooter1";
            this.GroupFooter1.StyleName = "baseControlStyle";
            // 
            // totalTable
            // 
            this.totalTable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(183)))), ((int)(((byte)(222)))));
            this.totalTable.BorderColor = System.Drawing.Color.White;
            this.totalTable.ForeColor = System.Drawing.Color.White;
            this.totalTable.LocationFloat = new DevExpress.Utils.PointFloat(489.6851F, 0F);
            this.totalTable.Name = "totalTable";
            this.totalTable.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.totalRow});
            this.totalTable.SizeF = new System.Drawing.SizeF(360.3129F, 37.96946F);
            this.totalTable.StylePriority.UseBackColor = false;
            this.totalTable.StylePriority.UseBorderColor = false;
            this.totalTable.StylePriority.UseForeColor = false;
            // 
            // totalRow
            // 
            this.totalRow.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.totalCaption,
            this.total});
            this.totalRow.Name = "totalRow";
            this.totalRow.Weight = 1.4D;
            // 
            // totalCaption
            // 
            this.totalCaption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(146)))), ((int)(((byte)(180)))));
            this.totalCaption.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.totalCaption.BorderWidth = 2F;
            this.totalCaption.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.totalCaption.Name = "totalCaption";
            this.totalCaption.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 5, 0, 0, 100F);
            this.totalCaption.StylePriority.UseBackColor = false;
            this.totalCaption.StylePriority.UseBorderColor = false;
            this.totalCaption.StylePriority.UseBorders = false;
            this.totalCaption.StylePriority.UseBorderWidth = false;
            this.totalCaption.StylePriority.UseFont = false;
            this.totalCaption.StylePriority.UseForeColor = false;
            this.totalCaption.StylePriority.UsePadding = false;
            this.totalCaption.StylePriority.UseTextAlignment = false;
            this.totalCaption.Text = "TOTAL DUE";
            this.totalCaption.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.totalCaption.Weight = 1.2930727215899744D;
            // 
            // total
            // 
            this.total.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(146)))), ((int)(((byte)(180)))));
            this.total.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.total.BorderWidth = 2F;
            this.total.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.total.Name = "total";
            this.total.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.total.StylePriority.UseBackColor = false;
            this.total.StylePriority.UseBorderColor = false;
            this.total.StylePriority.UseBorders = false;
            this.total.StylePriority.UseBorderWidth = false;
            this.total.StylePriority.UseFont = false;
            this.total.StylePriority.UseForeColor = false;
            this.total.StylePriority.UsePadding = false;
            this.total.StylePriority.UseTextAlignment = false;
            this.total.Text = "[Total]";
            this.total.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.total.TextFormatString = "{0:$0.00}";
            this.total.Weight = 1.1569762401510604D;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.headerTable});
            this.GroupHeader1.HeightF = 50.51282F;
            this.GroupHeader1.Name = "GroupHeader1";
            this.GroupHeader1.RepeatEveryPage = true;
            this.GroupHeader1.StyleName = "baseControlStyle";
            // 
            // headerTable
            // 
            this.headerTable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(183)))), ((int)(((byte)(222)))));
            this.headerTable.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.headerTable.ForeColor = System.Drawing.Color.White;
            this.headerTable.LocationFloat = new DevExpress.Utils.PointFloat(6.357829E-06F, 0F);
            this.headerTable.Name = "headerTable";
            this.headerTable.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.headerTableRow});
            this.headerTable.SizeF = new System.Drawing.SizeF(850F, 50.51282F);
            this.headerTable.StylePriority.UseBackColor = false;
            this.headerTable.StylePriority.UseFont = false;
            this.headerTable.StylePriority.UseForeColor = false;
            this.headerTable.StylePriority.UsePadding = false;
            // 
            // headerTableRow
            // 
            this.headerTableRow.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.productDesctiptionCaption,
            this.quantityCaption,
            this.unitPriceCaption,
            this.lineTotalCaption});
            this.headerTableRow.Name = "headerTableRow";
            this.headerTableRow.Weight = 11.5D;
            // 
            // productDesctiptionCaption
            // 
            this.productDesctiptionCaption.Name = "productDesctiptionCaption";
            this.productDesctiptionCaption.Padding = new DevExpress.XtraPrinting.PaddingInfo(20, 0, 0, 0, 100F);
            this.productDesctiptionCaption.StylePriority.UseBackColor = false;
            this.productDesctiptionCaption.StylePriority.UseFont = false;
            this.productDesctiptionCaption.StylePriority.UseForeColor = false;
            this.productDesctiptionCaption.StylePriority.UsePadding = false;
            this.productDesctiptionCaption.StylePriority.UseTextAlignment = false;
            this.productDesctiptionCaption.Text = "ITEM";
            this.productDesctiptionCaption.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.productDesctiptionCaption.Weight = 0.9934914095215055D;
            // 
            // quantityCaption
            // 
            this.quantityCaption.Name = "quantityCaption";
            this.quantityCaption.StylePriority.UseBackColor = false;
            this.quantityCaption.StylePriority.UseFont = false;
            this.quantityCaption.StylePriority.UseForeColor = false;
            this.quantityCaption.StylePriority.UseTextAlignment = false;
            this.quantityCaption.Text = "QUANTITY";
            this.quantityCaption.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.quantityCaption.Weight = 0.25016330094825251D;
            // 
            // unitPriceCaption
            // 
            this.unitPriceCaption.Name = "unitPriceCaption";
            this.unitPriceCaption.StylePriority.UseBackColor = false;
            this.unitPriceCaption.StylePriority.UseFont = false;
            this.unitPriceCaption.StylePriority.UseForeColor = false;
            this.unitPriceCaption.StylePriority.UseTextAlignment = false;
            this.unitPriceCaption.Text = "PRICE";
            this.unitPriceCaption.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.unitPriceCaption.Weight = 0.29430976344865206D;
            // 
            // lineTotalCaption
            // 
            this.lineTotalCaption.Name = "lineTotalCaption";
            this.lineTotalCaption.StylePriority.UseBackColor = false;
            this.lineTotalCaption.StylePriority.UseFont = false;
            this.lineTotalCaption.StylePriority.UseForeColor = false;
            this.lineTotalCaption.StylePriority.UseTextAlignment = false;
            this.lineTotalCaption.Text = "TOTAL";
            this.lineTotalCaption.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.lineTotalCaption.Weight = 0.26610681488785781D;
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSource = typeof(Pharmacy.Models.InvoiceVM);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // baseControlStyle
            // 
            this.baseControlStyle.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.baseControlStyle.Name = "baseControlStyle";
            this.baseControlStyle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            // 
            // rptInvoice
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.GroupHeader2,
            this.GroupFooter1,
            this.GroupHeader1});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1});
            this.DataSource = this.objectDataSource1;
            this.Font = new System.Drawing.Font("Arial", 9.75F);
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 39, 119);
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.baseControlStyle});
            this.Version = "21.2";
            ((System.ComponentModel.ISupportInitialize)(this.detailTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.invoiceInfoTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vendorTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.totalTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.headerTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.XRTable detailTable;
        private DevExpress.XtraReports.UI.XRTableRow detailTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell productName;
        private DevExpress.XtraReports.UI.XRTableCell quantity;
        private DevExpress.XtraReports.UI.XRTableCell unitPrice;
        private DevExpress.XtraReports.UI.XRTableCell lineTotal;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRPictureBox vendorLogo2;
        private DevExpress.XtraReports.UI.XRLabel thankYouLabel;
        private DevExpress.XtraReports.UI.XRLine xrLine1;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader2;
        private DevExpress.XtraReports.UI.XRTable invoiceInfoTable;
        private DevExpress.XtraReports.UI.XRTableRow invoiceInfoTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell invoiceLabel;
        private DevExpress.XtraReports.UI.XRTableRow invoiceInfoTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell totalCaption2;
        private DevExpress.XtraReports.UI.XRTableCell invoiceDateCaption;
        private DevExpress.XtraReports.UI.XRTableCell invoiceNumberCaption;
        private DevExpress.XtraReports.UI.XRTableRow invoiceInfoTableRow3;
        private DevExpress.XtraReports.UI.XRTableCell total2;
        private DevExpress.XtraReports.UI.XRTableCell invoiceDate;
        private DevExpress.XtraReports.UI.XRTableCell invoiceNumber;
        private DevExpress.XtraReports.UI.XRTable vendorTable;
        private DevExpress.XtraReports.UI.XRTableRow vendorNameRow;
        private DevExpress.XtraReports.UI.XRTableCell vendorName;
        private DevExpress.XtraReports.UI.XRTableRow vendorPhoneRow;
        private DevExpress.XtraReports.UI.XRTableCell vendorPhone;
        private DevExpress.XtraReports.UI.XRTableRow vendorEmailRow;
        private DevExpress.XtraReports.UI.XRTableCell vendorEmail;
        private DevExpress.XtraReports.UI.XRPictureBox vendorLogo;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.XtraReports.UI.XRTable totalTable;
        private DevExpress.XtraReports.UI.XRTableRow totalRow;
        private DevExpress.XtraReports.UI.XRTableCell totalCaption;
        private DevExpress.XtraReports.UI.XRTableCell total;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRTable headerTable;
        private DevExpress.XtraReports.UI.XRTableRow headerTableRow;
        private DevExpress.XtraReports.UI.XRTableCell productDesctiptionCaption;
        private DevExpress.XtraReports.UI.XRTableCell quantityCaption;
        private DevExpress.XtraReports.UI.XRTableCell unitPriceCaption;
        private DevExpress.XtraReports.UI.XRTableCell lineTotalCaption;
        private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource1;
        private DevExpress.XtraReports.UI.XRControlStyle baseControlStyle;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
    }
}
