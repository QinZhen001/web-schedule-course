using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace 排课系统.admin
{
    public partial class majoradd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    WebMessageBox.Show("请登录","../Default.aspx");
                }
                Label1.Text = Session["username"].ToString();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //添加
            if (TextBox1.Text == "")
            {
                WebMessageBox.Show("请输入专业名称"); return;
            }
            if (Operation.getDatatable("select * from t_major where name='" + TextBox1.Text + "'").Rows.Count > 0)
            {
                WebMessageBox.Show("此专业已经存在"); return;
            }
            if (TextBox2.Text == "")
            {
                WebMessageBox.Show("请输入专业描述"); return;
            }
            int tt;
            if (!int.TryParse(TextBox3.Text, out tt))
            {
                WebMessageBox.Show("请输入合法专业人数"); return;
            }
            Operation.runSql("insert into t_major(name,remark,nums) values('" + TextBox1.Text + "','" + TextBox2.Text + "','" + TextBox3.Text + "')");
            WebMessageBox.Show("添加完成","majormana.aspx");
        }
    }
}