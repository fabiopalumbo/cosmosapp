using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComosWebSDK.Data;
using System.ComponentModel;

namespace XComosMobile.Pages.maintenance
{

    public class ViewModel : Observable
    {
        public ViewModel(CColumn[] columns, CRow row)
        {
            this.Columns = columns;
            this.Row = row;
        }

        public CColumn[] Columns { get; private set; }

        public CRow Row { get; private set; }

        public CCell[] Items { get { return this.Row.Items; } }

        bool m_IsVisible = true;
        public bool IsVisible
        {
            get { return m_IsVisible; }
            set
            {
                if (m_IsVisible == value)
                    return; // Avoid triggering updates when not needed.
                m_IsVisible = value;
                OnPropertyChanged("IsVisible");
            }
        }
    }
}
