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
    public partial class coursetask : System.Web.UI.Page
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

                DataTable dt1 = Operation.getDatatable("select DISTINCT major from t_coursetask");
                DropDownList1.DataSource = dt1;//设置数据源
                DropDownList1.DataTextField = "major";//设置所要读取的数据表里的列名
                DropDownList1.DataBind();//数据绑定

                DataTable dt2 = Operation.getDatatable("select DISTINCT grade from t_coursetask");
                DropDownList2.DataSource = dt2;//设置数据源
                DropDownList2.DataTextField = "grade";//设置所要读取的数据表里的列名
                DropDownList2.DataBind();//数据绑定
                //绑定
                bind();
            }
        }
        public void bind()
        {
            string sqlstr = "select t_coursetask.*,t1.name as teachnamez,t2.name as teachnamef,t3.name as teachnames from ((t_coursetask left join t_teacher t1 on teachidz=t1.teachid) left join t_teacher t2 on teachidf=t2.teachid) left join t_teacher t3 on teachids=t3.teachid"+
                " where (t_coursetask.coursename like '%" + this.findinfo.Text + "%' OR LEN('" + this.findinfo.Text + "')=0) and t_coursetask.major='" +
               DropDownList1.SelectedValue.ToString() + "' and t_coursetask.grade='" + DropDownList2.SelectedValue.ToString() + "'";
            GridView1.DataSource = Operation.getDatatable(sqlstr);
            GridView1.DataKeyNames = new string[] { "id" };//主键
            GridView1.DataBind();
        }


        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string sqlstr = "delete from t_coursetask where id='" + GridView1.DataKeys[e.RowIndex].Value.ToString() + "'";
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

        protected void Button3_Click(object sender, EventArgs e)
        {
            //新增
            Response.Redirect("coursetaskadd.aspx");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
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
                    string major = "";  // 记录当前的专业 年级 专业人数
                    //最后一列的标号
                    int rowCount = sheet.LastRowNum; // 行数
                    if (rowCount < 4)
                    {
                        WebMessageBox.Show("导入失败，excel表数据不规范");
                        return;
                    }
                    if (sheet.GetRow(0) == null)
                    {
                        WebMessageBox.Show("导入失败，excel表数据不规范");
                        return;
                    }
                    if (sheet.GetRow(0).GetCell(0) == null)
                    {
                        WebMessageBox.Show("导入失败，excel表数据不规范");
                        return;
                    }

                    major = sheet.GetRow(0).GetCell(0).ToString();
                    if (major.IndexOf("系") >= 0) major = major.Substring(0, major.IndexOf("系")).Trim();
                    if (major.IndexOf("专业") >= 0) major = major.Substring(0, major.IndexOf("专业")).Trim();
                    if (major == "") return;
                    for (int i = 4; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　
                        int cellCount = row.LastCellNum;
                        if (cellCount < 22) continue; //没有数据的行默认是null　  22列

                        if (row.GetCell(0) == null || row.GetCell(1) == null) continue; //没有数据的行默认是null

                        string[] temp = new string[cellCount+1];
                        for (int j = 0; j <= cellCount; ++j)
                            if (row.GetCell(j) != null) temp[j] = row.GetCell(j).ToString().Trim();
                            else
                                temp[j] = "";
                        if (temp[3] == "" || temp[8] == "" || temp[10] == "" || temp[13] == "") continue;
                        string[] ss = temp[10].Split('-');
                        if (ss.Length != 2)
                        {
                            continue;
                        }
                        int zhouci1, zhouci2;
                        if (!int.TryParse(ss[0], out zhouci1) || !int.TryParse(ss[1], out zhouci2))
                        {
                            continue;
                        }
                        if (zhouci1 >= zhouci2 || zhouci1 < 1)
                        {
                            continue;
                        }
                        string teachidz="",teachidf="",teachids="";
                        dt = Operation.getDatatable("select teachid from t_teacher where name='" + temp[13] + "'");
                        if(dt.Rows.Count<1) continue;
                        teachidz=dt.Rows[0]["teachid"].ToString();

                        dt = Operation.getDatatable("select teachid from t_teacher where name='" + temp[16] + "'");
                        if (dt.Rows.Count >0) 
                        teachidf = dt.Rows[0]["teachid"].ToString();

                        dt = Operation.getDatatable("select teachid from t_teacher where name='" + temp[18] + "'");
                        if (dt.Rows.Count > 0)
                            teachids = dt.Rows[0]["teachid"].ToString();

                        if (Operation.getDatatable("select * from t_coursetask where xuhao='" + temp[0] + "'").Rows.Count > 0) continue;

                        //开始插入数据库
                        string sql = "insert into t_coursetask(xuhao,coursename,coursexingzhi,grade,major,xueshiall,xueshijiangshou,xueshishiyan,xueshiallz,xueshijiangshouz,xueshishiyanz,zhouci,khtype,courserongliang,teachidz,teachidf,teachids,dianjiao,shuangyu,remark) values('" +
                        temp[0] + "','" + temp[1] + "','" + temp[2] + "','" + temp[3] + "','" +major+"','"+
                        temp[4] + "','" + temp[5] + "','" + temp[6] + "','" + temp[7] + "','" + temp[8] + "','" + temp[9] + "','" + temp[10] + "','" + temp[11] + "','" + temp[12] + "','" +teachidz+ "','" +teachidf+ "','" +teachids+ "','" +temp[20]+ "','" +temp[21]+ "','" +temp[22]+ "')";
                        Operation.runSql(sql);
                        count++;
                    }
                    WebMessageBox.Show("导入完成，成功导入数据记录共"+count+"条","coursetask.aspx");
                }
                else
                {
                    WebMessageBox.Show("excel表没有数据");
                }

        }
    }
}