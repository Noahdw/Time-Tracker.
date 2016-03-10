using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterPoster
{
    class Program
    {
        static void Main(string[] args)
        {

            string timerText = System.IO.File.ReadAllText(@"C:\Users\Puter\Desktop\SleepTracker\myText.txt");
            PostStatusService post = new PostStatusService();
            post.tryRequest("Amount of time spent on the computer today: " + timerText);
        }
    }
}
