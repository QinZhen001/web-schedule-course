using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data;
namespace 排课系统.admin
{
    public partial class coursetaskadd : System.Web.UI.Page
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
                DropDownList3.Items.Add("否"); DropDownList3.Items.Add("是");
                DropDownList4.Items.Add("否"); DropDownList4.Items.Add("是");
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
                WebMessageBox.Show("请输入课程名称"); return;
            }
            if (TextBox2.Text == "")
            {
                WebMessageBox.Show("请输入课程性质"); return;
            }
            if (TextBox3.Text == "")
            {
                WebMessageBox.Show("请输入考核方式"); return;
            }
            float xf;
            if (!float.TryParse(TextBox4.Text,out xf))
            {
                WebMessageBox.Show("请输入有效的课程容量"); return;
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
            tt = 0; tt1 = 0; tt2 = 0;
            if (!int.TryParse(TextBox8.Text, out tt))
            {
                WebMessageBox.Show("请输入有效的周总学时"); return;
            }
            if (TextBox9.Text != "" && !int.TryParse(TextBox9.Text, out tt1))
            {
                WebMessageBox.Show("请输入有效的周讲授学时"); return;
            }
            if (TextBox10.Text != "" && !int.TryParse(TextBox10.Text, out tt2))
            {
                WebMessageBox.Show("请输入有效的周讲授学时"); return;
            }
            if (tt2 + tt1 > tt)
            {
                WebMessageBox.Show("周总学时需要大于周讲授学时与周实验学时之和"); return;
            }
            if (tt > 10)
            {
                WebMessageBox.Show("周总学时太大了，课程安排不来"); return;
            }
            if (TextBox11.Text == "" || TextBox11.Text.IndexOf('-')<0)
            {
                WebMessageBox.Show("请输入有效的上课周次，如1-16"); return;
            }
            string[] ss = TextBox11.Text.Split('-');
            if (ss.Length!=2)
            {
                WebMessageBox.Show("请输入有效的上课周次，如1-16"); return;
            }
            int zhouci1,zhouci2;
            if (!int.TryParse(ss[0], out zhouci1) || !int.TryParse(ss[1], out zhouci2))
            {
                WebMessageBox.Show("请输入有效的上课周次，如1-16"); return;
            }
            if(zhouci1>=zhouci2||zhouci1<1)
            {
                WebMessageBox.Show("请输入有效的上课周次，如1-16"); return;
            }
            if (Operation.getDatatable("select * from t_teacher where teachid='" + TextBox12.Text + "'").Rows.Count<1)
            {
                WebMessageBox.Show("主讲教师工号不存在"); return;
            }
            if (TextBox13.Text != "" && Operation.getDatatable("select * from t_teacher where teachid='" + TextBox13.Text + "'").Rows.Count < 1)
            {
                WebMessageBox.Show("辅导教师工号不存在"); return;
            }
            if (TextBox14.Text != "" && Operation.getDatatable("select * from t_teacher where teachid='" + TextBox14.Text + "'").Rows.Count < 1)
            {
                WebMessageBox.Show("实验教师工号不存在"); return;
            }
            

            string sql = "insert into t_coursetask(coursename,coursexingzhi,khtype,courserongliang,xueshiall,xueshijiangshou,xueshishiyan,xueshiallz,xueshijiangshouz,xueshishiyanz,zhouci,teachidz,teachidf,teachids,dianjiao,shuangyu,remark,major,grade) values('" +
                TextBox1.Text+"','"+TextBox2.Text+"','"+TextBox3.Text+"','"+TextBox4.Text+"','"+
                TextBox5.Text + "','" + TextBox6.Text + "','" + TextBox7.Text + "','" + TextBox8.Text + "','" +
                TextBox9.Text + "','" + TextBox10.Text + "','" + TextBox11.Text + "','" + TextBox12.Text + "','" +
                TextBox13.Text + "','" + TextBox14.Text + "','" + DropDownList3.SelectedValue.ToString() + "','" + DropDownList4.SelectedValue.ToString() + "','" + TextBox15.Text + "','" + DropDownList1.SelectedValue.ToString() + "','" + DropDownList2.SelectedValue.ToString() + "')";
            Operation.runSql(sql);
            WebMessageBox.Show("添加完成","coursetask.aspx");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("coursetask.aspx");
        }
    }
}