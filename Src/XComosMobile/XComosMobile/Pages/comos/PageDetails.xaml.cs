using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ComosWebSDK;
using ComosWebSDK.Data;
using ComosWebSDK.Extensions;

namespace XComosMobile.Pages.comos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageDetails : PageTemplate
    {
        IComosWeb m_ComosWeb { get { return Services.XServices.Instance.GetService<IComosWeb>(); } }
        string m_Navigator;
        CObject obj;
        public PageDetails(string navigator, CObject o, string language)
        {
            MainObject = o;
            InitializeComponent();
            m_Navigator = navigator;
            obj = o;
            Device.BeginInvokeOnMainThread(async () => { await UpdateDetails(o, language); });
            StartToolBar();
        }

        private void StartToolBar()
        {

            ToolbarItems.Add(new ToolbarItem("Navegar", "", () =>
            {
                var projectdata = Services.XServices.Instance.GetService<ViewModels.ProjectData>();
                projectdata.SaveLastSession();

                var m_Client = Services.XServices.Instance.GetService<ComosWebSDK.IComosWeb>();
                m_Client.StartHeartBeat();

                comos.PageNavigator nav = new comos.PageNavigator(projectdata.SelectedLayer, projectdata.SelectedLanguage.LCID);
                nav.NavigateToObject(obj);
                App.Navigation.PushAsync(nav);

            }));

            ToolbarItems.Add(new ToolbarItem("Salvar", "", () =>
            {


            }));

            ToolbarItems.Add(new ToolbarItem("Detalhes", "", () =>
            {


            }));
        }
        public async Task UpdateDetails(CObject obj, string language)
        {
            /*
            Device.BeginInvokeOnMainThread(() =>
            {
           this.ShowSpinner("Fetching Details ...");
            });
            */

            var specifications = await m_ComosWeb.GetObjectSpecification(
                    m_Navigator, obj.ProjectUID, obj.OverlayUID, language, obj.UID);

            if (specifications == null)
                return;
            TableView table = new TableView();
            table.Root = new TableRoot();
            foreach (var spec in specifications)
            {
                TableSection section = new TableSection(spec.Description);
                var view = new ViewCell();
                view.View = await GetAttributesView(spec, obj, language);
                view.Height = view.View.MinimumHeightRequest;
                section.Add(view);
                table.Root.Add(section);
                System.Diagnostics.Debug.Assert(spec.IsReauthenticationRequired == false);
            }

            this.Content = table;
            /*
            Device.BeginInvokeOnMainThread(() =>
            {
            this.HideSpinner();
            });
            */
        }

        public async Task<View> GetAttributesView(CSpecification spec, CObject obj, string language)
        {

            StackLayout layout = new StackLayout();
            var html = await m_ComosWeb.GetObjectSpecificationAsHtml(
                  m_Navigator, spec.ProjectUID, spec.OverlayUID, language, obj.UID, spec.Name);

            var attributes = CHtmlParser.ParseAttributes(html);
            foreach (var attribute in attributes.Items)
            {
                var lbl = new Label();
                lbl.Text = attribute.Description + " = " + attribute.Value;
                if (attribute.HasUnits)
                    lbl.Text += " " + attribute.Unit;
                layout.Children.Add(lbl);
            }
            return layout;
            /*
            StackLayout layout = new StackLayout();
            AbsoluteLayout stack = new AbsoluteLayout();
            var html = await m_ComosWeb.GetObjectSpecificationAsHtml(
                  m_Navigator.Database, spec.ProjectUID, spec.OverlayUID, language, obj.UID, spec.Name);
            int maxheight = 0;
            var attributes = CHtmlParser.ParseAttributes(html);
            foreach (var attribute in attributes.Items)
            {
                if(attribute.Controls != null)
                {
                    foreach (var ctrl in attribute.Controls)
                    {
                        System.Diagnostics.Debug.WriteLine(
                            string.Format("{0} {1} {2} {3}",
                            ctrl.X, ctrl.Y, ctrl.Width, ctrl.Height));
                        if (maxheight < ctrl.Y)
                            maxheight = ctrl.Y;
                        switch (ctrl.UIType)
                        {
                            case UIType.SUICheckBox:
                                {
                                    Switch c = new Switch()
                                    {
                                        IsToggled = bool.Parse(attribute.Value),
                                    };
                                    AbsoluteLayout.SetLayoutBounds(c, new Rectangle(
                                        ctrl.X, ctrl.Y, ctrl.Width, ctrl.Height));
                                    stack.Children.Add(c);
                                }
                                break;
                            case UIType.SUIText:
                                {
                                    Label c = new Label()
                                    {
                                        Text = attribute.Description,
                                    };
                                    AbsoluteLayout.SetLayoutBounds(c, new Rectangle(
                                        ctrl.X, ctrl.Y, ctrl.Width, ctrl.Height));
                                    stack.Children.Add(c);
                                }
                                break;
                            case UIType.SUIOpenButton:
                            case UIType.SUITimeButton:
                            default:
                                break;
                        }
                    }
                }
            }
            stack.MinimumHeightRequest = maxheight;
            stack.HeightRequest = maxheight;
            layout.Children.Add(stack);
            return layout;
            */
        }
    }
}