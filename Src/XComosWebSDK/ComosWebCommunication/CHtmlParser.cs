using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using ComosWebSDK.Data;
using ComosWebSDK.UI;
using ComosWebSDK.Extensions;

namespace ComosWebSDK
{
    public class CHtmlParser
    {
        public static CAttributes ParseAttributes(string html)
        {
            var options = Configuration.Default.WithCss();
            var parser = new HtmlParser(options);
            var doc = parser.Parse(html);

            CAttributes attributes = new CAttributes();

            foreach (var elm in doc.Body.Children)
            {
                ParseAttributes(attributes, elm, 1);
            }
            return attributes;
        }

        private static void ParseAttributes(CAttributes attributes, IElement element, int indent)
        {
            foreach (var elm in element.Children)
            {
                string nested_name = elm.GetAttribute("nested-name");
                if (!string.IsNullOrEmpty(nested_name))
                {
                    CAttribute attribute = attributes[nested_name];
                    if (attribute == null)
                    {
                        attribute = new CAttribute();
                        attribute.NestedName = nested_name;
                        attributes[nested_name] = attribute;
                    }
                    if (elm.TagName == "SPAN")
                    {
                        if (elm.TextContent[0] != '~')
                        {
                            attribute.Description = elm.TextContent;
                            attribute.ReadOnly = true;
                        }
                        else
                        {
                            // Is a readonly attribute
                            attribute.Description = elm.TextContent.Substring(1);
                        }
                    }
                    else if (elm.TagName == "INPUT")
                    {
                        switch (elm.ClassName)
                        {
                            case "SUICheckBox":
                                {
                                    string value = elm.GetAttribute("checked");
                                    if (string.IsNullOrEmpty(value) ||
                                        string.Compare(value, "checked") != 0)
                                        attribute.Value = false.ToString();
                                    else
                                        attribute.Value = true.ToString();
                                    attribute.IsBoolean = true;
                                }
                                break;
                            case "SUIText":
                                attribute.Value = elm.GetAttribute("value");
                                break;
                            default:
                                Debug.Assert(false);
                                break;
                        }
                    }
                    else if (elm.TagName == "SELECT")
                    {
                        ParseOptions(attribute, elm, indent + 1);
                        continue;
                    }
                    else if (elm.TagName == "TEXTAREA")
                    {
                        attribute.Value = elm.TextContent;
                        continue;
                    }


                    //else
                    //    DumpElement(elm, indent);

                    // Do the controls stuff
                    if (elm.ClassName != null)
                    {
                        UIType uitype = (UIType)Enum.Parse(typeof(UIType), elm.ClassName);
                    }
                }
                ParseAttributes(attributes, elm, indent + 1);
            }
        }

        static int[] GetLayout(AngleSharp.Dom.Css.ICssStyleDeclaration Style, int[] layout)
        {
            int[] result = { layout[0], layout[1], layout[2], layout[3] };
            if (!string.IsNullOrEmpty(Style.Height))
                result[3] = int.Parse(Style.Height.Substring(0, Style.Height.Length - 2));
            if (!string.IsNullOrEmpty(Style.Width))
                result[2] = int.Parse(Style.Width.Substring(0, Style.Width.Length - 2));
            if (!string.IsNullOrEmpty(Style.Left))
            {
                if (Style.Left != "0")
                    result[0] = int.Parse(Style.Left.Substring(0, Style.Left.Length - 2));
                //result[0] += int.Parse(Style.Left.Substring(0, Style.Left.Length - 2));
            }
            if (!string.IsNullOrEmpty(Style.Top))
            {
                if (Style.Top != "0")
                    result[1] = int.Parse(Style.Top.Substring(0, Style.Top.Length - 2));
                //result[1] += int.Parse(Style.Top.Substring(0, Style.Top.Length - 2));
            }
            return result;
        }

