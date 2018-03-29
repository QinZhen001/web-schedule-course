<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="courseplanadd.aspx.cs" Inherits="排课系统.admin.courseplanadd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>后台管理</title>
    <link rel="stylesheet" type="text/css" href="../css/common.css"/>
    <link rel="stylesheet" type="text/css" href="../css/main.css"/>
    <script type="text/javascript" src="../Scripts/modernizr.min.js"></script>
</head>
<body>
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
    <div class="main-wrap">
        <div class="crumb-wrap">
            <div class="crumb-list"><i class="icon-font"></i><a href="index.aspx">首页</a><span class="crumb-step">&gt;</span><a href="courseplan.aspx">教学计划</a><span class="crumb-step">&gt;</span><span class="crumb-name">新增计划</span></div>
        </div>
        <form id="login_form" runat="server">
        <div class="result-wrap">
            <div class="result-title">
               
            </div>
            <div class="result-content">
                <ul class="sys-info-list">
                    <li>
                        <label class="res-lab">专业名称</label><asp:DropDownList ID="DropDownList1" class="common-text"
                            runat="server" Height="24px" Width="355px">
                        </asp:DropDownList>
                    </li>
                     <li>
                    <label class="res-lab">年级</label><asp:DropDownList ID="DropDownList2" runat="server" class="common-text"
                             Height="24px" Width="356px">
                         </asp:DropDownList>
                    </li>
                    <li>
                        <label class="res-lab">课程编号</label><asp:TextBox ID="TextBox1" runat="server" class="common-text"
                            Height="25px" Width="341px"></asp:TextBox>
                    </li>
                    <li>
                        <label class="res-lab">课程名称</label><asp:TextBox ID="TextBox2" runat="server" class="common-text"
                            Height="25px" Width="341px"></asp:TextBox>
                    </li>
                    <li>
                        <label class="res-lab">考核方式</label><asp:TextBox ID="TextBox3" runat="server" class="common-text"
                            Height="25px" Width="341px"></asp:TextBox>
                    </li>
                    <li>
                        <label class="res-lab">学分</label><asp:TextBox ID="TextBox4" runat="server" class="common-text"

                            Height="25px" Width="341px"></asp:TextBox>
                    </li>
                    <li>
                        <label class="res-lab">总学时</label><asp:TextBox ID="TextBox5" runat="server" class="common-text"
                            Height="25px" Width="341px"></asp:TextBox>
                    </li>
                    <li>
                        <label class="res-lab">讲授学时</label><asp:TextBox ID="TextBox6" runat="server" class="common-text"
                            Height="25px" Width="341px"></asp:TextBox>
                    </li>
                    <li>
                        <label class="res-lab">实验学时</label><asp:TextBox ID="TextBox7" runat="server" class="common-text"
                            Height="25px" Width="341px"></asp:TextBox>
                    </li>
                    <li>
                        <label class="res-lab"></label>
                        <asp:Button ID="Button1" runat="server" class="btn btn-primary btn6 mr10"
                            Text="保存" Height="33px" Width="72px" onclick="Button1_Click" />

                            <asp:Button ID="Button2" runat="server" class="btn btn-success btn6 mr10"
                            Text="返回" Height="33px" Width="72px" onclick="Button2_Click"  />
                    </li>
                </ul>
            </div>
        </div>
        </form>
    </div>
    <!--/main-->
</div>
</body>
</html>
