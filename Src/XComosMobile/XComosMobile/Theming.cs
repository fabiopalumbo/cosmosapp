using System;
using Xamarin.Forms;
using XComosMobile.Services;

namespace XComosMobile
{    
    public static class Theming
    {
        public enum Themes
        {
            Comos,
            Siemens,
            Light,
            Black,            
        }
        public static void ChangeTheme(string theme)
        {
            if(theme !=null)
            Theming.ChangeTheme((Theming.Themes)Enum.Parse(typeof(Theming.Themes), theme));

        }

        public static void ChangeTheme(Themes theme)
        {
            var platform = Services.XServices.Instance.GetService<Services.IPlatformSystem>();

            platform.SetCurrentThemme(theme);

            switch (theme)
            {
                case Themes.Siemens:
                {
                    var background = Color.FromRgb(215,215,205);
                    var primary = Color.FromRgb(0, 100, 135);
                    var accent = Color.FromRgb(175 ,35 ,95);
                    var secondary = Color.FromRgb(100 ,25 ,70);

                    App.Current.Resources["UIBackgroundColor"] = background;
                    App.Current.Resources["UITextColor"] = Color.Black;
                    App.Current.Resources["UIBackgoundInputColor"] = Color.Transparent;
                    App.Current.Resources["UIButtonColor"] = primary;
                    App.Current.Resources["UIButtonTextColor"] = Color.White;
                    App.Current.Resources["UIFabButton"] = accent;
                    App.Current.Resources["UIFabButtonText"] = Color.White;
                    App.Current.Resources["ComosColorNavBar"] = primary;
                    App.Current.Resources["ComosColorNavBarText"] = Color.White;
                    App.Current.Resources["ComosColorModuleCard"] = Color.White;
                    App.Current.Resources["ComosColorButtonColor"] = background.AddLuminosity(0.15);
                    App.Current.Resources["UIAccent"] = accent;
                    App.Current.Resources["spec-only-cache"] = Color.DarkBlue;
                    App.Current.Resources["spec-changed-session"] = Color.DarkOrange;

                    var statusBarStyleManager = DependencyService.Get<IStatusBarStyleManager>();
                    statusBarStyleManager.SetStatusBarColor(primary.WithLuminosity(primary.Luminosity * 0.8));
                }

            break;
                case Themes.Comos:
                    {
                        var background = Color.FromRgb(233, 245, 248);
                        var backgroundTxt = Color.Black;
                        var primary = Color.FromRgb(31, 143, 159);
                        var primaryTxt = Color.White;

                        var accent = Color.FromRgb(88, 193, 50);

                        App.Current.Resources["UIBackgroundColor"] = background;
                        App.Current.Resources["UITextColor"] = backgroundTxt;
                        App.Current.Resources["UIBackgoundInputColor"] = Color.White;
                        App.Current.Resources["UIButtonColor"] = primary;
                        App.Current.Resources["UIButtonTextColor"] = primaryTxt;
                        App.Current.Resources["UIFabButton"] = primary;
                        App.Current.Resources["UIFabButtonText"] = primaryTxt;
                        App.Current.Resources["ComosColorNavBar"] = primary;
                        App.Current.Resources["ComosColorNavBarText"] = primaryTxt;
                        App.Current.Resources["ComosColorModuleCard"] = Color.White;
                        App.Current.Resources["ComosColorButtonColor"] = background;
                        App.Current.Resources["UIAccent"] = accent;
                        App.Current.Resources["spec-only-cache"] = Color.DarkBlue;
                        App.Current.Resources["spec-changed-session"] = Color.DarkOrange;

                        var statusBarStyleManager = DependencyService.Get<IStatusBarStyleManager>();
                        statusBarStyleManager.SetStatusBarColor(primary.WithLuminosity(primary.Luminosity * 0.8));
                    }
                    break;
                case Themes.Light:
                    {
                        var background = Color.FromRgb(230, 230, 230);
                        var backgroundTxt = Color.Black;
                        var primary = Color.FromRgb(0, 0, 0);
                        var primaryTxt = Color.White;
                        var accent = Color.FromRgb(193, 0, 0);
                        var secondary = Color.FromRgb(193, 0, 0);

                        App.Current.Resources["UIBackgroundColor"] = background;
                        App.Current.Resources["UITextColor"] = backgroundTxt;
                        App.Current.Resources["UIBackgoundInputColor"] = Color.Transparent;
                        App.Current.Resources["UIButtonColor"] = primary;
                        App.Current.Resources["UIButtonTextColor"] = primaryTxt;
                        App.Current.Resources["UIFabButton"] = secondary;
                        App.Current.Resources["UIFabButtonText"] = Color.White;
                        App.Current.Resources["ComosColorNavBar"] = primary;
                        App.Current.Resources["ComosColorNavBarText"] = Color.White;
                        App.Current.Resources["ComosColorModuleCard"] = Color.White;
                        App.Current.Resources["ComosColorButtonColor"] = background;
                        App.Current.Resources["UIAccent"] = secondary;
                        App.Current.Resources["spec-only-cache"] = Color.DarkBlue;
                        App.Current.Resources["spec-changed-session"] = Color.DarkOrange;

                        var statusBarStyleManager = DependencyService.Get<IStatusBarStyleManager>();
                        statusBarStyleManager.SetStatusBarColor(primary.WithLuminosity(primary.Luminosity * 0.8));
                    }
                    break;

                case Themes.Black:
                    {
                        var background = Color.Black;
                        var backgroundTxt = Color.White;
                        var primary = Color.FromRgb(0, 255, 254);
                        var primaryTxt = Color.Black;
                        var accent = Color.FromRgb(0, 255, 215);
                        var secondary = primary;
                        var secondaryTxt = Color.FromRgb(0, 0, 0);

                        App.Current.Resources["UIBackgroundColor"] = background;
                        App.Current.Resources["UITextColor"] = backgroundTxt;
                        App.Current.Resources["UIBackgoundInputColor"] = Color.Transparent;
                        App.Current.Resources["UIButtonColor"] = secondary;
                        App.Current.Resources["UIButtonTextColor"] = secondaryTxt;
                        App.Current.Resources["UIFabButton"] = secondary;
                        App.Current.Resources["UIFabButtonText"] = secondaryTxt;
                        App.Current.Resources["ComosColorNavBar"] = primary;
                        App.Current.Resources["ComosColorNavBarText"] = primaryTxt;
                        App.Current.Resources["ComosColorModuleCard"] = Color.FromRgb(
                                background.R + backgroundTxt.R * 0.1,
                                background.G + backgroundTxt.G * 0.1,
                                background.B + backgroundTxt.B * 0.1);
                        App.Current.Resources["ComosColorButtonColor"] = background;
                        App.Current.Resources["UIAccent"] = secondary;
                        App.Current.Resources["spec-only-cache"] = Color.Blue;
                        App.Current.Resources["spec-changed-session"] = Color.Orange;

                        var statusBarStyleManager = DependencyService.Get<IStatusBarStyleManager>();
                        statusBarStyleManager.SetStatusBarColor(primary.WithLuminosity(primary.Luminosity * 0.8));
                    }
                    break;
                //case Themes.Cores:
                //    {
                //        var background = Color.FromRgb(91, 188, 147);
                //        var backgroundTxt = Color.FromRgb(255, 255, 255);
                //        var primary = Color.FromRgb(245, 200, 81);
                //        var primaryTxt = Color.FromRgb(0, 0, 0);
                //        var accent = primary;
                //        var secondary = Color.FromRgb(14, 67, 117);
                //        var secondaryTxt = Color.FromRgb(0, 0, 0);


                //        App.Current.Resources["UIBackgroundColor"] = background;
                //        App.Current.Resources["UITextColor"] = backgroundTxt;
                //        App.Current.Resources["UIBackgoundInputColor"] = Color.Transparent;
                //        App.Current.Resources["UIButtonColor"] = primary;
                //        App.Current.Resources["UIButtonTextColor"] = primaryTxt;
                //        App.Current.Resources["UIFabButton"] = primary;
                //        App.Current.Resources["UIFabButtonText"] = primaryTxt;
                //        App.Current.Resources["ComosColorNavBar"] = primary;
                //        App.Current.Resources["ComosColorNavBarText"] = primaryTxt;
                //        App.Current.Resources["ComosColorModuleCard"] = secondary;
                //        App.Current.Resources["ComosColorButtonColor"] = Color.White.MultiplyAlpha(0.1);
                //        App.Current.Resources["UIAccent"] = accent;
                //        App.Current.Resources["spec-only-cache"] = Color.DarkBlue;
                //        App.Current.Resources["spec-changed-session"] = Color.DarkOrange;
                //    }
                //    break;
                default:
                    break;
            }
            App.ApplyAccentColor((Color)App.Current.Resources["UIAccent"]);

        }

    }
}
