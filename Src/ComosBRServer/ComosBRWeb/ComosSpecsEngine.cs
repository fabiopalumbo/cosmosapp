using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBRServiceContracts;

namespace ComosBRWeb

{
    public static class ComosSpecsEngine
    {

        static public string RetornaHTMLTela(CComosObject comosObj)
        {


            return RetornaHTMLObjeto(comosObj);

        }

        static public string RetornaHTMLObjeto(CComosObject comosObj)
        {

            string htmlReturn = "";
            string htmlAux = "";
            string botoesOk = "";
            string tabName = "";
            int qtdSpecs = 0;


            CComosObject comosTab = null;
            
            htmlAux = "<div class=~bar bar-header bar-light~>" +
                      "<h1 class=~title~>" + comosObj.Description + "</h1>" +
                      "</div><div>";



            qtdSpecs = comosObj.Objects.Count();

            for (int i = 0; i <= qtdSpecs - 1; i++)
            {
                comosTab = comosObj.Objects[i];

                tabName = comosTab.Name;

                if (tabName != "Y00T00001")
                {
                    htmlAux = htmlAux + RetornaHTMLTab(comosObj, comosTab);
                }

            }

            botoesOk = "<button class=~button button-balanced~ >" +
                          "OK" +
                        "</button>" +
                        "<button class=~button button-assertive~>" +
                         "Cancel" +
                        "</button>";


            htmlReturn =  htmlAux + botoesOk + "</div>";

            return htmlReturn;

        }

        static public string RetornaHTMLTab(CComosObject comosObj, CComosObject comosTab)
        {

            string htmlReturn = "";
            string htmlAux = "";
            string nestedName = "";
            string ct = "";

            CComosObject comosAtributo = null;


            htmlAux = "<ion-card >" +
                    "<ion-card-header>" +                    
                    "<ion-item><h3>" + comosTab.Description + "</h3></ion-item>" +                         
                    "</ion-card-header>" +
                    "<ion-card-content> <div class=~list~>";

            for (int i = 0; i <= comosTab.Objects.Count - 1; i++)
            {

                comosAtributo = comosTab.Objects[i];

                ct = comosAtributo.ControlType.ToUpper();

                nestedName = comosAtributo.NestedName;

                switch (ct)
                {
                    case "COMOSSUIBUTTON.SUIBUTTON":
                        htmlAux = htmlAux + RetornaHTMLBotao(comosAtributo);
                        break;
                    case "COMOSSUIEDIT.SUIEDIT":
                        htmlAux = htmlAux + RetornaHTMLEditField(comosAtributo);
                        break;
                    case "COMOSSUILINK.SUILINK":
                        //htmlAux = htmlAux + RetornaHTMLLink(comosAtributo);
                        break;

                    default:
                        break;
                }

            }


            htmlAux = htmlAux + "</div> </ion-card-content>" +
                                     "</ion-card>";

            htmlReturn = htmlAux;

            return htmlReturn;

        }


        static public string RetornaHTMLLink(CComosObject comosSpec)
        {

            //string displayValue = "< label class="item item-input item-stacked-label">"; //+ 
            //                "<span class=""input-label"">First Name</span>" + 
            //                  "<input type = ""text"" placeholder= """ + comosSpec.DisplayValue + """ >" + 
            //                    "</ label >";

            string valorLink = "";
            /*
                Object objetoLinkado = null;

                objetoLinkado = comosSpec.LinkObject;

                if (objetoLinkado != null)
                {
                    IComosDDevice objetoCastedDev = objetoLinkado as IComosDDevice;
                    if (objetoCastedDev == null)
                    {
                        IComosDDocument objetoCastedDoc = objetoLinkado as IComosDDocument;

                        if (objetoCastedDoc != null)
                        {
                            valorLink = objetoCastedDoc.Name;
                        }
                    }
                    else
                    {
                        valorLink = objetoCastedDev.Name;
                    }
                }
                */

            string displayValue = "<label id = " + comosSpec.SystemUid + "class=~item item-input item-stacked-label~>" +
                                    "<span class=~input-label~>" + comosSpec.Description + "</span>" +
                                    "<input type = ~text~ placeholder= ~" + valorLink + "~>" +
                                    "</label>";


            return displayValue;
        }

        static public string RetornaHTMLEditField(CComosObject comosSpec)
        {

            //string displayValue = "< label class="item item-input item-stacked-label">"; //+ 
            //                "<span class=""input-label"">First Name</span>" + 
            //                  "<input type = ""text"" placeholder= """ + comosSpec.DisplayValue + """ >" + 
            //                    "</ label >";


            string displayValue = "<label id = " + comosSpec.SystemUid + " class=~item item-input item-stacked-label~>" +
                                    "<span class=~input-label~>" + comosSpec.Description + "</span>" +
                                    "<input type = ~text~ placeholder= ~" + comosSpec.Value + "~>" +
                                    "</label> \n";




            return displayValue;
        }

        static public string RetornaHTMLBotao(CComosObject comosSpec)
        {
            //string idBotao =  "'" + comosSpec.OwnerUID + "','" + comosSpec.NestedName + "'";

            string idBotao = comosSpec.OwnerUID + "/" + comosSpec.NestedName;

            string inicioHtml = "<button id = " + idBotao + " class=~button button-light~>" + comosSpec.Description + " </button>";

            //string inicioHtml = "<button name = ~" + comosSpec.NestedName + "~ value = ~" + comosSpec.OwnerUID + "~ id =~" + idBotao + "~ class=~button button-light~ (click)=~OnClickButton(" + idBotao + ")~>" + comosSpec.Description + " </button>";

            //string inicioHtml = "<button id = " + idBotao + " class=~button button-light~ (click)=~OnClickButton(" + idBotao + ")~>" + comosSpec.Description + " </button>";

            //                 string inicioHtml = "<center> <button primary round (click)=~OnClickButton('" + idBotao "')~>" + comosSpec.Description + " </button> </center>";


            return inicioHtml;

        }

    }


}