<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="排课系统.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>系统登录</title>
    <link href="../css/adminLogin.css" rel="stylesheet" type="text/css" />
</head>
<body onkeydown="onEnterDown();">
    <div id="top">
    </div>
    <div id="login">
        <div id="login_box">
            <div class="title">
            </div>
            <div class="content">
                <div class="left">
                </div>
                <div class="main">
                    <form id="login_form" runat="server">
                    <div class="login_userid">
                        <span>角  色：</span>
                        <asp:RadioButton ID="RadioButton1" runat="server" 
                            Text="教师" GroupName="type"/>
                            &nbsp;&nbsp;
                            <asp:RadioButton ID="RadioButton2" runat="server" Text="管理员" 
                            GroupName="type" Checked="True" />
                    </div>
                    <div class="login_userid">
                        <span>账　号：</span>
                        <asp:TextBox ID="username" runat="server" Width="198" Type="text"></asp:TextBox>
                    </div>
                    <div class="login_password">
                        <span>密　码：</span>
                        <asp:TextBox  ID="password" runat="server" Width="198" TextMode="Password"></asp:TextBox>
                    </div>
                    <div class="login_code">
                        <span>验证码：</span>
                        <input name="code" type="text" class="input" id="code" style="width:50px;" maxlength="4" autocomplete="off" runat="server" />
                        <img alt="" src="../Control/validate.aspx" id="getcode_img" title="看不清请点击！" />
                    </div>
                    <div class="login_button">
                        <asp:Button  runat="server" id="submit" style="margin:auto;" onclick="submit_login" />&nbsp;&nbsp;
                        <input type="reset" name="reset" id="reset" style="margin:auto;" value="" onclick="doReset();" />
                       
                    </div>
                    </form>
                    <div class="note">
                        * 不要在公共场合保存登录信息；<br />
                        * 为了保证您的帐号安全，退出系统时请注销登录
                        <span id="msg_tip"></span>
                    </div>
                </div>
                <div class="right">
                </div>
            </div>
        </div>
    </div>
    <div id="foot">
        <p style="padding-top:20px;">版权所有：XXXXXXXXX学校 地址：XXXXXXXXXXXXXXXXXXXXXX</p>
        <p>联系人：XXXXXXX   电话：0000-00000000   联系传真：0000-00000000</p>
    </div>

    <script src="../Scripts/jquery-1.6.min.js" type="text/javascript"></script>
    <script src="../Scripts/js_login.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#userid").focus();
            $("#getcode_img").click(ShowValidImage);
        })

        function onEnterDown() {//body的onkeydown事件时调用
            if (window.event.keyCode == 13) {
                submit_login();
            }
        }
    </script>        
</body>
</html>
