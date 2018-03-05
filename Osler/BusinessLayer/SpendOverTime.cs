using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Osler.DataLayer;

namespace Osler.BusinessLayer
{
    [Serializable]
    public class SpendOverTime
    {
        public DataConnection objDC = new DataConnection();
        private List<ResultEntity> lResult = new List<ResultEntity>();
        Double dTimes;
        SpendOverTimeEntity objSOT = new SpendOverTimeEntity();
        List<KeyInsights> lKeyInsights;


        public List<String> SetReponse(LuisResult result)
        {
            List<String> lResponse = new List<String>();
            GetEntities(result);

            if (lResult.Count == 0)
                lResponse.Add("Sorry, data not available for the provided time period. Please choose a different time period");
            else if (lResult.Count == 1)
            {
                string strYear = (lResult[0].TimePeriod == objSOT.TimePeriodStart) ? objSOT.TimePeriodEnd : objSOT.TimePeriodStart;
                lResponse.Add(String.Format("Sorry, data not available for {0}. Please choose a different time period", strYear));
            }
            else
            {
                GetComparison();
                if (lResult[0].TimePeriodType == "Year")
                {
                    lResponse.Add(String.Format("Travel spend for {0} is {1}x of {2}\n\n", lResult[0].TimePeriod, dTimes.ToString(), lResult[1].TimePeriod));
                    //foreach (var rs in lResult)
                    lResponse.Add(string.Format("(${0:#,,.}M vs ${1:#,,.}M)", lResult[0].Amount.ToString(),lResult[1].Amount.ToString()));
                }
            }

            return lResponse;
        }

        private void GetComparison()
        {
            dTimes = (Convert.ToDouble(lResult[0].Amount) > Convert.ToDouble(lResult[1].Amount)) ?
                        Convert.ToDouble(lResult[0].Amount) / Convert.ToDouble(lResult[1].Amount) :
                        Convert.ToDouble(lResult[1].Amount) / Convert.ToDouble(lResult[0].Amount);

            dTimes = Math.Round(dTimes, 1);
        }

        private void GetEntities(LuisResult result)
        {            
            String strEntity = String.Empty;

            var vEntities = result.Entities.ToList().Select(x => x).Where(x => x.Type == Constants.Year).ToList();

            if (vEntities.Any((entity) => entity.Type == Constants.Year))
            {
                objSOT.TimePeriodStart = result.Entities.ToList()[0].Entity;
                objSOT.TimePeriodEnd = result.Entities.ToList()[1].Entity;                
            }
            if (result.Entities.ToList().Select(x => x).Where(x => x.Type == Constants.Year).ToList().Count() > 0)
            {
                objSOT.TimePeriod = "Year";
            }

            lResult = objDC.GetSpenOverTimeData(
                                                 objSOT.TimePeriod, 
                                                 objSOT.TimePeriodStart, 
                                                 objSOT.TimePeriodEnd);           
        }

        public List<Attachment> GetKeyInsightsCard()
        {
            List<Attachment> lInsights = new List<Attachment>();

            GetKeyInsights();

            foreach (var vInsight in lKeyInsights)
            {
                lInsights.Add(new HeroCard
                {
                    Title = vInsight.Title,
                    Subtitle = vInsight.Subtitle,
                    Tap = new CardAction(ActionTypes.OpenUrl, "View Chart", value: vInsight.TapUrl),
                    Text = vInsight.Description,
                    Images = new List<CardImage> { new CardImage(vInsight.PictureUrl) },
                    Buttons = new List<CardAction> { new CardAction(ActionTypes.DownloadFile, "View in Browser", value: vInsight.TapUrl) }
                }.ToAttachment());
            }

            return lInsights;
        }

        private void GetKeyInsights()
        {
            lKeyInsights = new List<KeyInsights>();

            lKeyInsights.Add(new KeyInsights
            {
                Title = "FY Comparison",
                Subtitle = "",
                TapUrl = "https://app.powerbi.com/view?r=eyJrIjoiYzg2YzNjOTktZDRlMi00ZTY4LTgyNTYtMWY2ODM2N2RmOGEwIiwidCI6ImM0OWEyMzk1LWQxNTMtNDljYy04MjA4LTAyMjQyNzQ1ZWYyMSIsImMiOjh9",
                PictureUrl = $"data:image/png;base64,{Convert.ToBase64String(File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Images/BUInsights/FYComparison.png")))}",
                Description = "Travel spend for 2016 is 4.4x of 2017 ($71.1M vs $16.3M)"
            });
        }
        }
}