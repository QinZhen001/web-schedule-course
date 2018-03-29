using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Data.SqlClient;

/// <summary>
/// DataBase 的摘要说明
/// </summary>
public class DataBase:IDisposable
{
	public DataBase()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    private SqlConnection con;  //创建连接对象
    #region   打开数据库连接
    /// <summary>
    /// 打开数据库连接.
    /// </summary>
    private void Open()
    {
        // 打开数据库连接
        if (con == null)
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConStr"].ToString());
        }
        if (con.State == System.Data.ConnectionState.Closed)
            con.Open();

    }
    #endregion

    #region  关闭连接
    /// <summary>
    /// 关闭数据库连接
    /// </summary>
    public void Close()
    {
        if (con != null)
            con.Close();
    }
    #endregion

    #region 释放数据库连接资源
    /// <summary>
    /// 释放资源
    /// </summary>
    public void Dispose()
    {
        // 确认连接是否已经关闭
        if (con != null)
        {
            con.Dispose();
            con = null;
        }
    }
    #endregion

    #region   传入参数并且转换为SqlParameter类型
    /// <summary>
    /// 转换参数
    /// </summary>
    /// <param name="ParamName">存储过程名称或命令文本</param>
    /// <param name="DbType">参数类型</param>
    /// <param name="Size">参数大小</param>
    /// <param name="Value">参数值</param>
    /// <returns>新的Parameter对象</returns>
    public SqlParameter MakeInParam(string ParamName, SqlDbType DbType, int Size, object Value)
    {
        return MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
    }

    /// <summary>
    /// 初始化参数值
    /// </summary>
    /// <param name="ParamName">存储过程名称或命令文本</param>
    /// <param name="DbType">参数类型</param>
    /// <param name="Size">参数大小</param>
    /// <param name="Direction">参数方向</param>
    /// <param name="Value">参数值</param>
    /// <returns>新的 parameter 对象</returns>
    public SqlParameter MakeParam(string ParamName, SqlDbType DbType, Int32 Size, ParameterDirection Direction, object Value)
    {
        SqlParameter param;

        if (Size > 0)
            param = new SqlParameter(ParamName, DbType, Size);
        else
            param = new SqlParameter(ParamName, DbType);

        param.Direction = Direction;
        if (!(Direction == ParameterDirection.Output && Value == null))
            param.Value = Value;
        return param;
    }
    #endregion

    #region   执行参数命令文本(无数据库中数据返回)
    /// <summary>
    /// 执行命令
    /// </summary>
    /// <param name="procName">命令文本</param>
    /// <param name="prams">参数对象</param>
    /// <returns></returns>
    public int RunProc(string procName, SqlParameter[] prams)
    {
        SqlCommand cmd = CreateCommand(procName, prams);
        cmd.ExecuteNonQuery();
        this.Close();
        //得到执行成功返回值
        return (int)cmd.Parameters["ReturnValue"].Value;
    }
    /// <summary>
    /// 直接执行SQL语句
    /// </summary>
    /// <param name="procName">命令文本</param>
    /// <returns></returns>
    public int RunProc(string procName)
    {
        this.Open();
        SqlCommand cmd = new SqlCommand(procName, con);
        cmd.ExecuteNonQuery();
        this.Close();
        return 1;
    }

    #endregion

    #region   执行参数命令文本(有返回值)
    /// <summary>
    /// 执行查询命令文本，并且返回DataSet数据集
    /// </summary>
    /// <param name="procName">命令文本</param>
    /// <param name="prams">参数对象</param>
    /// <param name="tbName">数据表名称</param>
    /// <returns></returns>
    public DataSet RunProcReturn(string procName, SqlParameter[] prams, string tbName)
    {
        SqlDataAdapter dap = CreateDataAdaper(procName, prams);
        DataSet ds = new DataSet();
        dap.Fill(ds, tbName);
        this.Close();
        //得到执行成功返回值
        return ds;
    }

    /// <summary>
    /// 执行命令文本，并且返回DataSet数据集
    /// </summary>
    /// <param name="procName">命令文本</param>
    /// <param name="tbName">数据表名称</param>
    /// <returns>DataSet</returns>
    public DataSet RunProcReturn(string procName, string tbName)
    {
        SqlDataAdapter dap = CreateDataAdaper(procName, null);
        DataSet ds = new DataSet();
        dap.Fill(ds, tbName);
        this.Close();
        //得到执行成功返回值
        return ds;
    }

    #endregion

    #region 将命令文本添加到SqlDataAdapter
    /// <summary>
    /// 创建一个SqlDataAdapter对象以此来执行命令文本
    /// </summary>
    /// <param name="procName">命令文本</param>
    /// <param name="prams">参数对象</param>
    /// <returns></returns>
    private SqlDataAdapter CreateDataAdaper(string procName, SqlParameter[] prams)
    {
        this.Open();
        SqlDataAdapter dap = new SqlDataAdapter(procName, con);
        dap.SelectCommand.CommandType = CommandType.Text;  //执行类型：命令文本
        if (prams != null)
        {
            foreach (SqlParameter parameter in prams)
                dap.SelectCommand.Parameters.Add(parameter);
        }
        //加入返回参数
        dap.SelectCommand.Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.Int, 4,
            ParameterDirection.ReturnValue, false, 0, 0,
            string.Empty, DataRowVersion.Default, null));

        return dap;
    }
    #endregion

    #region   将命令文本添加到SqlCommand
    /// <summary>
    /// 创建一个SqlCommand对象以此来执行命令文本
    /// </summary>
    /// <param name="procName">命令文本</param>
    /// <param name="prams"命令文本所需参数</param>
    /// <returns>返回SqlCommand对象</returns>
    private SqlCommand CreateCommand(string procName, SqlParameter[] prams)
    {
        // 确认打开连接
        this.Open();
        SqlCommand cmd = new SqlCommand(procName, con);
        cmd.CommandType = CommandType.Text;　　　　 //执行类型：命令文本

        // 依次把参数传入命令文本
        if (prams != null)
        {
            foreach (SqlParameter parameter in prams)
                cmd.Parameters.Add(parameter);
        }
        // 加入返回参数
        cmd.Parameters.Add(
            new SqlParameter("ReturnValue", SqlDbType.Int, 4,
            ParameterDirection.ReturnValue, false, 0, 0,
            string.Empty, DataRowVersion.Default, null));

        return cmd;
    }
    #endregion

}
