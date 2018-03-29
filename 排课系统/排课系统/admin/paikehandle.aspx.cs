using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
namespace 排课系统.admin
{
    public partial class paikehandle : System.Web.UI.Page
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
                if (Request["coursetaskid"] == null)
                    Response.Redirect("paikemana.aspx");

                string sqlstr = "select t_coursetask.*,t1.name as teachnamez,t2.name as teachnamef,t3.name as teachnames,(xueshijiangshouz+1)/2 as times from ((t_coursetask left join t_teacher t1 on teachidz=t1.teachid) left join t_teacher t2 on teachidf=t2.teachid) left join t_teacher t3 on teachids=t3.teachid " +
                " where t_coursetask.id=" + Request["coursetaskid"].ToString();
                GridView1.DataSource = Operation.getDatatable(sqlstr);
                GridView1.DataKeyNames = new string[] { "id" };//主键
                GridView1.DataBind();

                DropDownList1.Items.Add("请选择"); DropDownList1.Items.Add("星期一"); DropDownList1.Items.Add("星期二"); DropDownList1.Items.Add("星期三"); DropDownList1.Items.Add("星期四"); DropDownList1.Items.Add("星期五");
                DropDownList2.Items.Add("请选择"); DropDownList2.Items.Add("第一节(上午)"); DropDownList2.Items.Add("第二节(上午)"); DropDownList2.Items.Add("第三节(下午)"); DropDownList2.Items.Add("第四节(下午)");
                DataTable dt = new DataTable();
                dt.Columns.Add("id", Type.GetType("System.Int32"));
                dt.Columns.Add("weekdays", Type.GetType("System.String"));
                dt.Columns.Add("sections", Type.GetType("System.String"));
                Session["dthandle"] = dt;
                bind(dt);
            }
        }


        void bind(DataTable dt)
        {
            GridView2.DataSource = dt;
            GridView2.DataKeyNames = new string[] { "id" };//主键
            GridView2.DataBind();
        }
        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //删除
            DataTable dt = Session["dthandle"] as DataTable;
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                if (dt.Rows[i][1].ToString().Equals(GridView2.Rows[e.RowIndex].Cells[0].Text.ToString()) && dt.Rows[i][2].ToString() == GridView2.Rows[e.RowIndex].Cells[1].Text.ToString())
                {
                    dt.Rows.RemoveAt(i);
                }
            }
            bind(dt);
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            if (GridView1.Rows[0].Cells[13].Text == GridView2.Rows.Count.ToString())
            {
                WebMessageBox.Show("每周上课次数只能为" + GridView1.Rows[0].Cells[13].Text); return;
            }
            // 安排上课
            if (DropDownList1.SelectedIndex < 1)
            {
                WebMessageBox.Show("请选择上课星期"); return;
            }
            if (DropDownList2.SelectedIndex < 1)
            {
                WebMessageBox.Show("请选择上课节次"); return;
            }

            // 首先验证当前是否已经排课
            DataTable dt = Session["dthandle"] as DataTable;
            for (int i = 0; i < dt.Rows.Count;++i )
                if (dt.Rows[i][1].ToString() == DropDownList1.SelectedIndex.ToString() && dt.Rows[i][2].ToString() == DropDownList2.SelectedIndex.ToString())
                {
                    WebMessageBox.Show("此时间段当前已经安排"); return;
                }

            // 下面验证此排课是否有效
            int[,] teacher_table; int[,] major_table; int[,] or_table;  // 教师空闲课表  专业年级空闲时间  或操作之后的空暇表
            teacher_table = generalTeachTable(GridView1.Rows[0].Cells[7].Text, GridView1.Rows[0].Cells[6].Text);
            major_table = generalMajorTable(GridView1.Rows[0].Cells[9].Text, GridView1.Rows[0].Cells[9].Text, GridView1.Rows[0].Cells[6].Text);
            or_table = tableOrOperate(teacher_table, major_table);
            if (or_table[DropDownList2.SelectedIndex - 1, DropDownList1.SelectedIndex - 1] == 1)
            {
                WebMessageBox.Show("安排失败，此时间段已经被排课"); return;
            }

            // 可以排课
            DataRow r = dt.NewRow();
            r[0] = dt.Rows.Count;
            r[1] = DropDownList1.SelectedIndex;
            r[2] = DropDownList2.SelectedIndex;
            dt.Rows.Add(r);
            bind(dt);
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            // 保存排课
            if (GridView1.Rows[0].Cells[13].Text != GridView2.Rows.Count.ToString())
            {
                WebMessageBox.Show("保存失败，每周上课次数必须为" + GridView1.Rows[0].Cells[13].Text); return;
            }
            for (int i = 0; i < GridView2.Rows.Count; ++i)
            {
                Operation.runSql("insert into t_coursetable(taskid,weekdays,sections) values('" +
                                   GridView1.DataKeys[0].Value.ToString() + "','" + GridView2.Rows[i].Cells[0].Text + "','" + GridView2.Rows[i].Cells[1].Text
                                    + "')");
            }
            WebMessageBox.Show("保存完成", "paikemana.aspx");
        }


        #region 排课算法
        
        // 获得周次
        private int[] getZhouci(string s)
        { 
            int[] zc=new int[2]{0,0};
            string[] t = s.Split('-');
            if (t.Length > 1)
            {
                int.TryParse(t[0],out zc[0]);
                int.TryParse(t[1], out zc[1]);
            }
            return zc;
        }

        // 判断周次是否合适 zc1为待判断周次 zc2为忙碌周次  如果周次不相交  就是可以排课 那么就返回true
        private Boolean judgeZhouci(string zc1, string zc2)
        {
            int[] zhouci1 = getZhouci(zc1); int[] zhouci2 = getZhouci(zc2);
            if (zhouci1[1] < zhouci2[0] || zhouci1[0] > zhouci2[1])
                return true;
            else return false;
        }

        //产生教师时间空闲表
        private int[,] generalTeachTable(string teachid,string zhouci)
        { 
            //4*5时间空闲表  4节*5天  0表示空闲  1表示不空闲
            int[,] table=new int[4,5];
            int i,j;
            // 初始化都是空闲的额
            for (i = 0; i < 4; ++i)
                for (j = 0; j < 5; ++j)
                    table[i, j] = 0;

            //首先考虑教师自身禁忌排课的条件 
            string sqlstr = "select * from t_taboo where tabootype='0' and  shenhe='1'  and teachid='" + teachid.Trim() + "'"; //0就是教师
            DataTable dt = Operation.getDatatable(sqlstr);
            for (int n = 0; n < dt.Rows.Count; ++n)
            {
                i = int.Parse(dt.Rows[n]["sections"].ToString()) - 1;
                j = int.Parse(dt.Rows[n]["weekdays"].ToString()) - 1;

                // 如果周次安排不上 则为忙碌
                if (!judgeZhouci(zhouci, dt.Rows[n]["weeksstart"].ToString()+"-" + dt.Rows[n]["weeksend"].ToString()))
                    table[i, j] = 1;
            }

            //然后考虑已排课程
            sqlstr = "select t_coursetable.*,zhouci from t_coursetable left join t_coursetask on taskid=t_coursetask.id where t_coursetask.teachidz='" + teachid.Trim() + "'"; 
            dt = Operation.getDatatable(sqlstr);
            for (int n = 0; n < dt.Rows.Count; ++n)
            {
                i = int.Parse(dt.Rows[n]["sections"].ToString()) - 1;
                j = int.Parse(dt.Rows[n]["weekdays"].ToString()) - 1;

                // 如果周次安排不上 则为忙碌
                if (!judgeZhouci(zhouci, dt.Rows[n]["zhouci"].ToString()))
                    table[i, j] = 1;
            }
            
            // 返回空闲时间表  
            return table;
        }
        //产生专业年级空闲表
        private int[,] generalMajorTable(string major,string grade,string zhouci)
        {
            //4*5时间空闲表  4节*5天  0表示空闲  1表示不空闲
            int[,] table = new int[4, 5];
            int i, j;
            // 初始化都是空闲的额
            for (i = 0; i < 4; ++i)
                for (j = 0; j < 5; ++j)
                    table[i, j] = 0;

            //首先考虑教师自身禁忌排课的条件
            string sqlstr = "select * from t_taboo where tabootype='1'"; //1就是专业班级
            DataTable dt = Operation.getDatatable(sqlstr);
            for (int n = 0; n < dt.Rows.Count; ++n)
            {
                i = int.Parse(dt.Rows[n]["sections"].ToString()) - 1;
                j = int.Parse(dt.Rows[n]["weekdays"].ToString()) - 1;
                // 如果周次安排不上 则为忙碌
                if (!judgeZhouci(zhouci, dt.Rows[n]["weeksstart"].ToString() + "-" + dt.Rows[n]["weeksend"].ToString()))
                    table[i, j] = 1;
            }

            //然后考虑已排课程
            sqlstr = "select t_coursetable.*,zhouci from t_coursetable left join t_coursetask on taskid=t_coursetask.id where t_coursetask.major='" + major.Trim() + "' and t_coursetask.grade='"+grade.Trim()+"'"; 
            dt = Operation.getDatatable(sqlstr);
            for (int n = 0; n < dt.Rows.Count; ++n)
            {
                i = int.Parse(dt.Rows[n]["sections"].ToString()) - 1;
                j = int.Parse(dt.Rows[n]["weekdays"].ToString()) - 1;
                // 如果周次安排不上 则为忙碌
                if (!judgeZhouci(zhouci, dt.Rows[n]["zhouci"].ToString()))
                    table[i, j] = 1;
            }

            // 返回空闲时间表  
            return table;
        }

        //教师空闲表跟专业年级空闲表或操作
        private int[,] tableOrOperate(int[,] t1,int[,] t2)
        {
            int[,] table = new int[4, 5];
            int i, j;
            for (i = 0; i < 4; ++i)
                for (j = 0; j < 5; ++j)
                {
                    table[i, j] = 0;
                    if (t1[i, j] == 1 || t2[i, j] == 1) table[i, j] = 1;
                }
            // 返回空闲时间表  
            return table;
        }

        // 周学时 安排上课次数 的模式匹配优先法则  靠前越优先
        string[] zhouanpai3 = { "1-3-5", "1-2-4", "2-4-5", "1-2-5", "2-3-5", "1-3-4", "1-4-5", "1-2-3", "2-3-4", "3-4-5" };
        string[] zhouanpai2 = { "1-3", "2-4", "3-5", "1-4", "2-5", "1-5", "1-2", "2-3", "3-4", "4-5" };
        string[] zhouanpai1 = { "3", "2", "4", "1", "5" };
        string[] jiepai = { "2", "3", "1", "4" };

        // 安排课程 将课程taskid安排在table里面 每周上课time次  成功返回true
        private Boolean paike(string taskid, int times, int[,] or_table)
        {
            int ii, jj, kk, nn;
            switch (times)  // 根据每周上课次数安排课程 
            {
                case 1: // 每周上课一次
                    for (ii = 0; ii < zhouanpai1.Length; ++ii)
                        for (jj = 0; jj < jiepai.Length; ++jj)
                            if (or_table[int.Parse(jiepai[jj]) - 1, int.Parse(zhouanpai1[ii]) - 1] == 0)
                            {
                                // 如果找到空闲的
                                // 更新到数据中
                                Operation.runSql("insert into t_coursetable(taskid,weekdays,sections) values('" +
                                    taskid+ "','" + zhouanpai1[ii] + "','" + jiepai[jj]
                                    + "')");
                                return true;
                            }
                    break;
                case 2:
                    for (ii = 0; ii < zhouanpai2.Length; ++ii)
                        for (jj = 0; jj < jiepai.Length; ++jj)
                            for (kk = 0; kk < jiepai.Length; ++kk)
                                if (or_table[int.Parse(jiepai[jj]) - 1, int.Parse(zhouanpai2[ii].ElementAt(0).ToString()) - 1] == 0 &&
                                    or_table[int.Parse(jiepai[kk]) - 1, int.Parse(zhouanpai2[ii].ElementAt(2).ToString()) - 1] == 0)
                                {
                                    // 如果找到空闲的
                                    // 更新到数据中
                                    Operation.runSql("insert into t_coursetable(taskid,weekdays,sections) values('" +
                                        taskid + "','" + zhouanpai2[ii].ElementAt(0).ToString() + "','" + jiepai[jj]
                                        + "')");
                                    Operation.runSql("insert into t_coursetable(taskid,weekdays,sections) values('" +
                                        taskid + "','" + zhouanpai2[ii].ElementAt(2).ToString() + "','" + jiepai[kk]
                                        + "')");
                                    return true;
                                }
                    break;
                case 3:
                    for (ii = 0; ii < zhouanpai3.Length; ++ii)
                        for (jj = 0; jj < jiepai.Length; ++jj)
                            for (kk = 0; kk < jiepai.Length; ++kk)
                                for (nn = 0; nn < jiepai.Length; ++nn)
                                    if (or_table[int.Parse(jiepai[jj]) - 1, int.Parse(zhouanpai3[ii].ElementAt(0).ToString()) - 1] == 0 &&
                                    or_table[int.Parse(jiepai[kk]) - 1, int.Parse(zhouanpai3[ii].ElementAt(2).ToString()) - 1] == 0 &&
                                    or_table[int.Parse(jiepai[nn]) - 1, int.Parse(zhouanpai3[ii].ElementAt(4).ToString()) - 1] == 0)
                                    {
                                        // 如果找到空闲的
                                        // 更新到数据中
                                        Operation.runSql("insert into t_coursetable(taskid,weekdays,sections) values('" +
                                            taskid + "','" + zhouanpai3[ii].ElementAt(0).ToString() + "','" + jiepai[jj]
                                            + "')");
                                        Operation.runSql("insert into t_coursetable(taskid,weekdays,sections) values('" +
                                            taskid + "','" + zhouanpai3[ii].ElementAt(2).ToString() + "','" + jiepai[kk]
                                            + "')");
                                        Operation.runSql("insert into t_coursetable(taskid,weekdays,sections) values('" +
                                            taskid + "','" + zhouanpai3[ii].ElementAt(4).ToString() + "','" + jiepai[nn]
                                            + "')");
                                        return true;
                                    }
                    break;
                default:
                    break;
            }
            return false;
        }


        #endregion



        





    }
}