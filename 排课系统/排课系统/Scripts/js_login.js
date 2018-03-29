function submit_login() {
    //alert("5");
    var admin_uname = $("#username").val();
    var admin_psw = $("#password").val();
    var admin_yzm=$("#code").val();
    if(admin_uname==""){
        alert("请输入您用户账号！");
        $("#username").focus();
        return false
    }
    if(admin_psw==""){
        alert("请输入您的登录密码！");
        $("#password").focus();
        return false
    }
    if(admin_yzm==""){
        alert("验证码不能为空！");
        $("#code").focus();
        return false
    }
    if(document.getElementById("code").value.length!=4){
        alert("您输入的验证码不合理！");
        $("#code").select();
        return false
    }
    $.ajax({
        type: "get",
        url: "/Account/Login.aspx?paramName=" + escape(admin_uname) + "&paramPwd=" + admin_psw + "&validate=" + admin_yzm,
        eache: false,
        success: function(reg){
            var arr=reg.split('|');
            var returnval=arr[0].toString();
            var tip=arr[1].toString();
            
            if(returnval=="15")
            {
                $("#msg_tip").text(tip);
                document.getElementById("code").value="";
                $("#code").focus();
                ShowValidImage();
                return false;
            }
            if(returnval=="3")
            {
                $("#msg_tip").text(tip);
                document.getElementById("username").value = "";
                document.getElementById("password").value="";
                ShowValidImage();
                return false;
            }
            else if(returnval=="9")
            {
                $("#msg_tip").text(tip);
                document.getElementById("username").select();
                document.getElementById("code").value="";
                ShowValidImage();
                return false;
            }
            else if(returnval=="14")
            {
                $("#msg_tip").text(tip);
                document.getElementById("username").select();
                document.getElementById("code").value="";
                ShowValidImage();
                return false;
            }
            else if(returnval=="20" || returnval=="30")
            {
                $("#msg_tip").text(tip);
                document.getElementById("username").value = "";
                document.getElementById("password").value="";
                document.getElementById("code").value="";
                document.getElementById("username").focus();
                return false;
            }
            else if(returnval=="10")
            {
                $("#msg_tip").text("");
                window.location.href="Index.aspx";
            }
        },
        error: function(){
            $("#msg_tip").text("参数出错，请与管理员联系!");
            ShowValidImage();
            document.getElementById("code").value="";
        }    		
    });
}

function doReset() {            
    ShowValidImage();
    $("#username").focus();
}
 
//更换验证码
function ShowValidImage() { 
    var numkey = Math.random()+(new Date().getDate());        
    document.getElementById("getcode_img").src = "/Control/validate.aspx?NumKey="+numkey;
}
function doReset() {
    ShowValidImage();
}