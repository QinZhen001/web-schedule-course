using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
namespace 排课系统.admin
{
    public partial class paikemana : System.Web.UI.Page
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

                //绑定
                bind(); bind1();
            }
        }
        public void bind()
        {
            string sqlstr = "select t_coursetask.*,t1.name as teachnamez,t2.name as teachnamef,t3.name as teachnames from ((t_coursetask left join t_teacher t1 on teachidz=t1.teachid) left join t_teacher t2 on teachidf=t2.teachid) left join t_teacher t3 on teachids=t3.teachid " +
                " where (coursename like '%" + this.findinfo.Text + "%' OR LEN('" + this.findinfo.Text + "')=0) and t_coursetask.id not in (select taskid from t_coursetable) order by t_coursetask.id";
            GridView1.DataSource = Operation.getDatatable(sqlstr);
            GridView1.DataKeyNames = new string[] { "id" };//主键
            GridView1.DataBind();
        }
        public void bind1()
        {
            string
            sqlstr = "select t_coursetable.id as keyid,taskid,weekdays,sections,t_coursetask.*,t1.name as teachnamez,t2.name as teachnamef,t3.name as teachnames from (((t_coursetable left join t_coursetask on taskid=t_coursetask.id) left join t_teacher t1 on teachidz=t1.teachid) left join t_teacher t2 on teachidf=t2.teachid) left join t_teacher t3 on teachids=t3.teachid " +
                "where (coursename like '%" + this.findinfo1.Text + "%' OR LEN('" + this.findinfo1.Text + "')=0)";
            GridView2.DataSource = Operation.getDatatable(sqlstr);
            GridView2.DataKeyNames = new string[] { "keyid" };//主键
            GridView2.DataBind();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // 手动排课
            Response.Redirect("paikehandle.aspx?coursetaskid=" + GridView1.DataKeys[e.RowIndex].Value.ToString());
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // 未排课列表分页
            GridView1.PageIndex = e.NewPageIndex;
            //重新绑定　
            bind();
        }
        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // 已排课列表分页
            GridView2.PageIndex = e.NewPageIndex;
            //重新绑定　
            bind1();
        }
        protected void Button3_Click(object sender, EventArgs e)
        {
            bind(); // 未排课列表查询
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            bind1();// 已排课列表查询
        }



        #region 自动排课算法
        
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

            //首先考虑教师自身禁忌排课的条件tabootype为条件类型 0是教师条件 1是专业条件   shenhe为审核条件0表示未审核 1表示审核通过  2表示审核未通过
            string sqlstr = "select * from t_taboo where tabootype='0' and shenhe='1' and teachid='" + teachid.Trim() + "'"; //0就是教师
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            //自动排课
            //首先清空原先课表
            Operation.runSql("delete from t_coursetable");

            // 查看教学任务的专业和年级 进行排课
            DataTable dt_major = null, dt_grade = null;  
            dt_major = Operation.getDatatable("select DISTINCT major from t_coursetask");
            dt_grade = Operation.getDatatable("select DISTINCT grade from t_coursetask");


            //待排课datatable
            DataTable dt_coursetask = null;
            int i=0, j=0,k=0,n=0;
            string major=null,grade=null;
            string[] coursexingzhi = new string[3] { "专业必修","专业限选","专业任选"};

            string message = "";//排课失败的课程存储
            int[,] teacher_table; int[,] major_table; int[,] or_table;  // 教师空闲课表  专业年级空闲时间  或操作之后的空暇表
            int zhouxueshi = 0,coursetimes=0;  // 周学时 每周上课次数
            for (i = 0; i < dt_major.Rows.Count; ++i)
            { 
                // 逐一专业进行排课
                for (j = 0; j < dt_grade.Rows.Count; ++j)
                {
                    // 逐一年级进行排课
                    major=dt_major.Rows[i][0].ToString();
                    grade=dt_grade.Rows[j][0].ToString();
                    //WebMessageBox.Show(major+grade);
                    //课程优先顺序为：专业必修、专业限选、专业任选；
                    for (k = 0; k < 3; ++k)
                    {
                        dt_coursetask = Operation.getDatatable("select * from t_coursetask where major='" + major + "' and grade='" + grade + "' and coursexingzhi like '%"+coursexingzhi[k]+"%'");
                        for (n = 0; n < dt_coursetask.Rows.Count; ++n)
                        { 
                            // 逐门课程进行安排上课时间
                            /*算法思想：
                             * 当前课程：对教师获得一个4*5的时间空闲矩阵，对专业年级获得一个4*5的时间空闲矩阵  
                             * 然后进行或操作，空闲的地方就可以进行课程安排，如果空闲地方不足安排课程，那么就判断为自动排课失败
                             * 对于时间空闲矩阵需要考虑禁忌排课条件和自身的已经排课的条件
                             */
                            // 获得教师空闲时间表
                            teacher_table = generalTeachTable(dt_coursetask.Rows[n]["teachidz"].ToString(), dt_coursetask.Rows[n]["zhouci"].ToString());
                            //获得专业年级空闲时间表
                            major_table = generalMajorTable(dt_coursetask.Rows[n]["major"].ToString(), dt_coursetask.Rows[n]["grade"].ToString(), dt_coursetask.Rows[n]["zhouci"].ToString());
                            //空闲时间表或操作
                            or_table = tableOrOperate(teacher_table, major_table);
                            zhouxueshi = 2;  // 初始化为2吧
                            int.TryParse(dt_coursetask.Rows[n]["xueshijiangshouz"].ToString(),out zhouxueshi);
                            coursetimes = (zhouxueshi+1) / 2;
                            if (!paike(dt_coursetask.Rows[n]["id"].ToString(), coursetimes, or_table))  // 排课失败保存
                            { 
                                //排课失败
                                message = message + dt_coursetask.Rows[n]["coursename"].ToString() + Environment.NewLine;
                            }
                        }
                    }
                }
            }
            if (message.Length < 1) message = "自动排课完成";
            else message = "自动排课完成";// +Environment.NewLine + "下面为自动排课失败的课程" + Environment.NewLine + message;
            WebMessageBox.Show(message);
            bind();
        }
        #endregion

        #region 已排课程手动调节
        // 下面是手动调节已排课程
        protected void GridView2_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        { // 取消手动调节
            GridView2.EditIndex = -1;
            bind1();
        }

        protected void GridView2_RowEditing(object sender, GridViewEditEventArgs e)
        {// 点击手动调节
            GridView2.EditIndex = e.NewEditIndex;
            bind1();
        }

        protected void GridView2_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // 提交手动调节数据
            int weeksday = 0, sections = 0;
            if (!int.TryParse(((TextBox)(GridView2.Rows[e.RowIndex].Cells[13].Controls[0])).Text.ToString().Trim(), out weeksday) || weeksday < 1 || weeksday>5)
            {
                WebMessageBox.Show("星期只能为数字1,2,3,4,5"); return;
            }
            if (!int.TryParse(((TextBox)(GridView2.Rows[e.RowIndex].Cells[14].Controls[0])).Text.ToString().Trim(), out sections) || sections < 1 || sections>4)
            {
                WebMessageBox.Show("节次只能为数字1,2,3,4"); return;
            }

            // 下面进行手动调节  
            int[,] teacher_table; int[,] major_table; int[,] or_table;  // 教师空闲课表  专业年级空闲时间  或操作之后的空暇表
            teacher_table = generalTeachTable(GridView2.Rows[e.RowIndex].Cells[7].Text.Trim(), GridView2.Rows[e.RowIndex].Cells[6].Text.Trim());
            major_table = generalMajorTable(GridView2.Rows[e.RowIndex].Cells[9].Text.Trim(), GridView2.Rows[e.RowIndex].Cells[10].Text.Trim(), GridView2.Rows[e.RowIndex].Cells[6].Text.Trim());
            or_table = tableOrOperate(teacher_table, major_table);
            if (or_table[sections - 1, weeksday - 1] == 0)
            {
                //更新
                Operation.runSql("update t_coursetable set weekdays='" + weeksday + "',sections='" + sections + "' where id='" + GridView2.DataKeys[e.RowIndex].Value.ToString() + "'");
            }
            else
            {
                // 更新失败
                WebMessageBox.Show("调节失败，本专业年级和教师在此时间段不空闲"); //return;
            }
            GridView2.EditIndex = -1;
            bind1();
        }
        #endregion






    }
}