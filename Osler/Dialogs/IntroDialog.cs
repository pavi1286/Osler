using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Osler.Dialogs
{
    [Serializable]
    public class IntroDialog : IDialog<object>
    {
        private const string OptionCost = "Cost";
        private const string OptionVolume = "Volume";

        private const string OptionCompare = "Compare";
        private const string OptionView = "View";

        private const string OptionBU = "BU";
        private const string OptionTravelType = "TravelType - Domestic/International";
        private const string OptionUtilisation = "Utilisation - Billable/NonBillable";

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (message.Text.ToLower().Contains("help") || message.Text.ToLower().Contains("support") || message.Text.ToLower().Contains("problem"))
            {
                //await context.Forward(new TravelyseDialog(), this.ResumeAfterSupportDialog, message, CancellationToken.None);
            }
            else
            {
                this.ShowMainMenuOptions(context);
            }
        }

        private void ShowMainMenuOptions(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnMainMenuOptionSelected, new List<string>() { OptionCost, OptionVolume }, "What do you  want to analyse?", "Not a valid option", 3);            
        }

        private async Task OnMainMenuOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string optionSelected = await result;

                switch (optionSelected)
                {
                    case OptionCost:
                        PromptDialog.Choice(context, this.ResumeAfterCostOption, new List<String>() { OptionCompare, OptionView }, "What do you want to do with Cost?", "Not a valid option", 3);
                        break;

                    case OptionVolume:
                        PromptDialog.Choice(context, this.ResumeAfterVolumeOption, new List<String>() { OptionCompare, OptionView }, "What do you want to do with Volume?", "Not a valid option", 3);
                        break;
                }
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync($"Ooops! Too many attemps :(. But don't worry, I'm handling that exception and you can try again!");

                context.Wait(this.MessageReceivedAsync);
            }
        }

        private async Task ResumeAfterCostOption(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string optionSelected = await result;

                switch (optionSelected)
                {
                    case OptionCompare:
                        PromptDialog.Choice(context, this.ResumeAfterCostOption, new List<String>() { OptionBU, OptionTravelType, OptionUtilisation }, "What do you want to compare?", "Not a valid option", 3);
                        break;

                    case OptionView:
                        //PromptDialog.Choice(context, this.ResumeAfterVolumeOption, new List<String>() { OptionCompare, OptionView }, "What do you want to do with Volume?", "Not a valid option", 3);
                        await context.PostAsync($"This functionality is not implemented yet.");
                        break;
                }
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync($"Ooops! Too many attemps :(. But don't worry, I'm handling that exception and you can try again!");

                context.Wait(this.MessageReceivedAsync);
            }
        }

        private async Task ResumeAfterVolumeOption(IDialogContext context, IAwaitable<string> result)
        {
            string optionSelected = await result;
        }
    }
}