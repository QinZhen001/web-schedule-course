using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace 排课系统.admin
{
    public partial class paikecodition : System.Web.UI.Page
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

                DropDownList1.Items.Add("请选择"); DropDownList1.Items.Add("星期一"); DropDownList1.Items.Add("星期二"); DropDownList1.Items.Add("星期三"); DropDownList1.Items.Add("星期四"); DropDownList1.Items.Add("星期五");
                DropDownList2.Items.Add("请选择"); DropDownList2.Items.Add("第一节(上午)"); DropDownList2.Items.Add("第二节(上午)"); DropDownList2.Items.Add("第三节(下午)"); DropDownList2.Items.Add("第四节(下午)");
                DropDownList3.Items.Add("请选择"); DropDownList3.Items.Add("第1周"); DropDownList3.Items.Add("第2周"); DropDownList3.Items.Add("第3周"); DropDownList3.Items.Add("第4周"); DropDownList3.Items.Add("第5周"); DropDownList3.Items.Add("第6周");
                DropDownList3.Items.Add("第7周"); DropDownList3.Items.Add("第8周"); DropDownList3.Items.Add("第9周"); DropDownList3.Items.Add("第10周"); DropDownList3.Items.Add("第11周"); DropDownList3.Items.Add("第12周");
                DropDownList3.Items.Add("第13周"); DropDownList3.Items.Add("第14周"); DropDownList3.Items.Add("第15周"); DropDownList3.Items.Add("第16周"); DropDownList3.Items.Add("第17周"); DropDownList3.Items.Add("第18周"); DropDownList3.Items.Add("第19周");
                //DropDownList4.ControlStyle.s
                DropDownList4.Items.Add("请选择"); DropDownList4.Items.Add("第1周"); DropDownList4.Items.Add("第2周"); DropDownList4.Items.Add("第3周"); DropDownList4.Items.Add("第4周"); DropDownList4.Items.Add("第5周"); DropDownList4.Items.Add("第6周");
                DropDownList4.Items.Add("第7周"); DropDownList4.Items.Add("第8周"); DropDownList4.Items.Add("第9周"); DropDownList4.Items.Add("第10周"); DropDownList4.Items.Add("第11周");  DropDownList4.Items.Add("第12周");
                DropDownList4.Items.Add("第13周"); DropDownList4.Items.Add("第14周"); DropDownList4.Items.Add("第15周"); DropDownList4.Items.Add("第16周"); DropDownList4.Items.Add("第17周"); DropDownList4.Items.Add("第18周"); DropDownList4.Items.Add("第19周");
            }
        }
        public void bind()
        {
            string sqlstr = "select * from t_taboo where tabootype='1' order by id"; //1就是专业条件
            GridView1.DataSource = Operation.getDatatable(sqlstr);
            GridView1.DataKeyNames = new string[] { "id" };//主键
            GridView1.DataBind();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string sqlstr = "delete from t_taboo where id='" + GridView1.DataKeys[e.RowIndex].Value.ToString() + "'";
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

        protected void Button1_Click1(object sender, EventArgs e)
        {
            //添加
            if (DropDownList1.SelectedIndex < 1)
            {
                WebMessageBox.Show("请选择星期"); return;
            }
            if (DropDownList2.SelectedIndex < 1)
            {
                WebMessageBox.Show("请选择节次"); return;
            }
            if (DropDownList3.SelectedIndex < 1)
            {
                WebMessageBox.Show("请选择起始周"); return;
            }
            if (DropDownList4.SelectedIndex < 1)
            {
                WebMessageBox.Show("请选择截止周"); return;
            }
            if (DropDownList4.SelectedIndex < DropDownList3.SelectedIndex)
            {
                WebMessageBox.Show("截止周需要大于起始周"); return;
            }
            if (Operation.getDatatable("select * from t_taboo where weekdays='" + DropDownList1.SelectedIndex.ToString() + "' and sections='" + DropDownList2.SelectedIndex.ToString() + "' and tabootype='1'").Rows.Count > 0)
            {
                WebMessageBox.Show("当前星期的当前节次已经禁忌排课，无需再次添加"); return;
            }
            string sql = "insert into t_taboo(weekdays,sections,weeksstart,weeksend,tabootype) values('" + DropDownList1.SelectedIndex.ToString()
                + "','" + DropDownList2.SelectedIndex.ToString() + "','" + DropDownList3.SelectedIndex.ToString() + "','" + DropDownList4.SelectedIndex.ToString() + "','1')";
            Operation.runSql(sql);
            WebMessageBox.Show("添加禁忌排课完成","paikecodition.aspx");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            // 进入教师条件
            Response.Redirect("paikecoditionteach.aspx");
        }
    }
}