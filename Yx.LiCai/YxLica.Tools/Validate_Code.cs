using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using YxLiCai.Tools.Util;

namespace YxLiCai.Tools
{
    /// <summary>
    /// Validate_Code 的摘要说明。
    /// </summary>
    public class Validate_Code
    {
        [DllImport(@"urlmon.dll", CharSet = CharSet.Auto)]
        public extern static System.UInt32 FindMimeFromData(
            System.UInt32 pBC,
            [MarshalAs(UnmanagedType.LPStr)] System.String pwzUrl,
            [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
            System.UInt32 cbSize,
            [MarshalAs(UnmanagedType.LPStr)] System.String pwzMimeProposed,
            System.UInt32 dwMimeFlags,
            out System.UInt32 ppwzMimeOut,
            System.UInt32 dwReserverd
        );

        public const string VALID_CODE_SESSION_NAME = "Validate_Code";

 
        /// <summary>
        /// 显示验证码，并把验证码保存到Session["Validate_Code"]
        /// </summary>
        /// <param name="page">System.Web.UI.Page对象</param>
        public static void Show_Validate_Code(System.Web.UI.Page page)
        {
            try
            {
                Random ran = new Random();
                string strRandom = GetRandomCode(5);
                //将随机数(验证字串)写入Session
                page.Session[VALID_CODE_SESSION_NAME] = strRandom.ToLower();
                //字体名
                string strFontName = "Arial";
                //字体大小
                int intFontSize = 11;
                //图像宽
                int intWidth = 66;
                //图像长
                int intHeight = 28;
                //背景颜色
                //Color bgColor = ColorTranslator.FromHtml("#" + page.Request.QueryString["colorb"]);
                Color bgColor = Color.Black;
                //前景颜色
                //Color foreColor = ColorTranslator.FromHtml("#" + page.Request.QueryString["colorf"]);
                Color foreColor = Color.White;
                //产生字体
                Font forFont = new Font(strFontName, intFontSize, FontStyle.Bold);
                //生成图片
                Bitmap newBitmap = new Bitmap(intWidth, intHeight, PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(newBitmap);
                //定义一个四方形框与字片一样大小
                Rectangle newRect = new Rectangle(0, 0, intWidth, intHeight);
                //涂上背景色
                g.FillRectangle(new SolidBrush(bgColor), newRect);
                //写字
                g.DrawString(strRandom.ToString(), forFont, new SolidBrush(foreColor),6, 6);
                //生成一些随机的水平线
                Pen blackPen = new Pen(Color.Black, 0);
                Random rand = new Random();
                int y1 = rand.Next(10,intHeight-10);
                int y2 = rand.Next(10, intHeight - 10);
                g.DrawLine(blackPen, 0, y1, intWidth, y2);
                MemoryStream mStream = new MemoryStream();
                //存入MemoryStream
                newBitmap.Save(mStream, ImageFormat.Gif);
                g.Dispose();
                newBitmap.Dispose();
                //发送
                page.Response.ClearContent();
                page.Response.ContentType = "image/GIF";
                page.Response.BinaryWrite(mStream.ToArray()); 
   
                page.Response.End();
 
            }
            catch (Exception e)
            { 
            }
        }

        /// <summary>
        /// 将图片控制到指定的大小
        /// </summary>
        /// <param name="img">原图</param>
        /// <param name="width">长度</param>
        /// <param name="height">宽度</param>
        /// <returns></returns>
        public static byte[] ConvertImage(Image img, int width, int height)
        {
            Image nimg = new Bitmap(width, height);
            if (width > 0
                && height > 0
                && (img.Height != height && img.Width != width))
            {
                if (img.Height <= height && img.Width <= width)
                {
                    using (Graphics g = Graphics.FromImage(nimg))
                    {
                        g.Clear(Color.White);
                        g.DrawImage(img, (float)(width - img.Width) / 2, (float)(height - img.Height) / 2);
                        g.Save();
                    }
                }
                else
                {
                    Bitmap bmp = new Bitmap(width, height);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.Clear(Color.White);
                        if (img.Height <= height)//width超过了
                        {
                            //该计算方法在XP和服务器操作系统上不能正常显示
                            //nimg = img.GetThumbnailImage(width, (int)(img.Height * (float)width / img.Width), null, IntPtr.Zero);
                            //g.DrawImage(nimg, 0, ((float)height - nimg.Height) / 2);

                            g.DrawImage(img, new RectangleF(0, 
                                (height - img.Height * (float)width / img.Width) / 2, 
                                width, 
                                img.Height * (float)width / img.Width));
                        }
                        else if (img.Width <= width)//height超过了
                        {
                            //该计算方法在XP和服务器操作系统上不能正常显示
                            //nimg = img.GetThumbnailImage((int)(img.Width * (float)height / img.Height), height, null, IntPtr.Zero);
                            //g.DrawImage(nimg, ((float)width - nimg.Width) / 2, 0);

                            g.DrawImage(img, new RectangleF((width - img.Width * (float)height / img.Height) / 2, 
                                0, 
                                img.Width * (float)height / img.Height, 
                                height));
                        }
                        else
                        {
                            float xp = (float)width / img.Width;//x缩小倍数
                            float yp = (float)height / img.Height;//y缩小倍数
                            float p = xp < yp ? xp : yp;
                            //该计算方法在XP和服务器操作系统上不能正常显示
                            //nimg = img.GetThumbnailImage((int)(img.Width * p), (int)(img.Height * p), null, IntPtr.Zero);
                            //g.DrawImage(nimg, ((float)width - nimg.Width) / 2, ((float)height - nimg.Height) / 2);

                            g.DrawImage(img, new RectangleF(((float)width - img.Width * p) / 2, 
                                ((float)height - img.Height * p) / 2, 
                                img.Width * p, 
                                img.Height * p));
                        }
                        g.Save();
                    }
                    nimg = bmp;
                }
            }
            MemoryStream ms = new MemoryStream();
            nimg.Save(ms, img.RawFormat);
            return ms.ToArray();
        }

        public static string GetRandomCode(int CodeCount)
        {
            //string allChar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,M,N,P,Q,R,S,T,U,W,X,Y,Z";
            string allChar = "0,1,2,3,4,5,6,7,8,9";
            string[] allCharArray = allChar.Split(',');
            string RandomCode = "";
            int temp = -1;

            Random rand = new Random();
            for (int i = 0; i < CodeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(temp * i * ((int)DateTime.Now.Ticks));
                }

                int t = rand.Next(9);

                while (temp == t)
                {
                    t = rand.Next(9);
                }

                temp = t;
                RandomCode += allCharArray[t];
            }

            return RandomCode;
        }

    }
}