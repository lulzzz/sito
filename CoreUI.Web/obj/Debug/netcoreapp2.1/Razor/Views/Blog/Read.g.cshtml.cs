#pragma checksum "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Blog\Read.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6705c56a9a2b71386c3dbf68db52806adc59b605"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Blog_Read), @"mvc.1.0.view", @"/Views/Blog/Read.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Blog/Read.cshtml", typeof(AspNetCore.Views_Blog_Read))]
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
#line 1 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Blog\Read.cshtml"
using Maddalena.Core.Blog;

#line default
#line hidden
#line 2 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Blog\Read.cshtml"
using Maddalena.Core.Settings;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6705c56a9a2b71386c3dbf68db52806adc59b605", @"/Views/Blog/Read.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7a6ac20d0bddf0145b9ad3932322f32d4f540752", @"/Views/_ViewImports.cshtml")]
    public class Views_Blog_Read : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Maddalena.Core.Blog.BlogPost>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Blog", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "edit", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("title", new global::Microsoft.AspNetCore.Html.HtmlString("Edit the post"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 4 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Blog\Read.cshtml"
  
    ViewData["Title"] = Model.Title;
    ViewData["Description"] = Model.Excerpt;
    bool showFullPost = ViewContext.RouteData.Values.ContainsKey("slug");
    string host = Context.Request.Scheme + "://" + Context.Request.Host;
    bool isCodePreview = Model.Content.Contains("</code>");

    var link = $"{@host}/read/{Model.Slug}";

#line default
#line hidden
            BeginContext(446, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            DefineSection("Head", async() => {
                BeginContext(463, 25, true);
                WriteLiteral("\r\n  <link rel=\"canonical\"");
                EndContext();
                BeginWriteAttribute("href", " href=\"", 488, "\"", 500, 1);
#line 15 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Blog\Read.cshtml"
WriteAttributeValue("", 495, link, 495, 5, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(501, 5, true);
                WriteLiteral(" />\r\n");
                EndContext();
            }
            );
            BeginContext(509, 220, true);
            WriteLiteral("\r\n<section>\r\n  <article class=\"post container\" itemscope itemtype=\"http://schema.org/BlogPosting\" itemprop=\"blogPost\">\r\n    <div class=\"row\">\r\n      <div class=\"col-10\">\r\n        <h1 itemprop=\"name headline\">\r\n          ");
            EndContext();
            BeginContext(730, 11, false);
#line 23 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Blog\Read.cshtml"
     Write(Model.Title);

#line default
#line hidden
            EndContext();
            BeginContext(741, 71, true);
            WriteLiteral("\r\n        </h1>\r\n      </div>\r\n      <div class=\"col-2\">\r\n        <time");
            EndContext();
            BeginWriteAttribute("datetime", " datetime=\"", 812, "\"", 851, 1);
#line 27 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Blog\Read.cshtml"
WriteAttributeValue("", 823, Model.PubDate.ToString("s"), 823, 28, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(852, 38, true);
            WriteLiteral(" itemprop=\"datePublished\">\r\n          ");
            EndContext();
            BeginContext(891, 37, false);
#line 28 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Blog\Read.cshtml"
     Write(Model.PubDate.ToString("MMM d, yyyy"));

#line default
#line hidden
            EndContext();
            BeginContext(928, 21, true);
            WriteLiteral("\r\n        </time>\r\n\r\n");
            EndContext();
#line 31 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Blog\Read.cshtml"
         if (User.Identity.IsAuthenticated && User.IsInRole("blog"))
            {

#line default
#line hidden
            BeginContext(1034, 8, true);
            WriteLiteral("        ");
            EndContext();
            BeginContext(1042, 103, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "086e4d9164f64f6d8d89029107ca7a33", async() => {
                BeginContext(1132, 9, true);
                WriteLiteral("Edit post");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#line 33 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Blog\Read.cshtml"
                                                     WriteLiteral(Model.Id);

#line default
#line hidden
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(1145, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 34 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Blog\Read.cshtml"
            }

#line default
#line hidden
            BeginContext(1162, 151, true);
            WriteLiteral("      </div>\r\n    </div>\r\n\r\n    <div class=\"row\">\r\n      <div class=\"col-12\">\r\n        <div itemprop=\"articleBody mainEntityOfPage\" cdnify>\r\n          ");
            EndContext();
            BeginContext(1314, 23, false);
#line 41 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Blog\Read.cshtml"
     Write(Html.Raw(Model.Content));

#line default
#line hidden
            EndContext();
            BeginContext(1337, 154, true);
            WriteLiteral("\r\n        </div>\r\n      </div>\r\n    </div>\r\n\r\n    <div class=\"row\">\r\n      <div class=\"col-12\">\r\n        <footer>\r\n          <meta itemprop=\"dateModified\"");
            EndContext();
            BeginWriteAttribute("content", " content=\"", 1491, "\"", 1534, 1);
#line 49 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Blog\Read.cshtml"
WriteAttributeValue("", 1501, Model.LastModified.ToString("s"), 1501, 33, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1535, 48, true);
            WriteLiteral(" />\r\n          <meta itemprop=\"mainEntityOfPage\"");
            EndContext();
            BeginWriteAttribute("content", " content=\"", 1583, "\"", 1598, 1);
#line 50 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Blog\Read.cshtml"
WriteAttributeValue("", 1593, link, 1593, 5, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1599, 76, true);
            WriteLiteral(" />\r\n        </footer>\r\n      </div>\r\n    </div>\r\n  </article>\r\n</section>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Maddalena.Core.Blog.BlogPost> Html { get; private set; }
    }
}
#pragma warning restore 1591