        private static void ParseOptions(CAttribute attribute, IElement element, int indent)
        {
            attribute.Options = new Dictionary<string, string>();
            foreach (var elm in element.Children)
            {
                if (elm.TagName == "OPTION")
                {
                    // Get the value of the selection (id)
                    string value = elm.GetAttribute("value");
                    // Get the description of the option
                    string key = elm.TextContent.Replace('\u00A0', ' '); /// Make sur no break spaces are replaced by spaces.
                    // This can be used to lookup the comos value to change in case user select a description (key)
                    attribute.Options[key] = value; // !! comos web could have duplicate optons, assuming here values will be same.

                    string selected = elm.GetAttribute("selected");
                    if (selected != null && string.Compare(selected, "selected") == 0)
                    {
                        // We assume here that when a comos spec has a unit. The selection of the unit
                        // Is the last xhtml eleemnt of the spec.
                        if (attribute.Value != null)
                        {
                            attribute.HasUnits = true;
                            attribute.Unit = key;
                        }
                        else
                            attribute.Value = key;
                    }
                }
                else
                    Debug.Assert(false);
            }
        }

        private static void DumpElement(IElement elm, int indent)
        {
            Debug.WriteLine(string.Format("{0," + indent.ToString() + "}", " "));
            Debug.WriteLine(string.Format("{0} : {1} - {2} {3}", elm.TagName, elm.GetAttribute("nested-name"), elm.Id, elm.ClassName));
            if (elm.Style != null && elm.Style.Position != null)
                Debug.WriteLine(string.Format(" {4} : {0},{1},{2},{3}",
                    elm.Style.Left, elm.Style.Top, elm.Style.Width, elm.Style.Height,
                    elm.Style.Position));

            Debug.WriteLine(string.Format("{0}", elm.Slot));
        }

        public static UIBase[] ParseAttributesForUI(string html)
        {
            var options = Configuration.Default.WithCss();
            var parser = new HtmlParser(options);
            AngleSharp.Dom.Html.IHtmlDocument doc = null;

            try
            {

                doc = parser.Parse(html);
            }
            catch (Exception)
            {
                return null;
            }

            List<UIBase> attributes = new List<UIBase>();
            List<UIBase> attributesNotIn = new List<UIBase>();

            int[] layout = { 0, 0, 0, 0 };

            // Get all the elements as a big flat list in list attributes.
            foreach (var item in doc.Body.Children)
            {
                ParseAttributesForUI(attributes, attributesNotIn, item, layout);
            }

            // try to find again
            foreach (var spec in attributesNotIn)
            {
                foreach (var item in attributes)
                {
                    var frame = item as UIFrame;
                    if (frame != null)
                    {
                        if (spec.IsIn(frame))
                        {
                            frame.AddChild(spec);
                        }
                    }
                }
            }

            // Group now all the root franes, 
            List<UIFrame> rootframes = new List<UIFrame>();
            foreach (var candidate in attributes)
            {
                UIFrame frame_candidate = candidate as UIFrame;
                if (frame_candidate == null)
                {
                    continue;
                }
                foreach (var attribute in attributes)
                {
                    UIFrame frame = attribute as UIFrame;
                    if (frame == null)
                        continue; // not a frame so can not be root.
                    if (attribute == frame_candidate)
                        continue; // Is same object no need to test
                    if (frame_candidate.IsIn(frame)) // Test if the frame_candidate is part of
                    {                                //   this frame. If it is it can not be a root frame.
                        frame_candidate = null;
                        break;
                    }
                }
                if (frame_candidate != null)
                {
                    // Is a root frame
                    rootframes.Add(frame_candidate);
                }
            }

            // Now we have all the root frames.
            // remove thses frames from the attributes list
            foreach (var frame in rootframes)
            {
                attributes.Remove(frame);
            }

            // Now group all the remaining elements in the rootframes.
            UIBase elm = attributes.FirstOrDefault();
            while (elm != null)
            {
                foreach (var attribute in rootframes)
                {
                    UIFrame frame = attribute as UIFrame; // Could also be an none frame element as root.
                    if (frame == null)
                        continue;
                    if (elm.IsIn(frame))
                    {
                        frame.AddChild(elm);
                        attributes.Remove(elm);
                        elm = null;
                        break;
                    }
                }
                if (elm != null) // is a none frame that is a root.
                {
                    rootframes.Add((UIFrame)elm);
                    attributes.Remove(elm);
                }
                elm = attributes.FirstOrDefault();
            }

            UIFrame noframe = null;
            List<UIFrame> toremove = new List<UIFrame>();

            foreach (var item in rootframes)
            {
                UIFrame frame = item as UIFrame;
                if (frame.Children == null || frame.Children.Count() == 0)
                    toremove.Add(item);
                //rootframes.Remove(item);

                if (frame.NestedName == "NO_FRAME")
                {
                    noframe = frame;
                }
            }

            foreach (var item in toremove)
            {
                rootframes.Remove(item);
            }

            //rootframes.Remove(noframe);
            //rootframes.Add(noframe);

            //rootframes = rootframes.OrderBy(x => x.y).ToList();

            return rootframes.OrderBy(x => x.y).ToArray();

            //return rootframes.ToArray();
        }

