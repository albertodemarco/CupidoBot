using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CupidoBotV1.Services
{
    public class FaceService
    {
        public static async Task<Face[]> findFaceInfo(string faceUrl)
        {
            string visionKey = ConfigurationManager.AppSettings["VisionKeyWestUs"];
            string faceapiUrl = ConfigurationManager.AppSettings["CognitiveServiceApiEndpoint"] + "face/v1.0";
            IFaceServiceClient faceServiceClient = new FaceServiceClient(visionKey, faceapiUrl);
            Face[] faces;                   // The list of detected faces.           
            IEnumerable<FaceAttributeType> faceAttributes = new FaceAttributeType[] { FaceAttributeType.Gender, FaceAttributeType.Age, FaceAttributeType.Smile, FaceAttributeType.Emotion, FaceAttributeType.Glasses, FaceAttributeType.Hair, FaceAttributeType.Accessories, FaceAttributeType.Blur, FaceAttributeType.FacialHair, FaceAttributeType.HeadPose, FaceAttributeType.Makeup, FaceAttributeType.Noise, FaceAttributeType.Occlusion };           
            // Call the Face API.
            faces = await faceServiceClient.DetectAsync(faceUrl, returnFaceId: true, returnFaceLandmarks: false, returnFaceAttributes: faceAttributes);
            return faces;
        }
        public static string faceDescription(Face face)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("gender: ");
            // Add the gender, age, and smile.
            sb.Append(face.FaceAttributes.Gender);
            sb.Append(", age: ");
            sb.Append(face.FaceAttributes.Age);
            sb.Append(", ");
            sb.Append(String.Format("smile {0:F1}%, ", face.FaceAttributes.Smile * 100));
            // Add the emotions. Display all emotions over 10%.
            sb.Append("Emotion: ");
            EmotionScores emotionScores = face.FaceAttributes.Emotion;
            if (emotionScores.Anger >= 0.1f) sb.Append(String.Format("anger {0:F1}%, ", emotionScores.Anger * 100));
            if (emotionScores.Contempt >= 0.1f) sb.Append(String.Format("contempt {0:F1}%, ", emotionScores.Contempt * 100));
            if (emotionScores.Disgust >= 0.1f) sb.Append(String.Format("disgust {0:F1}%, ", emotionScores.Disgust * 100));
            if (emotionScores.Fear >= 0.1f) sb.Append(String.Format("fear {0:F1}%, ", emotionScores.Fear * 100));
            if (emotionScores.Happiness >= 0.1f) sb.Append(String.Format("happiness {0:F1}%, ", emotionScores.Happiness * 100));
            if (emotionScores.Neutral >= 0.1f) sb.Append(String.Format("neutral {0:F1}%, ", emotionScores.Neutral * 100));
            if (emotionScores.Sadness >= 0.1f) sb.Append(String.Format("sadness {0:F1}%, ", emotionScores.Sadness * 100));
            if (emotionScores.Surprise >= 0.1f) sb.Append(String.Format("surprise {0:F1}%, ", emotionScores.Surprise * 100));
            // Add glasses.
            sb.Append(face.FaceAttributes.Glasses);
            sb.Append(", ");
            // Add Accessories.
            sb.Append("Accessories: ");
            sb.Append(", ");
            Accessory[] acc = face.FaceAttributes.Accessories;
            foreach (Accessory c in acc)
            {
                if (c.Confidence >= 0.1f)
                {
                    sb.Append(c.Type.ToString());
                    sb.Append(String.Format(" {0:F1}% ", c.Confidence * 100));
                }
            }

            // Add hair.
            sb.Append("Hair: ");
            // Display baldness confidence if over 10%.
            if (face.FaceAttributes.Hair.Bald >= 0.1f)
                sb.Append(String.Format("bald {0:F1}% ", face.FaceAttributes.Hair.Bald * 100));
            // Display all hair color attributes over 10%.
            HairColor[] hairColors = face.FaceAttributes.Hair.HairColor;
            foreach (HairColor hairColor in hairColors)
            {
                if (hairColor.Confidence >= 0.1f)
                {
                    sb.Append(hairColor.Color.ToString());
                    sb.Append(String.Format(" {0:F1}% ", hairColor.Confidence * 100));
                }
                //For brevity we break at the first one to have just one output
                break;
            }
            // Return the built string.
            return sb.ToString();
        }
    }
}