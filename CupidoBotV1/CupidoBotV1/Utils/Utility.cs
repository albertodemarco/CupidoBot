using CupidoBotV1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CupidoBotV1.Services;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace CupidoBotV1.Utils
{
    public class Utility
    {
        public static CupidoProfile mapDbProfile(FbResponse fb)
        {
            CupidoProfile cp = new CupidoProfile();
            cp.FirstName = fb.first_name;
            cp.LastName = fb.last_name;
            cp.Gender = fb.gender;
            cp.picUrl = fb.profile_pic;
            return cp;
        }

        public static async Task<string> findImageDescription(string faceUrl)
        {
            var AnalsysResult = await VisionService.getImageAnalysis(faceUrl);
            return AnalsysResult.Description.Captions[0].Text;
        }






        public static void populateSuperModel(IMessageActivity replyToConversation, CupidoProfile cp, string description)
        {
            replyToConversation.AttachmentLayout = AttachmentLayoutTypes.List;
            replyToConversation.Attachments = new List<Attachment>();
            string maleUrl = "http://3.bp.blogspot.com/_m03cGx-c3FM/TNCGmbFBJyI/AAAAAAAAAI0/cHHgduezFKY/s1600/seanopry6.jpg";
            string femaleUrl = "https://qph.ec.quoracdn.net/main-qimg-12faf75897651d6cc546fb4c0ccb862c-c";
            string maleKoUrl = "http://wwwimage1.cbsstatic.com/thumbnails/photos/w370/gallery/matt2.jpg";
            string femaleKoUrl = "https://i.pinimg.com/originals/b4/43/54/b4435420b49ea667241a0f4f1644c9e7.jpg";
            string okStr = "You look like a Super Model!";
            string koStr = "Sorry you don't look like a Super Model!";
            string finalStr = okStr;
            string finalUrl = maleUrl;
            if (cp.Gender == "female")
            {
                finalUrl = femaleUrl;
            }
            if (cp.NormalPoints > 50)
            {
                finalStr = koStr;
                finalUrl = maleKoUrl;
                if (cp.Gender == "female")
                {
                    finalUrl = femaleKoUrl;
                }
            }
            HeroCard plCard = new HeroCard()
            {
                Title = $"{finalStr}",
                Subtitle = $"{description} ",
                Images = new List<CardImage> { new CardImage(finalUrl) },
                Buttons = new List<CardAction> { new CardAction(ActionTypes.ImBack, title: "Find a match!", value: "Find a match!") }
            };
            Attachment plAttachment = plCard.ToAttachment();
            replyToConversation.Attachments.Add(plAttachment);
        }

        public static void populateCarusel(IMessageActivity replyToConversation, List<CupidoProfile> lst)
        {
            replyToConversation.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            replyToConversation.Attachments = new List<Attachment>();
            foreach (CupidoProfile cp in lst)
            {
                HeroCard plCard = new HeroCard()
                {
                    Title = $"{cp.FirstName} {cp.LastName}",
                    Subtitle = $"{cp.Age} years old",
                    Images = new List<CardImage> { new CardImage(cp.picUrl) },
                    Buttons = new List<CardAction> { new CardAction(ActionTypes.ImBack, title: "Ok!", value: $"I pick {cp.UserId} on channel {cp.ChannelId}") }
                };
                Attachment plAttachment = plCard.ToAttachment();
                replyToConversation.Attachments.Add(plAttachment);
            }
        }
    }
}