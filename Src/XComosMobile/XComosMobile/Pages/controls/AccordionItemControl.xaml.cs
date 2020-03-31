using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XComosMobile.Pages.controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccordionItemControl : Button
    {
        private string title = "";
        private string endstring = "";
        public AccordionItemControl()
        {
            InitializeComponent();
        }

        public string Title
        { get
            {
                return title;

            } 
            set
            {
                //itemLabel.Text = value;
                title = value;
            }
        }

        public string EndString
        {
            get
            {
                return endstring;

            }
            set
            {
                //endLabel.Text = value;
                endstring = value;
            }
        }

    }
}