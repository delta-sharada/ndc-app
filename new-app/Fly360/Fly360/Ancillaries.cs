using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Fly360
{
    public class Datum
    {

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class LegFares
    {

        [JsonProperty("data")]
        public IList<Datum> Data { get; set; }
    }

    public class Data
    {

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class Video
    {

        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public class VideoAttachments
    {

        [JsonProperty("data")]
        public IList<Datum> Data { get; set; }
    }

    public class Relationships
    {

        [JsonProperty("leg_fares")]
        public LegFares LegFares { get; set; }

        [JsonProperty("video")]
        public Video Video { get; set; }

        [JsonProperty("video_attachments")]
        public VideoAttachments VideoAttachments { get; set; }
    }

    public class Attributes
    {

        [JsonProperty("image_thumb_url")]
        public string ImageThumbUrl { get; set; }

        [JsonProperty("image_medium_square_url")]
        public string ImageMediumSquareUrl { get; set; }

        [JsonProperty("embed_url")]
        public string EmbedUrl { get; set; }

        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("headline")]
        public string Headline { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("small_icon_url")]
        public string SmallIconUrl { get; set; }

        [JsonProperty("large_icon_url")]
        public string LargeIconUrl { get; set; }

        [JsonProperty("cta_text")]
        public string CtaText { get; set; }

        [JsonProperty("cta_url")]
        public string CtaUrl { get; set; }

        [JsonProperty("categories")]
        public IList<string> Categories { get; set; }

        [JsonProperty("fees")]
        public IList<object> Fees { get; set; }
    }

    public class Ancillaries
    {

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("relationships")]
        public Relationships Relationships { get; set; }

        [JsonProperty("attributes")]
        public Attributes Attributes { get; set; }
    }

    public static class Loader {

        public static async Task<IList<Ancillaries>> GetList()
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("https://s3.amazonaws.com");

                var response = await client.GetStringAsync("/fly360/routeHappy.json");
                var fixedStr = response
                    .Remove(response.LastIndexOf("}"))
                    .Replace("\"included\": ", string.Empty)
                                       ;

                var toReturn = JsonConvert.DeserializeObject<IList<Ancillaries>>(fixedStr);
                return toReturn;
            }
            catch(Exception ex)
            {
                Xamarin.Forms.Application.Current.MainPage.DisplayAlert(
                    "System error!",
                    "Unexpected error occured! Please try again.",
                    "Dismiss"
                );
            }

            return null;
        }
    }
}

