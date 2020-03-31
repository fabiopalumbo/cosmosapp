using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comos.Global;

namespace BRComos.IO.Utilities
{
    public static class ComosObjectSearch
    {
        public static IBRServiceContracts.TResult<IBRServiceContracts.CQueryResult> BuscarDevices(string strValorBuscar, string filter)
        {

            List<Plt.IComosDDevice> listaCOMOSDevices = SearchObjectByDescription(strValorBuscar, AppGlobal.Workset.GetCurrentProject());

            if (filter != "")
                listaCOMOSDevices = listaCOMOSDevices.Where(x => x.CObject.SystemFullName().Contains(filter)).ToList() ;

            var data = GetSearchData(listaCOMOSDevices);

            if (data == null)
            {
                return new IBRServiceContracts.TResult<IBRServiceContracts.CQueryResult>()
                {
                    Status = false,
                };
            }
            return new IBRServiceContracts.TResult<IBRServiceContracts.CQueryResult>()
            {
                data = data,
                Status = true,
            };

        }

        public static IBRServiceContracts.TResult<IBRServiceContracts.CQueryResult> BuscarTudo(string strValorBuscar)
        {
            List<Plt.IComosDDocument> listaCOMOSDocuments = SearchDocuments(strValorBuscar, AppGlobal.Workset.GetCurrentProject());
            List<Plt.IComosDDevice> listaCOMOSDevices = SearchObjectByDescription(strValorBuscar, AppGlobal.Workset.GetCurrentProject());

            var data = GetSearchData(listaCOMOSDevices, listaCOMOSDocuments);

            if (data == null)
            {
                return new IBRServiceContracts.TResult<IBRServiceContracts.CQueryResult>()
                {
                    Status = false,
                };
            }
            return new IBRServiceContracts.TResult<IBRServiceContracts.CQueryResult>()
            {
                data = data,
                Status = true,
            };

        }

        public static IBRServiceContracts.TResult<IBRServiceContracts.CQueryResult> BuscarDocuments(string strValorBuscar)
        {
            List<Plt.IComosDDocument> listaCOMOSDocuments = SearchDocuments(strValorBuscar, AppGlobal.Workset.GetCurrentProject());
            
            var data = GetSearchData(listaCOMOSDocuments);

            if (data == null)
            {
                return new IBRServiceContracts.TResult<IBRServiceContracts.CQueryResult>()
                {
                    Status = false,
                };
            }
            return new IBRServiceContracts.TResult<IBRServiceContracts.CQueryResult>()
            {
                data = data,
                Status = true,
            };

        }
        public static IBRServiceContracts.TResult<IBRServiceContracts.CQueryResult> Buscar(string strValorBuscar, Plt.IComosDProject project)
        {
            List<Plt.IComosDSpecification> listaCOMOSAtributos = null;
            List<Plt.IComosDDevice> listaCOMOSDevices = null;
            List<Plt.IComosDDocument> listaCOMOSDocuments = null;

            listaCOMOSAtributos = SearchObjectByValue(strValorBuscar, project);

            listaCOMOSDevices = SearchObjectByDescription(strValorBuscar, project);

            listaCOMOSDocuments = SearchDocuments(strValorBuscar, project);

            var data =  GetSearchData(listaCOMOSAtributos, listaCOMOSDevices, listaCOMOSDocuments);
            if (data == null)
            {
                return new IBRServiceContracts.TResult<IBRServiceContracts.CQueryResult>()
                {
                    Status = false,
                };
            }
            return new IBRServiceContracts.TResult<IBRServiceContracts.CQueryResult>()
            {
                data = data,
                Status = true,
            };

        }

        public static List<Plt.IComosDSpecification> SearchObjectByValue(string strValorBuscar, Plt.IComosDProject project)
        {

            Plt.IComosDSearchManager SearchManager = AppGlobal.Workset.GetSearchManager();
            //Plt.IComosDSearchCondition condicao = null;
            dynamic resultset = null;

            List<Plt.IComosDSpecification> listaRetorno = new List<Plt.IComosDSpecification>();

            System.Diagnostics.Debug.WriteLine(SearchManager == null);

            SearchManager.SystemType = 10;
            SearchManager.IsSearchCaseSensitive = false;
            SearchManager.RootObjects.Add(project);
            SearchManager.AppendSearchCondition("", "PROPERTY", "VALUE", "LIKE", "*" + strValorBuscar + "*");

            //condicao = (Plt.IComosDSearchCondition)SearchManager.SearchConditions.Item[1];
            //condicao.IsSearchCaseSensitive = false;

            resultset = SearchManager.Start();
            SearchManager.RetrieveData(0);

            for (int i = 1; i <= resultset.count; i++)
            {
                listaRetorno.Add(resultset.item(i));
                Plt.IComosDSpecification curSpec = (Plt.IComosDSpecification)resultset.item(i);
                if (curSpec.Layers != "~")
                {
                    listaRetorno.Add(resultset.item(i));
                }
            }

            SearchManager.Stop();
            return listaRetorno;
        }

