using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace Osler.BusinessLayer
{
    [Serializable]
    public class BUCostInsights
    {
        List<KeyInsights> lKeyInsights;

        public List<Attachment> GetKeyInsightsCard()
        {
            List<Attachment> lInsights = new List<Attachment>();

            GetKeyInsights();
            Int32 iCounter = 0;

            foreach (var vInsight in lKeyInsights)
            {
                List<CardAction> list = new List<CardAction>();
                //list.Add(new CardAction { Title = "View in Browser", Type = ActionTypes.OpenUrl, Value = "View" });
                list.Add(new CardAction { Title = "Share via Email", Type = ActionTypes.ImBack, Value = "SendEmail" });
                HeroCard hero = new HeroCard();
                hero.Title = vInsight.Title;
                hero.Text = vInsight.Description;
                hero.Images = new List<CardImage> { new CardImage(vInsight.PictureUrl) };
                hero.Tap = new CardAction(ActionTypes.OpenUrl, "View Chart", value: vInsight.TapUrl);
                hero.Buttons = list;

                lInsights.Add(hero.ToAttachment());
            }

            return lInsights;
        }

        private void GetKeyInsights()
        {
            lKeyInsights = new List<KeyInsights>();

            var YoYimageData = Convert.ToBase64String(File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Images/BUInsights/YoYComparison.png")));
            var EBimageData = Convert.ToBase64String(File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Images/BUInsights/EBDist.png")));
            var GDimageData = Convert.ToBase64String(File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Images/BUInsights/OriginDist.png")));

            lKeyInsights.Add(new KeyInsights
            {
                Title = "Year on Year Comparison",
                Subtitle = "",
                TapUrl = "https://app.powerbi.com/view?r=eyJrIjoiYmJlMTk5ZjItY2M3MS00ODE1LWIwODItNTkwNDc4YjVlZjZlIiwidCI6ImM0OWEyMzk1LWQxNTMtNDljYy04MjA4LTAyMjQyNzQ1ZWYyMSIsImMiOjh9",
                PictureUrl = $"data:image/png;base64,{YoYimageData}",
                Description = "Travel spend in 2016 is 3.9x the travel spend in 2017"
            });

            lKeyInsights.Add(new KeyInsights
            {
                Title = "Employee Band Distribution",
                Subtitle = "",
                TapUrl = "https://app.powerbi.com/view?r=eyJrIjoiMzI5ZDlkMTQtNzNlZS00MmRjLWIwOTYtNGFmZTgzMTUzZTU3IiwidCI6ImM0OWEyMzk1LWQxNTMtNDljYy04MjA4LTAyMjQyNzQ1ZWYyMSIsImMiOjh9",
                PictureUrl = $"data:image/png;base64,{EBimageData}",
                Description = "Bands 4,5,6,7 make up 80% of the travel spend"
            });

            lKeyInsights.Add(new KeyInsights
            {
                Title = "Geo distribution for origin country",
                Subtitle = "",
                TapUrl = "https://app.powerbi.com/view?r=eyJrIjoiYmE4M2U0ZjAtZTljNC00OGU2LWFmODYtNDg5NDc2OTcxMzQ0IiwidCI6ImM0OWEyMzk1LWQxNTMtNDljYy04MjA4LTAyMjQyNzQ1ZWYyMSIsImMiOjh9",
                PictureUrl = "http://104.211.189.58/arimon/wp-content/uploads/2017/04/OriginDistribution.png",
                Description = "67% of the total trips originate in India; and lead to 54% of the total spend"
            });
        }
    }
}