using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// StringFormat 的摘要说明
/// </summary>
public class StringFormat
{
    #region 部分变量声明
    private static StringBuilder outstr;
    private static Regex objregex;
    #endregion

    #region 输出字符串
    /// <summary>
    /// 返回用于编辑的字符串并进行Html解码
    /// </summary>
    /// <param name="instr">要输出的字符串</param>
    /// <returns></returns>
    public static string OutString(string instr)
    {
        instr = HttpContext.Current.Server.HtmlDecode(instr);
        instr = instr.Replace("<br />" + Environment.NewLine, Environment.NewLine);
        return instr;
    }

    /// <summary>
    /// 返回用于显示的字符串并删除超过限定字数的字符
    /// </summary>
    /// <param name="instr">要输出的文本</param>
    /// <param name="WordCount">要输出的字数</param>
    /// <returns></returns>
    public static string OutString(string instr, int WordCount)
    {
        byte[] mybyte = System.Text.Encoding.Default.GetBytes(instr);
        if (mybyte.Length > WordCount)
        {
            outstr = new StringBuilder();
            for (int i = 0; i < instr.Length; i++)
            {
                byte[] tempByte = System.Text.Encoding.Default.GetBytes(outstr.ToString());
                if (tempByte.Length < WordCount * 2)
                {
                    outstr.Append(instr.Substring(i, 1));
                }
                else
                {
                    break;
                }
            }
            return outstr.ToString();
        }
        else
        {
            return instr;
        }
    }

    /// <summary>
    /// 返回用于显示的字符串并用省略号代替超过限定字数的字符
    /// </summary>
    /// <param name="instr">要输出的文本</param>
    /// <param name="WordCount">要输出的字数</param>
    /// <param name="Prolong">增加延长符号</param>
    /// <returns></returns>
    public static string OutString(string instr, int WordCount, bool Prolong)
    {
        byte[] mybyte = System.Text.Encoding.Default.GetBytes(instr);
        if (mybyte.Length > WordCount)
        {
            outstr = new StringBuilder();
            for (int i = 0; i < instr.Length; i++)
            {
                byte[] tempByte = System.Text.Encoding.Default.GetBytes(outstr.ToString());
                if (tempByte.Length < WordCount * 2)
                {
                    outstr.Append(instr.Substring(i, 1));
                }
                else
                {
                    if (Prolong)
                    {
                        outstr.Append("...");
                    }
                    break;
                }
            }
            return outstr.ToString();
        }
        else
        {
            return instr;
        }
    }
    #endregion

    #region 输入字符串
    /// <summary>
    /// 返回单行输入的字符串并进行Html编码
    /// </summary>
    /// <param name="instr">要过滤的字符串</param>
    /// <returns></returns>
    public static string InString(string instr)
    {
        instr = instr.Trim();
        objregex = new Regex(" +");
        instr = objregex.Replace(instr, " ");
        instr = HttpContext.Current.Server.HtmlEncode(instr);
        instr = instr.Replace("'", "''");
        return instr;
    }

    /// <summary>
    /// 返回表示Url地址的字符串并进行Html编码
    /// </summary>
    /// <param name="instr">要过滤的字符串</param>
    /// <returns></returns>
    public static string InStrUrl(string instr)
    {
        instr = instr.Trim();
        objregex = new Regex(" +");
        instr = objregex.Replace(instr, " ");
        instr = instr.Replace(" ", "%20");
        instr = instr.Replace("'", "%27");
        instr = instr.Replace("\"", "%22");
        instr = instr.Replace("<", "%3C");
        instr = instr.Replace(">", "%3E");
        instr = instr.Replace("#", "%23");
        instr = instr.Replace("$", "%24");
        instr = instr.Replace("\\", "%5C");
        return instr;
    }

    /// <summary>
    /// 返回多行输入的字符串并删除超过限定字数的字符同时进行Html编码
    /// </summary>
    /// <param name="instr">要过滤的字符串</param>
    /// <returns></returns>
    public static string InMultiLine(string instr)
    {
        return MultiLineStrConv(instr, 0, true);
    }

