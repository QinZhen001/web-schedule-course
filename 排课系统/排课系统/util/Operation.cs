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

using System.Web.UI;

/// <summary>
/// Operation 网站业务流程类（封装所有业务方法）
/// </summary>
public class Operation
{
    public Operation()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    static DataBase data = new DataBase();

    static public DataTable getDatatable(string sql)
    {
        return data.RunProcReturn(sql, "sql").Tables[0];
    }
    static public void runSql(string sql)
    {
        data.RunProc(sql);
    }
}
