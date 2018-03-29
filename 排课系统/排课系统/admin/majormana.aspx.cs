using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace 排课系统.admin
{
    public partial class majormana : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    WebMessageBox.Show("请登录", "../Default.aspx");
                }
                Label1.Text = Session["username"].ToString();

                //绑定
                bind();
            }
        }
        public void bind()
        {
            string sqlstr = "select * from t_major where (name like '%" + this.findinfo.Text + "%' OR LEN('" + this.findinfo.Text + "')=0) order by id";
            GridView1.DataSource = Operation.getDatatable(sqlstr);
            GridView1.DataKeyNames = new string[] { "id" };//主键
            GridView1.DataBind();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            bind();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string sqlstr = "delete from t_major where id='" + GridView1.DataKeys[e.RowIndex].Value.ToString() + "'";
            Operation.runSql(sqlstr);
            bind();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            bind();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int tt;
            if (!int.TryParse(((TextBox)(GridView1.Rows[e.RowIndex].Cells[2].Controls[0])).Text.ToString().Trim(), out tt))
            {
                WebMessageBox.Show("请输入合法的专业人数"); return;
            }
            Operation.runSql("update t_major set name='" + ((TextBox)(GridView1.Rows[e.RowIndex].Cells[1].Controls[0])).Text.ToString().Trim()+"',nums='"+
                ((TextBox)(GridView1.Rows[e.RowIndex].Cells[2].Controls[0])).Text.ToString().Trim()+"',remark='"+
                ((TextBox)(GridView1.Rows[e.RowIndex].Cells[3].Controls[0])).Text.ToString().Trim() + "' where id='" + GridView1.DataKeys[e.RowIndex].Value.ToString() + "'");
            GridView1.EditIndex = -1;
            bind();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            bind();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            //重新绑定　
            bind();
        }
    }
}