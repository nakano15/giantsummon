using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon
{
    public class GuardianSchedule
    {
        public byte MoonPhase = 0;
        //public List<DayTasks> Tasks = new List<DayTasks>();
        public Task task = Task.Explore;

        public enum Task
        {
            FreeDay,
            Explore
        }

        /*public class DayTasks
        {
            //public int StartHour = 0, StartMinute = 0, EndHour = 0, EndMinute = 0;
            //private double StartTime = 0, EndTime = 0;
            //private bool StartIsDayTime = false, EndIsDayTime = false;
        }*/
    }
}
