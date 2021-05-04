using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MyCourse.Models.InputModels;

namespace MyCourse.Custom.TagHelpers
{
    // AnchorTagHelper permette di generare i link anchor (<a></a>)
    public class OrderLinkTagHelper : AnchorTagHelper
    {
        public string OrderBy { get; set; }
        public CourseListInputModel Input { get; set; }

        public OrderLinkTagHelper(IHtmlGenerator generator) : base(generator)
        {
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";

            //Imposto i valori del link
            RouteValues["search"] = Input.Search;
            RouteValues["orderby"] = OrderBy;
            RouteValues["ascending"] = (Input.OrderBy == OrderBy ? !Input.Ascending : Input.Ascending).ToString().ToLowerInvariant();
            
            //Faccio generare l'output all'AnchorTagHelper
            base.Process(context, output);

            //Aggiungo l'indicatore di direzione
            if (Input.OrderBy == OrderBy)
            {
                // aggiungo l'icona di font-awesome che da la direzione dell'ordinamento
                var direction = Input.Ascending ? "up" : "down";
                output.PostContent.SetHtmlContent($" <i class=\"fas fa-caret-{direction}\"></i>");
            }
        }
    }
}