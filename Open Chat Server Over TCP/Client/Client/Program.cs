using System;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Chat());
        }
        
    }

    class Semaphore
    {
        public int count;

        public Semaphore()
        {
            count = 0;
        }

        public Semaphore(int InitialVal)
        {
            count = InitialVal;
        }

        public void Wait()
        {
            lock (this)
            {
                count--;
                if (count < 0)
                    Monitor.Wait(this, Timeout.Infinite);
            }
        }

        public void Signal()
        {
            lock (this)
            {
                count++;
                if (count <= 0)
                    Monitor.Pulse(this);
            }
        }
    }

}
