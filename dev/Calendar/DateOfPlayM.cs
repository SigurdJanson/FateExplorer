using System;

namespace FateExplorer.Calendar
{
    public class DateOfPlayM : IDateOfPlay
    {

        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();  // this method hides the OnChange to simplify it


        public DateOfPlayM() : this(DateTime.Now) { }

        public DateOfPlayM(DateTime date)
        {
            Date = date;
        }


        private DateTime date;
        public DateTime Date 
        { 
            get => date; 
            set
            {
                date = value;
                NotifyStateChanged();
            }
        }
    }
}
