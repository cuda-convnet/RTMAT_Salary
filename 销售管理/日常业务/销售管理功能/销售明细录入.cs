﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using 销售管理.DAL.DataSetCustomersTableAdapters;
using 销售管理.DAL.DataSetSaleDetailsTableAdapters;
using 销售管理.DAL.DataSetProductsTableAdapters;
using 销售管理.DAL.DataSetUsersTableAdapters;
using System.Data.SqlClient;
using 销售管理.日常业务.销售管理功能;

namespace 销售管理.日常业务
{
    public partial class 销售明细录入 : Form
    {
        public 销售明细录入()
        {
            InitializeComponent();
        }
        public long mId = -1;
        private long ProductId = -1;
        private long CityId = -1;

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (CityId < 0)
            {
                if (Classes.PubClass.CompanyId ==0)
                {
                    MessageBox.Show("请选择客户");
                    return;
                }
                else
                {
                    MessageBox.Show("请先设置该客户所在地区");
                    return;
                }
            }

            if (cmbSettlementModes.Text.Trim() == "")
            {
                MessageBox.Show("请输入结款方式");
                return;
            }
            if (txtStockOutNo.Text.Trim() == "")
            {
                MessageBox.Show("出库单编号不能为空");
                return;
            }
            if (dgvDetails.Rows.Count < 1)
            {
                MessageBox.Show("出库内容不能为空");
                return;
            }
            try
            {
                int ret = 0;
                //if (mId == -1)
                //{
                //int ret = new T_SaleDetailsTableAdapter().Insert((long)cmbUserName.SelectedValue, cmbCustomerName.Text.Trim(), "", dtpDate1.Value.Date, ProductId, Convert.ToDecimal(txtAmount.Text), Convert.ToDecimal(txtPrice.Text), Convert.ToDecimal(txtSum.Text), cmbSettlementModes.Text, null, makeInvoice, txtInvoiceContent.Text.Trim(), "", dtpInvioceDate.Value.Date, cmbInvoiceType.Text, txtInvoiceNo.Text.Trim(), (cmbCustomerName.SelectedValue.ToString() == "" ? -1 : (long)cmbCustomerName.SelectedValue));

                using (SqlConnection mconn = new SqlConnection(Common.CommonClass.SqlConnStr))
                {
                    mconn.Open();
                    using (SqlTransaction mTrans = mconn.BeginTransaction())
                    {
                        try
                        {
                            var adapter = new T_SaleDetailsTableAdapter();
                            adapter.Connection = mconn;
                            adapter.Transaction = mTrans;

                            foreach (DataGridViewRow mRow in dgvDetails.Rows)
                            {
                               // ret = adapter.Insert((long)cmbUserName.SelectedValue, cmbCustomerName.SelectedValue.ToString(), "", dtpDate1.Value.Date, Convert.ToInt64(mRow.Cells["ColumnProductId"].Value), Convert.ToDecimal(mRow.Cells["ColumnAmount"].Value), Convert.ToDecimal(mRow.Cells["ColumnPrice"].Value), Convert.ToDecimal(mRow.Cells["ColumnSumMoney"].Value), cmbSettlementModes.Text, CityId, null, txtStockOutNo.Text.Trim(), mRow.Cells["InvoiceFlag"].Value.ToString(), mRow.Cells["ColumnUnit"].Value.ToString());
                               
                                ret = adapter.Insert((long)cmbUserName.SelectedValue, Classes.PubClass.CompanyId.ToString(), "", dtpDate1.Value.Date, Convert.ToInt64(mRow.Cells["ColumnProductId"].Value), Convert.ToDecimal(mRow.Cells["ColumnAmount"].Value), Convert.ToDecimal(mRow.Cells["ColumnPrice"].Value), Convert.ToDecimal(mRow.Cells["ColumnSumMoney"].Value), cmbSettlementModes.Text, CityId, null, txtStockOutNo.Text.Trim(), mRow.Cells["InvoiceFlag"].Value.ToString(), mRow.Cells["ColumnUnit"].Value.ToString());
                               
                                if (ret == 0)
                                {
                                    MessageBox.Show("第" + mRow.Index.ToString() + "项插入失败");
                                    mTrans.Rollback();
                                    return;
                                }
                            }
                            mTrans.Commit();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            return;
                        }
                    }

                }

                MessageBox.Show("保存成功");
                btnSave.Enabled = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 销售明细维护_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“dataSetUsers.T_Users”中。您可以根据需要移动或删除它。
            //获取销售名单
            cmbUserName.DisplayMember = "UserName";
            cmbUserName.ValueMember = "id";
            cmbUserName.DataSource = new T_UsersTableAdapter().GetSalers();
            cmbUserName.SelectedIndex = -1;
            //添加是否开票
            CmbInvoiceFlag.SelectedIndex = 0;
            //获取一级产品名单
            var mTable = new T_ProductsTableAdapter().GetDataByLevel(1); //获取level1的产品
            cmbProduct1.DataSource = mTable;
            cmbProduct1.SelectedIndex = -1;
            //foreach (销售管理.DAL.DataSetProducts.T_ProductsRow mRow in mTable.Rows)
            //{
            //    MyCmbList mCmblist = new MyCmbList();
            //    mCmblist.Id = mRow.Id;
            //    mCmblist.Name = mRow.Name;
            //    cmbProduct1.Items.Add(mCmblist);
            //}
            //if (mId == -1)
            //{
            //    //获取出库单编号
            //    //string m_date = DateTime.Now.ToString("yyyyMM");
            //    //txtStockOutNo.Text = new T_SaleDetailsTableAdapter().GetNextStockOutNo(m_date).ToString();

            //}
            //else
            //{
            //    DataTable dt = new DataTable();
            //    dt = new T_SaleDetailsTableAdapter().GetDataById(mId);

            //    if (dt.Rows.Count > 0)
            //    {
            //        var mRow = (销售管理.DAL.DataSetSaleDetails.T_SaleDetailsRow)dt.Rows[0];

            //        cmbUserName.SelectedValue = mRow.UserName;
            //        //cmbCustomerName.DisplayMember = "customername";
            //        //cmbCustomerName.DataSource = new T_CustomersTableAdapter().GetDataByUserId(mRow.UserName);
            //        cmbUserName_SelectionChangeCommitted(sender, e);
            //        if (mRow.IsCustomerNameNull() == false)
            //        {
            //            long mCustomerId;
            //            if (Int64.TryParse(mRow.CustomerName, out mCustomerId) == true)
            //                cmbCustomerName.SelectedValue = Convert.ToInt64(mRow.CustomerName);
            //            else
            //                cmbCustomerName.SelectedIndex = cmbCustomerName.FindStringExact(mRow.CustomerName);
            //        }
            //        cmbCustomerName_SelectionChangeCommitted(sender, e); //设置客户所在城市ID

            //        //获取产品信息
            //        //long pLevel, pId;
            //        var mTableProduct = new T_ProductsTableAdapter().GetDataByID(mRow.ProductName);
            //        if (mTableProduct.Rows.Count > 0)
            //        {
            //            销售管理.DAL.DataSetProducts.T_ProductsRow mProductRow = mTableProduct[0];
            //            //    for (int i = mProductRow.Level; i > 1; i--)
            //            //    {
            //            //        var mParentTable = new T_ProductsTableAdapter().GetDataByID(mProductRow.ParentId);
            //            //    }
            //            //设置产品
            //            SetProduct(mProductRow);
            //        }

            //        //cmbMonth.SelectedItem = mRow.SaleMonth;
            //        dtpDate1.Value = mRow.SaleDate;
            //        //txtProductName.Text = mRow.ProductName;
            //        txtAmount.Text = mRow.Amount.ToString();
            //        txtPrice.Text = mRow.Price.ToString();
            //        txtSum.Text = mRow.SumMoney.ToString();
            //        txtStockOutNo.Text = mRow.StockOutNo.ToString();
            //        if (mRow.IsSettlementModesNull() == false) cmbSettlementModes.SelectedText = mRow.SettlementModes;
            //        //if (mRow.IsMakeInvoiceNull() == false)
            //        //{
            //        //    if (mRow.MakeInvoice.ToString() == "是")
            //        //    {
            //        //        cbInvoice.Checked = true;
            //        //    }
            //        //    else
            //        //    {
            //        //        cbInvoice.Checked = false;
            //        //    }
            //        //}
            //        //if (mRow.IsInvoiceContentNull() == false) txtInvoiceContent.Text = mRow.InvoiceContent;
            //        ////cmbInvoiceMonth.SelectedItem = mRow.InvoiceMonth;
            //        //if (mRow.IsInvoiceDateNull() == false) dtpInvioceDate.Value = mRow.InvoiceDate;
            //        //if (mRow.IsInvoiceTypeNull() == false) cmbInvoiceType.SelectedItem = mRow.InvoiceType;
            //        //if (mRow.IsInvoiceNoNull() == false) txtInvoiceNo.Text = mRow.InvoiceNo;
            //    }
            //}

        }

