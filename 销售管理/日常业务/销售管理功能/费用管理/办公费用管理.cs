﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using 销售管理.DAL.DataSetSaleDetailsTableAdapters;
using 销售管理.DAL.DataSetUsersTableAdapters;
using Common;
using 销售管理.日常业务.销售管理功能;
namespace 销售管理.日常业务
{
    public partial class 办公费用管理 : 销售管理.UserControlBase
    {
        public 办公费用管理()
        {
            InitializeComponent();
        }

        private void btnSerch_Click(object sender, EventArgs e)
        {
            string mSql;          
            mSql = @"SELECT  a.Id ,
                    c.UserName ,
                    a.SalaryDate ,
                    a.Price1 ,
                    a.Price2 ,
                    a.Price3 ,
                    a.Price4 ,
                    a.Price5 ,
                    a.SumMoney
            FROM    dbo.T_OfficeExpenses a
                    LEFT JOIN T_Users c ON a.UserName = c.id 
            WHERE 1=1 ";

            if (cmbUserName.Text.Trim() != "")
            {
                mSql += " and UserName like '%" + cmbUserName.Text.Trim() + "%'";
            }

            if (dtpStart.Checked == true)
            {
                mSql += " and a.SalaryDate between '" + dtpStart.Value.Date.ToString("yyyy-MM-dd") + "' and '" + dtpEnd.Value.Date.ToString("yyyy-MM-dd") + " 23:59:59'";
            }

            if (CmbDepartmentName.Text != string.Empty)
            {
                mSql += "and c.DepartmentName ='" + CmbDepartmentName.Text + "'";
            }

            if (dtpStart.Checked == true)
            {
                mSql += " and a.SalaryDate between '" + dtpStart.Value.Date.ToString("yyyy-MM-dd") + "' and '" + dtpEnd.Value.Date.ToString("yyyy-MM-dd") + "'";
            }

            try
            {
                DataTable mTable = SqlHelper.GetData(mSql);
                DataRow mRow = mTable.NewRow();
                mRow["SumMoney"] = mTable.Compute("sum(SumMoney)", "true");
                mTable.Rows.Add(mRow);
                dgvOfficeExpenses.DataSource = mTable;
                dgvOfficeExpenses.Rows[dgvOfficeExpenses.Rows.Count - 1].Cells["ColModify"] = new DataGridViewTextBoxCell();
                dgvOfficeExpenses.Rows[dgvOfficeExpenses.Rows.Count - 1].Cells["ColModify"].Value = "总计";

                if ((dgvOfficeExpenses.DataSource as DataTable).Rows.Count < 0)
                {
                    MessageBox.Show("没有记录");
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void 销售明细管理_Load(object sender, EventArgs e)
        {         
            cmbUserName.DataSource = new T_UsersTableAdapter().GetSalers();
            cmbUserName.DisplayMember = "UserName";
            cmbUserName.ValueMember = "id";
            cmbUserName.SelectedIndex = -1;

            CmbDepartmentName.DisplayMember = "VcName";
            CmbDepartmentName.DataSource = SqlHelper.GetData(" SELECT VcName FROM [dbo].[PTDepartment]"); ;
            CmbDepartmentName.SelectedIndex = -1;

            if (!Common.AuthenticateRight.AuthOperation(111601) && !CommonClass.SttUser.blSuperUser)
            {
                cmbUserName.SelectedValue = Classes.PubClass.UserId;
                cmbUserName.Enabled = false;
            }           
            DateTime dt = DateTime.Now;
            dtpStart.Value = dt.AddMonths(-dt.Month + 1).AddDays(-dt.Day + 1);
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (产品赠送明细录入 mForm = new 产品赠送明细录入())
            {
                mForm.ShowDialog();
            }

        }
       

        private void dgvSaleDetails_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {

        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
           
        }

        private void cmbUserName_SelectionChangeCommitted(object sender, EventArgs e)
        {
            
        }

        private void cmbUserName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (办公费用录入 mForm = new 办公费用录入())
            {
                mForm.ShowDialog();
            }
        }

        private void dgvCustomerBills_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == dgvOfficeExpenses.Columns["ColModify"].Index)
                {
                    办公费用录入 mForm = new 办公费用录入();
                    mForm.mId = Convert.ToInt64(dgvOfficeExpenses.Rows[e.RowIndex].Cells["idDataGridViewTextBoxColumn"].Value);
                    mForm.ShowDialog();
                }
            }

        }

        private void llAdd_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Classes.ExcelDeport.Deport(dgvOfficeExpenses, 2);
        }
    }
    }

