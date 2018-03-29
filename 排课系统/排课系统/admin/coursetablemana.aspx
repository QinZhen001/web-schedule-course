<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="coursetablemana.aspx.cs" Inherits="排课系统.admin.coursetablemana" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>后台管理</title>
    <link rel="stylesheet" type="text/css" href="../css/common.css"/>
    <link rel="stylesheet" type="text/css" href="../css/main.css"/>
    <script type="text/javascript" src="../Scripts/modernizr.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
<div class="topbar-wrap white">
    <div class="topbar-inner clearfix">
        <div class="topbar-logo-wrap clearfix">
            <h1 class="topbar-logo none"><a href="index.aspx" class="navbar-brand">后台管理</a></h1>
            <ul class="navbar-list clearfix">
                <li><a class="on" href="index.aspx">首页</a></li>
            </ul>
        </div>
        <div class="top-info-wrap">
            <ul class="top-info-list clearfix">
                <li> 
                    <asp:Label ID="Label1" runat="server" Text=""></asp:Label> </li>
                <li><a href="../Default.aspx">退出</a></li>
            </ul>
        </div>
    </div>
</div>

<div class="container clearfix">
    <div class="sidebar-wrap">
        <div class="sidebar-title">
            <h1>菜单</h1>
        </div>
        <div class="sidebar-content">
            <ul class="sidebar-list">
                <li>
                    <a href="#"><i class="icon-font">&#xe003;</i>基本操作</a>
                    <ul class="sub-menu">
                        <li><a href="majormana.aspx"><i class="icon-font">&#xe008;</i>专业管理</a></li>
                        <li><a href="teacherman.aspx"><i class="icon-font">&#xe005;</i>教师管理</a></li>
                    </ul>
                </li>
                <li>
                    <a href="#"><i class="icon-font">&#xe003;</i>排课操作</a>
                    <ul class="sub-menu">
                        <li><a href="courseplan.aspx"><i class="icon-font">&#xe052;</i>教学计划</a></li>
                        <li><a href="coursetask.aspx"><i class="icon-font">&#xe033;</i>教学任务</a></li>
                        <li><a href="paikecodition.aspx"><i class="icon-font">&#xe008;</i>排课条件</a></li>
                        <li><a href="paikemana.aspx"><i class="icon-font">&#xe005;</i>排课管理</a></li>
                        <li><a href="coursetablemana.aspx"><i class="icon-font">&#xe006;</i>课表管理</a></li>
                    </ul>
                </li>
                <li>
                    <a href="#"><i class="icon-font">&#xe018;</i>系统管理</a>
                    <ul class="sub-menu">
                        <li><a href="modifypwd.aspx"><i class="icon-font">&#xe017;</i>个人密码</a></li>
                    </ul>
                </li>
            </ul>
        </div>
    </div>
    <!--/sidebar-->
    <!--/main-->

    
    <div class="main-wrap">

        <div class="crumb-wrap">
            <div class="crumb-list"><i class="icon-font"></i><a href="index.aspx">首页</a><span class="crumb-step">&gt;</span><span class="crumb-name">课表管理</span></div>
        </div>
        <div class="search-wrap">
            <div class="search-content">
                    <table class="search-tab">
                        <tr>
                        <th width="70">专业:</th>
                        <td>
                            <asp:DropDownList ID="DropDownList1" class="common-text" runat="server" 
                                Height="25px" Width="189px">
                            </asp:DropDownList>
                        </td>
                         <th width="70">年级:</th>
                        <td>
                            <asp:DropDownList ID="DropDownList2" class="common-text" runat="server" 
                                Height="25px" Width="98px">
                            </asp:DropDownList>
                        </td>
                        <th width="50"></th>
                            <td><asp:Button ID="Button1" class="btn btn-primary btn2" runat="server" Text="查询" 
                                    style="margin:auto;" onclick="Button1_Click" /></td>
                                    <th width="50"></th>
                            <td><asp:Button ID="Button2" class="btn btn-success btn2" runat="server" Text="导出本课表" 
                                    style="margin:auto;" onclick="Button2_Click" /></td>
                                    <th width="50"></th>
                            <td><asp:Button ID="Button3" class="btn btn-default btn2" runat="server" Text="导出所有课表" 
                                    style="margin:auto;" onclick="Button3_Click" /></td>
                        </tr>
                    </table>
            </div>
        </div>
        <div class="result-wrap">
        <div align="center">
            <asp:Label ID="title" runat="server" Font-Bold="False" Font-Size="X-Large"></asp:Label>
        </div>
                <div class="result-content">
                     <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                          GridLines="None"  AllowPaging="True"  CssClass="result-tab" 
                          Width="100%" PageSize="22">
                         <Columns>
                            <asp:BoundField DataField="weekdays" HeaderText="周次" ReadOnly="True" >
                             <ItemStyle Width="15%" HorizontalAlign="Center" VerticalAlign="Middle" />
                             </asp:BoundField>
                            <asp:BoundField DataField="sections" HeaderText="节次" >
                             <ItemStyle Width="20%" HorizontalAlign="Center" VerticalAlign="Middle" />
                             </asp:BoundField>
                            <asp:BoundField DataField="course" HeaderText="课程教室" >
                             <ItemStyle Width="60%" />
                             </asp:BoundField>
                        </Columns>
                        <RowStyle ForeColor="#000066" />
                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="Black" />

                   </asp:GridView>
                </div>

        </div>
    </div>


</div>

</form>
</body>
</html>
