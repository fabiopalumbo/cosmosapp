using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XComosMobile.Pages.maintenance
{
    public class TaskModel : Observable
    {
        string m_Title;
        public string Title
        {
            get { return m_Title; }
            set
            {
                m_Title = value;
                OnPropertyChanged("Title");
            }
        }
        string m_SubTitle;
        public string SubTitle
        {
            get { return m_SubTitle; }
            set
            {
                m_SubTitle = value;
                OnPropertyChanged("SubTitle");
            }
        }
        string m_Progress;
        public string Progress
        {
            get { return m_Progress; }
            set
            {
                m_Progress = value;
                OnPropertyChanged("Progress");
            }
        }
    }
}