        public void SetProduct(销售管理.DAL.DataSetProducts.T_ProductsRow mRow)
        {
            if (mRow.Level == 1)
            {
                cmbProduct1.SelectedValue = mRow.Id;
            }
            else
            {
                var mParentTable = new T_ProductsTableAdapter().GetDataByID(mRow.ParentId);
                if (mParentTable.Rows.Count > 0)
                {
                    SetProduct(mParentTable[0]);
                }
                if (mRow.Level == 2)
                {
                    cmbProduct2.SelectedValue = mRow.Id;
                }
                if (mRow.Level == 3)
                {
                    cmbProduct3.SelectedValue = mRow.Id;
                }
            }
        }

        public class MyCmbList
        {
            private Int64 mId;
            private string mName;

            public Int64 Id
            {
                set
                {
                    mId = value;
                }
                get
                {
                    return mId;
                }
            }

            public string Name
            {
                set
                {
                    mName = value;
                }
                get
                {
                    return mName;
                }

            }
            public override string ToString()
            {
                return Name;
            }
        }

        private void cmbProduct1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cmbProduct2.Items.Clear();
            //cmbProduct3.Items.Clear();
            //var mTable = new T_ProductsTableAdapter().GetDataByParentId(((MyCmbList)cmbProduct1.SelectedItem).Id); //获取level2的产品
            cmbProduct2.DataSource = null;
            cmbProduct3.DataSource = null;
            if (cmbProduct1.SelectedIndex == -1) return;
            var mTable = new T_ProductsTableAdapter().GetDataByParentId((long)cmbProduct1.SelectedValue); //获取level2的产品
            if (mTable.Rows.Count > 0)
            {
                cmbProduct2.Visible = true;
                cmbProduct3.Visible = false;
                //ProductId = -1;
                //foreach (销售管理.DAL.DataSetProducts.T_ProductsRow mRow in mTable.Rows)
                //{
                //    MyCmbList mCmblist = new MyCmbList();
                //    mCmblist.Id = mRow.Id;
                //    mCmblist.Name = mRow.Name;
                //    cmbProduct2.Items.Add(mCmblist);
                //}
                cmbProduct2.DisplayMember = "Name";
                cmbProduct2.ValueMember = "Id";
                cmbProduct2.DataSource = mTable;
                cmbProduct2.SelectedIndex = -1;
                ProductId = (long)cmbProduct1.SelectedValue;
            }
            else
            {
                cmbProduct2.Visible = false;
                cmbProduct3.Visible = false;
                //ProductId = ((MyCmbList)cmbProduct1.SelectedItem).Id;
                ProductId = (long)cmbProduct1.SelectedValue;
            }
        }

