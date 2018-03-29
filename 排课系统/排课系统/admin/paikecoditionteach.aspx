<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="paikecoditionteach.aspx.cs" Inherits="排课系统.admin.paikecoditionteach" %>

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
            <div class="crumb-list"><i class="icon-font"></i><a href="index.aspx">首页</a>
            <span class="crumb-step">&gt;</span><a href="paikecodition.aspx">排课条件</a>
            <span class="crumb-step">&gt;</span><span class="crumb-name">教师条件</span></div>
        </div>
        <div class="search-wrap">
            <div class="search-content">
                    <table class="search-tab">
                        <tr>
                            <th width="70">状态:</th>
                            <td>
                                <asp:DropDownList ID="DropDownList1" runat="server" class="common-text" 
                                    Height="25px" Width="121px">
                                </asp:DropDownList>
                            </td>
                            <th width="70">关键字:</th>
                            <td>
                               <asp:TextBox class="common-text" placeholder="关键字" ID="findinfo" runat="server" Type="text"></asp:TextBox>
                            </td>
                            
                            <td width="50"></td>
                            <td >
                                <asp:Button ID="Button1" runat="server" Text="查询" class="btn btn-primary btn2" 
                                    onclick="Button1_Click"/>
                            </td>
                            
                        </tr>
                    </table>
            </div>
        </div>
        <div class="result-wrap">
        <div align="center"><h1 style="font-size: large">教师申请禁忌排课列表</h1></div>
                <div class="result-content">
                     <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                          GridLines="None"  AllowPaging="True"  CssClass="result-tab" 
                         DataKeyNames="id" Width="100%" 
                         onpageindexchanging="GridView1_PageIndexChanging" 
                         onrowdeleting="GridView1_RowDeleting" 
                         onselectedindexchanging="GridView1_SelectedIndexChanging">
                         <Columns>
                            <asp:BoundField DataField="weekdays" HeaderText="星期" />
                            <asp:BoundField DataField="sections" HeaderText="节次" />
                            <asp:BoundField DataField="weeksstart" HeaderText="起止周" />
                            <asp:BoundField DataField="weeksend" HeaderText="截止周" />
                            <asp:BoundField DataField="teachid" HeaderText="申请教师工号" />
                            <asp:BoundField DataField="name" HeaderText="申请教师姓名" />
                            <asp:BoundField DataField="shenhestatus" HeaderText="审核状态" />
                            <asp:CommandField HeaderText="通过" ShowDeleteButton="True" DeleteText="通过" />
                             <asp:CommandField HeaderText="拒绝" SelectText="拒绝" ShowSelectButton="True" />
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
