#pragma checksum "D:\PROGETTI\sito\CoreUI.Web\Views\CoreUI\components-tables.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "25e02d5393652d13f63ba7011b88b6092d5d3226"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_CoreUI_components_tables), @"mvc.1.0.view", @"/Views/CoreUI/components-tables.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/CoreUI/components-tables.cshtml", typeof(AspNetCore.Views_CoreUI_components_tables))]
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
#line 1 "D:\PROGETTI\sito\CoreUI.Web\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#line 2 "D:\PROGETTI\sito\CoreUI.Web\Views\_ViewImports.cshtml"
using CoreUI.Web;

#line default
#line hidden
#line 3 "D:\PROGETTI\sito\CoreUI.Web\Views\_ViewImports.cshtml"
using CoreUI.Web.Models;

#line default
#line hidden
#line 4 "D:\PROGETTI\sito\CoreUI.Web\Views\_ViewImports.cshtml"
using CoreUI.Web.Models.AccountViewModels;

#line default
#line hidden
#line 5 "D:\PROGETTI\sito\CoreUI.Web\Views\_ViewImports.cshtml"
using CoreUI.Web.Models.ManageViewModels;

#line default
#line hidden
#line 6 "D:\PROGETTI\sito\CoreUI.Web\Views\_ViewImports.cshtml"
using Maddalena.Core.Identity;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"25e02d5393652d13f63ba7011b88b6092d5d3226", @"/Views/CoreUI/components-tables.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7a6ac20d0bddf0145b9ad3932322f32d4f540752", @"/Views/_ViewImports.cshtml")]
    public class Views_CoreUI_components_tables : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 17536, true);
            WriteLiteral(@"<!-- *PAGE* -->
<div class=""animated fadeIn"">
    <div class=""row"">
        <div class=""col-lg-6"">
            <div class=""card"">
                <div class=""card-header"">
                    <i class=""fa fa-align-justify""></i> Simple Table
                </div>
                <div class=""card-body"">
                    <table class=""table"">
                        <thead>
                            <tr>
                                <th>Username</th>
                                <th>Date registered</th>
                                <th>Role</th>
                                <th>Status</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>Samppa Nori</td>
                                <td>2012/01/01</td>
                                <td>Member</td>
                                <td>
                                    <span class=""badge badge-succe");
            WriteLiteral(@"ss"">Active</span>
                                </td>
                            </tr>
                            <tr>
                                <td>Estavan Lykos</td>
                                <td>2012/02/01</td>
                                <td>Staff</td>
                                <td>
                                    <span class=""badge badge-danger"">Banned</span>
                                </td>
                            </tr>
                            <tr>
                                <td>Chetan Mohamed</td>
                                <td>2012/02/01</td>
                                <td>Admin</td>
                                <td>
                                    <span class=""badge badge-secondary"">Inactive</span>
                                </td>
                            </tr>
                            <tr>
                                <td>Derick Maximinus</td>
                                <td>2012/03/01</td>
     ");
            WriteLiteral(@"                           <td>Member</td>
                                <td>
                                    <span class=""badge badge-warning"">Pending</span>
                                </td>
                            </tr>
                            <tr>
                                <td>Friderik Dávid</td>
                                <td>2012/01/21</td>
                                <td>Staff</td>
                                <td>
                                    <span class=""badge badge-success"">Active</span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <ul class=""pagination"">
                        <li class=""page-item""><a class=""page-link"" href=""#"">Prev</a></li>
                        <li class=""page-item active"">
                            <a class=""page-link"" href=""#"">1</a>
                        </li>
                        <li class=""page-it");
            WriteLiteral(@"em""><a class=""page-link"" href=""#"">2</a></li>
                        <li class=""page-item""><a class=""page-link"" href=""#"">3</a></li>
                        <li class=""page-item""><a class=""page-link"" href=""#"">4</a></li>
                        <li class=""page-item""><a class=""page-link"" href=""#"">Next</a></li>
                    </ul>
                </div>
            </div>
        </div>
        <!--/.col-->
        <div class=""col-lg-6"">
            <div class=""card"">
                <div class=""card-header"">
                    <i class=""fa fa-align-justify""></i> Striped Table
                </div>
                <div class=""card-body"">
                    <table class=""table table-striped"">
                        <thead>
                            <tr>
                                <th>Username</th>
                                <th>Date registered</th>
                                <th>Role</th>
                                <th>Status</th>
                            </");
            WriteLiteral(@"tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>Yiorgos Avraamu</td>
                                <td>2012/01/01</td>
                                <td>Member</td>
                                <td>
                                    <span class=""badge badge-success"">Active</span>
                                </td>
                            </tr>
                            <tr>
                                <td>Avram Tarasios</td>
                                <td>2012/02/01</td>
                                <td>Staff</td>
                                <td>
                                    <span class=""badge badge-danger"">Banned</span>
                                </td>
                            </tr>
                            <tr>
                                <td>Quintin Ed</td>
                                <td>2012/02/01</td>
                                <");
            WriteLiteral(@"td>Admin</td>
                                <td>
                                    <span class=""badge badge-secondary"">Inactive</span>
                                </td>
                            </tr>
                            <tr>
                                <td>Enéas Kwadwo</td>
                                <td>2012/03/01</td>
                                <td>Member</td>
                                <td>
                                    <span class=""badge badge-warning"">Pending</span>
                                </td>
                            </tr>
                            <tr>
                                <td>Agapetus Tadeáš</td>
                                <td>2012/01/21</td>
                                <td>Staff</td>
                                <td>
                                    <span class=""badge badge-success"">Active</span>
                                </td>
                            </tr>
                        </tbo");
            WriteLiteral(@"dy>
                    </table>
                    <ul class=""pagination"">
                        <li class=""page-item""><a class=""page-link"" href=""#"">Prev</a></li>
                        <li class=""page-item active"">
                            <a class=""page-link"" href=""#"">1</a>
                        </li>
                        <li class=""page-item""><a class=""page-link"" href=""#"">2</a></li>
                        <li class=""page-item""><a class=""page-link"" href=""#"">3</a></li>
                        <li class=""page-item""><a class=""page-link"" href=""#"">4</a></li>
                        <li class=""page-item""><a class=""page-link"" href=""#"">Next</a></li>
                    </ul>
                </div>
            </div>
        </div>
        <!--/.col-->
    </div>
    <!--/.row-->
    <div class=""row"">
        <div class=""col-lg-6"">
            <div class=""card"">
                <div class=""card-header"">
                    <i class=""fa fa-align-justify""></i> Condensed Table
     ");
            WriteLiteral(@"           </div>
                <div class=""card-body"">
                    <table class=""table table-sm"">
                        <thead>
                            <tr>
                                <th>Username</th>
                                <th>Date registered</th>
                                <th>Role</th>
                                <th>Status</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>Carwyn Fachtna</td>
                                <td>2012/01/01</td>
                                <td>Member</td>
                                <td>
                                    <span class=""badge badge-success"">Active</span>
                                </td>
                            </tr>
                            <tr>
                                <td>Nehemiah Tatius</td>
                                <td>2012/02/01</td>
   ");
            WriteLiteral(@"                             <td>Staff</td>
                                <td>
                                    <span class=""badge badge-danger"">Banned</span>
                                </td>
                            </tr>
                            <tr>
                                <td>Ebbe Gemariah</td>
                                <td>2012/02/01</td>
                                <td>Admin</td>
                                <td>
                                    <span class=""badge badge-secondary"">Inactive</span>
                                </td>
                            </tr>
                            <tr>
                                <td>Eustorgios Amulius</td>
                                <td>2012/03/01</td>
                                <td>Member</td>
                                <td>
                                    <span class=""badge badge-warning"">Pending</span>
                                </td>
                            </t");
            WriteLiteral(@"r>
                            <tr>
                                <td>Leopold Gáspár</td>
                                <td>2012/01/21</td>
                                <td>Staff</td>
                                <td>
                                    <span class=""badge badge-success"">Active</span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <ul class=""pagination"">
                        <li class=""page-item""><a class=""page-link"" href=""#"">Prev</a></li>
                        <li class=""page-item active"">
                            <a class=""page-link"" href=""#"">1</a>
                        </li>
                        <li class=""page-item""><a class=""page-link"" href=""#"">2</a></li>
                        <li class=""page-item""><a class=""page-link"" href=""#"">3</a></li>
                        <li class=""page-item""><a class=""page-link"" href=""#"">4</a></li>
                ");
            WriteLiteral(@"        <li class=""page-item""><a class=""page-link"" href=""#"">Next</a></li>
                    </ul>
                </div>
            </div>
        </div>
        <!--/.col-->
        <div class=""col-lg-6"">
            <div class=""card"">
                <div class=""card-header"">
                    <i class=""fa fa-align-justify""></i> Bordered Table
                </div>
                <div class=""card-body"">
                    <table class=""table table-bordered"">
                        <thead>
                            <tr>
                                <th>Username</th>
                                <th>Date registered</th>
                                <th>Role</th>
                                <th>Status</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>Pompeius René</td>
                                <td>2012/01/01</td>
                     ");
            WriteLiteral(@"           <td>Member</td>
                                <td>
                                    <span class=""badge badge-success"">Active</span>
                                </td>
                            </tr>
                            <tr>
                                <td>Paĉjo Jadon</td>
                                <td>2012/02/01</td>
                                <td>Staff</td>
                                <td>
                                    <span class=""badge badge-danger"">Banned</span>
                                </td>
                            </tr>
                            <tr>
                                <td>Micheal Mercurius</td>
                                <td>2012/02/01</td>
                                <td>Admin</td>
                                <td>
                                    <span class=""badge badge-secondary"">Inactive</span>
                                </td>
                            </tr>
                  ");
            WriteLiteral(@"          <tr>
                                <td>Ganesha Dubhghall</td>
                                <td>2012/03/01</td>
                                <td>Member</td>
                                <td>
                                    <span class=""badge badge-warning"">Pending</span>
                                </td>
                            </tr>
                            <tr>
                                <td>Hiroto Šimun</td>
                                <td>2012/01/21</td>
                                <td>Staff</td>
                                <td>
                                    <span class=""badge badge-success"">Active</span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <ul class=""pagination"">
                        <li class=""page-item""><a class=""page-link"" href=""#"">Prev</a></li>
                        <li class=""page-item active"">
      ");
            WriteLiteral(@"                      <a class=""page-link"" href=""#"">1</a>
                        </li>
                        <li class=""page-item""><a class=""page-link"" href=""#"">2</a></li>
                        <li class=""page-item""><a class=""page-link"" href=""#"">3</a></li>
                        <li class=""page-item""><a class=""page-link"" href=""#"">4</a></li>
                        <li class=""page-item""><a class=""page-link"" href=""#"">Next</a></li>
                    </ul>
                </div>
            </div>
        </div>
        <!--/.col-->
    </div>
    <!--/.row-->
    <div class=""row"">
        <div class=""col-lg-12"">
            <div class=""card"">
                <div class=""card-header"">
                    <i class=""fa fa-align-justify""></i> Combined All Table
                </div>
                <div class=""card-body"">
                    <table class=""table table-bordered table-striped table-sm"">
                        <thead>
                            <tr>
                    ");
            WriteLiteral(@"            <th>Username</th>
                                <th>Date registered</th>
                                <th>Role</th>
                                <th>Status</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>Vishnu Serghei</td>
                                <td>2012/01/01</td>
                                <td>Member</td>
                                <td>
                                    <span class=""badge badge-success"">Active</span>
                                </td>
                            </tr>
                            <tr>
                                <td>Zbyněk Phoibos</td>
                                <td>2012/02/01</td>
                                <td>Staff</td>
                                <td>
                                    <span class=""badge badge-danger"">Banned</span>
                                <");
            WriteLiteral(@"/td>
                            </tr>
                            <tr>
                                <td>Einar Randall</td>
                                <td>2012/02/01</td>
                                <td>Admin</td>
                                <td>
                                    <span class=""badge badge-secondary"">Inactive</span>
                                </td>
                            </tr>
                            <tr>
                                <td>Félix Troels</td>
                                <td>2012/03/01</td>
                                <td>Member</td>
                                <td>
                                    <span class=""badge badge-warning"">Pending</span>
                                </td>
                            </tr>
                            <tr>
                                <td>Aulus Agmundr</td>
                                <td>2012/01/21</td>
                                <td>Staff</td>
           ");
            WriteLiteral(@"                     <td>
                                    <span class=""badge badge-success"">Active</span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <nav>
                        <ul class=""pagination"">
                            <li class=""page-item""><a class=""page-link"" href=""#"">Prev</a></li>
                            <li class=""page-item active"">
                                <a class=""page-link"" href=""#"">1</a>
                            </li>
                            <li class=""page-item""><a class=""page-link"" href=""#"">2</a></li>
                            <li class=""page-item""><a class=""page-link"" href=""#"">3</a></li>
                            <li class=""page-item""><a class=""page-link"" href=""#"">4</a></li>
                            <li class=""page-item""><a class=""page-link"" href=""#"">Next</a></li>
                        </ul>
                    </nav>
          ");
            WriteLiteral("      </div>\r\n            </div>\r\n        </div>\r\n        <!--/.col-->\r\n    </div>\r\n    <!--/.row-->\r\n</div>\r\n<!-- /*PAGE* -->\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
