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
    public partial class teacherman : System.Web.UI.Page
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

                //绑定
                bind();
            }
        }
        public void bind()
        {
            string sqlstr = "select * from t_teacher where (name like '%" + this.findinfo.Text + "%' OR LEN('" + this.findinfo.Text + "')=0) order by teachid";
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
            string sqlstr = "delete from t_teacher where id='" + GridView1.DataKeys[e.RowIndex].Value.ToString() + "'";
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
            if (((TextBox)(GridView1.Rows[e.RowIndex].Cells[1].Controls[0])).Text.ToString().Trim()=="")
            {
                WebMessageBox.Show("请输入教师姓名"); return;
            }
            Operation.runSql("update t_teacher set name='" +((TextBox)(GridView1.Rows[e.RowIndex].Cells[1].Controls[0])).Text.ToString().Trim()+
                "',zhicheng='" + ((TextBox)(GridView1.Rows[e.RowIndex].Cells[2].Controls[0])).Text.ToString().Trim() +
                "',xueli='" + ((TextBox)(GridView1.Rows[e.RowIndex].Cells[3].Controls[0])).Text.ToString().Trim() +
                
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
            Response.Redirect("teachermanadd.aspx");
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
                    string teachid, name, zhicheng, xueli;
                    //最后一列的标号
                    int rowCount = sheet.LastRowNum; // 行数
                    for (int i = 1; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　
                        int cellCount = row.LastCellNum;
                        if (cellCount < 2) continue; //没有数据的行默认是null　
                        if (row.GetCell(0) == null) continue; //没有数据的行默认是null

                        if (row.GetCell(0).ToString().Trim() == "" || row.GetCell(1).ToString().Trim() == "") continue;
                        if (Operation.getDatatable("select * from t_teacher where teachid='" + row.GetCell(0).ToString().Trim() + "' or name='"+row.GetCell(1).ToString().Trim() + "'").Rows.Count > 0) continue;
                        teachid = row.GetCell(0).ToString().Trim(); name = row.GetCell(1).ToString().Trim(); zhicheng = ""; xueli = "";
                        if (row.GetCell(2) != null) zhicheng = row.GetCell(2).ToString().Trim();
                        if (row.GetCell(3) != null) xueli = row.GetCell(3).ToString().Trim();
                        string sql = "insert into t_teacher(teachid,name,zhicheng,xueli,pwd) values('" +
                        teachid+ "','" + name + "','" + zhicheng + "','" + xueli+"','"+teachid+ "')";
                        Operation.runSql(sql);
                        count++;
                    }
                    WebMessageBox.Show("导入完成，成功导入数据记录共"+count+"条","teacherman.aspx");
                }
                else
                {
                    WebMessageBox.Show("excel表没有数据");
                }

        }
    }
}