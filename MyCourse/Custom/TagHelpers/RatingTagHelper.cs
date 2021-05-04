using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MyCourse.Custom.TagHelpers
{
    // seguo la convezione per il nome 'RatingTagHelper', in quanto il tag si chiama 'rating'
    public class RatingTagHelper : TagHelper
    {
        // proprietà a cui viene assengato automaticamente tramite una sorta di ModelBinding il valore
        // dell'attributo 'value' del Tag Helper personalizzato
        public double Value { get; set; }

        // sovrascrivo metodo che viene chiamato quando la classe RatingTagHelper viene chiamata
        // per produrre output HTML
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // prendo il valore di 'value' che è la proprietà che ho definito io nel tag personalizzato,
            // in cui è presente il valore in decimale del corso, il suo rating (es. 4.6).
            // double value = (double) context.AllAttributes["value"].Value;
            // Posso fare la stessa cosa in modo più immediato definendo, come sopra, la proprità Value
            // a cui viene assegnato automaticamente(tipo ModelBinding) il valore di 'value' passato nel
            // tag helper personalizzato

            for(int i=1; i<=5; i++)
            {
                if(Value >= i)
                {
                    // uso oggetto 'output' per mandare appunto in output il codice HTML
                    output.Content.AppendHtml("<i class=\"fas fa-star\"></i>");
                }
                else if(Value > i-1)
                {
                    output.Content.AppendHtml("<i class=\"fas fa-star-half-alt\"></i>");
                    
                }
                else 
                {
                    output.Content.AppendHtml("<i class=\"far fa-star\"></i>");
                    
                }
            }  
        }
    }
}