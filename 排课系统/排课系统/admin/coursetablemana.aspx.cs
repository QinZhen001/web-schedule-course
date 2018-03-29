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

using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.POIFS;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;

namespace 排课系统.admin
{
    public partial class coursetablemana : System.Web.UI.Page
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
        // 获得周i+1  第j+1节的课程
        private string getCourse(DataTable dt, int i, int j)
        {
            string t = "";
            for (int ii = 0; ii < dt.Rows.Count; ++ii)
            {
                if (dt.Rows[ii]["weekdays"].ToString().Equals((i + 1).ToString()) && dt.Rows[ii]["sections"].ToString().Equals((j + 1).ToString()))
                {
                    string dianjiao = "",shuangyu="";
                    if (dt.Rows[ii]["dianjiao"].ToString().IndexOf("是")>=0) dianjiao = "   (电教)";
                    if (dt.Rows[ii]["shuangyu"].ToString().IndexOf("是") >= 0) dianjiao = "   (双语)";
                    t = dt.Rows[ii]["coursename"].ToString() + "   (" + dt.Rows[ii]["zhouci"].ToString() + ")   " + dt.Rows[ii]["teachname"].ToString()+dianjiao+shuangyu;
                    break;
                }
            }
            return t;
        }
        public void bind()
        {
            title.Text = DropDownList2.SelectedValue.ToString() + "级" + DropDownList1.SelectedValue.ToString();
            string sqlstr = "select t_coursetable.id as keyid,weekdays,sections,t_coursetask.coursename,t_coursetask.zhouci,t_teacher.name as teachname,dianjiao,shuangyu " +
                        "from (t_coursetable left join t_coursetask on taskid=t_coursetask.id) left join t_teacher on teachidz=teachid where major='"+
                        DropDownList1.SelectedValue.ToString() + "' and grade='" + DropDownList2.SelectedValue.ToString() + "'";
            DataTable dt = Operation.getDatatable(sqlstr);
            string [] temp = new string[20];
            // 构造课表
            int k;
            for(int i=0;i<5;i++)
                for (int j = 0; j < 4; j++)
                {
                    k = i * 4 + j;  // 当前索引
                    temp[k] = getCourse(dt,i,j);
                }
            dt = new DataTable();
            dt.Columns.Add("weekdays",Type.GetType("System.String"));
            dt.Columns.Add("sections", Type.GetType("System.String"));
            dt.Columns.Add("course", Type.GetType("System.String"));
            for(int i=0;i<5;i++)
                for (int j = 0; j < 4; j++)
                {
                    DataRow r = dt.NewRow();
                    r[0] = xingqi[i];
                    r[1]=jieci[j];
                    r[2]=temp[i * 4 + j];
                    dt.Rows.Add(r);
                }
            DataRow r1 = dt.NewRow(); r1[0] = "六";
            dt.Rows.Add(r1);
            DataRow r2 = dt.NewRow(); r2[0] = "日";
            dt.Rows.Add(r2);
            GridView1.DataSource = dt;
            GridView1.DataBind();
            GroupRows(GridView1,0);
        }

        public static void GroupRows(GridView GridView1, int cellNum)
        {

            int i = 0, rowSpanNum = 1;
            while (i < GridView1.Rows.Count - 1)
            {
                GridViewRow gvr = GridView1.Rows[i];

                for (++i; i < GridView1.Rows.Count; i++)
                {
                    GridViewRow gvrNext = GridView1.Rows[i];
                    if (gvr.Cells[cellNum].Text == gvrNext.Cells[cellNum].Text)
                    {
                        gvrNext.Cells[cellNum].Visible = false;
                        rowSpanNum++;
                    }
                    else
                    {
                        gvr.Cells[cellNum].RowSpan = rowSpanNum;

                        rowSpanNum = 1;
                        break;
                    }

                    if (i == GridView1.Rows.Count - 1)
                    {
                        gvr.Cells[cellNum].RowSpan = rowSpanNum;

                    }
                }
            }

        }