    /// <summary>
    /// 返回多行输入的字符串并删除超过限定字数的字符同时进行Html编码
    /// </summary>
    /// <param name="instr">要过滤的字符串</param>
    /// <param name="NewLine">设置一个值，该值表示是否显示换行符。</param>
    /// <returns></returns>
    public static string InMultiLine(string instr, bool NewLine)
    {
        return MultiLineStrConv(instr, 0, NewLine);
    }

    /// <summary>
    /// 返回多行输入的字符串并删除超过限定字数的字符同时进行Html编码
    /// </summary>
    /// <param name="instr">要过滤的字符串</param>
    /// <param name="WordCount">保留的字数</param>
    /// <returns></returns>
    public static string InMultiLine(string instr, int WordCount)
    {
        return MultiLineStrConv(instr, WordCount, true);
    }

    /// <summary>
    /// 返回多行输入的字符串并删除超过限定字数的字符同时进行
    /// </summary>
    /// <param name="instr">要过滤的字符串</param>
    /// <param name="WordCount">保留的字数</param>
    /// <param name="NewLine">设置一个值，该值表示是否显示换行符。</param>
    /// <returns></returns>
    public static string InMultiLine(string instr, int WordCount, bool NewLine)
    {
        return MultiLineStrConv(instr, WordCount, NewLine);
    }

    #region 用于处理多行文本的公共方法
    /// <summary>
    /// 返回多行输入的字符串并删除超过限定字数的字符同时进行Html编码
    /// </summary>
    /// <param name="instr">要过滤的字符串</param>
    /// <param name="WordCount">保留的字数</param>
    /// <param name="NewLine">设置一个值，该值表示是否显示换行符。</param>
    /// <returns></returns>
    private static string MultiLineStrConv(string instr, int WordCount, bool NewLine)
    {
        instr = instr.Trim();
        objregex = new Regex(" +");
        instr = objregex.Replace(instr, " ");
        instr = instr.Replace(Environment.NewLine + " ", Environment.NewLine);
        instr = instr.Replace(" " + Environment.NewLine, Environment.NewLine);
        instr = instr.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
        instr = HttpContext.Current.Server.HtmlEncode(instr);
        instr = instr.Replace("'", "''");
        if (NewLine)
        {
            instr = instr.Replace(Environment.NewLine, "<br />" + Environment.NewLine);
        }
        if (WordCount > 0 && instr.Length > WordCount)
        {
            instr = instr.Substring(0, WordCount);
        }
        return instr;
    }
    #endregion

    /// <summary>
    /// 返回MD5加密的字符串
    /// </summary>
    /// <param name="instr">要加密的字符串</param>
    /// <returns></returns>
    public static string EncryptPassWord(string instr)
    {
        instr = instr.Trim();
        instr = FormsAuthentication.HashPasswordForStoringInConfigFile(instr, "MD5");
        return instr;
    }
    #endregion

    /// <summary>
    /// 格式化输出的字符串,超出的部分使用....显示
    /// </summary>
    /// <param name="instr">需要格式化的字符串</param>
    /// <param name="count">截取多少位数</param>
    /// <returns></returns>
    public static string Out(string instr, int count)
    {
        return OutString(instr, count, true);//要格式化的字符串及要保留的字数
    }
    /// <summary>
    /// 高亮输出字符串,True显示红色,Blue显示兰色。
    /// </summary>
    /// <param name="instr">需要格式化的字符串</param>
    /// <param name="light">是否需要加高亮显示</param>
    /// <returns></returns>
    public static string HighLight(string instr, bool light)
    {
        if (light)
        {
            instr = "<span style='color:red'>" + instr + "</span>";//要加亮的文本，Red
        }
        else
        {
            instr = "<span style='color:blue'>" + instr + "</span>";//要加亮的文本，Blue
        }
        return instr;
    }
}
