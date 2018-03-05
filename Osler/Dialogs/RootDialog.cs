using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;


namespace Osler.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        protected Boolean IsFirstPrompt;
        protected readonly Boolean MultipleSelection;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
            //await this.InitialPrompt(context);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            //await this.InitialPrompt(context);
            var message = await result;
            //await this.InitialPrompt(context);

            await context.PostAsync($"What would you like to do? Ex: *compare cost*, *compare volume*, etc...");
            context.Wait(this.ActOnSearchResults);
        }

        protected virtual Task InitialPrompt(IDialogContext context)
        {
            string prompt = "What would you like to do? Ex: *compare cost*, *compare volume*, etc...";

            if (!this.IsFirstPrompt)
            {
                prompt = "What else would you like to do? Ex: *compare cost*, *compare volume*, etc...";
                //if (this.MultipleSelection)
                //{
                //    prompt += " You can also *list* all items you've added so far.";
                //}
            }

            this.IsFirstPrompt = false;
            PromptDialog.Text(context, this.Search, prompt);
            return Task.CompletedTask;
        }

        public async Task Search(IDialogContext context, IAwaitable<object> input)
        {
        }

        private async Task ActOnSearchResults(IDialogContext context, IAwaitable<IMessageActivity> input)
        {
            var activity = await input;
            var choice = activity.Text;

            switch (choice.ToLowerInvariant())
            {
                case "compare cost":
                    await context.PostAsync($"Yes of course! What would you like to compare? Ex: FY1 vs FY2, BU1 vs BU2, Billable vs Non Billable, etc..");
                    context.Call(new TravelyseDialog(), this.Search);
                    break;
                case "compare volume":
                    await context.PostAsync($"sorry this is not implemented yet");
                    await this.InitialPrompt(context);
                    break;
            }
        }
    }
}