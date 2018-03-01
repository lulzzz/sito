using Maddalena.Markdig.Renderers.Normalize;

namespace Maddalena.Markdig.Extensions.JiraLinks
{
    public class NormalizeJiraLinksRenderer : NormalizeObjectRenderer<JiraLink>
    {
        protected override void Write(NormalizeRenderer renderer, JiraLink obj)
        {
            renderer.Write(obj.ProjectKey);
            renderer.Write("-");
            renderer.Write(obj.Issue);
        }
    }
}
