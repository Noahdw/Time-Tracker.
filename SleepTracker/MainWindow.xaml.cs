using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Quartz;
using Quartz.Impl;


namespace SleepTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TimeRecorder timer = new TimeRecorder();
        public MainWindow()
        {
            this.DataContext = timer;
            InitializeComponent();
            keyHook.Subscribe();
            quartzScheduler();
            // var t = Task.Run(() => { timer.beginTimer(); });
            timer.beginTimer();
        }
       GlobalHook keyHook = new GlobalHook();
        

        public void quartzScheduler()
        {
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            // get a scheduler
            IScheduler sched = schedFact.GetScheduler();
            sched.Start();

            // construct job info
            IJobDetail jobDetail = JobBuilder.Create<HelloJob>()
                .WithIdentity("myJob")
                .Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("myTrigger")
                // fire every hour
                .WithSimpleSchedule(x =>x.WithIntervalInHours(24).RepeatForever())
                // start on the next even hour
                .StartAt(DateBuilder.FutureDate(1, IntervalUnit.Second))
                .Build();

            sched.ScheduleJob(jobDetail, trigger);
        }

        class HelloJob : Quartz.IJob
        {
            public void Execute(IJobExecutionContext context)
            {
                
                string timerText = System.IO.File.ReadAllText(System.IO.Path.Combine(Environment.CurrentDirectory, "myText.txt"));
                PostStatusService1 post = new PostStatusService1();
                post.tryRequest("Amount of time spent on the computer today: " + timerText);
               
            }
        }
    }
}
