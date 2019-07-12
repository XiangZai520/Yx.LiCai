using System;
using System.Collections.Generic;
using System.Linq;


using System.Threading;

using YxLiCai.Model;
using YxLiCai.Model.User;
using YxLiCai.Server.User;
namespace ConsoleApplication1
{
    class Program
    {
        private static Thread LogThread = new Thread(new ThreadStart(DoService));
        private static UserInfoService user;
        private static bool bStop = true;
        static void Main(string[] args)
        {
            //设置线程为后台线程,那样进程里就不会有未关闭的程序了
            LogThread.IsBackground = true;
            if (bStop == true)
            {
                user = new UserInfoService();
                LogThread.Start();//起线程
            }
            Console.ReadKey();


        }

        #region 线程开始执行
        private static void DoService()
        {
            while (true)
            {
                string mer_ord_id = Guid.NewGuid().ToString("N");
                bStop = false;
                string Phone = GetRandomCode(11);
                string password = "123456";
                password = YxLiCai.Tools.SafeEncrypt.MD5.MD5Convert(password, 32);
                UserInfoModel model = new UserInfoModel();
                model.Password = password;
                model.RegTime = DateTime.Now;
                model.Phone = Phone;
                model.LoginName = Phone;
                model.Status = 1;
                model.MyCode = Phone;
                model.Sallpassword = "";
                model.IP = "127.0.0.1";
                ResultInfo<int> resultinsert = user.AddUser(model);
               

                if (resultinsert.Data > 0)
                {
                    user.AddUserInitialization(resultinsert.Data, Phone);
                    Console.Write("注册成功：" + Phone + " \n");
                }
                else
                {
                    Console.Write("注册失败：" + Phone + " \n");
                }


                System.Threading.Thread.Sleep(0);
            }
        }
        #endregion


        public static bool IsPhone(string input)
        {

            return System.Text.RegularExpressions.Regex.IsMatch(input, @"^[1]+[3,5]+\d{9}");
        }

        public static bool IsIDcard(string str_idcard)
        {

            return System.Text.RegularExpressions.Regex.IsMatch(str_idcard, @"(^\d{18}$)|(^\d{15}$)|(^\d{17}(\d|X|x)$)");

        }

        public static string GetRandomCode(int CodeCount)
        {
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

            #region 注释掉
            //string allChar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,M,N,P,Q,R,S,T,U,W,X,Y,Z";
            //string[] allCharArray = allChar.Split(',');
            //string RandomCode = "";
            //int temp = -1;

            //Random rand = new Random();
            //for (int i = 0; i < CodeCount; i++)
            //{
            //    if (temp != -1)
            //    {
            //        rand = new Random(temp * i * ((int)DateTime.Now.Ticks));
            //    }

            //    int t = rand.Next(33);

            //    while (temp == t)
            //    {
            //        t = rand.Next(33);
            //    }

            //    temp = t;
            //    RandomCode += allCharArray[t];
            //}
            #endregion

            return RandomCode;
        }
    }
}