        string[] jieci = new string[4] { "1,2", "3,4", "5,6" ,"7,8"};
        string[] xingqi = new string[7] { "一","二","三","四","五","六","日"};
        //查询
        protected void Button1_Click(object sender, EventArgs e)
        {
            bind();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            //导出
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            ISheet sheet1 = hssfworkbook.CreateSheet("Sheet1");

            // 获取单元格 并设置样式
            ICellStyle styleCell = hssfworkbook.CreateCellStyle();
            //居中
            styleCell.Alignment =HorizontalAlignment.Center;
            //垂直居中
            styleCell.VerticalAlignment = VerticalAlignment.Center;
            //设置字体
            IFont fontColorRed = hssfworkbook.CreateFont();
            fontColorRed.FontHeight = 17 * 17;
            styleCell.SetFont(fontColorRed);
            styleCell.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin; //下边框为细线边框
            styleCell.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;//左边框
            styleCell.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;//上边框
            styleCell.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;//右边框

            
            ICellStyle styleCell1 = hssfworkbook.CreateCellStyle();
            styleCell1.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;//居中
            styleCell1.VerticalAlignment = VerticalAlignment.Center;//垂直居中
            IFont fontColorRed1 = hssfworkbook.CreateFont();
            fontColorRed1.FontHeight = 20 * 20;
            styleCell1.SetFont(fontColorRed1);


            DataTable dt = getDtFromDB(DropDownList1.SelectedValue.ToString(), DropDownList2.SelectedValue.ToString());
            int row=0;
            IRow row1 = sheet1.CreateRow(row++);
            ICell cell = row1.CreateCell(0);
            cell.CellStyle = styleCell1;
            cell.SetCellValue(DropDownList2.SelectedValue.ToString()+"级"+ DropDownList1.SelectedValue.ToString());
            sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, 2));// 合并单元格
            int k=0;
            for (int i = 0; i < 5; i++)
            {
                int tt = row;
                for (int j = 0; j < 4; j++)
                {
                    k = i * 4 + j;  // 当前索引
                    row1 = sheet1.CreateRow(row++);
                    cell = row1.CreateCell(0);
                    cell.CellStyle = styleCell;
                    cell.SetCellValue(dt.Rows[k][0].ToString());
                    cell = row1.CreateCell(1);
                    cell.CellStyle = styleCell;
                    cell.SetCellValue(dt.Rows[k][1].ToString());
                    cell = row1.CreateCell(2);
                    cell.CellStyle = styleCell;
                    cell.SetCellValue(dt.Rows[k][2].ToString());
                }
                sheet1.AddMergedRegion(new CellRangeAddress(tt, tt+3, 0,0));
            }
            ++k;
            row1 = sheet1.CreateRow(row++);
            cell = row1.CreateCell(0);
            cell.SetCellValue(dt.Rows[k][0].ToString());
            cell.CellStyle = styleCell;
            cell = row1.CreateCell(1); cell.CellStyle = styleCell; cell = row1.CreateCell(2); cell.CellStyle = styleCell;
            ++k;
            row1 = sheet1.CreateRow(row++);
            cell = row1.CreateCell(0);
            cell.SetCellValue(dt.Rows[k][0].ToString());
            cell.CellStyle = styleCell;
            cell = row1.CreateCell(1);cell.CellStyle = styleCell;cell = row1.CreateCell(2);cell.CellStyle = styleCell;

            // 设置行宽度
            sheet1.SetColumnWidth(0, 20 * 256);  // 设置第二列的宽度
            sheet1.SetColumnWidth(1, 20 * 256);  // 设置第二列的宽度
            sheet1.SetColumnWidth(2, 100 * 256);  // 设置第二列的宽度
            // 输出Excel
            string filename ="coursetable.xls";
            var context = HttpContext.Current;
            context.Response.ContentType = "application/vnd.ms-excel";
            context.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", context.Server.UrlEncode(filename)));
            context.Response.Clear();
            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);
            context.Response.BinaryWrite(file.GetBuffer());
            context.Response.End();
        }
        DataTable getDtFromDB(string major,string grade)
        {
            title.Text = grade + "级" + major;
            string sqlstr = "select t_coursetable.id as keyid,weekdays,sections,t_coursetask.coursename,t_coursetask.zhouci,t_teacher.name as teachname,dianjiao,shuangyu " +
                        "from (t_coursetable left join t_coursetask on taskid=t_coursetask.id) left join t_teacher on teachidz=teachid where major='" +
                        major + "' and grade='" + grade + "'";
            DataTable dt = Operation.getDatatable(sqlstr);
            string[] temp = new string[20];
            // 构造课表
            int k;
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 4; j++)
                {
                    k = i * 4 + j;  // 当前索引
                    temp[k] = getCourse(dt, i, j);
                }
            dt = new DataTable();
            dt.Columns.Add("weekdays", Type.GetType("System.String"));
            dt.Columns.Add("sections", Type.GetType("System.String"));
            dt.Columns.Add("course", Type.GetType("System.String"));
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 4; j++)
                {
                    DataRow r = dt.NewRow();
                    r[0] = xingqi[i];
                    r[1] = jieci[j];
                    r[2] = temp[i * 4 + j];
                    dt.Rows.Add(r);
                }
            DataRow r1 = dt.NewRow(); r1[0] = "六";
            dt.Rows.Add(r1);
            DataRow r2 = dt.NewRow(); r2[0] = "日";
            dt.Rows.Add(r2);
            return dt;
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            // 导出所有
            //导出
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            ISheet sheet1 = hssfworkbook.CreateSheet("Sheet1");

            // 获取单元格 并设置样式
            ICellStyle styleCell = hssfworkbook.CreateCellStyle();
            //居中
            styleCell.Alignment = HorizontalAlignment.Center;
            //垂直居中
            styleCell.VerticalAlignment = VerticalAlignment.Center;
            //设置字体
            IFont fontColorRed = hssfworkbook.CreateFont();
            fontColorRed.FontHeight = 17 * 17;
            styleCell.SetFont(fontColorRed);
            styleCell.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin; //下边框为细线边框
            styleCell.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;//左边框
            styleCell.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;//上边框
            styleCell.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;//右边框


            ICellStyle styleCell1 = hssfworkbook.CreateCellStyle();
            styleCell1.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;//居中
            styleCell1.VerticalAlignment = VerticalAlignment.Center;//垂直居中
            IFont fontColorRed1 = hssfworkbook.CreateFont();
            fontColorRed1.FontHeight = 20 * 20;
            styleCell1.SetFont(fontColorRed1);
            int row = 0;
            DataTable dt;
            IRow row1; ICell cell;
            
            for (int ii = 0; ii < DropDownList1.Items.Count; ++ii)
                for (int jj = 0; jj < DropDownList2.Items.Count; ++jj)
                {
                    dt = getDtFromDB(DropDownList1.Items[ii].Value.ToString(), DropDownList2.Items[jj].Value.ToString());
                    row1 = sheet1.CreateRow(row++);
                    cell = row1.CreateCell(0);
                    cell.CellStyle = styleCell1;
                    cell.SetCellValue(DropDownList2.Items[jj].Value.ToString() + "级" + DropDownList1.Items[ii].Value.ToString());
                    sheet1.AddMergedRegion(new CellRangeAddress(row - 1, row-1, 0, 2));// 合并单元格
                    int k = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        int tt = row;
                        for (int j = 0; j < 4; j++)
                        {
                            k = i * 4 + j;  // 当前索引
                            row1 = sheet1.CreateRow(row++);
                            cell = row1.CreateCell(0);
                            cell.CellStyle = styleCell;
                            cell.SetCellValue(dt.Rows[k][0].ToString());
                            cell = row1.CreateCell(1);
                            cell.CellStyle = styleCell;
                            cell.SetCellValue(dt.Rows[k][1].ToString());
                            cell = row1.CreateCell(2);
                            cell.CellStyle = styleCell;
                            cell.SetCellValue(dt.Rows[k][2].ToString());
                        }
                        sheet1.AddMergedRegion(new CellRangeAddress(tt, tt + 3, 0, 0));
                    }
                    ++k;
                    row1 = sheet1.CreateRow(row++);
                    cell = row1.CreateCell(0);
                    cell.SetCellValue(dt.Rows[k][0].ToString());
                    cell.CellStyle = styleCell;
                    cell = row1.CreateCell(1); cell.CellStyle = styleCell; cell = row1.CreateCell(2); cell.CellStyle = styleCell;
                    ++k;
                    row1 = sheet1.CreateRow(row++);
                    cell = row1.CreateCell(0);
                    cell.SetCellValue(dt.Rows[k][0].ToString());
                    cell.CellStyle = styleCell;
                    cell = row1.CreateCell(1); cell.CellStyle = styleCell; cell = row1.CreateCell(2); cell.CellStyle = styleCell;
                    row1 = sheet1.CreateRow(row++);
                    row1 = sheet1.CreateRow(row++);
                }

            // 设置行宽度
            sheet1.SetColumnWidth(0, 20 * 256);  // 设置第二列的宽度
            sheet1.SetColumnWidth(1, 20 * 256);  // 设置第二列的宽度
            sheet1.SetColumnWidth(2, 100 * 256);  // 设置第二列的宽度
            // 输出Excel
            string filename = "coursetable_all.xls";
            var context = HttpContext.Current;
            context.Response.ContentType = "application/vnd.ms-excel";
            context.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", context.Server.UrlEncode(filename)));
            context.Response.Clear();
            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);
            context.Response.BinaryWrite(file.GetBuffer());
            context.Response.End();
        }
    }
}