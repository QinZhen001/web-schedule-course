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
                        class="crumb-name">教学任务</span></div>
            </div>
            <div style="width: 664px;padding:10px">
                <p style="font-size:16px;">
                    本课程是Web前端基础应用，其最主要的目标是激发学生的学习兴趣，培养学生自我约束、自我学习的能力，最终使学生养成良好的学习习惯，为其今后的职业生活、继续学习和终生发展奠定坚实的基础。本课程介绍HTML和W3C标准以及HTML文件基本结构，主要讲述了网页制作的基础知识，重点学习了制作网页的基本标签，如链接、表格、表单、列表，框架等。通过基础知识的学习，对网页可以有一定的了解，知道如何在网页中添加基本的元素，结合教学案例，学以致用，最后制作出简单的网页。
                </p>
                <img src="/Images/web1.jpg" alt="Alternate Text" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
