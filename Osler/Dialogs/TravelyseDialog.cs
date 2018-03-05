using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Osler.BusinessLayer;
using Microsoft.Bot.Connector;


using Outlook = Microsoft.Office.Interop.Outlook;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Osler.Dialogs
{
    [Serializable]
    public class TravelyseDialog : LuisDialog<object>
    {
        public TravelyseDialog() : base(new LuisService(new LuisModelAttribute(
                ConfigurationManager.AppSettings["LuisAppId"],
                ConfigurationManager.AppSettings["LuisAPIKey"]
                )))
        {
        }

        [LuisIntent("Travelyse.SpendOverTime")]
        public async Task TravelyseSpendOverTimeIntent(IDialogContext context, LuisResult result)
        {
            SpendOverTime st = new SpendOverTime();
            List<String> lResponse = st.SetReponse(result);
            String strFinalResponse = String.Empty;

            foreach (String strResponse in lResponse)
                strFinalResponse = strFinalResponse + strResponse;

            await context.PostAsync($"" + strFinalResponse);

            var reply = context.MakeMessage();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = st.GetKeyInsightsCard();
            
            await context.PostAsync(reply);

            context.Wait(MessageReceived);
        }

        [LuisIntent("Travelyse.Insights.BU")]
        public async Task InsightsBUIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Please find the key insights");

            BUCostInsights objBUCostInsights = new BUCostInsights();
            var reply = context.MakeMessage();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = objBUCostInsights.GetKeyInsightsCard();
            
            await context.PostAsync(reply);
            
            context.Wait(ChartOptions);
        }

        private async Task ChartOptions(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result;
            var choice = activity.Text;

            Thread t = new Thread(CopyToClipboard);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();

            
            //await context.PostAsync(temp.ToString());
            context.Wait(MessageReceived);
        }

        private void CopyToClipboard()
        {
            // Load the webpage into a WebBrowser control
            WebBrowser wb = new WebBrowser();
            wb.ScrollBarsEnabled = false;
            wb.ScriptErrorsSuppressed = true;
            wb.Navigate("https://app.powerbi.com/view?r=eyJrIjoiYmJlMTk5ZjItY2M3MS00ODE1LWIwODItNTkwNDc4YjVlZjZlIiwidCI6ImM0OWEyMzk1LWQxNTMtNDljYy04MjA4LTAyMjQyNzQ1ZWYyMSIsImMiOjh9");
            System.Threading.Thread.Sleep(10000);

            while (wb.ReadyState != WebBrowserReadyState.Complete) { Application.DoEvents(); }

            wb.Width = wb.Document.Body.ScrollRectangle.Width;
            wb.Height = wb.Document.Body.ScrollRectangle.Height;

            Bitmap bitmap = new Bitmap(wb.Width, wb.Height);
            wb.DrawToBitmap(bitmap, new Rectangle(0, 0, wb.Width, wb.Height));
            wb.Dispose();

            var stream = new MemoryStream();
            bitmap.Save(@"C:\Users\admin\Pictures\Test\TestFilename.PNG", ImageFormat.Png);
            stream.Position = 0;

            Outlook.Application oApp = new Outlook.Application();
            Outlook._MailItem oMailItem = (Outlook._MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
            oMailItem.To = "pavithra.jayarajan@arimonconsulting.com";
            oMailItem.Subject = "Test";

            
            //oMailItem.Attachments.Add(new System.Net.Mail.Attachment(stream, "image/jpg"));
           // oMailItem.Attachments.Add(new System.Net.Mail.Attachment(stream, "image/jpg"));
            oMailItem.Attachments.Add(new System.Net.Mail.Attachment(@"C:\Users\admin\Pictures\Test\TestFilename.PNG"));

            oMailItem.Display(true);
        }

        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Sorry I cannot understand you. Could you rephrase your query please?");
            context.Wait(MessageReceived);
        }
        
        [LuisIntent("Greeting")]
        public async Task GreetingIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Hello! How can I help you today?");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Help")]
        public async Task HelpIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"You can ask me queries related to dashboard");
            context.Wait(MessageReceived);
        }

        [LuisIntent("SignOff")]
        public async Task SignOffIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Thanks for using Osler!");
            context.Done(this);
        }        
    }
}
