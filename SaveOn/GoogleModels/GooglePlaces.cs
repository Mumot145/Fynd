using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaveOn.GoogleModels
{
    public class Location
    {

        [JsonProperty("lat")]
        public double lat { get; set; }

        [JsonProperty("lng")]
        public double lng { get; set; }
    }

    public class Northeast
    {

        [JsonProperty("lat")]
        public double lat { get; set; }

        [JsonProperty("lng")]
        public double lng { get; set; }
    }

    public class Southwest
    {

        [JsonProperty("lat")]
        public double lat { get; set; }

        [JsonProperty("lng")]
        public double lng { get; set; }
    }

    public class Viewport
    {

        [JsonProperty("northeast")]
        public Northeast northeast { get; set; }

        [JsonProperty("southwest")]
        public Southwest southwest { get; set; }
    }

    public class Geometry
    {

        [JsonProperty("location")]
        public Location location { get; set; }

        [JsonProperty("viewport")]
        public Viewport viewport { get; set; }
    }

    public class OpeningHours
    {

        [JsonProperty("exceptional_date")]
        public IList<object> exceptional_date { get; set; }

        [JsonProperty("open_now")]
        public bool open_now { get; set; }

        [JsonProperty("weekday_text")]
        public IList<object> weekday_text { get; set; }
    }

    public class Photo
    {

        [JsonProperty("height")]
        public int height { get; set; }

        [JsonProperty("html_attributions")]
        public IList<string> html_attributions { get; set; }

        [JsonProperty("photo_reference")]
        public string photo_reference { get; set; }

        [JsonProperty("width")]
        public int width { get; set; }
    }

    public class Result
    {
        
        [JsonProperty("geometry")]
        public Geometry geometry { get; set; }

        [JsonProperty("icon")]
        public string icon { get; set; }

        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("formatted_address")]
        public string address { get; set; }

        [JsonProperty("opening_hours")]
        public OpeningHours opening_hours { get; set; }

        [JsonProperty("photos")]
        public IList<Photo> photos { get; set; }

        [JsonProperty("international_phone_number")]
        public string phone_number { get; set; }

        [JsonProperty("place_id")]
        public string place_id { get; set; }

        [JsonProperty("rating")]
        public double rating { get; set; }

        [JsonProperty("reference")]
        public string reference { get; set; }

        [JsonProperty("scope")]
        public string scope { get; set; }

        [JsonProperty("types")]
        public IList<string> types { get; set; }

        [JsonProperty("vicinity")]
        public string vicinity { get; set; }

        [JsonProperty("price_level")]
        public int? price_level { get; set; }
    }

    public class GooglePlaces
    {

        [JsonProperty("html_attributions")]
        public IList<object> html_attributions { get; set; }

        [JsonProperty("next_page_token")]
        public string next_page_token { get; set; }

        [JsonProperty("results")]
        public IList<Result> results { get; set; }

        [JsonProperty("result")]
        public Result result { get; set; }

        [JsonProperty("status")]
        public string status { get; set; }
    }
}