        public static List<Plt.IComosDDevice> SearchObjectByDescription(string strValorBuscar, Plt.IComosDProject project)
        {

            Plt.IComosDSearchManager SearchManager = AppGlobal.Workset.GetSearchManager();            
            dynamic resultset = null;

            List<Plt.IComosDDevice> listaRetorno = new List<Plt.IComosDDevice>();

            Log.WriteLog("SearchObjectByDescription: START::" + strValorBuscar + "::" + (project == null).ToString());

            System.Diagnostics.Debug.WriteLine(SearchManager == null);

            SearchManager.SystemType = 8;
            SearchManager.IsSearchCaseSensitive = false;
            SearchManager.RootObjects.Add(project);
            SearchManager.AppendSearchCondition("", "PROPERTY", "DESCRIPTION", "LIKE", "*" + strValorBuscar + "*");
            SearchManager.AppendSearchCondition("OR", "PROPERTY", "NAME", "LIKE", "*" + strValorBuscar + "*");

            resultset = SearchManager.Start();
            SearchManager.RetrieveData(0);

            for (int i = 1; i <= resultset.count; i++)
            {
                Plt.IComosDDevice curDev = (Plt.IComosDDevice)resultset.item(i);

                if (curDev.Layers != "~" && curDev.CObject !=null)
                {
                    listaRetorno.Add(resultset.item(i));
                }                    
            }

            SearchManager.Stop();
            Log.WriteLog("SearchManager.Stop: OK");

            return listaRetorno;
        }

        public static List<Plt.IComosDDocument> SearchDocuments(string strValorBuscar, Plt.IComosDProject project)
        {

            Plt.IComosDSearchManager SearchManager = AppGlobal.Workset.GetSearchManager();
            dynamic resultset = null;
            Plt.IComosDDocument curDocument = null;
            List<Plt.IComosDDocument> listaRetorno = new List<Plt.IComosDDocument>();

            System.Diagnostics.Debug.WriteLine(SearchManager == null);

            SearchManager.SystemType = 29;
            SearchManager.IsSearchCaseSensitive = false;
            SearchManager.RootObjects.Add(project);
            SearchManager.AppendSearchCondition("", "PROPERTY", "DESCRIPTION", "LIKE", "*" + strValorBuscar + "*");
            SearchManager.AppendSearchCondition("OR", "PROPERTY", "NAME", "LIKE", "*" + strValorBuscar + "*");

            resultset = SearchManager.Start();
            SearchManager.RetrieveData(0);

            Log.WriteLog("SearchDocuments.rowCount: " + resultset.count);

            for (int i = 1; i <= resultset.count; i++)
            {
                curDocument = (Plt.IComosDDocument)resultset.item(i);
                if (!curDocument.IsDdmVersion())
                {
                    if (curDocument.Layers != "~")
                    {
                        listaRetorno.Add(curDocument);
                    }                    
                }
                
            }

            SearchManager.Stop();
            return listaRetorno;
        }

        private static IBRServiceContracts.CQueryResult GetSearchData(List<Plt.IComosDSpecification> lista, List<Plt.IComosDDevice> listaCOMOSDevices, List<Plt.IComosDDocument> listaCOMOSDocuments)
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
            column.Name = "Tipo";
            column.Description = "Tipo";
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

            for (int index = 0; index <= rowCount - 1; ++index)
            {
                IBRServiceContracts.CRow row = new IBRServiceContracts.CRow();
                row.Values = new IBRServiceContracts.CCell[columnsize];
                Plt.IComosDSpecification rowobject = lista[index] as Plt.IComosDSpecification;

                objetoCOMOS = (Plt.IComosBaseObject)rowobject.GetSpecOwner();

                row.Values[0] = new IBRServiceContracts.CCell()
                {
                    Value = objetoCOMOS.Name,
                    SystemUID = objetoCOMOS.SystemUID(),
                    SystemType = 10
                    
                };

                row.Values[1] = new IBRServiceContracts.CCell()
                {
                    Value = rowobject.Description,
                    SystemUID = objetoCOMOS.SystemUID(),
                    SystemType = 10
                };

                row.Values[2] = new IBRServiceContracts.CCell()
                {
                    Value = rowobject.DisplayValue(),
                    SystemUID = objetoCOMOS.SystemUID(),
                    SystemType = 10
                };

                
                result.Rows.Add(row);
            }

            rowCount = listaCOMOSDevices.Count;