        private static void ParseAttributesForUI(List<UIBase> attributes, List<UIBase> attributesNotIn, IElement element, int[] layout)
        {
            int[] lay = GetLayout(element.Style, layout);

            if (element.TagName == "DIV")
            {
                // Test if the element could be a comos frame.
                if (element.Style.BorderRadius == "4px")
                {
                    // This is a frame
                    attributes.Add(new UIFrame()
                    {
                        x = lay[0],
                        y = lay[1],
                        width = lay[2],
                        height = lay[3]
                    });
                }
                // Test if element is label defining the frame.
                else if (element.Id != null && element.Id.StartsWith("Border"))
                {
                    // This defines a text of a frame
                    // This text is always after the border definition.
                    // => Get the matching UIFrame (is last in list)
                    UIFrame frame = (UIFrame)attributes.Last();
                    frame.Text = element.TextContent;
                }
                // Else the children elements of this div define a specific tab control attribute
                else if (element.Children.Count() == 1)
                {
                    ParseAttributeForUI(attributes, attributesNotIn, element.Children[0], lay);
                }
                else if (element.ClassName == "SUIQuery")
                {
                    // query spec
                    ParseQueryForUI(attributes, element, lay);
                }
                else
                    Debug.Assert(false); // All elements should be divs
            }
            else
                Debug.Assert(false); // All elements should be divs
        }

        private static void ParseQueryForUI(List<UIBase> attributes, IElement element, int[] layout)
        {

            int[] elmLayout = GetLayout(element.Style, layout);
            var tmp = new UIQuery()
            {
                x = elmLayout[0],
                y = elmLayout[1],
                width = elmLayout[2],
                height = elmLayout[3]
            };
            tmp.Text = element.TextContent;
            tmp.WidthLabel = element.GetWidth();
            tmp.NestedName = element.GetAttribute("nested-name");
            tmp.Owner = element.GetAttribute("owner");
            tmp.QueryUID = element.GetAttribute("query");

            if (tmp != null)
            {
                // find the frame it belongs to
                foreach (var item in attributes)
                {
                    var frame = item as UIFrame;
                    if (frame != null)
                    {
                        if (tmp.IsIn(frame))
                        {
                            frame.AddChild(tmp);
                            return;
                        }
                    }
                }

                UIFrame lastframe = null;

                foreach (var item in attributes)
                {
                    if (item.NestedName == "NO_FRAME")
                    {
                        lastframe = item as UIFrame;
                        break;
                    }
                }

                if (lastframe == null)
                {
                    lastframe = new UIFrame() { NestedName = "NO_FRAME", TabIndex = 99 };
                    attributes.Add(lastframe);
                }

                lastframe.AddChild(tmp);

            }

        }

