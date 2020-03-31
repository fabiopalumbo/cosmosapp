using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComosWebSDK.Data;

namespace ComosWebSDK.Extensions
{
    public static class ComosWebExtensions
    {
        public static async Task<CObject> GetObjectBySystemFullName(this IComosWeb comosweb, CWorkingLayer layer, string systemfullname)
        {
            CObject current = null;

            try
            {
                string[] names = systemfullname.Split('|');
                string target = layer.ProjectUID;
                bool found = false;

                var nodes = await comosweb.GetNavigatorNodes_TreeNodes(layer, "1033", target, "U");
                if (nodes == null)
                    return null;
                var currentnode = nodes.FirstOrDefault();
                if (currentnode != null)
                {
                    List<CObject> items = currentnode.Items;
                    for (int i = 0; i < names.Length; i++)
                    {
                        foreach (var node in items)
                        {
                            if (string.Compare(node.Name, names[i]) == 0)
                            {
                                found = true;
                                current = node;
                                target = current.UID;
                                items = await comosweb.GetNavigatorNodes_Children(layer.Database, layer.ProjectUID, layer.UID, "1033", target, "U");
                                break;
                            }
                        }
                        if (!found)
                            return null;
                        found = false;
                    }
                }
            }
            catch (Exception e)
            {

            }

            return current;
        }

        public static int GetPositionLeft(this AngleSharp.Dom.IElement element)
        {
            try
            {
                if (!string.IsNullOrEmpty(element.Style.Left))
                    return int.Parse(element.Style.Left.Substring(0, element.Style.Left.Length - 2));
            }
            catch (Exception e)
            {

            }

            return 0;
        }

        public static int GetPositionTop(this AngleSharp.Dom.IElement element)
        {
            try
            {
                if (!string.IsNullOrEmpty(element.Style.Top))
                    return int.Parse(element.Style.Top.Substring(0, element.Style.Top.Length - 2));
            }
            catch (Exception e)
            {

            }

            return 0;
        }

        public static int GetWidth(this AngleSharp.Dom.IElement element)
        {
            try
            {
                if (!string.IsNullOrEmpty(element.Style.Width))
                    return int.Parse(element.Style.Width.Substring(0, element.Style.Width.Length - 2));
            }
            catch (Exception e)
            {

            }

            return 0;
        }

        public static int GetHeight(this AngleSharp.Dom.IElement element)
        {
            try
            {
                if (!string.IsNullOrEmpty(element.Style.Height))
                    return int.Parse(element.Style.Height.Substring(0, element.Style.Height.Length - 2));
            }
            catch (Exception e)
            {

            }

            return 0;
        }
    }
}
