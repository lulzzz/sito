#pragma checksum "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Youtube\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e47e6db40fff3422bc344783dc66b254ee51f6d2"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Youtube_Index), @"mvc.1.0.view", @"/Views/Youtube/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Youtube/Index.cshtml", typeof(AspNetCore.Views_Youtube_Index))]
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
#line 6 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Youtube\Index.cshtml"
using YoutubeExplode.Models.MediaStreams;

#line default
#line hidden
#line 7 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Youtube\Index.cshtml"
using Maddalena.Core.Extensions;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e47e6db40fff3422bc344783dc66b254ee51f6d2", @"/Views/Youtube/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7a6ac20d0bddf0145b9ad3932322f32d4f540752", @"/Views/_ViewImports.cshtml")]
    public class Views_Youtube_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<YoutubeExplode.Models.MediaStreams.MediaStreamInfoSet>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("action", new global::Microsoft.AspNetCore.Html.HtmlString("/youtube"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 2 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Youtube\Index.cshtml"
  
    ViewData["Title"] = "View";

#line default
#line hidden
            BeginContext(42, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(183, 122, true);
            WriteLiteral("\r\n<section>\r\n  <section>\r\n    <div class=\"row\">\r\n      <div class=\"col-12\">\r\n        <h2>Youtube downloader</h2>\r\n        ");
            EndContext();
            BeginContext(305, 440, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "350aef28034e4b928b910367c62a6884", async() => {
                BeginContext(343, 395, true);
                WriteLiteral(@"
          <div class=""form-group"">
            <label for=""ytUrl"">Youtube Url</label>
            <input name=""url"" type=""url"" class=""form-control"" id=""ytUrl"" placeholder=""https://www.youtube.com/watch?v=ERhbuYk9HEk"">
          </div>
          <div class=""form-group"">
            <input type=""submit"" value=""Retrieve"" class=""btn btn-lg btn-outline-primary""/>
          </div>
        ");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(745, 44, true);
            WriteLiteral("\r\n      </div>\r\n    </div>\r\n  </section>\r\n\r\n");
            EndContext();
#line 28 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Youtube\Index.cshtml"
   if (Model != null)
  {

#line default
#line hidden
            BeginContext(817, 95, true);
            WriteLiteral("    <section>\r\n      <div class=\"row\">\r\n        <div class=\"col-4\">\r\n          <h3>Audio</h3>\r\n");
            EndContext();
#line 34 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Youtube\Index.cshtml"
           foreach (var ad in Model.Audio)
          {

#line default
#line hidden
            BeginContext(969, 170, true);
            WriteLiteral("            <section>\r\n              <table class=\"table table-striped table-bordered\">\r\n                <tr>\r\n                  <td>Encoding</td>\r\n                  <td>");
            EndContext();
            BeginContext(1140, 16, false);
#line 40 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Youtube\Index.cshtml"
                 Write(ad.AudioEncoding);

#line default
#line hidden
            EndContext();
            BeginContext(1156, 110, true);
            WriteLiteral("</td>\r\n                </tr>\r\n                <tr>\r\n                  <td>Bitrate</td>\r\n                  <td>");
            EndContext();
            BeginContext(1267, 19, false);
#line 44 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Youtube\Index.cshtml"
                 Write(ad.Bitrate.ToBaud());

#line default
#line hidden
            EndContext();
            BeginContext(1286, 107, true);
            WriteLiteral("</td>\r\n                </tr>\r\n                <tr>\r\n                  <td>Size</td>\r\n                  <td>");
            EndContext();
            BeginContext(1394, 20, false);
#line 48 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Youtube\Index.cshtml"
                 Write(ad.Size.ToByteSize());

#line default
#line hidden
            EndContext();
            BeginContext(1414, 130, true);
            WriteLiteral("</td>\r\n                </tr>\r\n                <tr>\r\n                  <td colspan=\"2\" class=\"text-center\">\r\n                    <a");
            EndContext();
            BeginWriteAttribute("href", " href=\"", 1544, "\"", 1558, 1);
#line 52 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Youtube\Index.cshtml"
WriteAttributeValue("", 1551, ad.Url, 1551, 7, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1559, 151, true);
            WriteLiteral(" class=\"btn btn-success\" target=\"_blank\">Donwload</a>\r\n                  </td>\r\n                </tr>\r\n              </table>\r\n            </section>\r\n");
            EndContext();
#line 57 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Youtube\Index.cshtml"

          }

#line default
#line hidden
            BeginContext(1725, 70, true);
            WriteLiteral("        </div>\r\n        <div class=\"col-4\">\r\n          <h3>Both</h3>\r\n");
            EndContext();
#line 62 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Youtube\Index.cshtml"
           foreach (var ad in Model.Muxed)
          {

#line default
#line hidden
            BeginContext(1852, 180, true);
            WriteLiteral("              <section>\r\n                <table class=\"table table-striped table-bordered\">\r\n                  <tr>\r\n                    <td>Encoding</td>\r\n                    <td>");
            EndContext();
            BeginContext(2033, 16, false);
#line 68 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Youtube\Index.cshtml"
                   Write(ad.AudioEncoding);

#line default
#line hidden
            EndContext();
            BeginContext(2049, 3, true);
            WriteLiteral(" - ");
            EndContext();
            BeginContext(2053, 16, false);
#line 68 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Youtube\Index.cshtml"
                                       Write(ad.VideoEncoding);

#line default
#line hidden
            EndContext();
            BeginContext(2069, 118, true);
            WriteLiteral("</td>\r\n                  </tr>\r\n                  <tr>\r\n                    <td>Quality</td>\r\n                    <td>");
            EndContext();
            BeginContext(2188, 20, false);
#line 72 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Youtube\Index.cshtml"
                   Write(ad.VideoQualityLabel);

#line default
#line hidden
            EndContext();
            BeginContext(2208, 111, true);
            WriteLiteral("</td>\r\n                  </tr>\r\n                  <tr>\r\n                    <td></td>\r\n                    <td>");
            EndContext();
            BeginContext(2320, 20, false);
#line 76 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Youtube\Index.cshtml"
                   Write(ad.Size.ToByteSize());

#line default
#line hidden
            EndContext();
            BeginContext(2340, 138, true);
            WriteLiteral("</td>\r\n                  </tr>\r\n                  <tr>\r\n                    <td colspan=\"2\" class=\"text-center\">\r\n                      <a");
            EndContext();
            BeginWriteAttribute("href", " href=\"", 2478, "\"", 2492, 1);
#line 80 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Youtube\Index.cshtml"
WriteAttributeValue("", 2485, ad.Url, 2485, 7, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(2493, 159, true);
            WriteLiteral(" class=\"btn btn-success\" target=\"_blank\">Donwload</a>\r\n                    </td>\r\n                  </tr>\r\n                </table>\r\n              </section>\r\n");
            EndContext();
#line 85 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Youtube\Index.cshtml"
          }

#line default
#line hidden
            BeginContext(2665, 71, true);
            WriteLiteral("        </div>\r\n        <div class=\"col-4\">\r\n          <h3>Video</h3>\r\n");
            EndContext();
#line 89 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Youtube\Index.cshtml"
           foreach (var ad in Model.Video)
          {

#line default
#line hidden
            BeginContext(2793, 180, true);
            WriteLiteral("              <section>\r\n                <table class=\"table table-striped table-bordered\">\r\n                  <tr>\r\n                    <td>Encoding</td>\r\n                    <td>");
            EndContext();
            BeginContext(2974, 16, false);
#line 95 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Youtube\Index.cshtml"
                   Write(ad.VideoEncoding);

#line default
#line hidden
            EndContext();
            BeginContext(2990, 118, true);
            WriteLiteral("</td>\r\n                  </tr>\r\n                  <tr>\r\n                    <td>Quality</td>\r\n                    <td>");
            EndContext();
            BeginContext(3109, 20, false);
#line 99 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Youtube\Index.cshtml"
                   Write(ad.VideoQualityLabel);

#line default
#line hidden
            EndContext();
            BeginContext(3129, 115, true);
            WriteLiteral("</td>\r\n                  </tr>\r\n                  <tr>\r\n                    <td>Size</td>\r\n                    <td>");
            EndContext();
            BeginContext(3245, 20, false);
#line 103 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Youtube\Index.cshtml"
                   Write(ad.Size.ToByteSize());

#line default
#line hidden
            EndContext();
            BeginContext(3265, 138, true);
            WriteLiteral("</td>\r\n                  </tr>\r\n                  <tr>\r\n                    <td colspan=\"2\" class=\"text-center\">\r\n                      <a");
            EndContext();
            BeginWriteAttribute("href", " href=\"", 3403, "\"", 3417, 1);
#line 107 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Youtube\Index.cshtml"
WriteAttributeValue("", 3410, ad.Url, 3410, 7, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(3418, 159, true);
            WriteLiteral(" class=\"btn btn-success\" target=\"_blank\">Donwload</a>\r\n                    </td>\r\n                  </tr>\r\n                </table>\r\n              </section>\r\n");
            EndContext();
#line 112 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Youtube\Index.cshtml"
          }

#line default
#line hidden
            BeginContext(3590, 46, true);
            WriteLiteral("        </div>\r\n      </div>\r\n    </section>\r\n");
            EndContext();
#line 116 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Youtube\Index.cshtml"
  }

#line default
#line hidden
            BeginContext(3641, 12, true);
            WriteLiteral("</section>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<YoutubeExplode.Models.MediaStreams.MediaStreamInfoSet> Html { get; private set; }
    }
}
#pragma warning restore 1591