            for (int index = 0; index <= rowCount - 1; ++index)
            {
                IBRServiceContracts.CRow row = new IBRServiceContracts.CRow();
                row.Values = new IBRServiceContracts.CCell[columnsize];
                Plt.IComosDDevice rowobject = listaCOMOSDevices[index] as Plt.IComosDDevice;

                row.Values[0] = new IBRServiceContracts.CCell()
                {
                    Value = rowobject.Name,
                    SystemUID = rowobject.SystemUID(),
                    UID = SystemUIDHandler.GetComosWebSystemUID(rowobject),
                    SystemType = 8
                };

                row.Values[1] = new IBRServiceContracts.CCell()
                {
                    Value = "Device",
                    SystemUID = rowobject.SystemUID(),
                    UID = SystemUIDHandler.GetComosWebSystemUID(rowobject),
                    SystemType = 8
                };

                row.Values[2] = new IBRServiceContracts.CCell()
                {
                    Value = rowobject.Description,
                    SystemUID = rowobject.SystemUID(),
                    UID = SystemUIDHandler.GetComosWebSystemUID(rowobject),
                    SystemType = 8
                };

                result.Rows.Add(row);
            }

            rowCount = listaCOMOSDocuments.Count;

            for (int index = 0; index <= rowCount - 1; ++index)
            {
                IBRServiceContracts.CRow row = new IBRServiceContracts.CRow();
                row.Values = new IBRServiceContracts.CCell[columnsize];
                Plt.IComosDDocument rowobject = listaCOMOSDocuments[index] as Plt.IComosDDocument;

                row.Values[0] = new IBRServiceContracts.CCell()
                {
                    Value = rowobject.Name,
                    SystemUID = rowobject.SystemUID(),
                    UID = SystemUIDHandler.GetComosWebSystemUID(rowobject),
                    SystemType = 29                   
                };

                row.Values[1] = new IBRServiceContracts.CCell()
                {
                    Value = "Document",
                    SystemUID = rowobject.SystemUID(),
                    UID = SystemUIDHandler.GetComosWebSystemUID(rowobject),
                    SystemType = 29
                };

                row.Values[2] = new IBRServiceContracts.CCell()
                {
                    Value = rowobject.Description,
                    SystemUID = rowobject.SystemUID(),
                    UID = SystemUIDHandler.GetComosWebSystemUID(rowobject),
                    SystemType = 29
                };

                result.Rows.Add(row);
            }

            result.RowCount = result.Rows.Count;

            return result;
        }
     
        private static IBRServiceContracts.CQueryResult GetSearchData(List<Plt.IComosDDevice> listaCOMOSDevices)
        {
            IBRServiceContracts.CQueryResult result = new IBRServiceContracts.CQueryResult();
            IBRServiceContracts.CColumn column = null;            

            result.Date = DateTime.Now;

            result.Columns = new List<IBRServiceContracts.CColumn>();
            result.Rows = new List<IBRServiceContracts.CRow>();

            column = new IBRServiceContracts.CColumn();
            column.Name = "Name";
            column.Description = "Name";
            column.ColumnIndex = 0;
            result.Columns.Add(column);

            column = new IBRServiceContracts.CColumn();
            column.Name = "Description";
            column.Description = "Description";
            column.ColumnIndex = 1;
            result.Columns.Add(column);

            int columnsize = 2;            

            int rowCount = listaCOMOSDevices.Count;

            Log.WriteLog("GetSearchData.rowCount: " + rowCount);

            for (int index = 0; index <= rowCount - 1; ++index)
            {
                IBRServiceContracts.CRow row = new IBRServiceContracts.CRow();
 

                row.Values = new IBRServiceContracts.CCell[columnsize];
                Plt.IComosDDevice rowobject = listaCOMOSDevices[index] as Plt.IComosDDevice;

                row.UID = SystemUIDHandler.GetComosWebSystemUID(rowobject);

                row.Values[0] = new IBRServiceContracts.CCell()
                {
                    Value = rowobject.Name,
                    SystemUID = rowobject.SystemUID(),
                    UID = SystemUIDHandler.GetComosWebSystemUID(rowobject),
                    SystemType = 8
                };

                row.Values[1] = new IBRServiceContracts.CCell()
                {
                    Value = rowobject.Description,
                    SystemUID = rowobject.SystemUID(),
                    UID = SystemUIDHandler.GetComosWebSystemUID(rowobject),
                    SystemType = 8
                };

                result.Rows.Add(row);
            }

            result.RowCount = result.Rows.Count;

            return result;
        }

