using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
namespace 排课系统
{
    
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //this.username.Text = "admin";
                //this.password.Text = "admin";
                //this.code.Value = Session["LVNum"].ToString();
                //submit_login(sender,e);
                //Session.Clear();
            }
        }

        protected void submit_login(object sender, EventArgs e)
        {
            //WebMessageBox.Show("用户名不能为空");
            if (this.username.Text.Length < 1)
            {
                
                WebMessageBox.Show("用户名不能为空"); return;
            }
            if (this.password.Text.Length < 1)
            {
                WebMessageBox.Show("密码不能为空"); return;
            }
            if (this.code.Value.Length < 1)
            {
                WebMessageBox.Show("验证码不能为空"); return;
            }
            String num = Session["LVNum"].ToString();
            if (!num.Equals(this.code.Value))
            {
                WebMessageBox.Show("验证码输入错误"); return;
            }

            if (RadioButton2.Checked)  //管理员
            {
                if (Operation.getDatatable("select * from t_admin where username='" + this.username.Text + "' and userpwd='" + this.password.Text + "'").Rows.Count < 1)
                {
                    WebMessageBox.Show("用户或密码错误"); return;
                }
                Session["username"] = username.Text;
                Response.Redirect("admin/index.aspx");
            }
            else { 
                //教师
                DataTable dt=Operation.getDatatable("select * from t_teacher where teachid='" + this.username.Text + "' and pwd='" + this.password.Text + "'");
                if (dt.Rows.Count < 1)
                {
                    WebMessageBox.Show("用户或密码错误"); return;
                }
                Session["teachid"] = username.Text;
                Session["teachname"] = dt.Rows[0]["name"].ToString();
                Response.Redirect("teacher/index.aspx");
            }
            
        }
    }
}