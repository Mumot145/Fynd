using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using SaveOn.GoogleModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SaveOn.GPS
{
    public class GetGPS
    {
       public GetGPS()
        {

        }
        public async Task<Location> updateLocation()
        {

            Location myLoc = new Location();
            Position position = new Position();
            //var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);


            try
            {
                position = null;
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;

                // Get the current device position. Leave it null if geo-location is disabled,
                // return position (0, 0) if unable to acquire.
                if (locator.IsGeolocationEnabled)
                {
                    // Allow ten seconds for geo-location determination.                    
                    position = await locator.GetPositionAsync(1000);


                    myLoc.lat = position.Latitude;
                    myLoc.lng = position.Longitude;
                    //locator = null;

                }
                else
                {
                    Debug.WriteLine("Location could not be acquired, geolocator is disabled.");
                }
            }
            catch (Exception le)
            {
                // TODO: Log this error.
                Debug.WriteLine("Location could not be acquired.");
                Debug.WriteLine(le.Message);
                Debug.WriteLine(le.StackTrace);     //43.212930, -79.728677
                position = new Plugin.Geolocator.Abstractions.Position() { Latitude = 43.212930, Longitude = -79.728677 };
                myLoc.lat = position.Latitude;
                myLoc.lng = position.Longitude;
            }
            return myLoc;
            // fakeLong = 43.217703;
            //fakeLat = -79.861937;
            //Longitude.Text = fakeLong.ToString();         //position.Longitude.ToString();
            // Latitude.Text = fakeLat.ToString();           //position.Latitude.ToString();
        }
    }
}
