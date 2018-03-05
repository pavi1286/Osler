using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Osler.Dialogs
{
    [Serializable]
    public class TestDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
            //await this.InitialPrompt(context);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var reply = context.MakeMessage();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = GetProfileHeroCard();

            await context.PostAsync(reply);
        }

        private List<Attachment> GetProfileHeroCard()
        {
            List<Attachment> lInsights = new List<Attachment>();

            for (int i = 0; i < 5; i++)
            {
                var vInsight = new HeroCard
                {
                    Title = "YoY Comparison ",
                    Subtitle = "Please click on the image to open chart in another window",
                    Tap = new CardAction(ActionTypes.OpenUrl, "View Chart", value: "https://app.powerbi.com/view?r=eyJrIjoiYmJlMTk5ZjItY2M3MS00ODE1LWIwODItNTkwNDc4YjVlZjZlIiwidCI6ImM0OWEyMzk1LWQxNTMtNDljYy04MjA4LTAyMjQyNzQ1ZWYyMSIsImMiOjh9"),
                    Text = "Travel spend in 2016 is 3.9x the travel spend in 2017",
                    Images = new List<CardImage> { new CardImage("https://azure.microsoft.com/svghandler/power-bi-embedded/?width=600&height=315") },
                    Buttons = new List<CardAction> { new CardAction(ActionTypes.DownloadFile, "Download Chart", value: "http://demo.digilytics.solutions/travel_dashboard") }
                }.ToAttachment();

                lInsights.Add(vInsight);
            }
            return lInsights;        
        }

        //public async Task ProcessSelectedOptionAsync(IDialogContext context, IAwaitable<string> argument)
        //{
        //    var message = await argument;

        //    var replyMessage = context.MakeMessage();

        //    Attachment attachment = null;

        //    switch (message)
        //    {
        //        case "1":
        //            attachment = GetInlineAttachment();
        //            break;
        //    }

        //    // The Attachments property allows you to send and receive images and other content
        //    //replyMessage.Attachments = new List<Attachment> { attachment };

        //    //await context.PostAsync(replyMessage);

        //    await this.DisplayOptionsAsync(context);
        //}

        //public async Task DisplayOptionsAsync(IDialogContext context)
        //{
        //    PromptDialog.Choice<string>(
        //        context,
        //        this.ProcessSelectedOptionAsync,
        //        this.options.Keys,
        //        "What sample option would you like to see?",
        //        "Ooops, what you wrote is not a valid option, please try again",
        //        3,
        //        PromptStyle.PerLine,
        //        this.options.Values);
        //}

        //private static Attachment GetInlineAttachment()
        //{
        //    //var imagePath = @"C:\Users\admin\Pictures\Saved Pictures" + @"\Screenshot.jpg";


        //    Bitmap memoryImage;
        //    memoryImage = new Bitmap(1000, 900);
        //    Size s = new Size(memoryImage.Width, memoryImage.Height);


        //    Graphics memoryGraphics = Graphics.FromImage(memoryImage);

        //    memoryGraphics.CopyFromScreen(0, 0, 0, 0, s);


        //    String str = String.Format(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
        //              @"\Screenshot.jpg");


        //    memoryImage.Save(str);

        //    var imagePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Screenshot.jpg";
        //    var imageData = Convert.ToBase64String(File.ReadAllBytes(imagePath));

        //    return new Attachment
        //    {
        //        Name = "Screenshot.jpg",
        //        ContentType = "image/jpg",
        //        ContentUrl = $"data:image/jpg;base64,{imageData}"
        //    };
        //}

    }
}