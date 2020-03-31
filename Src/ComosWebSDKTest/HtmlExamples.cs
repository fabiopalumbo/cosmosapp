using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComosWebSDKTest
{
    public static class HtmlExamples
    {
        public static string HtmlExample1 = @"<div unselectable=""on"" style=""border: #a9a9a9 1px solid; border-radius: 4px; -moz-border-radius: 4px; -webkit-border-radius: 4px; z-index:1; position:absolute;left: 32px; top: 264px; width: 576px; height: 280px; ""></div>
<div unselectable=""on"" id=""Border1"" style=""cursor:default; border: 0 0px none; background: white; z-index:2; white-space:nowrap;position:absolute; padding-left:5px; padding-right:5px; left: 42px; top: 256px; font-weight:bold; color:#000000; font-size:11px; font-family:MS Sans Serif; "">Configuration</div>
<div unselectable=""on"" style=""border: #a9a9a9 1px solid; border-radius: 4px; -moz-border-radius: 4px; -webkit-border-radius: 4px; z-index:1; position:absolute;left: 32px; top: 40px; width: 576px; height: 184px; ""></div>
<div unselectable=""on"" id=""Border2"" style=""cursor:default; border: 0 0px none; background: white; z-index:2; white-space:nowrap;position:absolute; padding-left:5px; padding-right:5px; left: 42px; top: 32px; font-weight:bold; color:#000000; font-size:11px; font-family:MS Sans Serif; "">Event data</div>
<div unselectable=""on"" style=""border: #a9a9a9 1px solid; border-radius: 4px; -moz-border-radius: 4px; -webkit-border-radius: 4px; z-index:1; position:absolute;left: 32px; top: 584px; width: 576px; height: 80px; ""></div>
<div unselectable=""on"" id=""Border3"" style=""cursor:default; border: 0 0px none; background: white; z-index:2; white-space:nowrap;position:absolute; padding-left:5px; padding-right:5px; left: 42px; top: 576px; color:#000000; font-size:11px; font-family:MS Sans Serif; "">Shift</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:288px;width:545px;height:25px'>
    <nobr>
        <span unselectable=""on"" id=""Link4_7"" nested-name=""Y00T00250.Y00A00703"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">E-mail configuration</span>
        <input type=""text"" value = ""A10"" id=""Link4_0"" nested-name=""Y00T00250.Y00A00703"" class=""SUIText"" readonly=""readonly"" maxlength=""2000"" data-ng-keydown=""OnKeyDown($event)"" style=""position:absolute;left:144px;width:314px;height:18px;font-style:italic;background-color:#e4e2e0;color:#000000;""/>
        <button class=""SUIOpenButton"" id=""Link4_8"" nested-name=""Y00T00250.Y00A00703"" style=""position:absolute;left: 466px;width:24px;height:24px"" disabled></button>
        <button class=""SUIDeleteButton"" id=""Link4_9"" nested-name=""Y00T00250.Y00A00703"" style=""position:absolute;left: 492px;width:24px;height:24px"" disabled></button>
        <button class=""SUIPropNavButton"" id=""Link4_10"" nested-name=""Y00T00250.Y00A00703"" style=""position:absolute;left: 518px;width:24px;height:24px"" data-ng-click=""OnClick($event)""></button>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:64px;width:545px;height:23px'>
    <nobr>
        <span unselectable=""on"" id=""Date5_7"" nested-name=""Y00T00250.Y00A00710"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:22px;overflow:hidden;color:#000000;font-size: 13px;font-family:Nina; "">Date</span>
        <input type=""text"" value = ""2017-06-08"" id=""Date5_0"" nested-name=""Y00T00250.Y00A00710"" class=""SUIText"" readonly=""readonly"" maxlength=""2000"" data-ng-keydown=""OnKeyDown($event)"" style=""position:absolute;left:144px;width:344px;height:16px;background-color:#e4e2e0;color:#000000;""/>
        <button class=""SUITimeButton"" id=""Date5_11"" nested-name=""Y00T00250.Y00A00710"" style=""position:absolute;left: 496px;width:22px;height:22px"" disabled></button>
        <button class=""SUIOpenButton"" id=""Date5_8"" nested-name=""Y00T00250.Y00A00710"" style=""position:absolute;left: 520px;width:22px;height:22px"" disabled></button>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:480px;width:545px;height:25px'>
    <nobr>
        <input type='checkbox' id=""Check6_12"" nested-name=""Y00T00250.Y00A00726"" class=""SUICheckBox"" style = ""position:absolute;left:0px;top:0px;vertical-align:middle;"" disabled=""disabled""/>
        <span unselectable=""on"" id=""Check6_7"" nested-name=""Y00T00250.Y00A00726"" style=""vertical-align:middle;position:absolute;left:18px;top:2px;width:528px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Canceled</span>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:448px;width:545px;height:25px'>
    <nobr>
        <input type='checkbox' id=""Check7_12"" nested-name=""Y00T00250.Y00A00726AA02"" class=""SUICheckBox"" style = ""position:absolute;left:0px;top:0px;vertical-align:middle;"" disabled=""disabled""/>
        <span unselectable=""on"" id=""Check7_7"" nested-name=""Y00T00250.Y00A00726AA02"" style=""vertical-align:middle;position:absolute;left:18px;top:2px;width:528px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Can be canceled</span>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:96px;width:545px;height:25px'>
    <nobr>
        <span unselectable=""on"" id=""Edit8_6"" nested-name=""Y00T00250.Y00A00862"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Time of day</span>
        <input type=""text"" value = ""09:08:33"" id=""Edit8_0"" nested-name=""Y00T00250.Y00A00862"" class=""SUIText"" readonly=""readonly"" maxlength=""2000"" data-ng-keydown=""OnKeyDown($event)"" style=""position:absolute;left:144px;width:392px;height:18px;background-color:#e4e2e0;color:#000000;""/>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:160px;width:545px;height:23px'>
    <nobr>
        <span unselectable=""on"" id=""Edit9_6"" nested-name=""Y00T00250.Y00A01088"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:22px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Status</span>
        <select id=""Edit9_0"" nested-name=""Y00T00250.Y00A01088"" style=""position:absolute;left:144px;width:400px;height:22px;color:#000000;background-color:#e4e2e0;"" data-comos-change=""OnSelect(event,2)"">
            <option value=""""></option>
            <option value=""N/A"">N/A</option>
            <option value=""New"">New</option>
            <option value=""For information"" selected=""selected"">For&nbsp;information</option>
            <option value=""Confirmed"">Confirmed</option>
            <option value=""Suspended"">Suspended</option>
            <option value=""Completed"">Completed</option>
            <option value=""Delegated"">Delegated</option>
        </select>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:616px;width:545px;height:25px'>
    <nobr>
        <span unselectable=""on"" id=""Link10_7"" nested-name=""Y00T00250.Y00A02764"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Link to shift object</span>
        <input type=""text"" value = """" id=""Link10_0"" nested-name=""Y00T00250.Y00A02764"" class=""SUIText"" readonly=""readonly"" maxlength=""2000"" data-ng-keydown=""OnKeyDown($event)"" style=""position:absolute;left:144px;width:314px;height:18px;background-color:#e4e2e0;color:#000000;""/>
        <button class=""SUIOpenButton"" id=""Link10_8"" nested-name=""Y00T00250.Y00A02764"" style=""position:absolute;left: 466px;width:24px;height:24px"" disabled></button>
        <button class=""SUIDeleteButton"" id=""Link10_9"" nested-name=""Y00T00250.Y00A02764"" style=""position:absolute;left: 492px;width:24px;height:24px"" disabled></button>
        <button class=""SUIPropNavButton"" id=""Link10_10"" nested-name=""Y00T00250.Y00A02764"" style=""position:absolute;left: 518px;width:24px;height:24px"" disabled></button>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:320px;width:545px;height:25px'>
    <nobr>
        <input type='checkbox' id=""Check11_12"" nested-name=""Y00T00250.Y00A02847"" class=""SUICheckBox"" style = ""position:absolute;left:0px;top:0px;vertical-align:middle;"" checked=""checked"" disabled=""disabled""/>
        <span unselectable=""on"" id=""Check11_7"" nested-name=""Y00T00250.Y00A02847"" style=""vertical-align:middle;position:absolute;left:18px;top:2px;width:528px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Activate automatic notification</span>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:352px;width:545px;height:25px'>
    <nobr>
        <input type='checkbox' id=""Check12_12"" nested-name=""Y00T00250.Y00A02858"" class=""SUICheckBox"" style = ""position:absolute;left:0px;top:0px;vertical-align:middle;"" checked=""checked"" disabled=""disabled""/>
        <span unselectable=""on"" id=""Check12_7"" nested-name=""Y00T00250.Y00A02858"" style=""vertical-align:middle;position:absolute;left:18px;top:2px;width:528px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Request confirmation upon status change</span>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:416px;width:545px;height:25px'>
    <nobr>
        <input type='checkbox' id=""Check13_12"" nested-name=""Y00T00250.Y00A02874"" class=""SUICheckBox"" style = ""position:absolute;left:0px;top:0px;vertical-align:middle;"" checked=""checked"" disabled=""disabled""/>
        <span unselectable=""on"" id=""Check13_7"" nested-name=""Y00T00250.Y00A02874"" style=""vertical-align:middle;position:absolute;left:18px;top:2px;width:528px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Apply set status from base object</span>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:128px;width:545px;height:25px'>
    <nobr>
        <span unselectable=""on"" id=""Edit14_6"" nested-name=""Y00T00250.Y00A02883"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Engineer</span>
        <input type=""text"" value = ""LIANA.SANTOS"" id=""Edit14_0"" nested-name=""Y00T00250.Y00A02883"" class=""SUIText"" readonly=""readonly"" maxlength=""2000"" data-ng-keydown=""OnKeyDown($event)"" style=""position:absolute;left:144px;width:392px;height:18px;background-color:#e4e2e0;color:#000000;""/>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:384px;width:545px;height:25px'>
    <nobr>
        <input type='checkbox' id=""Check15_12"" nested-name=""Y00T00250.Y00A03047"" class=""SUICheckBox"" style = ""position:absolute;left:0px;top:0px;vertical-align:middle;"" disabled=""disabled""/>
        <span unselectable=""on"" id=""Check15_7"" nested-name=""Y00T00250.Y00A03047"" style=""vertical-align:middle;position:absolute;left:18px;top:2px;width:528px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Activate mandatory workflow</span>
    </nobr>
</div>";

        public static string HtmlExample2 = @"<div unselectable=""on"" style=""border: #a9a9a9 1px solid; border-radius: 4px; -moz-border-radius: 4px; -webkit-border-radius: 4px; z-index:1; position:absolute;left: 32px; top: 40px; width: 576px; height: 696px; ""></div>
<div unselectable=""on"" id=""Border1"" style=""cursor:default; border: 0 0px none; background: white; z-index:2; white-space:nowrap;position:absolute; padding-left:5px; padding-right:5px; left: 42px; top: 32px; font-weight:bold; color:#000000; font-size:11px; font-family:MS Sans Serif; "">Resource planning</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:352px;width:545px;height:25px'>
    <nobr>
        <input type='checkbox' id=""Check2_12"" nested-name=""Y00T00235.Y00A00726"" class=""SUICheckBox"" style = ""position:absolute;left:0px;top:0px;vertical-align:middle;"" disabled=""disabled""/>
        <span unselectable=""on"" id=""Check2_7"" nested-name=""Y00T00235.Y00A00726"" style=""vertical-align:middle;position:absolute;left:18px;top:2px;width:528px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Canceled</span>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:544px;width:545px;height:161px'>
    <nobr>
        <table id=""List3"" style=""border-collapse: separate; border-spacing: 1px; background-color:black; width:476px; height:16px;"">
            <tr>
                <td style=""width:66px; height:16px; background-color:#e4e2e0;"">Predecessor</td>
                <td style=""width:77px; height:16px; background-color:#e4e2e0; text-align:center;"">System UID</td>
                <td style=""width:133px; height:16px; background-color:#e4e2e0; text-align:center;"">Type</td>
                <td style=""width:133px; height:16px; background-color:#e4e2e0; text-align:center;"">Offset</td>
                <td style=""width:133px; height:16px; background-color:#e4e2e0; text-align:center;"">Mode</td>
            </tr>
            <tr>
                <td style=""width:66px; height:16px; background-color:#e4e2e0;"">1</td>
                <td style=""width:77px; height:16px; background-color:white;""></td>
                <td style=""width:133px; height:16px; background-color:white;""></td>
                <td style=""width:133px; height:16px; background-color:white;""></td>
                <td style=""width:133px; height:16px; background-color:white;""></td>
            </tr>
        </table>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:128px;width:545px;height:25px'>
    <nobr>
        <span unselectable=""on"" id=""Edit4_6"" nested-name=""Y00T00235.Y00A00749"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:176px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Display text</span>
        <input type=""text"" value = """" id=""Edit4_0"" nested-name=""Y00T00235.Y00A00749"" class=""SUIText"" readonly=""readonly"" maxlength=""2000"" data-ng-keydown=""OnKeyDown($event)"" style=""position:absolute;left:176px;width:360px;height:18px;background-color:#e4e2e0;color:#000000;""/>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:448px;width:545px;height:25px'>
    <nobr>
        <input type='checkbox' id=""Check5_12"" nested-name=""Y00T00235.Y00A00807"" class=""SUICheckBox"" style = ""position:absolute;left:0px;top:0px;vertical-align:middle;"" disabled=""disabled""/>
        <span unselectable=""on"" id=""Check5_7"" nested-name=""Y00T00235.Y00A00807"" style=""vertical-align:middle;position:absolute;left:18px;top:2px;width:528px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Assignments (persons) locked</span>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:480px;width:545px;height:25px'>
    <nobr>
        <input type='checkbox' id=""Check6_12"" nested-name=""Y00T00235.Y00A02792"" class=""SUICheckBox"" style = ""position:absolute;left:0px;top:0px;vertical-align:middle;"" disabled=""disabled""/>
        <span unselectable=""on"" id=""Check6_7"" nested-name=""Y00T00235.Y00A02792"" style=""vertical-align:middle;position:absolute;left:18px;top:2px;width:528px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Dependencies locked</span>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:320px;width:545px;height:25px'>
    <nobr>
        <span unselectable=""on"" id=""Edit7_6"" nested-name=""Y00T00235.Y00A02794"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:176px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Turnaround phase</span>
        <select id=""Edit7_0"" nested-name=""Y00T00235.Y00A02794"" style=""position:absolute;left:176px;width:368px;height:24px;color:#000000;background-color:#e4e2e0;"" data-comos-change=""OnSelect(event,0)"">
            <option value=""""></option>
            <option value=""Pre phase"">Pre&nbsp;phase</option>
            <option value=""Turnaround phase"">Turnaround&nbsp;phase</option>
            <option value=""Post phase"">Post&nbsp;phase</option>
        </select>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:416px;width:545px;height:25px'>
    <nobr>
        <input type='checkbox' id=""Check8_12"" nested-name=""Y00T00235.Y00A02843"" class=""SUICheckBox"" style = ""position:absolute;left:0px;top:0px;vertical-align:middle;"" disabled=""disabled""/>
        <span unselectable=""on"" id=""Check8_7"" nested-name=""Y00T00235.Y00A02843"" style=""vertical-align:middle;position:absolute;left:18px;top:2px;width:528px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Load (units/time) locked</span>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:192px;width:545px;height:23px'>
    <nobr>
        <span unselectable=""on"" id=""Edit9_6"" nested-name=""Y00T00235.Y00A02875"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:176px;height:22px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Units/time</span>
        <input type=""text"" value = """" id=""Edit9_0"" nested-name=""Y00T00235.Y00A02875"" class=""SUIText"" readonly=""readonly"" maxlength=""2000"" data-ng-keydown=""OnKeyDown($event)"" style=""position:absolute;left:176px;width:264px;height:16px;background-color:#e4e2e0;color:#000000;""/>
        <select id=""Edit9_100"" nested-name=""Y00T00235.Y00A02875"" style=""position:absolute;left:448px;width:96px;height:22px;color:#000000;background-color:#e4e2e0;font-style:italic; "" data-comos-change=""OnSelect(event,2)"">
            <option value=""F25.AA100"">ppm</option>
            <option value=""F25.AA110"">1/1</option>
            <option value=""F25.AA120"" selected=""selected"">%</option>
            <option value=""F25.AA130"">‰</option>
        </select>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:256px;width:545px;height:25px'>
    <nobr>
        <span unselectable=""on"" id=""Edit10_6"" nested-name=""Y00T00235.Y00A02876"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:176px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Unit of factor</span>
        <input type=""text"" value = """" id=""Edit10_0"" nested-name=""Y00T00235.Y00A02876"" class=""SUIText"" readonly=""readonly"" maxlength=""2000"" data-ng-keydown=""OnKeyDown($event)"" style=""position:absolute;left:176px;width:360px;height:18px;background-color:#e4e2e0;color:#000000;""/>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:224px;width:545px;height:23px'>
    <nobr>
        <span unselectable=""on"" id=""Edit11_6"" nested-name=""Y00T00235.Y00A02885"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:176px;height:22px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Progress</span>
        <input type=""text"" value = ""0"" id=""Edit11_0"" nested-name=""Y00T00235.Y00A02885"" class=""SUIText"" readonly=""readonly"" maxlength=""2000"" data-ng-keydown=""OnKeyDown($event)"" style=""position:absolute;left:176px;width:264px;height:16px;font-style:italic;background-color:#e4e2e0;color:#000000;""/>
        <select id=""Edit11_100"" nested-name=""Y00T00235.Y00A02885"" style=""position:absolute;left:448px;width:96px;height:22px;color:#000000;background-color:#e4e2e0;font-style:italic; "" data-comos-change=""OnSelect(event,2)"">
            <option value=""F25.AA100"">ppm</option>
            <option value=""F25.AA110"">1/1</option>
            <option value=""F25.AA120"" selected=""selected"">%</option>
            <option value=""F25.AA130"">‰</option>
        </select>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:160px;width:545px;height:23px'>
    <nobr>
        <span unselectable=""on"" id=""Edit12_6"" nested-name=""Y00T00235.Y00A02888"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:176px;height:22px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Planned working units</span>
        <input type=""text"" value = ""4"" id=""Edit12_0"" nested-name=""Y00T00235.Y00A02888"" class=""SUIText"" readonly=""readonly"" maxlength=""2000"" data-ng-keydown=""OnKeyDown($event)"" style=""position:absolute;left:176px;width:264px;height:16px;font-style:italic;background-color:#e4e2e0;color:#000000;""/>
        <select id=""Edit12_100"" nested-name=""Y00T00235.Y00A02888"" style=""position:absolute;left:448px;width:96px;height:22px;color:#000000;background-color:#e4e2e0;font-style:italic; "" data-comos-change=""OnSelect(event,14)"">
            <option value=""J45.AA100"">y&#160;(sidereal)</option>
            <option value=""J45.AA110"">y&#160;(tropical)</option>
            <option value=""J45.AA120"">y&#160;(365&#160;days)</option>
            <option value=""J45.AA130"">mo</option>
            <option value=""J45.AA140"">sw&#160;(7&#160;days)</option>
            <option value=""J45.AA150"">wk</option>
            <option value=""J45.AA160"">d</option>
            <option value=""J45.AA170"">ks</option>
            <option value=""J45.AA180"">min</option>
            <option value=""J45.AA190"">s</option>
            <option value=""J45.AA200"">ms</option>
            <option value=""J45.AA210"">&#181;s</option>
            <option value=""J45.AA220"">ns</option>
            <option value=""J45.AA230"">ps</option>
            <option value=""J45.AA240"" selected=""selected"">h</option>
            <option value=""J45.BA100"">sm&#160;(28&#160;days)</option>
            <option value=""J45.BA110"">Shake</option>
            <option value=""J45.BA120"">y</option>
        </select>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:96px;width:545px;height:23px'>
    <nobr>
        <span unselectable=""on"" id=""DateTime13_7"" nested-name=""Y00T00235.Y00A02912"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:176px;height:22px;overflow:hidden;color:#000000;font-size: 13px;font-family:Nina; "">Planned end date</span>
        <input type=""text"" value = """" id=""DateTime13_0"" nested-name=""Y00T00235.Y00A02912"" class=""SUIText"" readonly=""readonly"" maxlength=""2000"" data-ng-keydown=""OnKeyDown($event)"" style=""position:absolute;left:176px;width:312px;height:16px;background-color:#e4e2e0;color:#000000;""/>
        <button class=""SUITimeButton"" id=""DateTime13_11"" nested-name=""Y00T00235.Y00A02912"" style=""position:absolute;left: 496px;width:22px;height:22px"" disabled></button>
        <button class=""SUIOpenButton"" id=""DateTime13_8"" nested-name=""Y00T00235.Y00A02912"" style=""position:absolute;left: 520px;width:22px;height:22px"" disabled></button>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:64px;width:545px;height:23px'>
    <nobr>
        <span unselectable=""on"" id=""DateTime14_7"" nested-name=""Y00T00235.Y00A02913"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:176px;height:22px;overflow:hidden;color:#000000;font-size: 13px;font-family:Nina; "">Planned start date</span>
        <input type=""text"" value = """" id=""DateTime14_0"" nested-name=""Y00T00235.Y00A02913"" class=""SUIText"" readonly=""readonly"" maxlength=""2000"" data-ng-keydown=""OnKeyDown($event)"" style=""position:absolute;left:176px;width:312px;height:16px;background-color:#e4e2e0;color:#000000;""/>
        <button class=""SUITimeButton"" id=""DateTime14_11"" nested-name=""Y00T00235.Y00A02913"" style=""position:absolute;left: 496px;width:22px;height:22px"" disabled></button>
        <button class=""SUIOpenButton"" id=""DateTime14_8"" nested-name=""Y00T00235.Y00A02913"" style=""position:absolute;left: 520px;width:22px;height:22px"" disabled></button>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:288px;width:545px;height:23px'>
    <nobr>
        <span unselectable=""on"" id=""Edit15_6"" nested-name=""Y00T00235.Y00A02976"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:176px;height:22px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Planning mode</span>
        <select id=""Edit15_0"" nested-name=""Y00T00235.Y00A02976"" style=""position:absolute;left:176px;width:368px;height:22px;color:#000000;background-color:#e4e2e0;font-style:italic; "" data-comos-change=""OnSelect(event,1)"">
            <option value=""""></option>
            <option value=""Virtual"">Virtual</option>
            <option value=""Normal"" selected=""selected"">Normal</option>
            <option value=""Planning stop at this level (sequential)"">Planning&nbsp;stop&nbsp;at&nbsp;this&nbsp;level&nbsp;(sequential)</option>
            <option value=""Planning stop at this level (parallel)"">Planning&nbsp;stop&nbsp;at&nbsp;this&nbsp;level&nbsp;(parallel)</option>
            <option value=""Failure of connected equipment"">Failure&nbsp;of&nbsp;connected&nbsp;equipment</option>
        </select>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:384px;width:545px;height:25px'>
    <nobr>
        <input type='checkbox' id=""Check16_12"" nested-name=""Y00T00235.Y00A03000"" class=""SUICheckBox"" style = ""position:absolute;left:0px;top:0px;vertical-align:middle;"" disabled=""disabled""/>
        <span unselectable=""on"" id=""Check16_7"" nested-name=""Y00T00235.Y00A03000"" style=""vertical-align:middle;position:absolute;left:18px;top:2px;width:528px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Start date (move) locked</span>
    </nobr>
</div>";

        public static string HtmlExampleOptionsDuplicate = @"<div unselectable=""on"" style=""border: #a9a9a9 1px solid; border-radius: 4px; -moz-border-radius: 4px; -webkit-border-radius: 4px; z-index:1; position:absolute;left: 32px; top: 40px; width: 448px; height: 120px; ""></div>
<div unselectable=""on"" id=""Border1"" style=""cursor:default; border: 0 0px none; background: white; z-index:2; white-space:nowrap;position:absolute; padding-left:5px; padding-right:5px; left: 42px; top: 32px; color:#000000; font-size:11px; font-family:MS Sans Serif; "">System settings</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:64px;top:88px;width:385px;height:25px'>
    <nobr>
        <span unselectable=""on"" id=""Edit3_6"" nested-name=""Y00T00001.Y00A02625"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:176px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">VSUI attributes</span>
        <input type=""text"" value = """" id=""Edit3_0"" nested-name=""Y00T00001.Y00A02625"" class=""SUIText"" readonly=""readonly"" maxlength=""2000"" data-ng-keydown=""OnKeyDown($event)"" style=""position:absolute;left:176px;width:200px;height:18px;background-color:#e4e2e0;color:#000000;""/>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:64px;top:64px;width:385px;height:25px'>
    <nobr>
        <span unselectable=""on"" id=""Edit4_6"" nested-name=""Y00T00001.Y00A03066"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:176px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Object class</span>
        <select id=""Edit4_0"" nested-name=""Y00T00001.Y00A03066"" style=""position:absolute;left:176px;width:208px;height:24px;color:#000000;background-color:#e4e2e0;font-style:italic; "" data-comos-change=""OnSelect(event,2)"">
            <option value=""""></option>
            <option value=""Process stream"">Process&nbsp;stream</option>
            <option value=""Material stream"">Material&nbsp;stream</option>
            <option value=""Boundary stream"" selected=""selected"">Boundary&nbsp;stream</option>
            <option value=""Material pointer"">Material&nbsp;pointer</option>
            <option value=""Process unit"">Process&nbsp;unit</option>
            <option value=""Process"">Process</option>
            <option value=""Equipment template"">Equipment&nbsp;template</option>
            <option value=""Nozzles"">Nozzles</option>
            <option value=""T-piece"">T-piece</option>
            <option value=""Valve"">Valve</option>
            <option value=""Battery limit"">Battery&nbsp;limit</option>
            <option value=""Folder variants"">Folder&nbsp;variants</option>
            <option value=""Note object"">Note&nbsp;object</option>
            <option value=""Equipment"">Equipment</option>
            <option value=""Template origin point"">Template&nbsp;origin&nbsp;point</option>
            <option value=""Package document mass balance"">Package&nbsp;document&nbsp;mass&nbsp;balance</option>
            <option value=""Simulation import"">Simulation&nbsp;import</option>
            <option value=""Process template"">Process&nbsp;template</option>
            <option value=""Query folder"">Query&nbsp;folder</option>
            <option value=""Simulation data folder"">Simulation&nbsp;data&nbsp;folder</option>
            <option value=""Material stream simulation"">Material&nbsp;stream&nbsp;simulation</option>
            <option value=""Simulation tray"">Simulation&nbsp;tray</option>
            <option value=""Process stream"">Process&nbsp;stream</option>
            <option value=""Equipment case"">Equipment&nbsp;case</option>
            <option value=""CM assembly"">CM&nbsp;assembly</option>
            <option value=""Document mass balance"">Document&nbsp;mass&nbsp;balance</option>
            <option value=""Simulation case"">Simulation&nbsp;case</option>
            <option value=""Cost basis"">Cost&nbsp;basis</option>
            <option value=""Cost basis object"">Cost&nbsp;basis&nbsp;object</option>
            <option value=""Cost object"">Cost&nbsp;object</option>
            <option value=""Offer basis"">Offer&nbsp;basis</option>
            <option value=""Offer basis object"">Offer&nbsp;basis&nbsp;object</option>
            <option value=""Hydraulic case"">Hydraulic&nbsp;case</option>
            <option value=""Mechanical case"">Mechanical&nbsp;case</option>
            <option value=""Material type"">Material&nbsp;type</option>
        </select>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:64px;top:112px;width:385px;height:23px'>
    <nobr>
        <span unselectable=""on"" id=""Edit5_6"" nested-name=""Y00T00001.Y00A04972"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:176px;height:22px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">DXF block name</span>
        <input type=""text"" value = """" id=""Edit5_0"" nested-name=""Y00T00001.Y00A04972"" class=""SUIText"" readonly=""readonly"" maxlength=""2000"" data-ng-keydown=""OnKeyDown($event)"" style=""position:absolute;left:176px;width:200px;height:16px;background-color:#e4e2e0;color:#000000;""/>
    </nobr>
</div>";

        public static string HtmlExample_TextArea = @"<div unselectable=""on"" style=""border: #a9a9a9 1px solid; border-radius: 4px; -moz-border-radius: 4px; -webkit-border-radius: 4px; z-index:1; position:absolute;left: 32px; top: 40px; width: 928px; height: 160px; ""></div>
<div unselectable=""on"" id=""Border1"" style=""cursor:default; border: 0 0px none; background: white; z-index:2; white-space:nowrap;position:absolute; padding-left:5px; padding-right:5px; left: 42px; top: 32px; color:#000000; font-size:13px; font-family:MS Sans Serif; "">General information 1</div>
<div unselectable=""on"" style=""border: #a9a9a9 1px solid; border-radius: 4px; -moz-border-radius: 4px; -webkit-border-radius: 4px; z-index:1; position:absolute;left: 32px; top: 240px; width: 928px; height: 112px; ""></div>
<div unselectable=""on"" id=""Border2"" style=""cursor:default; border: 0 0px none; background: white; z-index:2; white-space:nowrap;position:absolute; padding-left:5px; padding-right:5px; left: 42px; top: 232px; color:#000000; font-size:11px; font-family:MS Sans Serif; "">General information 2</div>
<div unselectable=""on"" style=""border: #a9a9a9 1px solid; border-radius: 4px; -moz-border-radius: 4px; -webkit-border-radius: 4px; z-index:1; position:absolute;left: 32px; top: 392px; width: 928px; height: 352px; ""></div>
<div unselectable=""on"" id=""Border3"" style=""cursor:default; border: 0 0px none; background: white; z-index:2; white-space:nowrap;position:absolute; padding-left:5px; padding-right:5px; left: 42px; top: 384px; color:#000000; font-size:11px; font-family:MS Sans Serif; "">Comments</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:512px;width:897px;height:73px'>
    <nobr>
        <span unselectable=""on"" id=""Memo4_7"" nested-name=""Y00T00156.Y00A00171"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:72px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Binding norms and standards</span>
        <textarea id=""Memo4_0"" nested-name=""Y00T00156.Y00A00171"" class=""SUITextArea"" style=""position:absolute;resize:none;left:146px; width:720px; height:66px;overflow:hidden;height:66px;background-color:#e4e2e0;color:#000000;"" data-comos-change=""OnChange(event)"" readonly=""readonly""></textarea>
        <button class=""SUIOpenButton"" id=""Memo4_8"" nested-name=""Y00T00156.Y00A00171"" style=""position:absolute;left: 874px;width:21px;height:21px"" data-ng-click=""OnClick($event)""></button>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:88px;width:433px;height:25px'>
    <nobr>
        <span unselectable=""on"" id=""Edit5_6"" nested-name=""Y00T00156.Y00A00370"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Manufacturer</span>
        <input type=""text"" value = """" id=""Edit5_0"" nested-name=""Y00T00156.Y00A00370"" class=""SUIText"" readonly=""readonly"" maxlength=""2000"" data-ng-keydown=""OnKeyDown($event)"" style=""position:absolute;left:144px;width:280px;height:18px;background-color:#e4e2e0;color:#000000;""/>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:112px;width:433px;height:25px'>
    <nobr>
        <span unselectable=""on"" id=""Edit6_6"" nested-name=""Y00T00156.Y00A00398"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Supplier</span>
        <input type=""text"" value = """" id=""Edit6_0"" nested-name=""Y00T00156.Y00A00398"" class=""SUIText"" readonly=""readonly"" maxlength=""2000"" data-ng-keydown=""OnKeyDown($event)"" style=""position:absolute;left:144px;width:280px;height:18px;background-color:#e4e2e0;color:#000000;""/>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:64px;width:433px;height:25px'>
    <nobr>
        <span unselectable=""on"" id=""Edit7_6"" nested-name=""Y00T00156.Y00A00541"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Type</span>
        <input type=""text"" value = """" id=""Edit7_0"" nested-name=""Y00T00156.Y00A00541"" class=""SUIText"" readonly=""readonly"" maxlength=""2000"" data-ng-keydown=""OnKeyDown($event)"" style=""position:absolute;left:144px;width:280px;height:18px;background-color:#e4e2e0;color:#000000;""/>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:608px;width:897px;height:65px'>
    <nobr>
        <span unselectable=""on"" id=""Memo8_7"" nested-name=""Y00T00156.Y00A00647"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:64px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">General remark</span>
        <textarea id=""Memo8_0"" nested-name=""Y00T00156.Y00A00647"" class=""SUITextArea"" style=""position:absolute;resize:none;left:146px; width:720px; height:58px;overflow:hidden;height:58px;background-color:#e4e2e0;color:#000000;"" data-comos-change=""OnChange(event)"" readonly=""readonly""></textarea>
        <button class=""SUIOpenButton"" id=""Memo8_8"" nested-name=""Y00T00156.Y00A00647"" style=""position:absolute;left: 874px;width:21px;height:21px"" data-ng-click=""OnClick($event)""></button>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:512px;top:88px;width:433px;height:25px'>
    <nobr>
        <span unselectable=""on"" id=""Edit9_6"" nested-name=""Y00T00156.Y00A00670"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Order no.</span>
        <input type=""text"" value = """" id=""Edit9_0"" nested-name=""Y00T00156.Y00A00670"" class=""SUIText"" readonly=""readonly"" maxlength=""2000"" data-ng-keydown=""OnKeyDown($event)"" style=""position:absolute;left:144px;width:280px;height:18px;background-color:#e4e2e0;color:#000000;""/>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:512px;top:160px;width:433px;height:25px'>
    <nobr>
        <span unselectable=""on"" id=""Edit10_6"" nested-name=""Y00T00156.Y00A00771"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Drawing no.</span>
        <input type=""text"" value = """" id=""Edit10_0"" nested-name=""Y00T00156.Y00A00771"" class=""SUIText"" readonly=""readonly"" maxlength=""2000"" data-ng-keydown=""OnKeyDown($event)"" style=""position:absolute;left:144px;width:280px;height:18px;background-color:#e4e2e0;color:#000000;""/>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:512px;top:64px;width:433px;height:25px'>
    <nobr>
        <span unselectable=""on"" id=""Edit11_6"" nested-name=""Y00T00156.Y00A00877"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Count</span>
        <input type=""text"" value = """" id=""Edit11_0"" nested-name=""Y00T00156.Y00A00877"" class=""SUIText"" readonly=""readonly"" maxlength=""2000"" data-ng-keydown=""OnKeyDown($event)"" style=""position:absolute;left:144px;width:280px;height:18px;background-color:#e4e2e0;color:#000000;""/>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:288px;width:433px;height:25px'>
    <nobr>
        <span unselectable=""on"" id=""Edit12_6"" nested-name=""Y00T00156.Y00A00974"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Design pressure</span>
        <input type=""text"" value = """" id=""Edit12_1"" nested-name=""Y00T00156.Y00A00974"" class=""SUIText"" readonly=""readonly"" maxlength=""2000"" data-ng-keydown=""OnKeyDown($event)"" style=""position:absolute;left:144px;width:104px;height:18px;background-color:#e4e2e0;color:#000000;""/>
        <input type=""text"" value = """" id=""Edit12_2"" nested-name=""Y00T00156.Y00A00974"" class=""SUIText"" readonly=""readonly"" maxlength=""2000"" data-ng-keydown=""OnKeyDown($event)"" style=""position:absolute;left:256px;width:104px;height:18px;background-color:#e4e2e0;color:#000000;""/>
        <select id=""Edit12_100"" nested-name=""Y00T00156.Y00A00974"" style=""position:absolute;left:368px;width:64px;height:24px;color:#000000;background-color:#e4e2e0;font-style:italic; "" data-comos-change=""OnSelect(event,9)"">
            <option value=""E80.AA100"">GPa</option>
            <option value=""E80.AA110"">kbar</option>
            <option value=""E80.AA120"">hbar</option>
            <option value=""E80.AA130"">kgf/mm&#178;</option>
            <option value=""E80.AA140"">MJ/m&#179;</option>
            <option value=""E80.AA150"">MPa</option>
            <option value=""E80.AA160"">MPa&#160;(g)</option>
            <option value=""E80.AA170"">N/mm&#178;</option>
            <option value=""E80.AA180"">atm</option>
            <option value=""E80.AA190"" selected=""selected"">bar</option>
            <option value=""E80.AA200"">at</option>
            <option value=""E80.AA210"">N/cm&#178;</option>
            <option value=""E80.AA220"">mH₂O</option>
            <option value=""E80.AA230"">cmHg</option>
            <option value=""E80.AA240"">cmHg&#160;(0&#160;&#176;C)</option>
            <option value=""E80.AA250"">kPa</option>
            <option value=""E80.AA260"">mmHg</option>
            <option value=""E80.AA270"">Torr</option>
            <option value=""E80.AA280"">hPa</option>
            <option value=""E80.AA290"">mbar</option>
            <option value=""E80.AA300"">mbar&#160;(g)</option>
            <option value=""E80.AA310"">cmH₂O</option>
            <option value=""E80.AA320"">cmH₂O&#160;(4&#160;&#176;C)</option>
            <option value=""E80.AA330"">daPa</option>
            <option value=""E80.AA340"">mmH₂O</option>
            <option value=""E80.AA350"">mWS</option>
            <option value=""E80.AA360"">J/m&#179;</option>
            <option value=""E80.AA370"">kg/(cm&#183;s&#178;)</option>
            <option value=""E80.AA380"">N/m&#178;</option>
            <option value=""E80.AA390"">Pa</option>
            <option value=""E80.AA400"">kg/m&#183;s&#178;</option>
            <option value=""E80.AA410"">&#181;bar</option>
            <option value=""E80.AA420"">mPa</option>
            <option value=""E80.AA430"">&#181;Pa</option>
            <option value=""E80.BA100"">klbf/in&#178;</option>
            <option value=""E80.BA110"">ksi</option>
            <option value=""E80.BA120"">bar&#160;(g)</option>
            <option value=""E80.BA130"">at&#160;(g)</option>
            <option value=""E80.BA140"">kgf/cm&#178;</option>
            <option value=""E80.BA150"">kgf/cm&#178;&#160;(g)</option>
            <option value=""E80.BA160"">ftHg</option>
            <option value=""E80.BA170"">psi&#160;(a)</option>
            <option value=""E80.BA180"">psi&#160;(g)</option>
            <option value=""E80.BA190"">inHg</option>
            <option value=""E80.BA200"">inHG&#160;(32&#160;&#176;F)</option>
            <option value=""E80.BA210"">inHg&#160;(60&#160;&#176;F)</option>
            <option value=""E80.BA220"">ftH₂O</option>
            <option value=""E80.BA230"">ftH₂O&#160;(39.2&#160;&#176;F)</option>
            <option value=""E80.BA240"">kPa&#160;(g)</option>
            <option value=""E80.BA250"">inH₂O</option>
            <option value=""E80.BA260"">inH₂O&#160;(39,2&#160;&#176;F)</option>
            <option value=""E80.BA270"">inH₂O&#160;(60&#160;&#176;F)</option>
            <option value=""E80.BA280"">pdl/in&#178;</option>
            <option value=""E80.BA290"">gf/cm&#178;</option>
            <option value=""E80.BA300"">lbf/ft&#178;</option>
            <option value=""E80.BA310"">kgf/m&#178;</option>
            <option value=""E80.BA320"">pdl/ft&#178;</option>
            <option value=""E80.BA330"">lb/(ft&#183;s&#178;)</option>
            <option value=""E80.BA340"">dyn/cm&#178;</option>
            <option value=""E80.BA350"">erg/cm&#179;</option>
        </select>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:312px;width:433px;height:25px'>
    <nobr>
        <span unselectable=""on"" id=""Edit13_6"" nested-name=""Y00T00156.Y00A00975"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Design temp.</span>
        <input type=""text"" value = """" id=""Edit13_1"" nested-name=""Y00T00156.Y00A00975"" class=""SUIText"" readonly=""readonly"" maxlength=""2000"" data-ng-keydown=""OnKeyDown($event)"" style=""position:absolute;left:144px;width:104px;height:18px;background-color:#e4e2e0;color:#000000;""/>
        <input type=""text"" value = """" id=""Edit13_2"" nested-name=""Y00T00156.Y00A00975"" class=""SUIText"" readonly=""readonly"" maxlength=""2000"" data-ng-keydown=""OnKeyDown($event)"" style=""position:absolute;left:256px;width:104px;height:18px;background-color:#e4e2e0;color:#000000;""/>
        <select id=""Edit13_100"" nested-name=""Y00T00156.Y00A00975"" style=""position:absolute;left:368px;width:64px;height:24px;color:#000000;background-color:#e4e2e0;font-style:italic; "" data-comos-change=""OnSelect(event,0)"">
            <option value=""J40.AA100"" selected=""selected"">&#176;C</option>
            <option value=""J40.AA110"">K</option>
            <option value=""J40.BA100"">&#176;F</option>
            <option value=""J40.BA110"">&#176;R</option>
        </select>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:704px;width:433px;height:25px'>
    <nobr>
        <span unselectable=""on"" id=""Link14_7"" nested-name=""Y00T00156.Y00A01111"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Sketch AutoCad</span>
        <input type=""text"" value = """" id=""Link14_0"" nested-name=""Y00T00156.Y00A01111"" class=""SUIText"" readonly=""readonly"" maxlength=""2000"" data-ng-keydown=""OnKeyDown($event)"" style=""position:absolute;left:144px;width:202px;height:18px;background-color:#e4e2e0;color:#000000;""/>
        <button class=""SUIOpenButton"" id=""Link14_8"" nested-name=""Y00T00156.Y00A01111"" style=""position:absolute;left: 354px;width:24px;height:24px"" disabled></button>
        <button class=""SUIDeleteButton"" id=""Link14_9"" nested-name=""Y00T00156.Y00A01111"" style=""position:absolute;left: 380px;width:24px;height:24px"" disabled></button>
        <button class=""SUIPropNavButton"" id=""Link14_10"" nested-name=""Y00T00156.Y00A01111"" style=""position:absolute;left: 406px;width:24px;height:24px"" disabled></button>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:416px;width:897px;height:65px'>
    <nobr>
        <span unselectable=""on"" id=""Memo15_7"" nested-name=""Y00T00156.Y00A04689"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:64px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Comment</span>
        <textarea id=""Memo15_0"" nested-name=""Y00T00156.Y00A04689"" class=""SUITextArea"" style=""position:absolute;resize:none;left:146px; width:720px; height:58px;overflow:hidden;height:58px;background-color:#e4e2e0;color:#000000;"" data-comos-change=""OnChange(event)"" readonly=""readonly""></textarea>
        <button class=""SUIOpenButton"" id=""Memo15_8"" nested-name=""Y00T00156.Y00A04689"" style=""position:absolute;left: 874px;width:21px;height:21px"" data-ng-click=""OnClick($event)""></button>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:136px;width:433px;height:23px'>
    <nobr>
        <span unselectable=""on"" id=""Date16_7"" nested-name=""Y00T00156.Y00A04744"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:22px;overflow:hidden;color:#000000;font-size: 13px;font-family:Nina; "">Date of commissioning</span>
        <input type=""text"" value = """" id=""Date16_0"" nested-name=""Y00T00156.Y00A04744"" class=""SUIText"" readonly=""readonly"" maxlength=""2000"" data-ng-keydown=""OnKeyDown($event)"" style=""position:absolute;left:144px;width:232px;height:16px;background-color:#e4e2e0;color:#000000;""/>
        <button class=""SUITimeButton"" id=""Date16_11"" nested-name=""Y00T00156.Y00A04744"" style=""position:absolute;left: 384px;width:22px;height:22px"" disabled></button>
        <button class=""SUIOpenButton"" id=""Date16_8"" nested-name=""Y00T00156.Y00A04744"" style=""position:absolute;left: 408px;width:22px;height:22px"" disabled></button>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:512px;top:136px;width:433px;height:23px'>
    <nobr>
        <span unselectable=""on"" id=""Date17_7"" nested-name=""Y00T00156.Y00A04745"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:22px;overflow:hidden;color:#000000;font-size: 13px;font-family:Nina; "">Date of installation</span>
        <input type=""text"" value = """" id=""Date17_0"" nested-name=""Y00T00156.Y00A04745"" class=""SUIText"" readonly=""readonly"" maxlength=""2000"" data-ng-keydown=""OnKeyDown($event)"" style=""position:absolute;left:144px;width:232px;height:16px;background-color:#e4e2e0;color:#000000;""/>
        <button class=""SUITimeButton"" id=""Date17_11"" nested-name=""Y00T00156.Y00A04745"" style=""position:absolute;left: 384px;width:22px;height:22px"" disabled></button>
        <button class=""SUIOpenButton"" id=""Date17_8"" nested-name=""Y00T00156.Y00A04745"" style=""position:absolute;left: 408px;width:22px;height:22px"" disabled></button>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:512px;top:112px;width:433px;height:23px'>
    <nobr>
        <span unselectable=""on"" id=""Date18_7"" nested-name=""Y00T00156.Y00A04746"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:22px;overflow:hidden;color:#000000;font-size: 13px;font-family:Nina; "">Order date</span>
        <input type=""text"" value = """" id=""Date18_0"" nested-name=""Y00T00156.Y00A04746"" class=""SUIText"" readonly=""readonly"" maxlength=""2000"" data-ng-keydown=""OnKeyDown($event)"" style=""position:absolute;left:144px;width:232px;height:16px;background-color:#e4e2e0;color:#000000;""/>
        <button class=""SUITimeButton"" id=""Date18_11"" nested-name=""Y00T00156.Y00A04746"" style=""position:absolute;left: 384px;width:22px;height:22px"" disabled></button>
        <button class=""SUIOpenButton"" id=""Date18_8"" nested-name=""Y00T00156.Y00A04746"" style=""position:absolute;left: 408px;width:22px;height:22px"" disabled></button>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:160px;width:433px;height:25px'>
    <nobr>
        <span unselectable=""on"" id=""Edit19_6"" nested-name=""Y00T00156.Y00A04748"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Lifecycle status</span>
        <input type=""text"" value = """" id=""Edit19_0"" nested-name=""Y00T00156.Y00A04748"" class=""SUIText"" readonly=""readonly"" maxlength=""2000"" data-ng-keydown=""OnKeyDown($event)"" style=""position:absolute;left:144px;width:280px;height:18px;background-color:#e4e2e0;color:#000000;""/>
    </nobr>
</div>
<div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:264px;width:433px;height:25px'>
    <nobr>
        <span unselectable=""on"" id=""Edit20_6"" nested-name=""Y00T00156.Y00A04808"" style=""vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; "">Main constr. material</span>
        <select id=""Edit20_0"" nested-name=""Y00T00156.Y00A04808"" style=""position:absolute;left:144px;width:288px;height:24px;color:#000000;background-color:#e4e2e0;"" data-comos-change=""OnSelect(event,0)"">
            <option value=""""></option>
            <option value=""GG-10"">GG-10</option>
            <option value=""GG-15"">GG-15</option>
            <option value=""GG-20"">GG-20</option>
            <option value=""GG-25"">GG-25</option>
            <option value=""GG-30"">GG-30</option>
            <option value=""GG-35"">GG-35</option>
            <option value=""GG-150 HB"">GG-150&nbsp;HB</option>
            <option value=""GG-170 HB"">GG-170&nbsp;HB</option>
            <option value=""GG-190 HB"">GG-190&nbsp;HB</option>
            <option value=""GG-220 HB"">GG-220&nbsp;HB</option>
            <option value=""GG-240 HB"">GG-240&nbsp;HB</option>
            <option value=""16 Mo 3"">16&nbsp;Mo&nbsp;3</option>
            <option value=""13 CrMo 44"">13&nbsp;CrMo&nbsp;44</option>
            <option value=""10 CrMo 9-10"">10&nbsp;CrMo&nbsp;9-10</option>
            <option value=""12 CrMo 195"">12&nbsp;CrMo&nbsp;195</option>
            <option value=""P92"">P92</option>
            <option value=""St 37.2"">St&nbsp;37.2</option>
            <option value=""St 37"">St&nbsp;37</option>
            <option value=""St 42"">St&nbsp;42</option>
            <option value=""St 42.2"">St&nbsp;42.2</option>
            <option value=""St 35.8"">St&nbsp;35.8</option>
            <option value=""St 45"">St&nbsp;45</option>
            <option value=""St 55"">St&nbsp;55</option>
            <option value=""St 52"">St&nbsp;52</option>
            <option value=""St 52.4"">St&nbsp;52.4</option>
            <option value=""St 52.3"">St&nbsp;52.3</option>
            <option value=""St 35.4"">St&nbsp;35.4</option>
            <option value=""St 45.4"">St&nbsp;45.4</option>
            <option value=""St 35.8 I"">St&nbsp;35.8&nbsp;I</option>
            <option value=""St 45.8"">St&nbsp;45.8</option>
            <option value=""Tax 37.2"">Tax&nbsp;37.2</option>
            <option value=""RSt 37.2"">RSt&nbsp;37.2</option>
            <option value=""C 22.3"">C&nbsp;22.3</option>
            <option value=""C 21"">C&nbsp;21</option>
            <option value=""StE 355"">StE&nbsp;355</option>
            <option value=""C 22.8"">C&nbsp;22.8</option>
            <option value=""St 44.0"">St&nbsp;44.0</option>
            <option value=""St 44.2"">St&nbsp;44.2</option>
            <option value=""St 37.8 I"">St&nbsp;37.8&nbsp;I</option>
            <option value=""1.0305-St 35.8 III"">1.0305-St&nbsp;35.8&nbsp;III</option>
            <option value=""1.0315-St 37.8 III"">1.0315-St&nbsp;37.8&nbsp;III</option>
            <option value=""X6CrNiMoTi17-12-2"">X6CrNiMoTi17-12-2</option>
            <option value=""X 10 CrNiNb 189"">X&nbsp;10&nbsp;CrNiNb&nbsp;189</option>
            <option value=""X6CrNiTi18-10"">X6CrNiTi18-10</option>
            <option value=""X 5 CrNi 189"">X&nbsp;5&nbsp;CrNi&nbsp;189</option>
            <option value=""X 2 CrNi 1911"">X&nbsp;2&nbsp;CrNi&nbsp;1911</option>
            <option value=""X5CrNiMo17-12-2"">X5CrNiMo17-12-2</option>
            <option value=""X2CrNiMo17-12-2"">X2CrNiMo17-12-2</option>
            <option value=""X2CrNiMoN22-5-3"">X2CrNiMoN22-5-3</option>
            <option value=""X5CrNi19-11"">X5CrNi19-11</option>
            <option value=""X 2 CrNiMo 17 132"">X&nbsp;2&nbsp;CrNiMo&nbsp;17&nbsp;132</option>
            <option value=""X 5 CrNiMo 17 122"">X&nbsp;5&nbsp;CrNiMo&nbsp;17&nbsp;122</option>
            <option value=""X6CrNiMoTi 17-12-2"">X6CrNiMoTi&nbsp;17-12-2</option>
            <option value=""GX6CrNiMo18-10"">GX6CrNiMo18-10</option>
            <option value=""GX5CrNiMoNb19-11-2"">GX5CrNiMoNb19-11-2</option>
            <option value=""GX5CrNiNb19-11"">GX5CrNiNb19-11</option>
            <option value=""It 200"">It&nbsp;200</option>
            <option value=""It 300"">It&nbsp;300</option>
            <option value=""It 400"">It&nbsp;400</option>
            <option value=""It S"">It&nbsp;S</option>
            <option value=""It O"">It&nbsp;O</option>
            <option value=""Graphite/metal layer"">Graphite/metal&nbsp;layer</option>
            <option value=""Aramid fiber"">Aramid&nbsp;fiber</option>
            <option value=""A 161 Grd T1"">A&nbsp;161&nbsp;Grd&nbsp;T1</option>
            <option value=""A 213 Grd T12"">A&nbsp;213&nbsp;Grd&nbsp;T12</option>
            <option value=""A 213 Grd T22"">A&nbsp;213&nbsp;Grd&nbsp;T22</option>
            <option value=""A 213 Grd T5"">A&nbsp;213&nbsp;Grd&nbsp;T5</option>
            <option value=""A 214"">A&nbsp;214</option>
            <option value=""A 53  Grd A"">A&nbsp;53&nbsp;&nbsp;Grd&nbsp;A</option>
            <option value=""A 53 Grd B"">A&nbsp;53&nbsp;Grd&nbsp;B</option>
            <option value=""A 252 Grd 3"">A&nbsp;252&nbsp;Grd&nbsp;3</option>
            <option value=""A 381 Grd Y50"">A&nbsp;381&nbsp;Grd&nbsp;Y50</option>
            <option value=""A 179"">A&nbsp;179</option>
            <option value=""A 106 Grd A"">A&nbsp;106&nbsp;Grd&nbsp;A</option>
            <option value=""A 106 Grd B"">A&nbsp;106&nbsp;Grd&nbsp;B</option>
            <option value=""A 53"">A&nbsp;53</option>
            <option value=""A 181 Grd I"">A&nbsp;181&nbsp;Grd&nbsp;I</option>
            <option value=""A 105"">A&nbsp;105</option>
            <option value=""API 5L"">API&nbsp;5L</option>
            <option value=""A 105 Grd I"">A&nbsp;105&nbsp;Grd&nbsp;I</option>
            <option value=""A 192"">A&nbsp;192</option>
            <option value=""A 217 WC9 Grd A"">A&nbsp;217&nbsp;WC9&nbsp;Grd&nbsp;A</option>
            <option value=""AISI  Grd TP316Ti"">AISI&nbsp;&nbsp;Grd&nbsp;TP316Ti</option>
            <option value=""A 312 Grd TP347"">A&nbsp;312&nbsp;Grd&nbsp;TP347</option>
            <option value=""A 335 P22"">A&nbsp;335&nbsp;P22</option>
            <option value=""A 182 Grd F304"">A&nbsp;182&nbsp;Grd&nbsp;F304</option>
            <option value=""A 234 WPB"">A&nbsp;234&nbsp;WPB</option>
            <option value=""A 182 F 22"">A&nbsp;182&nbsp;F&nbsp;22</option>
            <option value=""A 182 F12"">A&nbsp;182&nbsp;F12</option>
            <option value=""25 CrMo 4"">25&nbsp;CrMo&nbsp;4</option>
            <option value=""24 CrMo 5"">24&nbsp;CrMo&nbsp;5</option>
            <option value=""Ck 14"">Ck&nbsp;14</option>
            <option value=""Ck 22"">Ck&nbsp;22</option>
            <option value=""Ck 35"">Ck&nbsp;35</option>
            <option value=""PTFE"">PTFE</option>
            <option value=""P235TR2"">P235TR2</option>
            <option value=""P235GH"">P235GH</option>
            <option value=""P265GH"">P265GH</option>
            <option value=""16Mo 3"">16Mo&nbsp;3</option>
            <option value=""13CrMo4-5"">13CrMo4-5</option>
            <option value=""10CrMo9-10"">10CrMo9-10</option>
            <option value=""P215NL"">P215NL</option>
            <option value=""P255QL"">P255QL</option>
            <option value=""P355NH"">P355NH</option>
            <option value=""P355NL1"">P355NL1</option>
            <option value=""P355NL2"">P355NL2</option>
            <option value=""12Ni14"">12Ni14</option>
            <option value=""X5CrNi18-10"">X5CrNi18-10</option>
            <option value=""X2CrNi19-11"">X2CrNi19-11</option>
            <option value=""P245GH"">P245GH</option>
            <option value=""PE"">PE</option>
            <option value=""P265GH"">P265GH</option>
            <option value=""Graphite"">Graphite</option>
            <option value=""P250GH"">P250GH</option>
            <option value=""EN JS1025"">EN&nbsp;JS1025</option>
            <option value=""GP240GH"">GP240GH</option>
            <option value=""sa 106C"">sa&nbsp;106C</option>
            <option value=""A 106 Grd D"">A&nbsp;106&nbsp;Grd&nbsp;D</option>
            <option value=""A 234  Gr. P91"">A&nbsp;234&nbsp;&nbsp;Gr.&nbsp;P91</option>
            <option value=""acc. to definition"">acc.&nbsp;to&nbsp;definition</option>
            <option value=""See supplier"">See&nbsp;supplier</option>
            <option value=""X2CrNi 18 9"">X2CrNi&nbsp;18&nbsp;9</option>
            <option value=""A 106 Grd C"">A&nbsp;106&nbsp;Grd&nbsp;C</option>
            <option value=""A 209 Grd T1"">A&nbsp;209&nbsp;Grd&nbsp;T1</option>
            <option value=""A 209 Grd T1a"">A&nbsp;209&nbsp;Grd&nbsp;T1a</option>
            <option value=""A 209 Grd T1b"">A&nbsp;209&nbsp;Grd&nbsp;T1b</option>
            <option value=""A 335 Grd P1"">A&nbsp;335&nbsp;Grd&nbsp;P1</option>
            <option value=""A 335 Grd P2"">A&nbsp;335&nbsp;Grd&nbsp;P2</option>
            <option value=""A 335 Grd P5"">A&nbsp;335&nbsp;Grd&nbsp;P5</option>
            <option value=""A 213 Grd TP304"">A&nbsp;213&nbsp;Grd&nbsp;TP304</option>
            <option value=""A 213 Grd TP316"">A&nbsp;213&nbsp;Grd&nbsp;TP316</option>
            <option value=""A 213 Grd TP321"">A&nbsp;213&nbsp;Grd&nbsp;TP321</option>
            <option value=""A 249 Grd TP304"">A&nbsp;249&nbsp;Grd&nbsp;TP304</option>
            <option value=""A 249 Grd TP316"">A&nbsp;249&nbsp;Grd&nbsp;TP316</option>
            <option value=""A 249 Grd TP321"">A&nbsp;249&nbsp;Grd&nbsp;TP321</option>
            <option value=""A 269 Grd TP304"">A&nbsp;269&nbsp;Grd&nbsp;TP304</option>
            <option value=""A 269 Grd TP316"">A&nbsp;269&nbsp;Grd&nbsp;TP316</option>
            <option value=""A 269 Grd TP321"">A&nbsp;269&nbsp;Grd&nbsp;TP321</option>
            <option value=""A 312 Grd TP304"">A&nbsp;312&nbsp;Grd&nbsp;TP304</option>
            <option value=""A 312 Grd TP316"">A&nbsp;312&nbsp;Grd&nbsp;TP316</option>
            <option value=""A 312 Grd TP321"">A&nbsp;312&nbsp;Grd&nbsp;TP321</option>
            <option value=""A 358 Grd TP304"">A&nbsp;358&nbsp;Grd&nbsp;TP304</option>
            <option value=""A 358 Grd TP316"">A&nbsp;358&nbsp;Grd&nbsp;TP316</option>
            <option value=""A 358 Grd TP321"">A&nbsp;358&nbsp;Grd&nbsp;TP321</option>
            <option value=""A 105 M-98"">A&nbsp;105&nbsp;M-98</option>
            <option value=""A 181 M-95 b Class 60"">A&nbsp;181&nbsp;M-95&nbsp;b&nbsp;Class&nbsp;60</option>
            <option value=""A 181 M-95 b Class 70"">A&nbsp;181&nbsp;M-95&nbsp;b&nbsp;Class&nbsp;70</option>
            <option value=""A 350 LF1"">A&nbsp;350&nbsp;LF1</option>
            <option value=""A 350 LF2"">A&nbsp;350&nbsp;LF2</option>
            <option value=""A 350 LF3"">A&nbsp;350&nbsp;LF3</option>
            <option value=""A 182 F1"">A&nbsp;182&nbsp;F1</option>
            <option value=""A 182 F2"">A&nbsp;182&nbsp;F2</option>
            <option value=""A 182 F5"">A&nbsp;182&nbsp;F5</option>
            <option value=""A 182 F9"">A&nbsp;182&nbsp;F9</option>
            <option value=""A 182 F304"">A&nbsp;182&nbsp;F304</option>
            <option value=""A 182 F316"">A&nbsp;182&nbsp;F316</option>
            <option value=""A 182 F321"">A&nbsp;182&nbsp;F321</option>
            <option value=""A 182 F347"">A&nbsp;182&nbsp;F347</option>
            <option value=""A 182 F348"">A&nbsp;182&nbsp;F348</option>
            <option value=""A 182 F310"">A&nbsp;182&nbsp;F310</option>
            <option value=""A 182 F316L"">A&nbsp;182&nbsp;F316L</option>
            <option value=""A 193-2H"">A&nbsp;193-2H</option>
            <option value=""A 193-B16"">A&nbsp;193-B16</option>
            <option value=""A 193-B7"">A&nbsp;193-B7</option>
            <option value=""A 193-B8 Cl.22"">A&nbsp;193-B8&nbsp;Cl.22</option>
            <option value=""A 194-8M"">A&nbsp;194-8M</option>
            <option value=""A 234 WPB"">A&nbsp;234&nbsp;WPB</option>
            <option value=""A 261 WCB"">A&nbsp;261&nbsp;WCB</option>
            <option value=""A 312 TP316"">A&nbsp;312&nbsp;TP316</option>
            <option value=""A 312 TP316"">A&nbsp;312&nbsp;TP316</option>
            <option value=""A 312 TP316L"">A&nbsp;312&nbsp;TP316L</option>
            <option value=""A 403 TP316L"">A&nbsp;403&nbsp;TP316L</option>
            <option value=""A 672 B60 Cl.22"">A&nbsp;672&nbsp;B60&nbsp;Cl.22</option>
            <option value=""S 235JRG2"">S&nbsp;235JRG2</option>
            <option value=""sa 105"">sa&nbsp;105</option>
            <option value=""SA 182 F1"">SA&nbsp;182&nbsp;F1</option>
            <option value=""SA 182 F12"">SA&nbsp;182&nbsp;F12</option>
            <option value=""SA 182 F22"">SA&nbsp;182&nbsp;F22</option>
            <option value=""sa 193-B16"">sa&nbsp;193-B16</option>
            <option value=""sa 194-3"">sa&nbsp;194-3</option>
            <option value=""SA 216 WCB"">SA&nbsp;216&nbsp;WCB</option>
            <option value=""SA 217 WC6"">SA&nbsp;217&nbsp;WC6</option>
            <option value=""SA 217 WB"">SA&nbsp;217&nbsp;WB</option>
            <option value=""SA 234 WP1"">SA&nbsp;234&nbsp;WP1</option>
            <option value=""SA 234 WP12"">SA&nbsp;234&nbsp;WP12</option>
            <option value=""SA 234 WP22"">SA&nbsp;234&nbsp;WP22</option>
            <option value=""SA 321 TP316"">SA&nbsp;321&nbsp;TP316</option>
            <option value=""SA 335 P1"">SA&nbsp;335&nbsp;P1</option>
            <option value=""SA 335 P12"">SA&nbsp;335&nbsp;P12</option>
            <option value=""SA 335 P22"">SA&nbsp;335&nbsp;P22</option>
            <option value=""SA 691 2 1/4 Cr. Cl.22"">SA&nbsp;691&nbsp;2&nbsp;1/4&nbsp;Cr.&nbsp;Cl.22</option>
            <option value=""SA 691 1Cr. Cl.22"">SA&nbsp;691&nbsp;1Cr.&nbsp;Cl.22</option>
            <option value=""316NG"">316NG</option>
            <option value=""316SS / graphite coat"">316SS&nbsp;/&nbsp;graphite&nbsp;coat</option>
            <option value=""316SS / rubber inlay"">316SS&nbsp;/&nbsp;rubber&nbsp;inlay</option>
            <option value=""1.4541 / graphite"">1.4541&nbsp;/&nbsp;graphite</option>
            <option value=""SA 216 WCB"">SA&nbsp;216&nbsp;WCB</option>
            <option value=""A 182 F304L"">A&nbsp;182&nbsp;F304L</option>
            <option value=""A 182 F22 Cl.3"">A&nbsp;182&nbsp;F22&nbsp;Cl.3</option>
            <option value=""A 182 F11 Cl.2"">A&nbsp;182&nbsp;F11&nbsp;Cl.2</option>
            <option value=""See remark"">See&nbsp;remark</option>
            <option value=""Variable"">Variable</option>
            <option value=""SA 106 Gr B"">SA&nbsp;106&nbsp;Gr&nbsp;B</option>
            <option value=""SA 672 B60 CI.22"">SA&nbsp;672&nbsp;B60&nbsp;CI.22</option>
            <option value=""17-4 PH PTFE coat"">17-4&nbsp;PH&nbsp;PTFE&nbsp;coat</option>
            <option value=""Low CS"">Low&nbsp;CS</option>
            <option value=""P295GH"">P295GH</option>
            <option value=""316L SS &amp; PTFE"">316L&nbsp;SS&nbsp;&amp;&nbsp;PTFE</option>
            <option value=""316NG"">316NG</option>
            <option value=""347NG"">347NG</option>
            <option value=""CS &amp; PTFE"">CS&nbsp;&amp;&nbsp;PTFE</option>
        </select>
    </nobr>
</div>";

        public static string HtmlExample3 = "<div unselectable=\"on\" style=\"border: #a9a9a9 1px solid; border-radius: 4px; -moz-border-radius: 4px; -webkit-border-radius: 4px; z-index:1; position:absolute;left: 32px; top: 40px; width: 464px; height: 520px; \"></div><div unselectable=\"on\" id=\"Border1\" style=\"cursor:default; border: 0 0px none; background: white; z-index:2; white-space:nowrap;position:absolute; padding-left:5px; padding-right:5px; left: 42px; top: 32px; color:#000000; font-size:11px; font-family:MS Sans Serif; \">Process properties</div><div unselectable=\"on\" style=\"border: #a9a9a9 1px solid; border-radius: 4px; -moz-border-radius: 4px; -webkit-border-radius: 4px; z-index:1; position:absolute;left: 32px; top: 592px; width: 464px; height: 120px; \"></div><div unselectable=\"on\" id=\"Border2\" style=\"cursor:default; border: 0 0px none; background: white; z-index:2; white-space:nowrap;position:absolute; padding-left:5px; padding-right:5px; left: 42px; top: 584px; color:#000000; font-size:11px; font-family:MS Sans Serif; \">Comments</div><div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:208px;top:112px;width:41px;height:25px'><span unselectable=\"on\" id=\"Label3_7\" nested-name=\"Y00T00041.LA001\" style=\"vertical-align:middle;position:absolute;left:0px;top:0px;width:40px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; \">min.</span></div><div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:272px;top:112px;width:49px;height:25px'><span unselectable=\"on\" id=\"Label4_7\" nested-name=\"Y00T00041.LA002\" style=\"vertical-align:middle;position:absolute;left:0px;top:0px;width:48px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; \">norm.</span></div><div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:344px;top:112px;width:49px;height:25px'><span unselectable=\"on\" id=\"Label5_7\" nested-name=\"Y00T00041.LA003\" style=\"vertical-align:middle;position:absolute;left:0px;top:0px;width:48px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; \">max.</span></div><div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:136px;width:433px;height:25px'><nobr><span unselectable=\"on\" id=\"Edit6_6\" nested-name=\"Y00T00041.Y00A00477\" style=\"vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; \">Ambient temp.</span><input type=\"text\" value = \"\" id=\"Edit6_1\" nested-name=\"Y00T00041.Y00A00477\" class=\"SUIText\" readonly=\"readonly\" maxlength=\"2000\" data-ng-keydown=\"OnKeyDown($event)\" style=\"position:absolute;left:144px;width:66px;height:18px;background-color:#e4e2e0;color:#000000;\"/><input type=\"text\" value = \"\" id=\"Edit6_0\" nested-name=\"Y00T00041.Y00A00477\" class=\"SUIText\" readonly=\"readonly\" maxlength=\"2000\" data-ng-keydown=\"OnKeyDown($event)\" style=\"position:absolute;left:218px;width:66px;height:18px;background-color:#e4e2e0;color:#000000;\"/><input type=\"text\" value = \"\" id=\"Edit6_2\" nested-name=\"Y00T00041.Y00A00477\" class=\"SUIText\" readonly=\"readonly\" maxlength=\"2000\" data-ng-keydown=\"OnKeyDown($event)\" style=\"position:absolute;left:292px;width:66px;height:18px;background-color:#e4e2e0;color:#000000;\"/><select id=\"Edit6_100\" nested-name=\"Y00T00041.Y00A00477\" style=\"position:absolute;left:366px;width:64px;height:24px;color:#000000;background-color:#e4e2e0;font-style:italic; \" data-comos-change=\"OnSelect(event,0)\"><option value=\"J40.AA100\" selected=\"selected\">&#176;C</option><option value=\"J40.AA110\">K</option><option value=\"J40.BA100\">&#176;F</option><option value=\"J40.BA110\">&#176;R</option></select></nobr></div><div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:472px;width:433px;height:25px'><nobr><span unselectable=\"on\" id=\"Edit7_6\" nested-name=\"Y00T00041.Y00A01011\" style=\"vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; \">Permitted pressure drop</span><input type=\"text\" value = \"\" id=\"Edit7_0\" nested-name=\"Y00T00041.Y00A01011\" class=\"SUIText\" readonly=\"readonly\" maxlength=\"2000\" data-ng-keydown=\"OnKeyDown($event)\" style=\"position:absolute;left:144px;width:216px;height:18px;background-color:#e4e2e0;color:#000000;\"/><select id=\"Edit7_100\" nested-name=\"Y00T00041.Y00A01011\" style=\"position:absolute;left:368px;width:64px;height:24px;color:#000000;background-color:#e4e2e0;font-style:italic; \" data-comos-change=\"OnSelect(event,9)\"><option value=\"E80.AA100\">GPa</option><option value=\"E80.AA110\">kbar</option><option value=\"E80.AA120\">hbar</option><option value=\"E80.AA130\">kgf/mm&#178;</option><option value=\"E80.AA140\">MJ/m&#179;</option><option value=\"E80.AA150\">MPa</option><option value=\"E80.AA160\">MPa&#160;(g)</option><option value=\"E80.AA170\">N/mm&#178;</option><option value=\"E80.AA180\">atm</option><option value=\"E80.AA190\" selected=\"selected\">bar</option><option value=\"E80.AA200\">at</option><option value=\"E80.AA210\">N/cm&#178;</option><option value=\"E80.AA220\">mH₂O</option><option value=\"E80.AA230\">cmHg</option><option value=\"E80.AA240\">cmHg&#160;(0&#160;&#176;C)</option><option value=\"E80.AA250\">kPa</option><option value=\"E80.AA260\">mmHg</option><option value=\"E80.AA270\">Torr</option><option value=\"E80.AA280\">hPa</option><option value=\"E80.AA290\">mbar</option><option value=\"E80.AA300\">mbar&#160;(g)</option><option value=\"E80.AA310\">cmH₂O</option><option value=\"E80.AA320\">cmH₂O&#160;(4&#160;&#176;C)</option><option value=\"E80.AA330\">daPa</option><option value=\"E80.AA340\">mmH₂O</option><option value=\"E80.AA350\">mWS</option><option value=\"E80.AA360\">J/m&#179;</option><option value=\"E80.AA370\">kg/(cm&#183;s&#178;)</option><option value=\"E80.AA380\">N/m&#178;</option><option value=\"E80.AA390\">Pa</option><option value=\"E80.AA400\">kg/(m&#183;s&#178;)</option><option value=\"E80.AA410\">&#181;bar</option><option value=\"E80.AA420\">mPa</option><option value=\"E80.AA430\">&#181;Pa</option><option value=\"E80.BA100\">klbf/in&#178;</option><option value=\"E80.BA110\">ksi</option><option value=\"E80.BA120\">bar&#160;(g)</option><option value=\"E80.BA130\">at&#160;(g)</option><option value=\"E80.BA140\">kgf/cm&#178;</option><option value=\"E80.BA150\">kgf/cm&#178;&#160;(g)</option><option value=\"E80.BA160\">ftHg</option><option value=\"E80.BA170\">psi&#160;(a)</option><option value=\"E80.BA180\">psi&#160;(g)</option><option value=\"E80.BA190\">inHg</option><option value=\"E80.BA200\">inHg&#160;(32&#160;&#176;F)</option><option value=\"E80.BA210\">inHg&#160;(60&#160;&#176;F)</option><option value=\"E80.BA220\">ftH₂O</option><option value=\"E80.BA230\">ftH₂O&#160;(39.2&#160;&#176;F)</option><option value=\"E80.BA240\">kPa&#160;(g)</option><option value=\"E80.BA250\">inH₂O</option><option value=\"E80.BA260\">inH₂O&#160;(39.2&#160;&#176;F)</option><option value=\"E80.BA270\">inH₂O&#160;(60&#160;&#176;F)</option><option value=\"E80.BA280\">pdl/in&#178;</option><option value=\"E80.BA290\">gf/cm&#178;</option><option value=\"E80.BA300\">lbf/ft&#178;</option><option value=\"E80.BA310\">kgf/m&#178;</option><option value=\"E80.BA320\">pdl/ft&#178;</option><option value=\"E80.BA330\">lb/(ft&#183;s&#178;)</option><option value=\"E80.BA340\">dyn/cm&#178;</option><option value=\"E80.BA350\">erg/cm&#179;</option></select></nobr></div><div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:280px;width:433px;height:25px'><nobr><span unselectable=\"on\" id=\"Edit8_6\" nested-name=\"Y00T00041.Y00A01012\" style=\"vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; \">Density</span><input type=\"text\" value = \"\" id=\"Edit8_0\" nested-name=\"Y00T00041.Y00A01012\" class=\"SUIText\" readonly=\"readonly\" maxlength=\"2000\" data-ng-keydown=\"OnKeyDown($event)\" style=\"position:absolute;left:144px;width:216px;height:18px;background-color:#e4e2e0;color:#000000;\"/><select id=\"Edit8_100\" nested-name=\"Y00T00041.Y00A01012\" style=\"position:absolute;left:368px;width:64px;height:24px;color:#000000;background-color:#e4e2e0;font-style:italic; \" data-comos-change=\"OnSelect(event,9)\"><option value=\"B70.AA100\">kg/cm&#179;</option><option value=\"B70.AA110\">g/cm&#179;</option><option value=\"B70.AA120\">g/ml</option><option value=\"B70.AA130\">kg/dm&#179;</option><option value=\"B70.AA140\">kg/l</option><option value=\"B70.AA150\">Mg/m&#179;</option><option value=\"B70.AA160\">t/m&#179;</option><option value=\"B70.AA170\">g/dm&#179;</option><option value=\"B70.AA180\">g/l</option><option value=\"B70.AA190\" selected=\"selected\">kg/m&#179;</option><option value=\"B70.AA200\">g/m&#179;</option><option value=\"B70.AA210\">mg/l</option><option value=\"B70.AA220\">&#181;g/l</option><option value=\"B70.AA230\">mg/m&#179;</option><option value=\"B70.AA240\">&#181;g/m&#179;</option><option value=\"B70.BA100\">lb/in&#179;</option><option value=\"B70.BA110\">oz/in&#179;</option><option value=\"B70.BA120\">ton.l/yd&#179;&#160;(UK)</option><option value=\"B70.BA130\">ton.s/yd&#179;&#160;(US)</option><option value=\"B70.BA140\">slug/ft&#179;</option><option value=\"B70.BA150\">lb/gal&#160;(US&#160;liq.)</option><option value=\"B70.BA160\">lb/gal&#160;(US)</option><option value=\"B70.BA170\">lb/gal&#160;(UK)</option><option value=\"B70.BA180\">lb/ft&#179;</option><option value=\"B70.BA190\">oz/gal&#160;(US)</option><option value=\"B70.BA200\">oz/gal&#160;(UK)</option><option value=\"B70.BA220\">lb/yd&#179;</option><option value=\"B70.BA230\">oz/yd&#179;</option><option value=\"B70.BA240\">gr/gal&#160;(US)</option></select></nobr></div><div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:304px;width:433px;height:25px'><nobr><span unselectable=\"on\" id=\"Edit9_6\" nested-name=\"Y00T00041.Y00A01028\" style=\"vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; \">Dynamic viscosity</span><input type=\"text\" value = \"\" id=\"Edit9_0\" nested-name=\"Y00T00041.Y00A01028\" class=\"SUIText\" readonly=\"readonly\" maxlength=\"2000\" data-ng-keydown=\"OnKeyDown($event)\" style=\"position:absolute;left:144px;width:216px;height:18px;background-color:#e4e2e0;color:#000000;\"/><select id=\"Edit9_100\" nested-name=\"Y00T00041.Y00A01028\" style=\"position:absolute;left:368px;width:64px;height:24px;color:#000000;background-color:#e4e2e0;font-style:italic; \" data-comos-change=\"OnSelect(event,4)\"><option value=\"B85.AA100\">(N&#183;s)/m&#178;</option><option value=\"B85.AA110\">Pa&#183;s</option><option value=\"B85.AA120\">kg/(m&#183;s)</option><option value=\"B85.AA130\">g/(cm&#183;s)</option><option value=\"B85.AA140\" selected=\"selected\">mPa&#183;s</option><option value=\"B85.AA150\">kg/(m&#183;h)</option><option value=\"B85.AA160\">kg/(m&#183;min)</option><option value=\"B85.AA170\">kg/(m&#183;day)</option><option value=\"B85.BA100\">lbf&#183;s/in&#178;</option><option value=\"B85.BA110\">(pdl&#183;s)/in&#178;</option><option value=\"B85.BA120\">lbf&#183;s/ft&#178;</option><option value=\"B85.BA130\">slug/(ft&#183;s)</option><option value=\"B85.BA140\">(pdl&#183;s)/ft&#178;</option><option value=\"B85.BA150\">lb/(ft&#183;s)</option><option value=\"B85.BA160\">P</option><option value=\"B85.BA170\">Cp</option><option value=\"B85.BA180\">&#181;P</option><option value=\"B85.BA190\">lb/(ft&#183;h)</option><option value=\"B85.BA200\">lb/(ft&#183;min)</option><option value=\"B85.BA210\">lb/(ft&#183;day)</option></select></nobr></div><div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:160px;width:433px;height:25px'><nobr><span unselectable=\"on\" id=\"Edit10_6\" nested-name=\"Y00T00041.Y00A01036\" style=\"vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; \">Operating temp.</span><input type=\"text\" value = \"\" id=\"Edit10_1\" nested-name=\"Y00T00041.Y00A01036\" class=\"SUIText\" readonly=\"readonly\" maxlength=\"2000\" data-ng-keydown=\"OnKeyDown($event)\" style=\"position:absolute;left:144px;width:66px;height:18px;background-color:#e4e2e0;color:#000000;\"/><input type=\"text\" value = \"\" id=\"Edit10_0\" nested-name=\"Y00T00041.Y00A01036\" class=\"SUIText\" readonly=\"readonly\" maxlength=\"2000\" data-ng-keydown=\"OnKeyDown($event)\" style=\"position:absolute;left:218px;width:66px;height:18px;background-color:#e4e2e0;color:#000000;\"/><input type=\"text\" value = \"\" id=\"Edit10_2\" nested-name=\"Y00T00041.Y00A01036\" class=\"SUIText\" readonly=\"readonly\" maxlength=\"2000\" data-ng-keydown=\"OnKeyDown($event)\" style=\"position:absolute;left:292px;width:66px;height:18px;background-color:#e4e2e0;color:#000000;\"/><select id=\"Edit10_100\" nested-name=\"Y00T00041.Y00A01036\" style=\"position:absolute;left:366px;width:64px;height:24px;color:#000000;background-color:#e4e2e0;font-style:italic; \" data-comos-change=\"OnSelect(event,0)\"><option value=\"J40.AA100\" selected=\"selected\">&#176;C</option><option value=\"J40.AA110\">K</option><option value=\"J40.BA100\">&#176;F</option><option value=\"J40.BA110\">&#176;R</option></select></nobr></div><div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:232px;width:433px;height:25px'><nobr><span unselectable=\"on\" id=\"Edit11_6\" nested-name=\"Y00T00041.Y00A01143\" style=\"vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; \">Volume flow</span><input type=\"text\" value = \"\" id=\"Edit11_1\" nested-name=\"Y00T00041.Y00A01143\" class=\"SUIText\" readonly=\"readonly\" maxlength=\"2000\" data-ng-keydown=\"OnKeyDown($event)\" style=\"position:absolute;left:144px;width:66px;height:18px;background-color:#e4e2e0;color:#000000;\"/><input type=\"text\" value = \"\" id=\"Edit11_0\" nested-name=\"Y00T00041.Y00A01143\" class=\"SUIText\" readonly=\"readonly\" maxlength=\"2000\" data-ng-keydown=\"OnKeyDown($event)\" style=\"position:absolute;left:218px;width:66px;height:18px;background-color:#e4e2e0;color:#000000;\"/><input type=\"text\" value = \"\" id=\"Edit11_2\" nested-name=\"Y00T00041.Y00A01143\" class=\"SUIText\" readonly=\"readonly\" maxlength=\"2000\" data-ng-keydown=\"OnKeyDown($event)\" style=\"position:absolute;left:292px;width:66px;height:18px;background-color:#e4e2e0;color:#000000;\"/><select id=\"Edit11_100\" nested-name=\"Y00T00041.Y00A01143\" style=\"position:absolute;left:366px;width:64px;height:24px;color:#000000;background-color:#e4e2e0;font-style:italic; \" data-comos-change=\"OnSelect(event,6)\"><option value=\"K05.AA100\">m&#179;/s</option><option value=\"K05.AA110\">dm&#179;/s</option><option value=\"K05.AA120\">l/s</option><option value=\"K05.AA130\">cm&#179;/s</option><option value=\"K05.AA140\">ml/s</option><option value=\"K05.AA150\">kl/h</option><option value=\"K05.AA160\" selected=\"selected\">m&#179;/h</option><option value=\"K05.AA170\">dm&#179;/h</option><option value=\"K05.AA180\">l/h</option><option value=\"K05.AA190\">cm&#179;/h</option><option value=\"K05.AA200\">ml/h</option><option value=\"K05.AA210\">m&#179;/min</option><option value=\"K05.AA220\">dm&#179;/min</option><option value=\"K05.AA230\">l/min</option><option value=\"K05.AA240\">cm&#179;/min</option><option value=\"K05.AA250\">ml/min</option><option value=\"K05.AA260\">m&#179;/day</option><option value=\"K05.AA270\">dm&#179;/day</option><option value=\"K05.AA280\">l/day</option><option value=\"K05.AA290\">cm&#179;/day</option><option value=\"K05.AA300\">ml/day</option><option value=\"K05.BA100\">yd&#179;/s</option><option value=\"K05.BA110\">bu&#160;(US&#160;dry)/h</option><option value=\"K05.BA120\">pt&#160;(UK)/min</option><option value=\"K05.BA130\">qt&#160;(US&#160;liq.)/s</option><option value=\"K05.BA140\">pk&#160;(UK)/s</option><option value=\"K05.BA150\">pk&#160;(US&#160;dry)/s</option><option value=\"K05.BA160\">yd&#179;/day</option><option value=\"K05.BA170\">fl&#160;oz&#160;(US)/h</option><option value=\"K05.BA180\">fl&#160;oz&#160;(UK)/h</option><option value=\"K05.BA190\">pt&#160;(US&#160;liq.)/min</option><option value=\"K05.BA200\">ft&#179;/h</option><option value=\"K05.BA210\">gal&#160;(UK)/min</option><option value=\"K05.BA220\">pt&#160;(UK)/day</option><option value=\"K05.BA230\">gal&#160;(US&#160;liq.)/min</option><option value=\"K05.BA240\">bu&#160;(UK)/min</option><option value=\"K05.BA250\">bu&#160;(US&#160;dry)/min</option><option value=\"K05.BA260\">pt&#160;(UK)/s</option><option value=\"K05.BA270\">pt&#160;(US&#160;liq.)/day</option><option value=\"K05.BA280\">gal&#160;(UK)/day</option><option value=\"K05.BA290\">fl&#160;oz&#160;(US)/min</option><option value=\"K05.BA300\">fl&#160;oz&#160;(UK)/min</option><option value=\"K05.BA310\">pt&#160;(US&#160;liq.)/s</option><option value=\"K05.BA320\">ft&#179;/min</option><option value=\"K05.BA330\">SCFM</option><option value=\"K05.BA340\">gal&#160;(UK)/s</option><option value=\"K05.BA350\">in&#179;/h</option><option value=\"K05.BA360\">bbl&#160;(UK&#160;liq.)/h</option><option value=\"K05.BA370\">bbl&#160;(US)/h</option><option value=\"K05.BA380\">bl/h</option><option value=\"K05.BA390\">gal&#160;(US&#160;liq.)/day</option><option value=\"K05.BA400\">bu&#160;(UK)/day</option><option value=\"K05.BA410\">bu&#160;(US&#160;dry)/day</option><option value=\"K05.BA420\">gi&#160;(UK)/h</option><option value=\"K05.BA430\">gal&#160;(US&#160;liq.)/s</option><option value=\"K05.BA440\">bu&#160;(UK)/s</option><option value=\"K05.BA450\">bu&#160;(US&#160;dry)/s</option><option value=\"K05.BA460\">fl&#160;oz&#160;(US)/day</option><option value=\"K05.BA470\">fl&#160;oz&#160;(UK)/day</option><option value=\"K05.BA480\">gi&#160;(US)/h</option><option value=\"K05.BA490\">ft&#179;/day</option><option value=\"K05.BA500\">qt&#160;(UK&#160;liq.)/h</option><option value=\"K05.BA510\">fl&#160;oz&#160;(US)/s</option><option value=\"K05.BA520\">ft&#179;/s</option><option value=\"K05.BA530\">fl&#160;oz&#160;(UK)/s</option><option value=\"K05.BA540\">in&#179;/min</option><option value=\"K05.BA550\">bbl&#160;(UK&#160;liq.)/min</option><option value=\"K05.BA560\">bbl&#160;(US)/min</option><option value=\"K05.BA570\">qt&#160;(US&#160;liq.)/h</option><option value=\"K05.BA580\">pk&#160;(UK)/h</option><option value=\"K05.BA590\">pk&#160;(US&#160;dry)/h</option><option value=\"K05.BA600\">gi&#160;(UK)/min</option><option value=\"K05.BA610\">yd&#179;/h</option><option value=\"K05.BA620\">gi&#160;(US)/min</option><option value=\"K05.BA630\">qt&#160;(UK&#160;liq.)/min</option><option value=\"K05.BA640\">bbl&#160;(UK&#160;liq.)/day</option><option value=\"K05.BA650\">bbl&#160;(US)/day</option><option value=\"K05.BA660\">bbl&#160;(UK&#160;liq.)/s</option><option value=\"K05.BA670\">bbl&#160;(US)/s</option><option value=\"K05.BA680\">gi&#160;(UK)/day</option><option value=\"K05.BA690\">in&#179;/s</option><option value=\"K05.BA700\">pt&#160;(UK)/h</option><option value=\"K05.BA710\">qt&#160;(US&#160;liq.)/min</option><option value=\"K05.BA720\">pk&#160;(UK)/min</option><option value=\"K05.BA730\">pk&#160;(US&#160;dry)/min</option><option value=\"K05.BA740\">gi&#160;(UK)/s</option><option value=\"K05.BA750\">gi&#160;(US)/day</option><option value=\"K05.BA760\">qt&#160;(UK&#160;liq.)/day</option><option value=\"K05.BA770\">pt&#160;(US&#160;liq.)/h</option><option value=\"K05.BA780\">yd&#179;/min</option><option value=\"K05.BA790\">gal&#160;(UK)/h</option><option value=\"K05.BA800\">gi&#160;(US)/s</option><option value=\"K05.BA810\">qt&#160;(UK&#160;liq.)/s</option><option value=\"K05.BA820\">qt&#160;(US&#160;liq.)/day</option><option value=\"K05.BA830\">pk&#160;(UK)/day</option><option value=\"K05.BA840\">gal&#160;(US&#160;liq.)/h</option><option value=\"K05.BA850\">pk&#160;(US&#160;dry)/day</option><option value=\"K05.BA860\">bu&#160;(UK)/h</option></select></nobr></div><div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:64px;width:433px;height:25px'><nobr><span unselectable=\"on\" id=\"Edit12_6\" nested-name=\"Y00T00041.Y00A01157\" style=\"vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; \">Medium</span><select id=\"Edit12_0\" nested-name=\"Y00T00041.Y00A01157\" style=\"position:absolute;left:144px;width:288px;height:24px;color:#000000;background-color:#e4e2e0;font-style:italic; \" data-comos-change=\"OnSelect(event,0)\"><option value=\"\"></option><option value=\"None\" selected=\"selected\">None</option><option value=\"Acetylene\">Acetylene</option><option value=\"Water / steam\">Water&nbsp;/&nbsp;steam</option><option value=\"Ammonium\">Ammonium</option><option value=\"n-butane\">n-butane</option><option value=\"Chlorine\">Chlorine</option><option value=\"Hydrogen chloride\">Hydrogen&nbsp;chloride</option><option value=\"Dichloromomofluoromethane\">Dichloromomofluoromethane</option><option value=\"Ethane\">Ethane</option><option value=\"Ethylene\">Ethylene</option><option value=\"Helium-4\">Helium-4</option><option value=\"Carbon dioxyde\">Carbon&nbsp;dioxyde</option><option value=\"Krypton\">Krypton</option><option value=\"Air (dry)\">Air&nbsp;(dry)</option><option value=\"Methane\">Methane</option><option value=\"Neon\">Neon</option><option value=\"Pentane\">Pentane</option><option value=\"Phosgene\">Phosgene</option><option value=\"Propane\">Propane</option><option value=\"Propylene\">Propylene</option><option value=\"Salicyl acid\">Salicyl&nbsp;acid</option><option value=\"Nitric acid\">Nitric&nbsp;acid</option><option value=\"Oxygen\">Oxygen</option><option value=\"Sulphuric dioxide\">Sulphuric&nbsp;dioxide</option><option value=\"Hydrogen sulphide\">Hydrogen&nbsp;sulphide</option><option value=\"Nitrogen\">Nitrogen</option><option value=\"Nitrogen monoxide\">Nitrogen&nbsp;monoxide</option></select></nobr></div><div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:376px;width:433px;height:25px'><nobr><span unselectable=\"on\" id=\"Edit13_6\" nested-name=\"Y00T00041.Y00A01182\" style=\"vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; \">Mass flow</span><input type=\"text\" value = \"\" id=\"Edit13_0\" nested-name=\"Y00T00041.Y00A01182\" class=\"SUIText\" readonly=\"readonly\" maxlength=\"2000\" data-ng-keydown=\"OnKeyDown($event)\" style=\"position:absolute;left:144px;width:216px;height:18px;background-color:#e4e2e0;color:#000000;\"/><select id=\"Edit13_100\" nested-name=\"Y00T00041.Y00A01182\" style=\"position:absolute;left:368px;width:64px;height:24px;color:#000000;background-color:#e4e2e0;font-style:italic; \" data-comos-change=\"OnSelect(event,8)\"><option value=\"E30.AA100\">t/s</option><option value=\"E30.AA110\">t/min</option><option value=\"E30.AA120\">kg/s</option><option value=\"E30.AA130\">g/s</option><option value=\"E30.AA140\">mg/s</option><option value=\"E30.AA150\">t/mo</option><option value=\"E30.AA160\">t/h</option><option value=\"E30.AA170\">t/year</option><option value=\"E30.AA180\" selected=\"selected\">kg/h</option><option value=\"E30.AA190\">g/h</option><option value=\"E30.AA200\">mg/h</option><option value=\"E30.AA210\">kg/min</option><option value=\"E30.AA220\">g/min</option><option value=\"E30.AA230\">mg/min</option><option value=\"E30.AA240\">t/day</option><option value=\"E30.AA250\">kg/day</option><option value=\"E30.AA260\">g/day</option><option value=\"E30.AA270\">mg/day</option><option value=\"E30.BA100\">slug/s</option><option value=\"E30.BA110\">oz/h</option><option value=\"E30.BA120\">lb/min</option><option value=\"E30.BA130\">lb/day</option><option value=\"E30.BA140\">lb/s</option><option value=\"E30.BA150\">oz/min</option><option value=\"E30.BA160\">slug/h</option><option value=\"E30.BA170\">ton.s&#160;(US)/h</option><option value=\"E30.BA180\">oz/day</option><option value=\"E30.BA190\">oz/s</option><option value=\"E30.BA200\">slug/min</option><option value=\"E30.BA210\">slug/d</option><option value=\"E30.BA220\">klb/h</option><option value=\"E30.BA230\">lb/h</option><option value=\"E30.BA240\">Ton&#160;(UK)/day</option><option value=\"E30.BA250\">Ton&#160;(US)/day</option></select></nobr></div><div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:184px;width:433px;height:25px'><nobr><span unselectable=\"on\" id=\"Edit14_6\" nested-name=\"Y00T00041.Y00A01677\" style=\"vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; \">Operating pressure</span><input type=\"text\" value = \"\" id=\"Edit14_1\" nested-name=\"Y00T00041.Y00A01677\" class=\"SUIText\" readonly=\"readonly\" maxlength=\"2000\" data-ng-keydown=\"OnKeyDown($event)\" style=\"position:absolute;left:144px;width:66px;height:18px;background-color:#e4e2e0;color:#000000;\"/><input type=\"text\" value = \"\" id=\"Edit14_0\" nested-name=\"Y00T00041.Y00A01677\" class=\"SUIText\" readonly=\"readonly\" maxlength=\"2000\" data-ng-keydown=\"OnKeyDown($event)\" style=\"position:absolute;left:218px;width:66px;height:18px;background-color:#e4e2e0;color:#000000;\"/><input type=\"text\" value = \"\" id=\"Edit14_2\" nested-name=\"Y00T00041.Y00A01677\" class=\"SUIText\" readonly=\"readonly\" maxlength=\"2000\" data-ng-keydown=\"OnKeyDown($event)\" style=\"position:absolute;left:292px;width:66px;height:18px;background-color:#e4e2e0;color:#000000;\"/><select id=\"Edit14_100\" nested-name=\"Y00T00041.Y00A01677\" style=\"position:absolute;left:366px;width:64px;height:24px;color:#000000;background-color:#e4e2e0;font-style:italic; \" data-comos-change=\"OnSelect(event,9)\"><option value=\"E80.AA100\">GPa</option><option value=\"E80.AA110\">kbar</option><option value=\"E80.AA120\">hbar</option><option value=\"E80.AA130\">kgf/mm&#178;</option><option value=\"E80.AA140\">MJ/m&#179;</option><option value=\"E80.AA150\">MPa</option><option value=\"E80.AA160\">MPa&#160;(g)</option><option value=\"E80.AA170\">N/mm&#178;</option><option value=\"E80.AA180\">atm</option><option value=\"E80.AA190\" selected=\"selected\">bar</option><option value=\"E80.AA200\">at</option><option value=\"E80.AA210\">N/cm&#178;</option><option value=\"E80.AA220\">mH₂O</option><option value=\"E80.AA230\">cmHg</option><option value=\"E80.AA240\">cmHg&#160;(0&#160;&#176;C)</option><option value=\"E80.AA250\">kPa</option><option value=\"E80.AA260\">mmHg</option><option value=\"E80.AA270\">Torr</option><option value=\"E80.AA280\">hPa</option><option value=\"E80.AA290\">mbar</option><option value=\"E80.AA300\">mbar&#160;(g)</option><option value=\"E80.AA310\">cmH₂O</option><option value=\"E80.AA320\">cmH₂O&#160;(4&#160;&#176;C)</option><option value=\"E80.AA330\">daPa</option><option value=\"E80.AA340\">mmH₂O</option><option value=\"E80.AA350\">mWS</option><option value=\"E80.AA360\">J/m&#179;</option><option value=\"E80.AA370\">kg/(cm&#183;s&#178;)</option><option value=\"E80.AA380\">N/m&#178;</option><option value=\"E80.AA390\">Pa</option><option value=\"E80.AA400\">kg/(m&#183;s&#178;)</option><option value=\"E80.AA410\">&#181;bar</option><option value=\"E80.AA420\">mPa</option><option value=\"E80.AA430\">&#181;Pa</option><option value=\"E80.BA100\">klbf/in&#178;</option><option value=\"E80.BA110\">ksi</option><option value=\"E80.BA120\">bar&#160;(g)</option><option value=\"E80.BA130\">at&#160;(g)</option><option value=\"E80.BA140\">kgf/cm&#178;</option><option value=\"E80.BA150\">kgf/cm&#178;&#160;(g)</option><option value=\"E80.BA160\">ftHg</option><option value=\"E80.BA170\">psi&#160;(a)</option><option value=\"E80.BA180\">psi&#160;(g)</option><option value=\"E80.BA190\">inHg</option><option value=\"E80.BA200\">inHg&#160;(32&#160;&#176;F)</option><option value=\"E80.BA210\">inHg&#160;(60&#160;&#176;F)</option><option value=\"E80.BA220\">ftH₂O</option><option value=\"E80.BA230\">ftH₂O&#160;(39.2&#160;&#176;F)</option><option value=\"E80.BA240\">kPa&#160;(g)</option><option value=\"E80.BA250\">inH₂O</option><option value=\"E80.BA260\">inH₂O&#160;(39.2&#160;&#176;F)</option><option value=\"E80.BA270\">inH₂O&#160;(60&#160;&#176;F)</option><option value=\"E80.BA280\">pdl/in&#178;</option><option value=\"E80.BA290\">gf/cm&#178;</option><option value=\"E80.BA300\">lbf/ft&#178;</option><option value=\"E80.BA310\">kgf/m&#178;</option><option value=\"E80.BA320\">pdl/ft&#178;</option><option value=\"E80.BA330\">lb/(ft&#183;s&#178;)</option><option value=\"E80.BA340\">dyn/cm&#178;</option><option value=\"E80.BA350\">erg/cm&#179;</option></select></nobr></div><div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:256px;width:433px;height:25px'><nobr><span unselectable=\"on\" id=\"Edit15_6\" nested-name=\"Y00T00041.Y00A01749\" style=\"vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; \">Solids content</span><input type=\"text\" value = \"\" id=\"Edit15_0\" nested-name=\"Y00T00041.Y00A01749\" class=\"SUIText\" readonly=\"readonly\" maxlength=\"2000\" data-ng-keydown=\"OnKeyDown($event)\" style=\"position:absolute;left:144px;width:216px;height:18px;background-color:#e4e2e0;color:#000000;\"/><select id=\"Edit15_100\" nested-name=\"Y00T00041.Y00A01749\" style=\"position:absolute;left:368px;width:64px;height:24px;color:#000000;background-color:#e4e2e0;font-style:italic; \" data-comos-change=\"OnSelect(event,2)\"><option value=\"F25.AA100\">ppm</option><option value=\"F25.AA110\">1/1</option><option value=\"F25.AA120\" selected=\"selected\">%</option><option value=\"F25.AA130\">‰</option></select></nobr></div><div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:400px;width:433px;height:25px'><nobr><span unselectable=\"on\" id=\"Edit16_6\" nested-name=\"Y00T00041.Y00A01947\" style=\"vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; \">Molar mass</span><input type=\"text\" value = \"\" id=\"Edit16_0\" nested-name=\"Y00T00041.Y00A01947\" class=\"SUIText\" readonly=\"readonly\" maxlength=\"2000\" data-ng-keydown=\"OnKeyDown($event)\" style=\"position:absolute;left:144px;width:216px;height:18px;background-color:#e4e2e0;color:#000000;\"/><select id=\"Edit16_100\" nested-name=\"Y00T00041.Y00A01947\" style=\"position:absolute;left:368px;width:64px;height:24px;color:#000000;background-color:#e4e2e0;font-style:italic; \" data-comos-change=\"OnSelect(event,0)\"><option value=\"E90.AA100\" selected=\"selected\">kg/mol</option><option value=\"E90.AA110\">g/mol</option><option value=\"E90.AA120\">kg/kmol</option></select></nobr></div><div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:88px;width:433px;height:25px'><nobr><span unselectable=\"on\" id=\"Edit17_6\" nested-name=\"Y00T00041.Y00A02610\" style=\"vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; \">Aggregate state</span><select id=\"Edit17_0\" nested-name=\"Y00T00041.Y00A02610\" style=\"position:absolute;left:144px;width:288px;height:24px;color:#000000;background-color:#e4e2e0;font-style:italic; \" data-comos-change=\"OnSelect(event,0)\"><option value=\"\"></option><option value=\"None\" selected=\"selected\">None</option><option value=\"Solid\">Solid</option><option value=\"Liquid\">Liquid</option><option value=\"Gaseous\">Gaseous</option></select></nobr></div><div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:616px;width:433px;height:81px'><nobr><span unselectable=\"on\" id=\"Memo18_7\" nested-name=\"Y00T00041.Y00A04689\" style=\"vertical-align:middle;position:absolute;left:0px;top:0px;width:80px;height:80px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; \">Comment</span><textarea id=\"Memo18_0\" nested-name=\"Y00T00041.Y00A04689\" class=\"SUITextArea\" style=\"position:absolute;resize:none;left:82px; width:320px; height:74px;overflow:hidden;height:74px;background-color:#e4e2e0;color:#000000;\" data-comos-change=\"OnChange(event)\" readonly=\"readonly\"></textarea><button class=\"SUIOpenButton\" id=\"Memo18_8\" nested-name=\"Y00T00041.Y00A04689\" style=\"position:absolute;left: 410px;width:21px;height:21px\" data-ng-click=\"OnClick($event)\"></button></nobr></div><div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:448px;width:433px;height:25px'><nobr><span unselectable=\"on\" id=\"Edit19_6\" nested-name=\"Y00T00041.Y00A04751\" style=\"vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; \">Total counter pressure</span><input type=\"text\" value = \"\" id=\"Edit19_0\" nested-name=\"Y00T00041.Y00A04751\" class=\"SUIText\" readonly=\"readonly\" maxlength=\"2000\" data-ng-keydown=\"OnKeyDown($event)\" style=\"position:absolute;left:144px;width:216px;height:18px;background-color:#e4e2e0;color:#000000;\"/><select id=\"Edit19_100\" nested-name=\"Y00T00041.Y00A04751\" style=\"position:absolute;left:368px;width:64px;height:24px;color:#000000;background-color:#e4e2e0;font-style:italic; \" data-comos-change=\"OnSelect(event,9)\"><option value=\"E80.AA100\">GPa</option><option value=\"E80.AA110\">kbar</option><option value=\"E80.AA120\">hbar</option><option value=\"E80.AA130\">kgf/mm&#178;</option><option value=\"E80.AA140\">MJ/m&#179;</option><option value=\"E80.AA150\">MPa</option><option value=\"E80.AA160\">MPa&#160;(g)</option><option value=\"E80.AA170\">N/mm&#178;</option><option value=\"E80.AA180\">atm</option><option value=\"E80.AA190\" selected=\"selected\">bar</option><option value=\"E80.AA200\">at</option><option value=\"E80.AA210\">N/cm&#178;</option><option value=\"E80.AA220\">mH₂O</option><option value=\"E80.AA230\">cmHg</option><option value=\"E80.AA240\">cmHg&#160;(0&#160;&#176;C)</option><option value=\"E80.AA250\">kPa</option><option value=\"E80.AA260\">mmHg</option><option value=\"E80.AA270\">Torr</option><option value=\"E80.AA280\">hPa</option><option value=\"E80.AA290\">mbar</option><option value=\"E80.AA300\">mbar&#160;(g)</option><option value=\"E80.AA310\">cmH₂O</option><option value=\"E80.AA320\">cmH₂O&#160;(4&#160;&#176;C)</option><option value=\"E80.AA330\">daPa</option><option value=\"E80.AA340\">mmH₂O</option><option value=\"E80.AA350\">mWS</option><option value=\"E80.AA360\">J/m&#179;</option><option value=\"E80.AA370\">kg/(cm&#183;s&#178;)</option><option value=\"E80.AA380\">N/m&#178;</option><option value=\"E80.AA390\">Pa</option><option value=\"E80.AA400\">kg/(m&#183;s&#178;)</option><option value=\"E80.AA410\">&#181;bar</option><option value=\"E80.AA420\">mPa</option><option value=\"E80.AA430\">&#181;Pa</option><option value=\"E80.BA100\">klbf/in&#178;</option><option value=\"E80.BA110\">ksi</option><option value=\"E80.BA120\">bar&#160;(g)</option><option value=\"E80.BA130\">at&#160;(g)</option><option value=\"E80.BA140\">kgf/cm&#178;</option><option value=\"E80.BA150\">kgf/cm&#178;&#160;(g)</option><option value=\"E80.BA160\">ftHg</option><option value=\"E80.BA170\">psi&#160;(a)</option><option value=\"E80.BA180\">psi&#160;(g)</option><option value=\"E80.BA190\">inHg</option><option value=\"E80.BA200\">inHg&#160;(32&#160;&#176;F)</option><option value=\"E80.BA210\">inHg&#160;(60&#160;&#176;F)</option><option value=\"E80.BA220\">ftH₂O</option><option value=\"E80.BA230\">ftH₂O&#160;(39.2&#160;&#176;F)</option><option value=\"E80.BA240\">kPa&#160;(g)</option><option value=\"E80.BA250\">inH₂O</option><option value=\"E80.BA260\">inH₂O&#160;(39.2&#160;&#176;F)</option><option value=\"E80.BA270\">inH₂O&#160;(60&#160;&#176;F)</option><option value=\"E80.BA280\">pdl/in&#178;</option><option value=\"E80.BA290\">gf/cm&#178;</option><option value=\"E80.BA300\">lbf/ft&#178;</option><option value=\"E80.BA310\">kgf/m&#178;</option><option value=\"E80.BA320\">pdl/ft&#178;</option><option value=\"E80.BA330\">lb/(ft&#183;s&#178;)</option><option value=\"E80.BA340\">dyn/cm&#178;</option><option value=\"E80.BA350\">erg/cm&#179;</option></select></nobr></div><div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:520px;width:433px;height:25px'><nobr><span unselectable=\"on\" id=\"Edit20_6\" nested-name=\"Y00T00041.Y00A04753\" style=\"vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; \">Cause of overpressure</span><input type=\"text\" value = \"\" id=\"Edit20_0\" nested-name=\"Y00T00041.Y00A04753\" class=\"SUIText\" readonly=\"readonly\" maxlength=\"2000\" data-ng-keydown=\"OnKeyDown($event)\" style=\"position:absolute;left:144px;width:280px;height:18px;background-color:#e4e2e0;color:#000000;\"/></nobr></div><div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:424px;width:433px;height:25px'><nobr><span unselectable=\"on\" id=\"Edit21_6\" nested-name=\"Y00T00041.Y00A04762\" style=\"vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; \">Set pressure</span><input type=\"text\" value = \"\" id=\"Edit21_0\" nested-name=\"Y00T00041.Y00A04762\" class=\"SUIText\" readonly=\"readonly\" maxlength=\"2000\" data-ng-keydown=\"OnKeyDown($event)\" style=\"position:absolute;left:144px;width:216px;height:18px;background-color:#e4e2e0;color:#000000;\"/><select id=\"Edit21_100\" nested-name=\"Y00T00041.Y00A04762\" style=\"position:absolute;left:368px;width:64px;height:24px;color:#000000;background-color:#e4e2e0;font-style:italic; \" data-comos-change=\"OnSelect(event,9)\"><option value=\"E80.AA100\">GPa</option><option value=\"E80.AA110\">kbar</option><option value=\"E80.AA120\">hbar</option><option value=\"E80.AA130\">kgf/mm&#178;</option><option value=\"E80.AA140\">MJ/m&#179;</option><option value=\"E80.AA150\">MPa</option><option value=\"E80.AA160\">MPa&#160;(g)</option><option value=\"E80.AA170\">N/mm&#178;</option><option value=\"E80.AA180\">atm</option><option value=\"E80.AA190\" selected=\"selected\">bar</option><option value=\"E80.AA200\">at</option><option value=\"E80.AA210\">N/cm&#178;</option><option value=\"E80.AA220\">mH₂O</option><option value=\"E80.AA230\">cmHg</option><option value=\"E80.AA240\">cmHg&#160;(0&#160;&#176;C)</option><option value=\"E80.AA250\">kPa</option><option value=\"E80.AA260\">mmHg</option><option value=\"E80.AA270\">Torr</option><option value=\"E80.AA280\">hPa</option><option value=\"E80.AA290\">mbar</option><option value=\"E80.AA300\">mbar&#160;(g)</option><option value=\"E80.AA310\">cmH₂O</option><option value=\"E80.AA320\">cmH₂O&#160;(4&#160;&#176;C)</option><option value=\"E80.AA330\">daPa</option><option value=\"E80.AA340\">mmH₂O</option><option value=\"E80.AA350\">mWS</option><option value=\"E80.AA360\">J/m&#179;</option><option value=\"E80.AA370\">kg/(cm&#183;s&#178;)</option><option value=\"E80.AA380\">N/m&#178;</option><option value=\"E80.AA390\">Pa</option><option value=\"E80.AA400\">kg/(m&#183;s&#178;)</option><option value=\"E80.AA410\">&#181;bar</option><option value=\"E80.AA420\">mPa</option><option value=\"E80.AA430\">&#181;Pa</option><option value=\"E80.BA100\">klbf/in&#178;</option><option value=\"E80.BA110\">ksi</option><option value=\"E80.BA120\">bar&#160;(g)</option><option value=\"E80.BA130\">at&#160;(g)</option><option value=\"E80.BA140\">kgf/cm&#178;</option><option value=\"E80.BA150\">kgf/cm&#178;&#160;(g)</option><option value=\"E80.BA160\">ftHg</option><option value=\"E80.BA170\">psi&#160;(a)</option><option value=\"E80.BA180\">psi&#160;(g)</option><option value=\"E80.BA190\">inHg</option><option value=\"E80.BA200\">inHg&#160;(32&#160;&#176;F)</option><option value=\"E80.BA210\">inHg&#160;(60&#160;&#176;F)</option><option value=\"E80.BA220\">ftH₂O</option><option value=\"E80.BA230\">ftH₂O&#160;(39.2&#160;&#176;F)</option><option value=\"E80.BA240\">kPa&#160;(g)</option><option value=\"E80.BA250\">inH₂O</option><option value=\"E80.BA260\">inH₂O&#160;(39.2&#160;&#176;F)</option><option value=\"E80.BA270\">inH₂O&#160;(60&#160;&#176;F)</option><option value=\"E80.BA280\">pdl/in&#178;</option><option value=\"E80.BA290\">gf/cm&#178;</option><option value=\"E80.BA300\">lbf/ft&#178;</option><option value=\"E80.BA310\">kgf/m&#178;</option><option value=\"E80.BA320\">pdl/ft&#178;</option><option value=\"E80.BA330\">lb/(ft&#183;s&#178;)</option><option value=\"E80.BA340\">dyn/cm&#178;</option><option value=\"E80.BA350\">erg/cm&#179;</option></select></nobr></div><div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:352px;width:433px;height:25px'><nobr><span unselectable=\"on\" id=\"Edit22_6\" nested-name=\"Y00T00041.Y00A04789\" style=\"vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; \">Abrasive</span><select id=\"Edit22_0\" nested-name=\"Y00T00041.Y00A04789\" style=\"position:absolute;left:144px;width:288px;height:24px;color:#000000;background-color:#e4e2e0;font-style:italic; \" data-comos-change=\"OnSelect(event,1)\"><option value=\"\"></option><option value=\"Yes\">Yes</option><option value=\"No\" selected=\"selected\">No</option></select></nobr></div><div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:328px;width:433px;height:25px'><nobr><span unselectable=\"on\" id=\"Edit23_6\" nested-name=\"Y00T00041.Y00A04793\" style=\"vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; \">Corrosive</span><select id=\"Edit23_0\" nested-name=\"Y00T00041.Y00A04793\" style=\"position:absolute;left:144px;width:288px;height:24px;color:#000000;background-color:#e4e2e0;font-style:italic; \" data-comos-change=\"OnSelect(event,1)\"><option value=\"\"></option><option value=\"Yes\">Yes</option><option value=\"No\" selected=\"selected\">No</option></select></nobr></div><div unselectable='on' style='border: black 0px solid;  z-index:2; position:absolute; left:48px;top:496px;width:433px;height:25px'><nobr><span unselectable=\"on\" id=\"Edit24_6\" nested-name=\"Y00T00041.Y00A04857\" style=\"vertical-align:middle;position:absolute;left:0px;top:0px;width:144px;height:24px;overflow:hidden;color:#000000;font-size: 11px;font-family:MS Sans Serif; \">Vapor pressure factor</span><input type=\"text\" value = \"\" id=\"Edit24_0\" nested-name=\"Y00T00041.Y00A04857\" class=\"SUIText\" readonly=\"readonly\" maxlength=\"2000\" data-ng-keydown=\"OnKeyDown($event)\" style=\"position:absolute;left:144px;width:216px;height:18px;background-color:#e4e2e0;color:#000000;\"/><select id=\"Edit24_100\" nested-name=\"Y00T00041.Y00A04857\" style=\"position:absolute;left:368px;width:64px;height:24px;color:#000000;background-color:#e4e2e0;font-style:italic; \" data-comos-change=\"OnSelect(event,9)\"><option value=\"E80.AA100\">GPa</option><option value=\"E80.AA110\">kbar</option><option value=\"E80.AA120\">hbar</option><option value=\"E80.AA130\">kgf/mm&#178;</option><option value=\"E80.AA140\">MJ/m&#179;</option><option value=\"E80.AA150\">MPa</option><option value=\"E80.AA160\">MPa&#160;(g)</option><option value=\"E80.AA170\">N/mm&#178;</option><option value=\"E80.AA180\">atm</option><option value=\"E80.AA190\" selected=\"selected\">bar</option><option value=\"E80.AA200\">at</option><option value=\"E80.AA210\">N/cm&#178;</option><option value=\"E80.AA220\">mH₂O</option><option value=\"E80.AA230\">cmHg</option><option value=\"E80.AA240\">cmHg&#160;(0&#160;&#176;C)</option><option value=\"E80.AA250\">kPa</option><option value=\"E80.AA260\">mmHg</option><option value=\"E80.AA270\">Torr</option><option value=\"E80.AA280\">hPa</option><option value=\"E80.AA290\">mbar</option><option value=\"E80.AA300\">mbar&#160;(g)</option><option value=\"E80.AA310\">cmH₂O</option><option value=\"E80.AA320\">cmH₂O&#160;(4&#160;&#176;C)</option><option value=\"E80.AA330\">daPa</option><option value=\"E80.AA340\">mmH₂O</option><option value=\"E80.AA350\">mWS</option><option value=\"E80.AA360\">J/m&#179;</option><option value=\"E80.AA370\">kg/(cm&#183;s&#178;)</option><option value=\"E80.AA380\">N/m&#178;</option><option value=\"E80.AA390\">Pa</option><option value=\"E80.AA400\">kg/(m&#183;s&#178;)</option><option value=\"E80.AA410\">&#181;bar</option><option value=\"E80.AA420\">mPa</option><option value=\"E80.AA430\">&#181;Pa</option><option value=\"E80.BA100\">klbf/in&#178;</option><option value=\"E80.BA110\">ksi</option><option value=\"E80.BA120\">bar&#160;(g)</option><option value=\"E80.BA130\">at&#160;(g)</option><option value=\"E80.BA140\">kgf/cm&#178;</option><option value=\"E80.BA150\">kgf/cm&#178;&#160;(g)</option><option value=\"E80.BA160\">ftHg</option><option value=\"E80.BA170\">psi&#160;(a)</option><option value=\"E80.BA180\">psi&#160;(g)</option><option value=\"E80.BA190\">inHg</option><option value=\"E80.BA200\">inHg&#160;(32&#160;&#176;F)</option><option value=\"E80.BA210\">inHg&#160;(60&#160;&#176;F)</option><option value=\"E80.BA220\">ftH₂O</option><option value=\"E80.BA230\">ftH₂O&#160;(39.2&#160;&#176;F)</option><option value=\"E80.BA240\">kPa&#160;(g)</option><option value=\"E80.BA250\">inH₂O</option><option value=\"E80.BA260\">inH₂O&#160;(39.2&#160;&#176;F)</option><option value=\"E80.BA270\">inH₂O&#160;(60&#160;&#176;F)</option><option value=\"E80.BA280\">pdl/in&#178;</option><option value=\"E80.BA290\">gf/cm&#178;</option><option value=\"E80.BA300\">lbf/ft&#178;</option><option value=\"E80.BA310\">kgf/m&#178;</option><option value=\"E80.BA320\">pdl/ft&#178;</option><option value=\"E80.BA330\">lb/(ft&#183;s&#178;)</option><option value=\"E80.BA340\">dyn/cm&#178;</option><option value=\"E80.BA350\">erg/cm&#179;</option></select></nobr></div>";
    }
}
