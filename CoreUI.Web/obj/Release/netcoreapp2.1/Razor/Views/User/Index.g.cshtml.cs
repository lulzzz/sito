#pragma checksum "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\User\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b11d8c58bd4bd3a6c41f993989f2b932f56d68a6"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_User_Index), @"mvc.1.0.view", @"/Views/User/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/User/Index.cshtml", typeof(AspNetCore.Views_User_Index))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#line 2 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\_ViewImports.cshtml"
using CoreUI.Web;

#line default
#line hidden
#line 3 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\_ViewImports.cshtml"
using CoreUI.Web.Models;

#line default
#line hidden
#line 4 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\_ViewImports.cshtml"
using CoreUI.Web.Models.AccountViewModels;

#line default
#line hidden
#line 5 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\_ViewImports.cshtml"
using CoreUI.Web.Models.ManageViewModels;

#line default
#line hidden
#line 6 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\_ViewImports.cshtml"
using Maddalena.Core.Identity;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b11d8c58bd4bd3a6c41f993989f2b932f56d68a6", @"/Views/User/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7a6ac20d0bddf0145b9ad3932322f32d4f540752", @"/Views/_ViewImports.cshtml")]
    public class Views_User_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<Maddalena.Core.Identity.MaddalenaUser>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("action", new global::Microsoft.AspNetCore.Html.HtmlString("/user/delete"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(59, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 3 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\User\Index.cshtml"
  
  ViewData["Title"] = "Index";

#line default
#line hidden
            BeginContext(100, 767, true);
            WriteLiteral(@"
<div class=""row"">
  <div class=""col-12"">
    <a href=""/user/settings"">Web site settings</a>
  </div>
</div>

<ul class=""nav nav-tabs"" role=""tablist"">
  <li class=""nav-item"">
    <a class=""nav-link active"" data-toggle=""tab"" href=""#home"" role=""tab"" aria-controls=""home"">Users</a>
  </li>
  <li class=""nav-item"">
    <a class=""nav-link"" data-toggle=""tab"" href=""#profile"" role=""tab"" aria-controls=""profile"">Settings</a>
  </li>
  <li class=""nav-item"">
    <a class=""nav-link"" data-toggle=""tab"" href=""#messages"" role=""tab"" aria-controls=""messages"">Messages</a>
  </li>
</ul>
<div class=""tab-content"">
  <div class=""tab-pane active"" id=""home"" role=""tabpanel"">
    <table class=""table table-striped"">
      <thead>
      <tr>
        <th>
          ");
            EndContext();
            BeginContext(868, 47, false);
#line 30 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\User\Index.cshtml"
     Write(Html.DisplayNameFor(model => model.DisplayName));

#line default
#line hidden
            EndContext();
            BeginContext(915, 41, true);
            WriteLiteral("\r\n        </th>\r\n        <th>\r\n          ");
            EndContext();
            BeginContext(957, 44, false);
#line 33 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\User\Index.cshtml"
     Write(Html.DisplayNameFor(model => model.UserName));

#line default
#line hidden
            EndContext();
            BeginContext(1001, 41, true);
            WriteLiteral("\r\n        </th>\r\n        <th>\r\n          ");
            EndContext();
            BeginContext(1043, 41, false);
#line 36 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\User\Index.cshtml"
     Write(Html.DisplayNameFor(model => model.Email));

#line default
#line hidden
            EndContext();
            BeginContext(1084, 41, true);
            WriteLiteral("\r\n        </th>\r\n        <th>\r\n          ");
            EndContext();
            BeginContext(1126, 47, false);
#line 39 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\User\Index.cshtml"
     Write(Html.DisplayNameFor(model => model.PhoneNumber));

#line default
#line hidden
            EndContext();
            BeginContext(1173, 61, true);
            WriteLiteral("\r\n        </th>\r\n      </tr>\r\n      </thead>\r\n      <tbody>\r\n");
            EndContext();
#line 44 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\User\Index.cshtml"
       foreach (var item in Model)
      {

#line default
#line hidden
            BeginContext(1279, 42, true);
            WriteLiteral("        <tr>\r\n          <td>\r\n            ");
            EndContext();
            BeginContext(1322, 47, false);
#line 48 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\User\Index.cshtml"
       Write(Html.DisplayNameFor(model => model.DisplayName));

#line default
#line hidden
            EndContext();
            BeginContext(1369, 47, true);
            WriteLiteral("\r\n          </td>\r\n          <td>\r\n            ");
            EndContext();
            BeginContext(1417, 43, false);
#line 51 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\User\Index.cshtml"
       Write(Html.DisplayFor(modelItem => item.UserName));

#line default
#line hidden
            EndContext();
            BeginContext(1460, 47, true);
            WriteLiteral("\r\n          </td>\r\n          <td>\r\n            ");
            EndContext();
            BeginContext(1508, 40, false);
#line 54 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\User\Index.cshtml"
       Write(Html.DisplayFor(modelItem => item.Email));

#line default
#line hidden
            EndContext();
            BeginContext(1548, 47, true);
            WriteLiteral("\r\n          </td>\r\n          <td>\r\n            ");
            EndContext();
            BeginContext(1596, 46, false);
#line 57 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\User\Index.cshtml"
       Write(Html.DisplayFor(modelItem => item.PhoneNumber));

#line default
#line hidden
            EndContext();
            BeginContext(1642, 49, true);
            WriteLiteral("\r\n          </td>\r\n          <td>\r\n            <a");
            EndContext();
            BeginWriteAttribute("href", " href=\"", 1691, "\"", 1723, 2);
            WriteAttributeValue("", 1698, "/user/edit/", 1698, 11, true);
#line 60 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\User\Index.cshtml"
WriteAttributeValue("", 1709, item.UserName, 1709, 14, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1724, 95, true);
            WriteLiteral(" class=\"btn btn-sm btn-outline-primary\">Edit</a>\r\n          </td>\r\n          <td>\r\n            ");
            EndContext();
            BeginContext(1819, 246, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "d3aa2d2d47db4b788bb071b126f9906c", async() => {
                BeginContext(1861, 16, true);
                WriteLiteral("\r\n              ");
                EndContext();
                BeginContext(1878, 23, false);
#line 64 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\User\Index.cshtml"
         Write(Html.AntiForgeryToken());

#line default
#line hidden
                EndContext();
                BeginContext(1901, 46, true);
                WriteLiteral("\r\n              <input type=\"hidden\" name=\"id\"");
                EndContext();
                BeginWriteAttribute("value", " value=\"", 1947, "\"", 1963, 1);
#line 65 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\User\Index.cshtml"
WriteAttributeValue("", 1955, item.Id, 1955, 8, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(1964, 94, true);
                WriteLiteral(" />\r\n              <input type=\"submit\" class=\"btn btn-danger\" value=\"Delete\" />\r\n            ");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(2065, 34, true);
            WriteLiteral("\r\n          </td>\r\n        </tr>\r\n");
            EndContext();
#line 70 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\User\Index.cshtml"
      }

#line default
#line hidden
            BeginContext(2108, 1097, true);
            WriteLiteral(@"      </tbody>
    </table>
  </div>
  <div class=""tab-pane"" id=""profile"" role=""tabpanel"">
    2. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure
    dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
  </div>
  <div class=""tab-pane"" id=""messages"" role=""tabpanel"">
    3. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure
    dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in");
            WriteLiteral(" culpa qui officia deserunt mollit anim id est laborum.\r\n  </div>\r\n</div>");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<Maddalena.Core.Identity.MaddalenaUser>> Html { get; private set; }
    }
}
#pragma warning restore 1591
