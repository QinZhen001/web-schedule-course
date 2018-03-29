using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace 排课系统.admin
{
    public partial class courseplanadd : System.Web.UI.Page
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
                DropDownList1.DataSource = Operation.getDatatable("select name from t_major");//设置数据源
                DropDownList1.DataTextField = "name";//设置所要读取的数据表里的列名
                DropDownList1.DataBind();//数据绑定
                DropDownList2.Items.Add("2010");
                DropDownList2.Items.Add("2011");
                DropDownList2.Items.Add("2012");
                DropDownList2.Items.Add("2013");
                DropDownList2.Items.Add("2014");
                DropDownList2.Items.Add("2015");
                DropDownList2.Items.Add("2016");
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (DropDownList1.SelectedIndex < 0)
            {
                WebMessageBox.Show("请选择专业"); return;
            }
            if (DropDownList2.SelectedIndex < 0)
            {
                WebMessageBox.Show("请选择年级"); return;
            }
            //添加
            if (TextBox1.Text == "")
            {
                WebMessageBox.Show("请输入课程编号"); return;
            }
            if (TextBox2.Text == "")
            {
                WebMessageBox.Show("请输入课程名称"); return;
            }
            if (TextBox3.Text == "")
            {
                WebMessageBox.Show("请输入考核方式"); return;
            }
            float xf;
            if (!float.TryParse(TextBox4.Text,out xf))
            {
                WebMessageBox.Show("请输入有效的学分"); return;
            }
            int tt=0,tt1=0,tt2=0;
            if (!int.TryParse(TextBox5.Text,out tt))
            {
                WebMessageBox.Show("请输入有效的总学时"); return;
            }
            if (TextBox6.Text != "" && !int.TryParse(TextBox6.Text, out tt1))
            {
                WebMessageBox.Show("请输入有效的讲授学时"); return;
            }
            if (TextBox7.Text != "" && !int.TryParse(TextBox7.Text, out tt2))
            {
                WebMessageBox.Show("请输入有效的讲授学时"); return;
            }
            if (tt2+tt1 > tt)
            {
                WebMessageBox.Show("总学时需要大于讲授学时与实验学时之和"); return;
            }
            string sql = "insert into t_courseplan(courseid,coursename,khtype,score,xueshiall,xueshijiangshou,xueshishiyan,major,grade) values('"+
                TextBox1.Text+"','"+TextBox2.Text+"','"+TextBox3.Text+"','"+TextBox4.Text+"','"+
                TextBox5.Text+"','"+TextBox6.Text+"','"+TextBox7.Text+"','"+DropDownList1.SelectedValue.ToString()+"','"+DropDownList2.SelectedValue.ToString()+"')";
            Operation.runSql(sql);
            WebMessageBox.Show("添加完成","courseplan.aspx");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("courseplan.aspx");
        }
    }
}