        private static void ParseAttributeForUI(List<UIBase> attributes, List<UIBase> attributesNotIn, IElement element, int[] layout)
        {
            UIBase elm = null;
            int nbChildren = element.Children.Count();

            if ((element.Id != null) && (element.Id.ToUpper().Contains("LABEL")))
            {
                elm = ParseAttributeForLabel(element, layout);
            }


            if (nbChildren == 2)
            {
                // Could be an edit field, option list or checkbox
                if (element.Children[0].ClassName == "SUICheckBox")
                {
                    // Is a left check box
                    elm = ParseAttributeForCheckBox(element, layout, CheckBoxOrientation.LEFT);
                }
                else if (element.Children[1].ClassName == "SUICheckBox")
                {
                    // Is a right check box
                    elm = ParseAttributeForCheckBox(element, layout, CheckBoxOrientation.RIGHT);
                }
                else if (element.Children[1].ClassName == "SUIText")
                {
                    // Is an edit field
                    elm = ParseAttributeForTextField(element, layout);
                }
                else if (element.Children[0].ClassName == "SUITextArea")
                {
                    // Is an memo field
                    elm = ParseAttributeForMemoField(element, layout, 0);
                }
                else if (element.Children[1].ClassName == "SUITextArea")
                {
                    // Is an memo field
                    elm = ParseAttributeForMemoField(element, layout, 1);
                }
                else if (element.Children[1].TagName == "SELECT")
                {
                    // Is an options field
                    elm = ParseAttributeForOptionsField(element, layout);
                }
            }
            else if (nbChildren == 3)
            {
                if (element.Children[1].ClassName == "SUIText" && element.Children[2].TagName == "SELECT")
                {
                    // Is a text field with Unit selection.
                    elm = ParseAttributeForTextFieldWidthUnit(element, layout);
                }
                else if (element.Children[1].ClassName == "SUITextArea")
                {
                    // Is an memo field with button
                    elm = ParseAttributeForMemoField(element, layout, 1);
                }
            }
            else if (nbChildren == 4)
            {
                if(element.Children[2].ClassName == "SUITimeButton")
                {
                    // Comos Date
                    elm = ParseAttributeForDateField(element, layout);
                }
                else
                {
                    elm = ParseAttributeForTextField(element, layout);
                }              
            }
            else if (nbChildren == 5)
            {
                // Comos Link
                Debug.WriteLine("Unsupported at this stage.");
            }
            else
            {
                // Comos Button                
                Debug.WriteLine("Unsupported at this stage.");
            }

            bool found = false;

            if (elm != null)
            {
                // find the frame it belongs to
                foreach (var item in attributes)
                {
                    var frame = item as UIFrame;
                    if (frame != null)
                    {
                        if (elm.IsIn(frame))
                        {
                            frame.AddChild(elm);
                            found = true;
                        }
                    }
                }
                if (!found)
                    attributesNotIn.Add(elm);
            }
        }

        private static UIBase ParseAttributeForMemoField(IElement element, int[] layout, int textarea)
        {
            int[] elmLayout = GetLayout(element.Children[0].Style, layout);
            var tmp = new UIMemo()
            {
                x = elmLayout[0],
                y = elmLayout[1],
                width = elmLayout[2],
                height = elmLayout[3],
            };

            try
            {
                if (textarea == 1)
                {
                    tmp.ShowLabel = true;
                    if (element.Children[0].TextContent[0] != '~')
                    {   // Is readonly attribute
                        tmp.Text = element.Children[0].TextContent;
                        tmp.ReadOnly = true;
                    }
                    else
                    {
                        tmp.Text = element.Children[0].TextContent.Substring(1);
                    }
                    tmp.WidthLabel = element.Children[0].GetWidth();
                }

                tmp.NestedName = element.Children[textarea].GetAttribute("nested-name");
                tmp.ValueType = element.Children[textarea].GetAttribute("type");
                tmp.Value = element.Children[textarea].TextContent;
            }
            catch (Exception)
            {


            }



            return tmp;
        }

        private static UIBase ParseAttributeQuery(IElement element, int[] layout)
        {
            return new UIQuery();
        }

