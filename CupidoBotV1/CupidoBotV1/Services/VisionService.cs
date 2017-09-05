using Microsoft.IdentityModel.Protocols;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CupidoBotV1.Services
{
    public class VisionService
    {
        public static async Task<AnalysisResult> getImageAnalysis(string faceUrl)
        {
            string visionKey = ConfigurationManager.AppSettings["VisionKeyWestUs"];
            string visionUrl = ConfigurationManager.AppSettings["CognitiveServiceApiEndpoint"] + "vision/v1.0";
            VisionServiceClient cli = new VisionServiceClient(visionKey, visionUrl);
            var AnalsysResult = await cli.DescribeAsync(faceUrl);
            return AnalsysResult;
        }
    }
}