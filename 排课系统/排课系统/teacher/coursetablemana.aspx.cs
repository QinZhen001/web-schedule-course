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
namespace 排课系统.teacher
{
    public partial class coursetablemana : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["teachid"] == null)
                {
                    WebMessageBox.Show("请登录", "../Default.aspx");
                }
                Label1.Text = "欢迎您," + Session["teachname"].ToString();
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
                    t = dt.Rows[ii]["coursename"].ToString() + "   (" + dt.Rows[ii]["zhouci"].ToString() + ")   " + dt.Rows[ii]["grade"].ToString() + shuangyu+dt.Rows[ii]["major"].ToString() + dianjiao + shuangyu;
                    break;
                }
            }
            return t;
        }
        public void bind()
        {
            string sqlstr = "select weekdays,sections,coursename,t_coursetask.zhouci,major,grade,dianjiao,shuangyu " +
                            "from (t_coursetable left join t_coursetask on taskid=t_coursetask.id) left join t_teacher on teachidz=teachid where teachidz='" + Session["teachid"].ToString() + "'";
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
       
    }
}