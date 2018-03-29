using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI;
using NPOI.HSSF;
using NPOI.HPSF;
using NPOI.SS;
using NPOI.Util;
using NPOI.SS.Util;

namespace 排课系统.admin
{
    public partial class courseplan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] == null)
                {
                    WebMessageBox.Show("请登录", "../Default.aspx");
                }else

                Label1.Text = Session["username"].ToString();

                DataTable dt1 = Operation.getDatatable("select DISTINCT major from t_courseplan");
                DropDownList1.DataSource = dt1;//设置数据源
                DropDownList1.DataTextField = "major";//设置所要读取的数据表里的列名
                DropDownList1.DataBind();//数据绑定

                DataTable dt2 = Operation.getDatatable("select DISTINCT grade from t_courseplan");
                DropDownList2.DataSource = dt2;//设置数据源
                DropDownList2.DataTextField = "grade";//设置所要读取的数据表里的列名
                DropDownList2.DataBind();//数据绑定
                //绑定
                bind();
            }
        }
        public void bind()
        {
            string sqlstr = "select * from t_courseplan where (coursename like '%" + this.findinfo.Text + "%' OR LEN('" + this.findinfo.Text + "')=0) and major='" +
               DropDownList1.SelectedValue.ToString() + "' and grade='" + DropDownList2.SelectedValue.ToString()+"'";
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
            string sqlstr = "delete from t_courseplan where id='" + GridView1.DataKeys[e.RowIndex].Value.ToString() + "'";
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
            float ss;
            if (!float.TryParse(((TextBox)(GridView1.Rows[e.RowIndex].Cells[3].Controls[0])).Text.ToString().Trim(), out ss))
            {
                WebMessageBox.Show("请输入有效学分"); return;
            }
            int tt;
            if (!int.TryParse(((TextBox)(GridView1.Rows[e.RowIndex].Cells[4].Controls[0])).Text.ToString().Trim(), out tt))
            {
                WebMessageBox.Show("请输入有效总学时"); return;
            }
            int tt1;
            if (!int.TryParse(((TextBox)(GridView1.Rows[e.RowIndex].Cells[5].Controls[0])).Text.ToString().Trim(), out tt1))
            {
                WebMessageBox.Show("请输入有效讲授学时"); return;
            }
            int tt2;
            if (!int.TryParse(((TextBox)(GridView1.Rows[e.RowIndex].Cells[6].Controls[0])).Text.ToString().Trim(), out tt2))
            {
                WebMessageBox.Show("请输入有效实验学时"); return;
            }
            if (tt1 + tt2 > tt)
            {
                WebMessageBox.Show("总学时需要大于讲授学时与实验学时之和"); return;
            }
            Operation.runSql("update t_courseplan set coursename='" +((TextBox)(GridView1.Rows[e.RowIndex].Cells[1].Controls[0])).Text.ToString().Trim()+
                "',khtype='" + ((TextBox)(GridView1.Rows[e.RowIndex].Cells[2].Controls[0])).Text.ToString().Trim() +
                "',score='" + ((TextBox)(GridView1.Rows[e.RowIndex].Cells[3].Controls[0])).Text.ToString().Trim() +
                "',xueshiall='" + ((TextBox)(GridView1.Rows[e.RowIndex].Cells[4].Controls[0])).Text.ToString().Trim() +
                "',xueshijiangshou='" + ((TextBox)(GridView1.Rows[e.RowIndex].Cells[5].Controls[0])).Text.ToString().Trim() +
                "',xueshishiyan='" + ((TextBox)(GridView1.Rows[e.RowIndex].Cells[6].Controls[0])).Text.ToString().Trim() +
                "' where id='" + GridView1.DataKeys[e.RowIndex].Value.ToString() + "'");
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

        protected void Button3_Click(object sender, EventArgs e)
        {
            //新增
            Response.Redirect("courseplanadd.aspx");
        }


        protected void Button2_Click(object sender, EventArgs e)
        {
                //导入
                if (FileUpload1.FileName.Length < 1)
                {
                    WebMessageBox.Show("请选择规范化excel文件"); return;
                }
                if (Path.GetExtension(FileUpload1.FileName).ToLower() != ".xls" && Path.GetExtension(FileUpload1.FileName).ToLower() != ".xlsx")
                {
                    WebMessageBox.Show("请选择规范化excel文件"); return;
                }

                IWorkbook workbook = null; FileStream fs = null;
                ISheet sheet = null;

                string filepath =Server.MapPath("~//upload//") + FileUpload1.FileName;  //Server.MapPath("~//upload//") +
                if (File.Exists(filepath))
                    File.Delete(filepath);
                FileUpload1.SaveAs(filepath);

                fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);//new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (Path.GetExtension(filepath).ToLower() == ".xlsx") // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (Path.GetExtension(filepath).ToLower() == ".xls") // 2003版本
                    workbook = new HSSFWorkbook(fs);
                if (workbook == null)
                {
                    WebMessageBox.Show("导入excel文件失败"); return;
                }
                sheet = workbook.GetSheetAt(0);  // 读取sheet
                int count = 0;
                if (sheet != null)
                {
                    string major = "", grade = "", nums = "", temp;  // 记录当前的专业 年级 专业人数
                    //最后一列的标号
                    int rowCount = sheet.LastRowNum; // 行数
                    //WebMessageBox.Show(rowCount.ToString());
                    for (int i = 0; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　
                        
                        int cellCount = row.LastCellNum;
                        if (cellCount < 1) continue; //没有数据的行默认是null　
                        if (row.GetCell(0) == null) continue; //没有数据的行默认是null

                        temp = row.GetCell(0).ToString();
                        if (temp.IndexOf("专业") >= 0 && temp.IndexOf("级") >= 0 && temp.IndexOf("人数") >= 0)
                        {
                            //WebMessageBox.Show(temp);
                            // 新专业 更新
                            grade = temp.Substring(0, temp.IndexOf("级")).Trim();
                            //temp.Remove(0, temp.IndexOf(":") + 1);
                            major = temp.Substring(temp.IndexOf("级") + 2, temp.IndexOf("专业") - temp.IndexOf("级") -2).Trim();
                            //temp = temp.Substring(temp.IndexOf("人数")+1);
                            nums = temp.Substring(temp.IndexOf("人数") + 3).Trim();
                        }
                        else
                        {
                            // 如果是数据行 则开始判断
                            if (major == "" || grade == "") continue;
                            if (temp.IndexOf("课程") >= 0 || temp.IndexOf("代码") >= 0)
                            {
                                //这里新数据库 专业数据表
                                if (Operation.getDatatable("select * from t_major where name='" + major + "'").Rows.Count < 1)
                                {
                                    int tt = 0;
                                    int.TryParse(nums, out tt);
                                    Operation.runSql("insert into t_major(name,remark,nums) values('" + major + "','" + major + "','" + tt.ToString() + "')");
                                }
                            }
                            else
                            {
                                // 教学计划数据
                                if (cellCount < 9) continue;
                                string sql = "select * from t_courseplan where major='" + major + "' and grade='" + grade + "' and courseid='" + temp.Trim() + "'";
                                if (Operation.getDatatable(sql).Rows.Count > 0)
                                    continue;
                                sql = "insert into t_courseplan(courseid,coursename,khtype,score,xueshiall,xueshijiangshou,xueshishiyan,major,grade) values('" +
                                row.GetCell(0).ToString() + "','" + row.GetCell(1).ToString() + "','" + row.GetCell(2).ToString() + "','" + row.GetCell(3).ToString() + "','" +
                                row.GetCell(4).ToString() + "','" + row.GetCell(5).ToString() + "','" + row.GetCell(6).ToString() + "','" + major + "','" + grade + "')";
                                Operation.runSql(sql);
                                count++;
                            }
                        }
                    }
                    WebMessageBox.Show("导入完成，成功导入数据记录共"+count+"条","courseplan.aspx");
                }
                else
                {
                    WebMessageBox.Show("excel表没有数据");
                }

        }
    }
}