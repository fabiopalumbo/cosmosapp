using System;
using Xamarin.Forms.Xaml;
using ComosWebSDK.Data;
using XComosMobile.Pages.maintenance;
using System.Collections.Generic;

namespace XComosMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageSearchMaterial : PageTemplate
    {
        bool isCalledFromWorkpackage;
        CObject workpackage;
        ViewModels.ProjectData projectdata = Services.XServices.Instance.GetService<ViewModels.ProjectData>();

        string _nroDeItem = "";
        public string nroDeItem { get => _nroDeItem; set { _nroDeItem = value; OnPropertyChanged("nroDeItem"); } }
        string _Field1 = "";
        public string Field1 { get => _Field1; set { _Field1 = value; OnPropertyChanged("Field1"); } }
        string _Field2 = "";
        public string Field2 { get => _Field2; set { _Field2 = value; OnPropertyChanged("Field2"); } }
        string _Field3 = "";
        public string Field3 { get => _Field3; set { _Field3 = value; OnPropertyChanged("Field3"); } }

        string _familia = "";
        public string familia { get => _familia; set { _familia = value; OnPropertyChanged("familia"); } }
        string _subfamilia = "";
        public string subfamilia { get => _subfamilia; set { _subfamilia = value; OnPropertyChanged("subfamilia"); } }
        string _paniol = "";
        public string paniol { get => _paniol; set { _paniol = value; OnPropertyChanged("paniol"); } }

        public PageSearchMaterial()
        {
            InitializeComponent();
            this.isCalledFromWorkpackage = false;
            this.BindingContext = this;
            autoFillPanol();
        }

        public PageSearchMaterial(CObject workpackage)
        {
            InitializeComponent();
            this.isCalledFromWorkpackage = true;
            this.workpackage = workpackage;
            this.BindingContext = this;
            autoFillPanol();
        }

        async void OnSearchMaterialClicked(object sender, EventArgs e)
        {
            try
            {
                if (paniol != "")
                {
                    if (paniolAccepted())
                    {
                        if (userKnowsItemNumberOrOtherFields())
                        {
                            var plataform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();

                            if (plataform.IsOnline)
                            {
                                if (isCalledFromWorkpackage)
                                {
                                    await App.Navigation.PushAsync(new SearchMaterial(workpackage, nroDeItem, Field1, Field2, Field3, familia, subfamilia, paniol));
                                }
                                else
                                {
                                    await App.Navigation.PushAsync(new SearchMaterial(nroDeItem, Field1, Field2, Field3, familia, subfamilia, paniol));
                                }
                            }
                            else
                            {
                                this.ShowToast("No puede realizar la búsqueda sin conexión");
                            }
                        }
                        else
                        {
                            this.ShowToast("Debe completar el Nro. de item o al menos uno de los otros campos");
                        }
                    }
                    else
                    {
                        this.ShowToast("Número de pañol inválido para esta planta");
                    }
                }
                else
                {
                    this.ShowToast("El número de pañol no puede estar vacío");
                }
            }
            catch (Exception ex)
            {
                this.ShowToast($"Error: {ex.Message}");
            }

        }

        public bool paniolAccepted()
        {
#if DEBUG
            return true;
#elif I2G
				return true;
#else
				try
					{
						if (isCalledFromWorkpackage)
						{
							foreach (KeyValuePair<string, List<string>> myPlant in comos.Constants.panolDictionary)
							{
								if (myPlant.Key == projectdata.SelectedProject.Name)
								{
									if (myPlant.Value.Contains(paniol)) { return true; };
								}
							}
							return false;
						}
						else  //if not called from WP
						{
							return true;
						}
					}
					catch (Exception ex)
					{
						this.ShowToast($"Error al validar el pañol. Error: {ex.Message}");
						return false;
					}
#endif
        }

        public bool userKnowsItemNumberOrOtherFields()
        {
            if (nroDeItem != "")
            {
                Field1 = ""; Field2 = ""; Field3 = ""; familia = ""; subfamilia = "";    //ignore all other fields if nro de item isnt blank
                return true;
            }
            else if (Field1 != "" || Field2 != "" || Field3 != "" || familia != "" || subfamilia != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void autoFillPanol()
        {
            try
            {
                foreach (KeyValuePair<string, List<string>> myPlant in comos.Constants.panolDictionary)
                {
                    if (myPlant.Key == projectdata.SelectedProject.Name)
                    {
                        _paniol = myPlant.Value[0];
                        paniol = _paniol;
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowToast($"Error al autollenar el pañol. Error: {ex.Message}");
            }

        }

    }

}