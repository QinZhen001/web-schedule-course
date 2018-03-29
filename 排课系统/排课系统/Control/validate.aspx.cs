using System;
using System.Drawing;
using System.Web;

public partial class control_validate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string Vnum = MakeValidateCode();
            Session["LVNum"] = Vnum.ToLower();//写入session
            CreateCheckCodeImage(Vnum);
        }
    }

    string MakeValidateCode()
    {
        //char[] s = new char[]{'2','3','4','6','7','8','9','b'
        //,'c','d','e','f','g','h','j','k','m','n','p','q'
        //,'a','r','t','w','x','y','A','C','D','E','F','G'
        //,'H','J','K','M','N','P','Q','R','S','T','U','W'
        //,'X','Y'};
        char[] s = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        string num = "";
        Random r = new Random();
        for (int i = 0; i < 4; i++)
        {
            num += s[r.Next(0, s.Length)].ToString();
        }
        return num.ToUpper();//注意是大写的哦
    }

    public void CreateCheckCodeImage(string checkCode)
    {
        if (checkCode == null || checkCode.Trim() == String.Empty)
            return;
        System.Drawing.Bitmap image = new System.Drawing.Bitmap((int)Math.Ceiling((checkCode.Length * 14.5)), 22);
        Graphics g = Graphics.FromImage(image);
        try
        {
            //生成随机生成器
            Random random = new Random();
            //清空图片背景色
            g.Clear(Color.White);
            //画图片的背景噪音线
            for (int i = 0; i < 3; i++)
            {
                int x1 = random.Next(image.Width);
                int x2 = random.Next(image.Width);
                int y1 = random.Next(image.Height);
                int y2 = random.Next(image.Height);
                g.DrawLine(new Pen(Color.Black), x1, y1, x2, y2);
            }
            Font font = new System.Drawing.Font("Arial", 12, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
            System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Green, Color.DarkRed, 1.2f, true);
            g.DrawString(checkCode, font, brush, 2, 2);
            //画图片的前景噪音点
            for (int i = 0; i < 30; i++)
            {
                int x = random.Next(image.Width);
                int y = random.Next(image.Height);
                image.SetPixel(x, y, Color.FromArgb(random.Next()));
            }
            //画图片的边框线
            g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            Response.ClearContent();
            Response.ContentType = "image/Gif";
            Response.BinaryWrite(ms.ToArray());
        }
        finally
        {
            g.Dispose();
            image.Dispose();
        }
    }
}