        private void cmbProduct2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cmbProduct3.Items.Clear();
            //var mTable = new T_ProductsTableAdapter().GetDataByParentId(((MyCmbList)cmbProduct2.SelectedItem).Id); //获取level3的产品
            cmbProduct3.DataSource = null;
            if (cmbProduct2.SelectedIndex == -1) return;
            var mTable = new T_ProductsTableAdapter().GetDataByParentId((long)cmbProduct2.SelectedValue); //获取level3的产品
            if (mTable.Rows.Count > 0)
            {
                cmbProduct3.Visible = true;
                //ProductId = -1;
                //foreach (销售管理.DAL.DataSetProducts.T_ProductsRow mRow in mTable.Rows)
                //{
                //    MyCmbList mCmblist = new MyCmbList();
                //    mCmblist.Id = mRow.Id;
                //    mCmblist.Name = mRow.Name;
                //    cmbProduct3.Items.Add(mCmblist);
                //}

                //cmbProduct3.DisplayMember = "Name";
                //cmbProduct3.ValueMember = "Id";
                //cmbProduct3.DataSource = mTable;
                //cmbProduct3.SelectedIndex = -1;
                //ProductId = (long)cmbProduct2.SelectedValue;

            }
            else
            {
                cmbProduct3.Visible = false;
                //ProductId = ((MyCmbList)cmbProduct2.SelectedItem).Id;
                ProductId = (long)cmbProduct2.SelectedValue;
            }
        }
      
        private void cmbProduct3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ProductId = ((MyCmbList)cmbProduct3.SelectedItem).Id;
            if (cmbProduct3.SelectedIndex == -1) return;
            ProductId = (long)cmbProduct3.SelectedValue;
        }

        private void cmbCustomerName_DataSourceChanged(object sender, EventArgs e)
        {
            cmbCustomerName.SelectedIndex = -1;
        }

        private void cmbUserName_SelectionChangeCommitted(object sender, EventArgs e)
        {

            //cmbCustomerName.DataSource = null;
            //if (cmbUserName.SelectedIndex > -1)
            //{
            //    cmbCustomerName.DisplayMember = "CompanyName";
            //    cmbCustomerName.ValueMember = "id";
            //    cmbCustomerName.DataSource = new T_CustomersTableAdapter().GetDataByUserId((long)cmbUserName.SelectedValue);
            //    cmbCustomerName.SelectedIndex = -1;
            //}

            cmbCustomerName.Text = "";
            Classes.PubClass.CompanyName = "";

            if (cmbUserName.SelectedIndex > -1)
            {
                Classes.PubClass.UserId = (long)cmbUserName.SelectedValue;
                CustomerNameList mForm = new CustomerNameList();
                mForm.ShowDialog();
            }
            
            if(Classes.PubClass.CompanyName!=""){
                cmbCustomerName.Text = Classes.PubClass.CompanyName;
                var table = new T_CustomersTableAdapter().GetDataById((long)Classes.PubClass.CompanyId);
                if (table.Rows.Count > 0)
                {
                    if (((销售管理.DAL.DataSetCustomers.T_CustomersRow)table.Rows[0]).IsCityIDNull() == true)
                    {
                        MessageBox.Show("请先设置该客户所在地区");
                        btnSave.Enabled = false;
                    }
                    else btnSave.Enabled = true;
                    CityId = ((销售管理.DAL.DataSetCustomers.T_CustomersRow)table.Rows[0]).IsCityIDNull() == true ? -1 : ((销售管理.DAL.DataSetCustomers.T_CustomersRow)table.Rows[0]).CityID;
                }
                else
                {
                    CityId = -1;
                }        
            }

        }

        private void cmbCustomerName_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmbCustomerName.SelectedIndex > -1)
            {
                var table = new T_CustomersTableAdapter().GetDataById((long)cmbCustomerName.SelectedValue);
                if (table.Rows.Count > 0)
                {
                    if (((销售管理.DAL.DataSetCustomers.T_CustomersRow)table.Rows[0]).IsCityIDNull() == true)
                    {
                        MessageBox.Show("请先设置该客户所在地区");
                        btnSave.Enabled = false;
                    }
                    else btnSave.Enabled = true;
                    CityId = ((销售管理.DAL.DataSetCustomers.T_CustomersRow)table.Rows[0]).IsCityIDNull() == true ? -1 : ((销售管理.DAL.DataSetCustomers.T_CustomersRow)table.Rows[0]).CityID;
                }
                else
                {
                    CityId = -1;
                }
            }
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            Decimal price, amount;
            if (Decimal.TryParse(txtPrice.Text, out price) == true && Decimal.TryParse(txtAmount.Text, out amount) == true)
            {
                txtSum.Text = (price * amount).ToString("0.00");
            }
        }


        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ("-0123456789".IndexOf(e.KeyChar) < 0 && (int)e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (".0123456789".IndexOf(e.KeyChar) < 0 && (int)e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ProductId < 0)
            {
                MessageBox.Show("请选择产品类型");
                return;
            }
            if (txtAmount.Text.Trim() == "")
            {
                MessageBox.Show("请输入数量");
                return;
            }
            if (txtPrice.Text.Trim() == "")
            {
                MessageBox.Show("请输入单价");
                return;
            }
            if (txtSum.Text.Trim() == "")
            {
                MessageBox.Show("请输入金额");
                return;
            }
            if (cmbUnit.Text.Trim() == "")
            {
                MessageBox.Show("请输入单位");
                return;
            }

            int index = dgvDetails.Rows.Add();
            dgvDetails.Rows[index].Cells["ColumnProductName"].Value = new T_ProductsTableAdapter().GetNameById(ProductId).ToString();
            dgvDetails.Rows[index].Cells["ColumnProductId"].Value = ProductId;
            dgvDetails.Rows[index].Cells["ColumnPrice"].Value = Convert.ToDecimal(txtPrice.Text);
            dgvDetails.Rows[index].Cells["ColumnAmount"].Value = Convert.ToDecimal(txtAmount.Text);
            dgvDetails.Rows[index].Cells["ColumnSumMoney"].Value = Convert.ToDecimal(txtSum.Text);
            dgvDetails.Rows[index].Cells["ColumnUnit"].Value = cmbUnit.Text.Trim();
            //添加是否开票
            dgvDetails.Rows[index].Cells["InvoiceFlag"].Value = CmbInvoiceFlag.Text.Trim();

        }

        private void cmbUserName_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void cmbCustomerName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void cmbProduct2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //  如果没有3级菜单返回
            if (cmbProduct3.Visible == false) {
                return;
            }

            if (cmbProduct2.SelectedIndex > -1) { 
                Classes.PubClass.Product2Id = (long)cmbProduct2.SelectedValue;
                Product3List mForm = new Product3List();
                mForm.ShowDialog();
            }

            if (Classes.PubClass.Product3Name != "") {
                cmbProduct3.Text = Classes.PubClass.Product3Name;
                ProductId = Classes.PubClass.Product3Id;
            }
           
        }
    }
}
