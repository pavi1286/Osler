using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;

namespace Osler.BusinessLayer
{
    [Serializable]
    public class KeyInsightsStyler
    {
        public void Apply<T>(ref IMessageActivity message, string prompt, IReadOnlyList<T> options, IReadOnlyList<string> descriptions = null, string speak = null)
        {
            var hits = options as IList<KeyInsights>;
            if (hits != null)
            {
                var cards = hits.Select(h => new ThumbnailCard
                {
                    Title = h.Title,
                    Images = new[] { new CardImage(h.PictureUrl) },
                    Buttons = new[] { new CardAction(ActionTypes.ImBack, "Pick this one", value: h.Key) },
                    Text = h.Description
                });

                message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                message.Attachments = cards.Select(c => c.ToAttachment()).ToList();
                message.Text = prompt;
                message.Speak = speak;
            }
            else
            {
                //base.Apply<T>(ref message, prompt, options, descriptions, speak);
            }
        }
    }
}