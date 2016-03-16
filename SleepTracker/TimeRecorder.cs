using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Gma.System.MouseKeyHook;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;

namespace SleepTracker
{
    
    class TimeRecorder : GlobalHook, INotifyPropertyChanged
    {
        
        int maxTillAway = 0;
        Stopwatch mainTimer;
        TimeSpan timeSpan;
        AudioDetection audio = new AudioDetection();

        private string _displayTime;

        public string displayTime
        {
            get { return _displayTime; }
            set { _displayTime = value; INotifyPropertyChanged("displayTime"); }
        }
        //NotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void INotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        //Creates new thread for the timer to run on // perhaps redunant
        public void beginTimer()
        {
            Thread thread = new Thread(timerThread);
            thread.Start();    
        }
        //This method is responsible for the logic of when to start and stop timers
        private void SecondaryTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            maxTillAway++;

            if (maxTillAway > 30)
            {
                mainTimer.Stop();
                
            }
            else
            {
                mainTimer.Start();
                timeSpan = mainTimer.Elapsed;
                
                displayTime = string.Format("Time: {0}h {1}m {2}s {3}ms", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
            }
            if (hasEvent == true)
            {
                maxTillAway = 0;
                hasEvent = false;
            }
            if (audio.IsAudioPlaying(audio.GetDefaultRenderDevice()) == true)
            {
                maxTillAway = 0;
            }
            outputToFile(string.Format("Time: {0}h {1}m {2}s {3}ms", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds));

        }
        //Maybe remove later
        public void timerThread()
        {
            var secondaryTimer = new System.Timers.Timer(1000);
            
                
                mainTimer = new Stopwatch();
                mainTimer.Start();


                secondaryTimer.Elapsed += SecondaryTimer_Elapsed;
                secondaryTimer.Enabled = true;
   
        }

        public void outputToFile(string file)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "myText.txt");
            
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(file);
                }
            }
            else
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(file);
                }
            }
        }

        
    }

    
}
