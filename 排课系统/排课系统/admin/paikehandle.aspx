<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="paikehandle.aspx.cs" Inherits="排课系统.admin.paikehandle" %>

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
            <div class="crumb-list"><i class="icon-font"></i><a href="index.aspx">首页</a><span class="crumb-step">&gt;</span>
           <a href="paikemana.aspx">排课管理</a> <span class="crumb-step">&gt;</span>
            <span class="crumb-name">手动管理</span></div>
        </div>
        <div class="search-wrap">
            <div class="search-content">
                    <table class="search-tab">
                        <tr>
                            <th width="50"></th>
                            <th width="80">星期</th>
                            <td>
                                <asp:DropDownList ID="DropDownList1" runat="server" class="common-text" 
                                    Height="25px" Width="136px">
                                </asp:DropDownList>
                            </td>
                            <th width="80">节次</th>
                            <td>
                            <asp:DropDownList ID="DropDownList2" runat="server" class="common-text" 
                                    Height="25px" Width="136px">
                                </asp:DropDownList>
                            </td>
                            <td><asp:Button ID="Button2" class="btn btn-success btn2" runat="server" Text="安排上课" 
                                    style="margin:auto;" onclick="Button2_Click"  /></td>
                                    <th width="80"></th>
                           <td><asp:Button ID="Button1" class="btn btn-default btn2" runat="server" Text="保存排课" 
                                    style="margin:auto;" onclick="Button1_Click"  /></td>
                        </tr>
                    </table>
            </div>
        </div>
        <div class="result-wrap">
            <div align="center" style="font-size: large; font-weight: bold">待排课程任务</div>
                <div class="result-content">
                     <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                          GridLines="None"  AllowPaging="True"  CssClass="result-tab" 
                         DataKeyNames="id" Width="100%" PageSize="1">
                         <Columns>
                            <asp:BoundField DataField="xuhao" HeaderText="课号" ReadOnly="True" />
                            <asp:BoundField DataField="coursename" HeaderText="课程名称" />
                            <asp:BoundField DataField="coursexingzhi" HeaderText="课程性质" />
                            <asp:BoundField DataField="xueshiallz" HeaderText="周总学时" />
                            <asp:BoundField DataField="xueshijiangshouz" HeaderText="周讲授学时" />
                            <asp:BoundField DataField="xueshishiyanz" HeaderText="周实验学时" />
                            <asp:BoundField DataField="zhouci" HeaderText="上课周次" />
                            <asp:BoundField DataField="teachidz" HeaderText="主讲教师工号" />
                            <asp:BoundField DataField="teachnamez" HeaderText="主讲教师" />
                            <asp:BoundField DataField="major" HeaderText="专业" />
                            <asp:BoundField DataField="grade" HeaderText="年级" />
                            <asp:BoundField DataField="dianjiao" HeaderText="是否使用电教" />
                            <asp:BoundField DataField="shuangyu" HeaderText="是否双语教学" />
                            <asp:BoundField DataField="times" HeaderText="每周上课次数" />
                        </Columns>
                        <RowStyle ForeColor="#000066" />
                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="Black" />

                   </asp:GridView>
                </div>
        </div>
        <div class="result-wrap">
            <div align="center" style="font-size: large; font-weight: bold">每周上课时间</div>
                <div class="result-content">
                     <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" 
                          GridLines="None"  AllowPaging="True"  CssClass="result-tab" 
                         DataKeyNames="id" Width="100%" PageSize="3" 
                         onrowdeleting="GridView2_RowDeleting">
                         <Columns>
                            <asp:BoundField DataField="weekdays" HeaderText="星期" ReadOnly="True" />
                            <asp:BoundField DataField="sections" HeaderText="节次" />
                            <asp:CommandField HeaderText="删除" ShowDeleteButton="True" />
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