        private static UIBase ParseAttributeForCheckBox(IElement element, int[] layout, CheckBoxOrientation checkBoxOrientation)
        {
            // Test for check box
            int[] elmLayout = GetLayout(element.Children[0].Style, layout);
            // This is a Checkbox
            var checkbox = new UICheckBox()
            {
                x = elmLayout[0],
                y = elmLayout[1],
                width = elmLayout[2],
                height = elmLayout[3]
            };
            checkbox.NestedName = element.Children[0].GetAttribute("nested-name");
            string value;
            if (checkBoxOrientation == CheckBoxOrientation.RIGHT)
            {
                value = element.Children[1].GetAttribute("checked");
            }
            else
            {
                value = element.Children[0].GetAttribute("checked");
            }

            if (string.IsNullOrEmpty(value) ||
                string.Compare(value, "checked") != 0)
                checkbox.Value = false;
            else
                checkbox.Value = true;

            if (element.Children[(int)checkBoxOrientation].TextContent[0] != '~')
            {   // Is readonly attribute
                checkbox.Text = element.Children[(int)checkBoxOrientation].TextContent;
                checkbox.ReadOnly = true;
            }
            else
            {
                checkbox.Text = element.Children[(int)checkBoxOrientation].TextContent.Substring(1);

            }
            checkbox.WidthLabel = element.Children[(int)checkBoxOrientation].GetWidth();
            elmLayout = GetLayout(element.Children[1].Style, layout);
            if (checkbox.x < elmLayout[0])
                checkbox.x = elmLayout[0];
            if (checkbox.y < elmLayout[1])
                checkbox.y = elmLayout[1];
            if (checkbox.width < elmLayout[2])
                checkbox.width = elmLayout[2];
            if (checkbox.height < elmLayout[3])
                checkbox.height = elmLayout[3];

            return checkbox;
        }

        private static UIBase ParseAttributeForDateField(IElement element, int[] layout)
        {
            int[] elmLayout = GetLayout(element.Children[0].Style, layout);
            
            if(element.Children[1].Id.Contains("Time"))
            {
                var tmp = new UIDateTime()
                {
                    x = elmLayout[0],
                    y = elmLayout[1],
                    width = elmLayout[2],
                    height = elmLayout[3]
                };
                if (element.Children[0].TextContent[0] != '~')
                {   // Is readonly attribute
                    tmp.Text = element.Children[0].TextContent;
                    tmp.ReadOnly = true;
                }
                else
                {

                    tmp.Text = element.Children[0].TextContent.Substring(1);
                }
                tmp.WidthLabel = element.Children[0].GetWidth();
                tmp.NestedName = element.Children[1].GetAttribute("nested-name");
                tmp.Value = element.Children[1].GetAttribute("value");
                tmp.ValueType = element.Children[1].GetAttribute("type");

                return tmp;
            }
            else
            {
                var tmp = new UIDate()
                {
                    x = elmLayout[0],
                    y = elmLayout[1],
                    width = elmLayout[2],
                    height = elmLayout[3]
                };
                if (element.Children[0].TextContent[0] != '~')
                {   // Is readonly attribute
                    tmp.Text = element.Children[0].TextContent;
                    tmp.ReadOnly = true;
                }
                else
                {

                    tmp.Text = element.Children[0].TextContent.Substring(1);
                }
                tmp.WidthLabel = element.Children[0].GetWidth();
                tmp.NestedName = element.Children[1].GetAttribute("nested-name");
                tmp.Value = element.Children[1].GetAttribute("value");
                tmp.ValueType = element.Children[1].GetAttribute("type");

                return tmp;
            }
        }

        private static UIBase ParseAttributeForLabel(IElement element, int[] layout)
        {
            int[] elmLayout = GetLayout(element.Style, layout);
            var tmp = new UILabel()
            {
                x = elmLayout[0],
                y = elmLayout[1],
                width = elmLayout[2],
                height = elmLayout[3]
            };

            tmp.Text = element.TextContent;
            tmp.WidthLabel = element.GetWidth();
            tmp.NestedName = element.GetAttribute("nested-name");

            return tmp;
        }
        
