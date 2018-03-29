using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace 排课系统.admin
{
    public partial class paikecoditionteach : System.Web.UI.Page
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

                DropDownList1.Items.Add("全部"); 
                DropDownList1.Items.Add("未审核"); DropDownList1.Items.Add("审核通过"); DropDownList1.Items.Add("审核未通过"); 
                //DropDownList1.Items.Add("星期四"); DropDownList1.Items.Add("星期五");
            }
        }
        public void bind()
        {
            string type="";
            if(DropDownList1.SelectedIndex>0)
                type=(DropDownList1.SelectedIndex-1).ToString();
            string sql = "select t_taboo.*,name,case shenhe  when '0' then '未审核' when '1' then '审核通过' else '审核未通过' end as shenhestatus from t_taboo left join t_teacher on t_teacher.teachid=t_taboo.teachid where tabootype='0' and name like '%" + findinfo.Text + "%' and shenhe like '%" + type + "%' order by t_taboo.id";
            GridView1.DataSource = Operation.getDatatable(sql);
            GridView1.DataKeyNames = new string[] { "id" };//主键
            GridView1.DataBind();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string sqlstr = "update t_taboo set shenhe='1' where id='" + GridView1.DataKeys[e.RowIndex].Value.ToString() + "'";
            Operation.runSql(sqlstr);
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

        protected void GridView1_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            // 拒绝
            string sqlstr = "update t_taboo set shenhe='2' where id='" + GridView1.DataKeys[e.NewSelectedIndex].Value.ToString() + "'";
            Operation.runSql(sqlstr);
            bind();
        }

    }
}