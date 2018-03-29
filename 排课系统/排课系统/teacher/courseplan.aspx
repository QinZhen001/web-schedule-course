<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="courseplan.aspx.cs" Inherits="排课系统.teacher.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>后台管理</title>
    <link rel="stylesheet" type="text/css" href="../css/common.css" />
    <link rel="stylesheet" type="text/css" href="../css/main.css" />
    <script type="text/javascript" src="../Scripts/modernizr.min.js"></script>
</head>
<body>
    <form id="form2" runat="server">
    <div class="topbar-wrap white">
        <div class="topbar-inner clearfix">
            <div class="topbar-logo-wrap clearfix">
                <h1 class="topbar-logo none">
                    <a href="index.aspx" class="navbar-brand">后台管理</a></h1>
                <ul class="navbar-list clearfix">
                    <li><a class="on" href="index.aspx">首页</a></li>
                </ul>
            </div>
            <div class="top-info-wrap">
                <ul class="top-info-list clearfix">
                    <li>
                        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                    </li>
                    <li><a href="../Default.aspx">退出</a></li>
                </ul>
            </div>
        </div>
    </div>
    <div class="container clearfix">
        <div class="sidebar-wrap">
            <div class="sidebar-title">
                <h1>
                    菜单</h1>
            </div>
            <div class="sidebar-content">
                <ul class="sidebar-list">
                    <li><a href="#"><i class="icon-font">&#xe003;</i>基本操作</a>
                        <ul class="sub-menu">
                            <li><a href="majormana.aspx"><i class="icon-font">&#xe008;</i>专业管理</a></li>
                            <li><a href="teacherman.aspx"><i class="icon-font">&#xe005;</i>教师管理</a></li>
                        </ul>
                    </li>
                    <li><a href="#"><i class="icon-font">&#xe003;</i>排课操作</a>
                        <ul class="sub-menu">
                            <li><a href="courseplan.aspx"><i class="icon-font">&#xe052;</i>教学计划</a></li>
                            <li><a href="coursetask.aspx"><i class="icon-font">&#xe033;</i>教学任务</a></li>
                            <li><a href="paikecodition.aspx"><i class="icon-font">&#xe008;</i>排课条件</a></li>
                            <li><a href="paikemana.aspx"><i class="icon-font">&#xe005;</i>排课管理</a></li>
                            <li><a href="coursetablemana.aspx"><i class="icon-font">&#xe006;</i>课表管理</a></li>
                        </ul>
                    </li>
                    <li><a href="#"><i class="icon-font">&#xe018;</i>系统管理</a>
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
                <div class="crumb-list">
                    <i class="icon-font"></i><a href="index.aspx">首页</a><span class="crumb-step">&gt;</span><span
                        class="crumb-name">教学计划</span></div>
            </div>
            <ul style="padding:10px;">
                <li style="margin: 8px; line-height=20px;font-size=24px;">
                    <div>
                        <a href="#">项目1 初识Dreamweaver CS6-站点的创建和管理</a>
                    </div>
                </li>
                <li style="margin: 8px; line-height=20px;font-size=24px;">
                    <div>
                        <a href="#">项目2 “周庄，中国第一个水乡“——第一个页面</a>
                    </div>
                </li>
                <li style="margin: 8px; line-height=20px;font-size=24px;">
                    <div>
                        <a href="#">项目3“教材简介”—文本 与图像的应用</a>         
                    </div>
                </li>
                <li style="margin: 8px; line-height=20px;font-size=24px;">
                    <div>
                        <a href="#">  项目4 “教材简介”——超 链接的应用</a>
                    </div>
                </li>
                <li style="margin: 8px; line-height=20px;font-size=24px;">
                    <div>
                        <a href="#">项目5“教材简介”——CSS 样式的应用</a>
                    </div>
                </li>
                <li style="margin: 8px; line-height=20px;font-size=24px;">
                    <div>
                        <a href="#">项目6 “中国网通”——表 格布局</a>
                    </div>
                </li>
                <li style="margin: 8px; line-height=20px;font-size=24px;">
                    <div>
                        <a href="#">项目7 “我的个人简历” — —Div+CSS布局</a>             
                    </div>
                </li>
                <li style="margin: 8px; line-height=20px;font-size=24px;">
                    <div>
                        <a href="#">项目8 “QQ邮箱”——框架 布局</a>             
                    </div>
                </li>
            </ul>
        </div>
    </div>
    </form>
</body>
</html>
