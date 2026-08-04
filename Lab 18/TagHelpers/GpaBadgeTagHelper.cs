
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Globalization;

namespace StudentPortalWeb.TagHelpers
{

    [HtmlTargetElement("gpa-badge" , TagStructure = TagStructure.WithoutEndTag)]
    public class GpaBadgeTagHelper : TagHelper
    {
        public double For { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string cssClass;
            string label;
            if(For >= 3.5) { cssClass = "bg-success"; label = "First"; }
            else if (For >= 3.0) { cssClass = "bg-primary"; label = "Second"; }
            else { cssClass = "bg-secondary"; label = "Pass"; }

            output.TagName = "span";
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.SetAttribute("class", $"badge {cssClass}");
            output.Content.SetContent($"{For.ToString("F2", CultureInfo.InvariantCulture)} - {label}");
        }
    }
}

#region 📋 Full TODO Checklist
// ---------------------------------------------------------------------
// Views/Students/Index.cshtml
//   TODO 1: A Razor code block, a printed summary, and an empty guard   [Block 1]
//   TODO 4: Replace the loop body with a call to the partial            [Block 3]
//
// Views/Shared/_Layout.cshtml
//   TODO 2: Add an optional named section to the layout                 [Block 2]
//
// Views/Students/Details.cshtml
//   TODO 3: Fill that section from one page only                        [Block 2]
//
// Views/Shared/_StudentRow.cshtml   (new file)
//   TODO 5: Build the partial: one strongly-typed table row             [Block 3]
//   TODO 6: Swap the typed URL for real tag helpers                     [Block 4]
//   TODO 9: Use your own tag helper in the GPA cell                     [Block 5]
//
// TagHelpers/GpaBadgeTagHelper.cs  (this file)
//   TODO 7: Write a tag helper the framework will call                  [Block 5]
//
// Views/_ViewImports.cshtml
//   TODO 8: Register your tag helper so Razor can see it                [Block 5]
// ---------------------------------------------------------------------
#endregion
