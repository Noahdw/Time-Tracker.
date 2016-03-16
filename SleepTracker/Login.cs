using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace SleepTracker
{
    class Login
    {
        public void userLogin()
        {
            if (ConfigurationManager.AppSettings["tokenKey"].Equals(""))
            {
                try
                {
                    PostStatusService1 post = new PostStatusService1();
                    post.tryRequests();
                }
                catch (Exception e)
                {

                    Console.WriteLine(e + "This was the login");
                }
                System.Diagnostics.Process.Start("http://google.com");
            }
            
        }
    }
}
