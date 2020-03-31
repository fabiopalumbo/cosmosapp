using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comos.Global;

namespace Comos.IO
{
    public static class ObjectSearch
    {
        public static IBRServiceContracts.CQueryResult Buscar(string strValorBuscar,Plt.IComosDProject project)
        {
            List<Plt.IComosDSpecification> listaCOMOS = null;


            listaCOMOS = SearchObjectByValue(strValorBuscar, project);

            return GetSearchData(listaCOMOS);


        }
        public static List<Plt.IComosDSpecification> SearchObjectByValue(string strValorBuscar, Plt.IComosDProject project)
        {

            Plt.IComosDSearchManager SearchManager = AppGlobal.Workset.GetSearchManager();
            //Plt.IComosDSearchCondition condicao = null;
            dynamic resultset = null;
            
            List<Plt.IComosDSpecification> listaRetorno = new List<Plt.IComosDSpecification>();            

            System.Diagnostics.Debug.WriteLine(SearchManager == null);            

            SearchManager.SystemType = 10;

            SearchManager.IsSearchCaseSensitive = true;

            SearchManager.RootObjects.Add(project);

            SearchManager.AppendSearchCondition("", "PROPERTY", "VALUE", "LIKE", "*" + strValorBuscar + "*");

            //condicao = (Plt.IComosDSearchCondition)SearchManager.SearchConditions.Item[1];

            //condicao.IsSearchCaseSensitive = false;

            resultset = SearchManager.Start();
            SearchManager.RetrieveData(0);
            

            for (int i = 1; i <= resultset.count; i++)
                {

                listaRetorno.Add(resultset.item(i));

                }


            SearchManager.Stop();

            return listaRetorno;
        }


        private static IBRServiceContracts.CQueryResult GetSearchData(List<Plt.IComosDSpecification> lista)
        {
            IBRServiceContracts.CQueryResult result = new IBRServiceContracts.CQueryResult();
            IBRServiceContracts.CColumn column = null;
            Plt.IComosBaseObject objetoCOMOS = null;

            result.Date = DateTime.Now;
            
            result.Columns = new List<IBRServiceContracts.CColumn>();

            column = new IBRServiceContracts.CColumn();
            column.Name = "Objeto";
            column.Description = "Objeto";
            column.ColumnIndex = 0;       
            result.Columns.Add(column);

            column = new IBRServiceContracts.CColumn();
            column.Name = "Atributo";
            column.Description = "Atributo";
            column.ColumnIndex = 1;
            result.Columns.Add(column);

            column = new IBRServiceContracts.CColumn();
            column.Name = "Valor";
            column.Description = "Valor";
            column.ColumnIndex = 2;
            result.Columns.Add(column);


            int rowCount = lista.Count;
            int columnsize = 3;
            result.Rows = new List<IBRServiceContracts.CRow>();

            for (int index = 0; index <= rowCount -1; ++index)
            {                
                IBRServiceContracts.CRow row = new IBRServiceContracts.CRow();
                row.Values = new IBRServiceContracts.CCell[columnsize];
                Plt.IComosDSpecification rowobject = lista[index] as Plt.IComosDSpecification;

                objetoCOMOS = (Plt.IComosBaseObject)rowobject.GetSpecOwner();

                row.Values[0] = new IBRServiceContracts.CCell()
                {
                    Value = objetoCOMOS.Name,
                    SystemUID = objetoCOMOS.SystemUID()
                };

                row.Values[1] = new IBRServiceContracts.CCell()
                {
                    Value = rowobject.Description,
                    SystemUID = objetoCOMOS.SystemUID()
                };

                row.Values[2] = new IBRServiceContracts.CCell()
                {
                    Value = rowobject.DisplayValue(),
                    SystemUID = objetoCOMOS.SystemUID()
                };


                result.Rows.Add(row);
            }            
            return result;
        }

    }
}