        private static IBRServiceContracts.CQueryResult GetSearchData(List<Plt.IComosDDocument> listaCOMOSDocuments)
        {
            IBRServiceContracts.CQueryResult result = new IBRServiceContracts.CQueryResult();
            IBRServiceContracts.CColumn column = null;

            result.Date = DateTime.Now;

            result.Columns = new List<IBRServiceContracts.CColumn>();
            result.Rows = new List<IBRServiceContracts.CRow>();

            column = new IBRServiceContracts.CColumn();
            column.Name = "Name";
            column.Description = "Name";
            column.ColumnIndex = 0;
            result.Columns.Add(column);

            column = new IBRServiceContracts.CColumn();
            column.Name = "Description";
            column.Description = "Description";
            column.ColumnIndex = 1;
            result.Columns.Add(column);

            int columnsize = 2;

            int rowCount = listaCOMOSDocuments.Count;

            Log.WriteLog("GetSearchData.rowCount: " + rowCount);

            for (int index = 0; index <= rowCount - 1; ++index)
            {
                IBRServiceContracts.CRow row = new IBRServiceContracts.CRow();


                row.Values = new IBRServiceContracts.CCell[columnsize];
                Plt.IComosDDocument rowobject = listaCOMOSDocuments[index] as Plt.IComosDDocument;

                row.UID = SystemUIDHandler.GetComosWebSystemUID(rowobject);

                row.Values[0] = new IBRServiceContracts.CCell()
                {
                    Value = rowobject.Name,
                    SystemUID = rowobject.SystemUID(),
                    UID = SystemUIDHandler.GetComosWebSystemUID(rowobject),
                    SystemType = rowobject.SystemType()
                };

                row.Values[1] = new IBRServiceContracts.CCell()
                {
                    Value = rowobject.Description,
                    SystemUID = rowobject.SystemUID(),
                    UID = SystemUIDHandler.GetComosWebSystemUID(rowobject),
                    SystemType = rowobject.SystemType()
                };

                result.Rows.Add(row);
            }

            result.RowCount = result.Rows.Count;

            return result;
        }

        private static IBRServiceContracts.CQueryResult GetSearchData(List<Plt.IComosDDevice> listaCOMOSDevices, List<Plt.IComosDDocument> listaCOMOSDocuments)
        {
            IBRServiceContracts.CQueryResult result = new IBRServiceContracts.CQueryResult();
            IBRServiceContracts.CColumn column = null;
            
            result.Date = DateTime.Now;

            result.Columns = new List<IBRServiceContracts.CColumn>();

            column = new IBRServiceContracts.CColumn();
            column.Name = "Name";
            column.Description = "Name";
            column.ColumnIndex = 0;
            result.Columns.Add(column);

            column = new IBRServiceContracts.CColumn();
            column.Name = "Description";
            column.Description = "Description";
            column.ColumnIndex = 1;
            result.Columns.Add(column);            

            
            int columnsize = 2;
            result.Rows = new List<IBRServiceContracts.CRow>();

            int rowCount = listaCOMOSDevices.Count;
            for (int index = 0; index <= rowCount - 1; ++index)
            {
                IBRServiceContracts.CRow row = new IBRServiceContracts.CRow();
                row.Values = new IBRServiceContracts.CCell[columnsize];
                Plt.IComosDDevice rowobject = listaCOMOSDevices[index] as Plt.IComosDDevice;

                row.Values[0] = new IBRServiceContracts.CCell()
                {
                    Value = rowobject.Name,
                    SystemUID = rowobject.SystemUID(),
                    UID = SystemUIDHandler.GetComosWebSystemUID(rowobject),
                    SystemType = 8
                };

                row.Values[1] = new IBRServiceContracts.CCell()
                {
                    Value = rowobject.Description,
                    SystemUID = rowobject.SystemUID(),
                    UID = SystemUIDHandler.GetComosWebSystemUID(rowobject),
                    SystemType = 8
                };
                
                result.Rows.Add(row);
            }

            rowCount = listaCOMOSDocuments.Count;

            for (int index = 0; index <= rowCount - 1; ++index)
            {
                IBRServiceContracts.CRow row = new IBRServiceContracts.CRow();
                row.Values = new IBRServiceContracts.CCell[columnsize];
                Plt.IComosDDocument rowobject = listaCOMOSDocuments[index] as Plt.IComosDDocument;

                row.Values[0] = new IBRServiceContracts.CCell()
                {
                    Value = rowobject.Name,
                    SystemUID = rowobject.SystemUID(),
                    UID = SystemUIDHandler.GetComosWebSystemUID(rowobject),
                    SystemType = 29
                };

                row.Values[1] = new IBRServiceContracts.CCell()
                {
                    Value = rowobject.Description,
                    SystemUID = rowobject.SystemUID(),
                    UID = SystemUIDHandler.GetComosWebSystemUID(rowobject),
                    SystemType = 29
                };
              
                result.Rows.Add(row);
            }

            result.RowCount = result.Rows.Count;

            return result;
        }        

    }
}
