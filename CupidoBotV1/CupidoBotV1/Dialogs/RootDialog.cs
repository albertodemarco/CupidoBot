using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Configuration;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using Microsoft.ProjectOxford.Common.Contract;
using System.Text;
using Microsoft.Cognitive.CustomVision;
using Microsoft.ProjectOxford.Vision;
using System.Data.SqlClient;
using System.Collections;
using CupidoBotV1.Data;
using CupidoBotV1.Services;
using CupidoBotV1.Utils;

namespace CupidoBotV1.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private FbResponse fbProfile=null;
        private CupidoProfile cpProfile = null;
        
        public Task StartAsync(IDialogContext context)
        {            
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            IBotDataBag pr = context.PrivateConversationData;
            bool found=pr.TryGetValue<FbResponse>("fbResponse", out fbProfile);         
            var wrp = await result as Activity;        
            if (found)
            {
                if (cpProfile == null || cpProfile.Gender ==null)
                {
                    cpProfile = Utility.mapDbProfile(fbProfile);
                    cpProfile.UserId = context.Activity.From.Id;
                    cpProfile.ChannelId = context.Activity.ChannelId;
                    if (wrp.Text.Equals("Sure!"))
                    {
                        string imageDesc = await Utility.findImageDescription(fbProfile.profile_pic);
                        await context.PostAsync($"Nice let's start! Looking now at your picture. I can see : " + imageDesc);
                        Face[] fc = await FaceService.findFaceInfo(fbProfile.profile_pic);
                        string fcInfo = String.Empty;
                        foreach (Face f in fc)
                        {
                            fcInfo = fcInfo + FaceService.faceDescription(f);
                            cpProfile.Age = System.Convert.ToInt32(Math.Round(f.FaceAttributes.Age, 0));
                            cpProfile.Gender = f.FaceAttributes.Gender;
                            cpProfile.HairBald = (f.FaceAttributes.Hair.Bald > 0.5) ? 1 : 0;
                        }
                        await context.PostAsync("From your face I can see that: " + fcInfo);
                        ArrayList customVision = await CustomVisionService.customVisionPredictions(fbProfile.profile_pic);
                        cpProfile.SuperModelPoints = (customVision[1] as CupidoProfile).SuperModelPoints;
                        cpProfile.NormalPoints = (customVision[1] as CupidoProfile).NormalPoints;
                        await SqlAdapter.saveData(cpProfile);                        
                        var messageModel = context.MakeMessage();
                        Utility.populateSuperModel(messageModel, cpProfile, customVision[0] as string);
                        await context.PostAsync(messageModel);
                    }
                }
                else if (wrp.Text.Equals("Find a match!"))
                {
                    await context.PostAsync("Let me find some candidates...");
                    //Find candidates
                    List<CupidoProfile> lst = SqlAdapter.getSimilarProfiles(cpProfile);
                    if (lst.Count > 0)
                    {
                        var messageSel = context.MakeMessage();
                        Utility.populateCarusel(messageSel, lst);
                        await context.PostAsync(messageSel);
                    }
                    else
                    {
                        await context.PostAsync("Sorry still no candidates for you, but come back and try again!");
                    }
                }
                else if (wrp.Text.Equals("No thanks"))
                {
                    await context.PostAsync($"Sorry");
                }
                else if (wrp.Text.StartsWith("I pick "))
                {
                    await context.PostAsync($"Good Choice!");
                }
                else
                {
                    await context.PostAsync($"Welcome Back! Resetting your data to start again");
                    pr.RemoveValue("fbResponse");
                    cpProfile = null;

                }
            }
            else
            {
                    FbResponse resp;                   
                    if (wrp.ChannelId == "facebook")
                    {
                        resp = await FacebookProfile.findInfoFromFb(wrp.From.Id);                        
                    }
                    else
                    {
                       // Fake data to be used into the emulator for testing
                        resp = new FbResponse();
                        resp.first_name = "Bot Name";
                        resp.last_name = "Bot Last Name";
                        resp.profile_pic = "http://wwwimage2.cbsstatic.com/thumbnails/photos/w370/gallery/matt2.jpg";

                    }
                    pr.SetValue<FbResponse>("fbResponse", resp);
                     var heroCard = new HeroCard
                    {
                        Title = "Cupido is here!",
                        Subtitle = "Find the perfect match!",
                        Text = $"Hello {resp.first_name}! Do you agree to continue?",
                        Images = new List<CardImage> { new CardImage(resp.profile_pic) },
                        Buttons = new List<CardAction> { new CardAction(ActionTypes.ImBack, title: "Sure!", value: "Sure!"), new CardAction(ActionTypes.ImBack, title: "No thanks", value: "No thanks") }
                    };    
                    var message = context.MakeMessage();
                    message.Attachments.Add(heroCard.ToAttachment());
                    await context.PostAsync(message);                  
                    fbProfile = resp;  
            }
            context.Wait(MessageReceivedAsync);
        }         
      
    }
}