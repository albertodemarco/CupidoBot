using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Threading.Tasks;
using CupidoBotV1.Data;
using Microsoft.Cognitive.CustomVision;

namespace CupidoBotV1.Services
{
    public class CustomVisionService
    {
        public static async Task<ArrayList> customVisionPredictions(string imageUrl)
        {
            ArrayList lst = new ArrayList();
            CupidoProfile cp = new CupidoProfile();
            string retVal = String.Empty;
            string predictionKey = ConfigurationManager.AppSettings["CustomVisionKey"];
            string iterationId = ConfigurationManager.AppSettings["CustomVisionIterationId"];
            string projectId = ConfigurationManager.AppSettings["ProjectIdKey"];
            string customVisionUrl = ConfigurationManager.AppSettings["CustomVisionUrl"];
            Guid projeczIdGuid = new Guid(projectId);
            Guid iterationGuid = new Guid(iterationId);
            Microsoft.Cognitive.CustomVision.Models.ImageUrl url = new Microsoft.Cognitive.CustomVision.Models.ImageUrl(imageUrl);
            PredictionEndpointCredentials predictionEndpointCredentials = new PredictionEndpointCredentials(predictionKey);
            PredictionEndpoint endpoint = new PredictionEndpoint(predictionEndpointCredentials);
            endpoint.BaseUri = new Uri(customVisionUrl);
            Microsoft.Cognitive.CustomVision.Models.ImagePredictionResultModel result = await endpoint.PredictImageUrlAsync(projeczIdGuid, url, iterationGuid);
            int counter = 0;
            foreach (var c in result.Predictions)
            {
                retVal = retVal + $"{c.Tag}: {c.Probability:P1}";
                if (c.Tag.Contains("Model1"))
                {
                    cp.SuperModelPoints = System.Convert.ToInt32(Math.Round(c.Probability * 100, 0));
                }
                else
                {
                    cp.NormalPoints = System.Convert.ToInt32(Math.Round(c.Probability * 100, 0));
                }
                counter++;
            }
            retVal = retVal.Replace("Model2", " Normal ");
            retVal = retVal.Replace("Model1", " Super Model ");
            lst.Add(retVal);
            lst.Add(cp);
            return lst;

        }
    }
}