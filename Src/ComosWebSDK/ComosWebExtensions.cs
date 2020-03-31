using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		/*
        public static async Task<CWorkingLayer> GetWorkingLayerByUrl(this ComosWeb comosweb, string url)
        {
            // db1/projects/U:2:A3BQHFA8AR:/wl/U:42:A3W00RYY9Z:/details
            string[] parts = url.Split('/');
            if (parts.Length != 6)
            {
                throw new Exception("Urls is not of correct path. Example of correct path 'db1/projects/U:2:A3BQHFA8AR:/wl/U:42:A3W00RYY9Z:/details'");
            }
            if (parts[1] != "projects" && parts[3] != "wl")
            {
                throw new Exception("Urls is not of correct path. Example of correct path 'db1/projects/U:2:A3BQHFA8AR:/wl/U:42:A3W00RYY9Z:/details'");
            }

            string db = parts[0];
            string projectuid = parts[2];
            string layer = parts[4];

            var value  = await comosweb.GetObject(db, projectuid, layer, layer, "1033");
            var workingoverlay = new CWorkingLayer()
            {
                ClassType = null,
                Database = db,
                Description = value.Description,
                IsClientPicture = value.IsClientPicture,
                Name = value.Name,
                OverlayUID = layer,
                ProjectUID = projectuid,
                Picture = value.Picture,
                SystemFullName = "",
                UID = layer,
            };
            return workingoverlay;
        }
		*/

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
