#pragma checksum "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e44c9796dfcf72c009748126db86de377cdd15ef"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Stat), @"mvc.1.0.view", @"/Views/Home/Stat.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/Stat.cshtml", typeof(AspNetCore.Views_Home_Stat))]
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
#line 1 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
using ServerSideAnalytics;

#line default
#line hidden
#line 2 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
using ServerSideAnalytics.Extensions;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e44c9796dfcf72c009748126db86de377cdd15ef", @"/Views/Home/Stat.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7a6ac20d0bddf0145b9ad3932322f32d4f540752", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Stat : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(98, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 5 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
  
  var from = DateTime.MinValue;
  var to = DateTime.MaxValue;

#line default
#line hidden
            BeginContext(171, 701, true);
            WriteLiteral(@"
<section>
  <h2>Server side analytics</h2>
  <div class=""row"">
    <div class=""col-12"">
      <p>
        This is a simple demo of my <a href=""https://www.nuget.org/packages/ServerSideAnalytics/"">nuget package</a>.
        The system assign automatically an identity to every not registered user based on AspNet Core cookie system and connection id.
      </p>
      <p>
        Discover more on how It works <a href=""/read/server-side-analytics"">here</a> 
      </p>
    </div>
  </div>
</section>

<section>
  <div class=""row"">
    <div class=""col-sm-6 col-lg-3"">
      <div class=""card text-white bg-primary"">
        <div class=""card-body pb-0"">
          <h4 class=""mb-0"">
");
            EndContext();
#line 31 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
              
              var total = await _store.CountAsync(from, to);
            

#line default
#line hidden
            BeginContext(965, 12, true);
            WriteLiteral("            ");
            EndContext();
            BeginContext(978, 5, false);
#line 34 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
       Write(total);

#line default
#line hidden
            EndContext();
            BeginContext(983, 274, true);
            WriteLiteral(@"
          </h4>
          <p>Requests served</p>
        </div>
      </div>
    </div>
    <!--/.col-->
    <div class=""col-sm-6 col-lg-3"">
      <div class=""card text-white bg-info"">
        <div class=""card-body pb-0"">
          <h4 class=""mb-0"">
            ");
            EndContext();
            BeginContext(1258, 50, false);
#line 45 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
       Write(await _store.CountUniqueIndentitiesAsync(from, to));

#line default
#line hidden
            EndContext();
            BeginContext(1308, 277, true);
            WriteLiteral(@"
          </h4>
          <p>Unique visitors</p>
        </div>
      </div>
    </div>
    <!--/.col-->
    <div class=""col-sm-6 col-lg-3"">
      <div class=""card text-white bg-warning"">
        <div class=""card-body pb-0"">
          <h4 class=""mb-0"">
            ");
            EndContext();
            BeginContext(1587, 53, false);
#line 56 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
        Write((await _store.DailyAverage(from, to)).ToString("0,0"));

#line default
#line hidden
            EndContext();
            BeginContext(1641, 274, true);
            WriteLiteral(@"
          </h4>
          <p>Daily average</p>
        </div>
      </div>
    </div>
    <!--/.col-->
    <div class=""col-sm-6 col-lg-3"">
      <div class=""card text-white bg-danger"">
        <div class=""card-body pb-0"">
          <h4 class=""mb-0"">
            ");
            EndContext();
            BeginContext(1916, 74, false);
#line 67 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
       Write(await _store.CountAsync(DateTime.Now - TimeSpan.FromDays(1), DateTime.Now));

#line default
#line hidden
            EndContext();
            BeginContext(1990, 1109, true);
            WriteLiteral(@"
          </h4>
          <p>Served today</p>
        </div>
      </div>
    </div>
    <!--/.col-->
  </div>



  <div class=""card"">
    <div class=""card-body"">
      <div class=""row"">
        <div class=""col-sm-5"">
          <h4 class=""card-title mb-0"">Request served by day</h4>
        </div>
      </div>
      <div class=""chart-wrapper"" style=""height: 300px; margin-top: 40px;"">
        <canvas id=""canvasByDay"" class=""chart"" height=""300""></canvas>
      </div>
    </div>
  </div>

  <div class=""card"">
    <div class=""card-body"">
      <div class=""row"">
        <div class=""col-sm-5"">
          <h4 class=""card-title mb-0"">Request served by hour</h4>
        </div>
      </div>
      <div class=""chart-wrapper"" style=""height: 300px; margin-top: 40px;"">
        <canvas id=""canvasByHour"" class=""chart"" height=""300""></canvas>
      </div>
    </div>
  </div>

  <!--/.row-->
  <div class=""row"">
    <div class=""col-md-12"">
      <div class=""card"">
        <div class=""card-h");
            WriteLiteral("eader\">\r\n          Requests by URL\r\n        </div>\r\n        <div class=\"card-body\">\r\n");
            EndContext();
#line 112 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
            
            var urlServed = await _store.UrlServed(from, to);
          

#line default
#line hidden
            BeginContext(3189, 360, true);
            WriteLiteral(@"
          <div class=""row"">
            <div class=""col-4"">
              <b>Url</b>
            </div>
            <div class=""col-2"">
              <b>Requests served</b>
            </div>
            <div class=""col-2"">
              <b>Percentage</b>
            </div>
            <div class=""col-4"">
            </div>
          </div>

");
            EndContext();
#line 130 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
           foreach (var served in urlServed)
          {
            var percentage = (served.Served * 100) / total;

            if (percentage >= 5)
            {

#line default
#line hidden
            BeginContext(3720, 88, true);
            WriteLiteral("              <div class=\"row\">\r\n                <div class=\"col-4\">\r\n                  ");
            EndContext();
            BeginContext(3809, 10, false);
#line 138 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
             Write(served.Url);

#line default
#line hidden
            EndContext();
            BeginContext(3819, 81, true);
            WriteLiteral("\r\n                </div>\r\n                <div class=\"col-2\">\r\n                  ");
            EndContext();
            BeginContext(3901, 13, false);
#line 141 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
             Write(served.Served);

#line default
#line hidden
            EndContext();
            BeginContext(3914, 81, true);
            WriteLiteral("\r\n                </div>\r\n                <div class=\"col-2\">\r\n                  ");
            EndContext();
            BeginContext(3996, 10, false);
#line 144 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
             Write(percentage);

#line default
#line hidden
            EndContext();
            BeginContext(4006, 221, true);
            WriteLiteral("%\r\n                </div>\r\n                <div class=\"col-4\">\r\n                  <div class=\"bars\">\r\n                    <div class=\"progress \">\r\n                      <div class=\"progress-bar bg-info\" role=\"progressbar\"");
            EndContext();
            BeginWriteAttribute("style", " style=\"", 4227, "\"", 4254, 3);
            WriteAttributeValue("", 4235, "width:", 4235, 6, true);
#line 149 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
WriteAttributeValue(" ", 4241, percentage, 4242, 11, false);

#line default
#line hidden
            WriteAttributeValue("", 4253, "%", 4253, 1, true);
            EndWriteAttribute();
            BeginWriteAttribute("aria-valuenow", " aria-valuenow=\"", 4255, "\"", 4282, 1);
#line 149 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
WriteAttributeValue("", 4271, percentage, 4271, 11, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(4283, 147, true);
            WriteLiteral(" aria-valuemin=\"0\" aria-valuemax=\"100\"></div>\r\n                    </div>\r\n                  </div>\r\n                </div>\r\n              </div>\r\n");
            EndContext();
#line 154 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
            }
          }

#line default
#line hidden
            BeginContext(4458, 275, true);
            WriteLiteral(@"        </div>
      </div>
    </div>
    <!--/.col-->
  </div>

  <!--/.row-->
  <div class=""row"">
    <div class=""col-md-12"">
      <div class=""card"">
        <div class=""card-header"">
          Requests by URL
        </div>
        <div class=""card-body"">
");
            EndContext();
#line 170 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
            
            var servedByCountry = await _store.ServedByCountry(from, to);
          

#line default
#line hidden
            BeginContext(4835, 364, true);
            WriteLiteral(@"
          <div class=""row"">
            <div class=""col-4"">
              <b>Country</b>
            </div>
            <div class=""col-2"">
              <b>Requests served</b>
            </div>
            <div class=""col-2"">
              <b>Percentage</b>
            </div>
            <div class=""col-4"">
            </div>
          </div>

");
            EndContext();
#line 188 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
           foreach (var served in servedByCountry)
          {
            var percentage = (served.Served * 100) / total;

            if (percentage >= 5)
            {

#line default
#line hidden
            BeginContext(5376, 70, true);
            WriteLiteral("              <div class=\"row\">\r\n                <div class=\"col-4\">\r\n");
            EndContext();
#line 196 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
                    
                    var country = $"{served.Country}.png";
                  

#line default
#line hidden
            BeginContext(5549, 22, true);
            WriteLiteral("                  <img");
            EndContext();
            BeginWriteAttribute("src", " src=\"", 5571, "\"", 5596, 2);
            WriteAttributeValue("", 5577, "/img/flags/", 5577, 11, true);
#line 199 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
WriteAttributeValue("", 5588, country, 5588, 8, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginWriteAttribute("alt", " alt=\"", 5597, "\"", 5611, 1);
#line 199 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
WriteAttributeValue("", 5603, country, 5603, 8, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(5612, 43, true);
            WriteLiteral(" style=\"height: 24px;\">\r\n                  ");
            EndContext();
            BeginContext(5656, 14, false);
#line 200 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
             Write(served.Country);

#line default
#line hidden
            EndContext();
            BeginContext(5670, 81, true);
            WriteLiteral("\r\n                </div>\r\n                <div class=\"col-2\">\r\n                  ");
            EndContext();
            BeginContext(5752, 13, false);
#line 203 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
             Write(served.Served);

#line default
#line hidden
            EndContext();
            BeginContext(5765, 81, true);
            WriteLiteral("\r\n                </div>\r\n                <div class=\"col-2\">\r\n                  ");
            EndContext();
            BeginContext(5847, 10, false);
#line 206 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
             Write(percentage);

#line default
#line hidden
            EndContext();
            BeginContext(5857, 221, true);
            WriteLiteral("%\r\n                </div>\r\n                <div class=\"col-4\">\r\n                  <div class=\"bars\">\r\n                    <div class=\"progress \">\r\n                      <div class=\"progress-bar bg-info\" role=\"progressbar\"");
            EndContext();
            BeginWriteAttribute("style", " style=\"", 6078, "\"", 6105, 3);
            WriteAttributeValue("", 6086, "width:", 6086, 6, true);
#line 211 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
WriteAttributeValue(" ", 6092, percentage, 6093, 11, false);

#line default
#line hidden
            WriteAttributeValue("", 6104, "%", 6104, 1, true);
            EndWriteAttribute();
            BeginWriteAttribute("aria-valuenow", " aria-valuenow=\"", 6106, "\"", 6133, 1);
#line 211 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
WriteAttributeValue("", 6122, percentage, 6122, 11, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(6134, 147, true);
            WriteLiteral(" aria-valuemin=\"0\" aria-valuemax=\"100\"></div>\r\n                    </div>\r\n                  </div>\r\n                </div>\r\n              </div>\r\n");
            EndContext();
#line 216 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
            }
          }

#line default
#line hidden
            BeginContext(6309, 68, true);
            WriteLiteral("        </div>\r\n      </div>\r\n    </div>\r\n  </div>\r\n</section>\r\n\r\n\r\n");
            EndContext();
#line 225 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
  

      var daily = (await _store.DailyServed(from, to)).OrderBy(x => x.Day);
      var hourly = (await _store.HourlyServed(from, to)).OrderBy(x => x.Hour);

      var dayPoints = string.Join(',', daily.Select(x => x.Served.ToString()));
      var dayLabels = string.Join(',', daily.Select(x => "'" + x.Day.ToString("dd/MM") + "'"));

      var hourPoints = string.Join(',', hourly.Select(x => x.Served.ToString()));
      var hourLabels = string.Join(',', hourly.Select(x => "'" + x.Hour.ToString() + "'"));
    

#line default
#line hidden
            BeginContext(6905, 1363, true);
            WriteLiteral(@"
<script>
            function getConfig(labels, points, description)
    {
      return {
        type: 'line',
                    data: {
          labels: labels,
                        datasets: [{
            label: description,
                                data: points,
                                fill: true,
                                cubicInterpolationMode: 'monotone'
                            }]
                        },
                        options: {
          responsive: true,
                            tooltips: {
            mode: 'index'
                            },
                            scales: {
            xAxes: [{
              display: true,
                                    scaleLabel: {
                display: true
                                    }
            }],
                                yAxes: [{
              display: true,
                                    scaleLabel: {
                display: true,
       ");
            WriteLiteral(@"                                 labelString: 'Value'
                                    }
            }]
                            }
        }
      }
    }

    window.onload = function ()
    {
      var ctxDay = document.getElementById('canvasByDay').getContext('2d');
      window.dayLine = new Chart(ctxDay, getConfig([");
            EndContext();
            BeginContext(8269, 19, false);
#line 278 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
                                               Write(Html.Raw(dayLabels));

#line default
#line hidden
            EndContext();
            BeginContext(8288, 4, true);
            WriteLiteral("], [");
            EndContext();
            BeginContext(8293, 9, false);
#line 278 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
                                                                       Write(dayPoints);

#line default
#line hidden
            EndContext();
            BeginContext(8302, 176, true);
            WriteLiteral("], \'Number of request served by day\'));\r\n\r\n      var ctxHour = document.getElementById(\'canvasByHour\').getContext(\'2d\');\r\n      window.hourLine = new Chart(ctxHour, getConfig([");
            EndContext();
            BeginContext(8479, 20, false);
#line 281 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
                                                 Write(Html.Raw(hourLabels));

#line default
#line hidden
            EndContext();
            BeginContext(8499, 4, true);
            WriteLiteral("], [");
            EndContext();
            BeginContext(8504, 10, false);
#line 281 "D:\PROGETTI\sito\AspNetCore2CoreUI-2.0\src\CoreUI.Web\Views\Home\Stat.cshtml"
                                                                          Write(hourPoints);

#line default
#line hidden
            EndContext();
            BeginContext(8514, 59, true);
            WriteLiteral("], \'Number of request served by hour\'));\r\n    };\r\n</script>");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public IAnalyticStore _store { get; private set; }
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