        private static UIBase ParseAttributeForTextField(IElement element, int[] layout)
        {
            int[] elmLayout = GetLayout(element.Children[0].Style, layout);
            var tmp = new UIEdit()
            {
                x = elmLayout[0],
                y = elmLayout[1],
                width = elmLayout[2],
                height = elmLayout[3]
            };
            if (element.Children[0].TextContent[0] != '~')
            {   // Is readonly attribute
                tmp.Text = element.Children[0].TextContent;
                tmp.ReadOnly = true;
            }
            else
            {

                tmp.Text = element.Children[0].TextContent.Substring(1);
            }
            tmp.WidthLabel = element.Children[0].GetWidth();
            tmp.NestedName = element.Children[1].GetAttribute("nested-name");
            tmp.Value = element.Children[1].GetAttribute("value");
            tmp.ValueType = element.Children[1].GetAttribute("type");

            return tmp;
        }

        private static UIBase ParseAttributeForTextFieldWidthUnit(IElement element, int[] layout)
        {
            int[] elmLayout = GetLayout(element.Children[0].Style, layout);
            var tmp = new UIEdit()
            {
                x = elmLayout[0],
                y = elmLayout[1],
                width = elmLayout[2],
                height = elmLayout[3],
            };
            if (element.Children[0].TextContent[0] != '~')
            {   // Is readonly attribute
                tmp.Text = element.Children[0].TextContent;

                tmp.ReadOnly = true;
            }
            else
            {
                tmp.Text = element.Children[0].TextContent.Substring(1);
            }
            tmp.WidthLabel = element.Children[0].GetWidth();
            tmp.NestedName = element.Children[1].GetAttribute("nested-name");
            tmp.Value = element.Children[1].GetAttribute("value");
            tmp.ValueType = element.Children[1].GetAttribute("type");
            foreach (var option in element.Children[2].Children)
            {
                string value = option.GetAttribute("selected");
                if (value == "selected")
                    tmp.Unit = option.TextContent;
            }
            return tmp;
        }

        private static UIBase ParseAttributeForOptionsField(IElement element, int[] layout)
        {
            int[] elmLayout = GetLayout(element.Children[0].Style, layout);
            var tmp = new UIOptions()
            {
                x = elmLayout[0],
                y = elmLayout[1],
                width = elmLayout[2],
                height = elmLayout[3]
            };


            try
            {
                if (element.Children[0].TextContent[0] != '~')
                {   // Is readonly attribute
                    tmp.Text = element.Children[0].TextContent;
                    tmp.ReadOnly = true;
                }
                else
                {
                    tmp.Text = element.Children[0].TextContent.Substring(1);

                }
                tmp.WidthLabel = element.Children[0].GetWidth();
                tmp.NestedName = element.Children[1].GetAttribute("nested-name");
                tmp.Value = element.Children[1].GetAttribute("value");

                tmp.Options = new Dictionary<string, string>();
                tmp.Options.Add("", "");

                foreach (var elm in element.Children[1].Children)
                {
                    if (elm.TagName == "OPTION")
                    {
                        // Get the value of the selection (id)
                        string value = elm.GetAttribute("value");
                        // Get the description of the option
                        string key = elm.TextContent.Replace('\u00A0', ' '); /// Make sur no break spaces are replaced by spaces.
                        // This can be used to lookup the comos value to change in case user select a description (key)
                        tmp.Options[key] = value; // !! comos web could have duplicate optons, assuming here values will be same.

                        string selected = elm.GetAttribute("selected");
                        if (selected != null && string.Compare(selected, "selected") == 0)
                        {
                            // We assume here that when a comos spec has a unit. The selection of the unit
                            // Is the last xhtml eleemnt of the spec.
                            tmp.Value = key;
                        }

                    }
                    else
                        Debug.Assert(false);
                }
                if (tmp.Value == null)
                    tmp.Value = " ";
            }
            catch (Exception)
            {

                //throw;
            }



            return tmp;
        }
    }

    public enum CheckBoxOrientation
    {
        RIGHT,
        LEFT
    }
}
