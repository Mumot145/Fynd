using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;
using System.Diagnostics;
using SaveOn.Models;
using SaveOn.ViewModels;
using System.Threading;
using System.Linq;

namespace SaveOn.Azure
{
    public class AzureDataService
    {
       // public MobileServiceClient MobileService { get; set; }
        //IMobileServiceSyncTable coffeeTable;
        List<String> coupons;

        SqlConnection connection = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataReader reader;
        public void Initialize()
        {
            // string conString = ConfigurationManager.ConnectionStrings["connectionString"].ToString();
            //db_con = new OleDbConnection(conString);
            connection = new SqlConnection(Constants.AzureSQLConnection);

        }

        public string FindUserFB(string fbName, string fbId)
        {
            string userName = "";


            cmd.CommandText = "SELECT Name FROM Users WHERE FacebookId = " + fbId;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            connection.Open();

            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                userName = String.Format("{0}", reader[0]);
                Console.WriteLine(String.Format("{0}", reader[0]));
            }
            connection.Close();

            if (!String.IsNullOrEmpty(fbId))
            {
                if (String.IsNullOrEmpty(userName))
                {
                    //register user 
                   // RegisterUser(fbName, fbId);
                }
            }

            Debug.WriteLine("user that is found -" + userName);
            //coupons = await GetCouponImages(place);
            return userName;
        }
        public List<CouponSchedule> GetCoupons(List<CouponSchedule> couponSched = null)
        {
            // List<String> place = new List<String>();
            List<Coupon> coupons = new List<Coupon>();
            List<CouponSchedule> retList = new List<CouponSchedule>();
            CouponSchedule retC;
            //Coupon coupon;
            String placeStr = "";
            int i = 0;
            if(couponSched.Count > 0)
            {
                foreach (var p in couponSched)
                {
                    placeStr = placeStr + "'" + p.CouponId + "', ";
                    i++;
                }
                placeStr = placeStr.Substring(0, placeStr.Length - 2);
                //coupon = new Coupon();
                //SELECT c.Id, c.CouponId FROM Coupons c LEFT JOIN StoreLocations sl ON sl.StoreId = c.Id WHERE sl.PlaceId = ''
                var query = "SELECT Id, CouponId FROM Coupons WHERE Id IN (" + placeStr + ")";
               
                
                coupons = (List<Coupon>)AzureConnect(query, "CouponList");
            }
            
            foreach(var cs in couponSched)
            {
                retC = new CouponSchedule();
                retC.thisCoupon = new Coupon();
                if(!(coupons.Count > 0))
                {
                    var checkNull = coupons.Where(c => c.id == cs.CouponId).FirstOrDefault();
                    if (checkNull != null)
                    {
                        retC.thisCoupon = checkNull;
                        retC.notInSystem = false;
                    }
                    else
                    {
                        retC.notInSystem = true;
                    }
                } else
                {
                    retC.notInSystem = true;
                }
                         
                retC.thisCoupon.GoogleData = cs.thisCoupon.GoogleData;
                retList.Add(retC);
            }
            return retList;
        }
        private void RegisterUser(string facebookName, string facebookId)
        {
            cmd.CommandText = "INSERT INTO Users (Name, FacebookId) VALUES ('" + facebookName + "', '" + facebookId + "')";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            connection.Open();

            int newrows = cmd.ExecuteNonQuery();
            Console.WriteLine($"Inserted {newrows} row(s).");
            connection.Close();
        }
        public User GetUser(string Info, string Method)
        {
            User _user = new User();
            string query = "";
            if (Method == "fbId")
            {
                query = "SELECT Id, Name, FacebookId FROM Users WHERE FacebookId = '" + Info + "'";

            }
            else if (Method == "rId")
            {
                query = "SELECT Id, Name, FacebookId FROM Users WHERE Id = '" + Info + "'";
            }
            else if (Method == "Name")
            {
                query = "SELECT Id, Name, FacebookId FROM Users WHERE Name = '" + Info + "'";
            }
           // query = "SELECT * FROM Users WHERE FacebookId = '" + Info + "'";
            _user = (User)AzureConnect(query, "User");
            return _user;
        }

        //public User GetUserInfo(String FacebookId)
        //{

        //    User user = new User();
        //    bool check = false;
        //    connection = new SqlConnection(Constants.AzureSQLConnection);
        //    cmd.CommandText = "SELECT * FROM Users WHERE FacebookId = '" + FacebookId + "'";
        //    cmd.CommandType = CommandType.Text;
        //    cmd.Connection = connection;
        //    connection.Open();

        //    reader = cmd.ExecuteReader();
        //    if(reader != null)
        //    {
        //        while (reader.Read())
        //        {
        //            user.Id = String.Format("{0}", reader[0]);
        //            user.name = String.Format("{0}", reader[1]);          
        //        }
        //    }

        //    connection.Close();
        //    user.FacebookId = FacebookId;

        //    return user;
        //}



        public User.FoodPreferences GetUserPreferences(User user)
        {
            var query = "SELECT * FROM UserFoodPreferences WHERE UserId = '" + user.Id + "'";

            User.FoodPreferences foodPref = (User.FoodPreferences) AzureConnect(query, "FoodPreference");

            return foodPref;
        }

        //public Task<List<Coupon>> GetBackpack(User _user)
        //{

        //    List<String> _couponId = new List<String>();
        //    List<Coupon> _coupons = new List<Coupon>();


        //    cmd.CommandText = "SELECT * FROM UserBackpack WHERE UserId = '" + _user.Id + "'";
        //    cmd.CommandType = CommandType.Text;
        //    cmd.Connection = connection;
        //    connection.Open();

        //    reader = cmd.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        _couponId.Add(String.Format("{0}", reader[0]));
        //        //Console.WriteLine(String.Format("{0}", reader[0]));
        //    }
        //    connection.Close();
        //    //backpack.couponList = ;
        //    // _coupons = GetCouponInfo(_couponId);
        //    //coupons = await GetCouponImages(place);

        //    return null;
        



        private static readonly int[] RetriableClasses = { 13, 16, 17, 18, 19, 20, 21, 22, 24 };
        public object AzureConnect(string Query, string taskType)
        {
            bool rebuildConnection = true; // First try connection must be open
            object returnValue = null;
            for (int i = 0; i < RetriableClasses[5]; ++i)
            {
                try
                {
                    // (Re)Create connection to SQL Server
                    if (rebuildConnection)
                    {
                        if (connection != null)
                            connection.Dispose();

                        // Create connection and open it...
                        Initialize();
                        cmd.CommandText = Query;
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = connection;
                        connection.Open();
                        
                    }

                    // inserts information

                    if (taskType == "Edit")
                    {
                        int rows = cmd.ExecuteNonQuery();
                        Console.WriteLine($"edited {rows} row(s).");
                        
                        return rows;
                    }
                    else //finds information
                    {
                        reader = cmd.ExecuteReader();
                        if (taskType == "User")
                        {
                            returnValue = readUser(reader);
                        }
                        else if (taskType == "FoodPreference")
                        {
                            returnValue = readFoodPreferences(reader);
                        }
                        else if (taskType == "CouponList")
                        {
                            returnValue = readCouponList(reader);
                        }
                        //else if (taskType == "GroupUserList")
                        //{
                        //    returnValue = readGroupUsers(reader);
                        //}
                        //else if (taskType == "UserList")
                        //{
                        //    returnValue = readSingleGroupUsers(reader);
                        //}
                        //else if (taskType == "ToDoList")
                        //{
                        //    returnValue = readToDoList(reader);
                        //}
                        //else if (taskType == "Schedule")
                        //{
                        //    returnValue = readSchedule(reader);
                        //}
                    }

                    connection.Close();
                    // No exceptions, task has been completed
                    return returnValue;
                }
                catch (SqlException e)
                {
                    if (e.Errors.Cast<SqlError>().All(x => CanRetry(x)))
                    {
                        // What to do? Handle that here, also checking Number property.
                        // For Class < 20 you may simply Thread.Sleep(DelayOnError);
                        Thread.Sleep(2500);
                        rebuildConnection = e.Errors
                            .Cast<SqlError>()
                            .Any(x => x.Class >= 20);

                        continue;
                    }

                    throw;
                }
            }
            return null;
        }
        private User readUser(SqlDataReader _reader)
        {
            User _user = new User();
            if (_reader != null)
            {
                while (reader.Read())
                {
                    _user.Id = String.Format("{0}", reader[0]);
                    _user.name = String.Format("{0}", reader[1]);
                    _user.FacebookId = String.Format("{0}", reader[2]);
                    //_user.Admin = Convert.ToBoolean(reader[3]);
                }
            }
            
            return _user;
        }

        private List<Coupon> readCouponList(SqlDataReader _reader)
        {
            List<Coupon> _coupons = new List<Coupon>();
            Coupon _coupon;
            if (_reader != null)
            {
                while (reader.Read())
                {
                    _coupon = new Coupon();
                     _coupon.id = Int32.Parse(String.Format("{0}", reader[0]));
                    _coupon.ImageUrl = String.Format("{0}", reader[1]);                  
                 //   _coupon.CouponType = String.Format("{0}", reader[4]);
                    _coupons.Add(_coupon);
                }
            }
            return _coupons;
        }
        private User.FoodPreferences readFoodPreferences(SqlDataReader _reader)
        {
            User.FoodPreferences foodPref = new User.FoodPreferences();

            while (reader.Read())
            {
                foodPref.FastFood = Int32.Parse(String.Format("{0}", reader[1]));
                foodPref.Vegan = Int32.Parse(String.Format("{0}", reader[2]));
                foodPref.Vegetarian = Int32.Parse(String.Format("{0}", reader[3]));
                foodPref.GlutenFree = Int32.Parse(String.Format("{0}", reader[4]));
                foodPref.Cafe = Int32.Parse(String.Format("{0}", reader[5]));
                foodPref.Organic = Int32.Parse(String.Format("{0}", reader[6]));
                foodPref.Dessert = Int32.Parse(String.Format("{0}", reader[7]));
                foodPref.Chinese = Int32.Parse(String.Format("{0}", reader[8]));
                foodPref.Mexican = Int32.Parse(String.Format("{0}", reader[9]));
                foodPref.Italian = Int32.Parse(String.Format("{0}", reader[10]));
                foodPref.Japanese = Int32.Parse(String.Format("{0}", reader[11]));
                foodPref.Greek = Int32.Parse(String.Format("{0}", reader[12]));
                foodPref.French = Int32.Parse(String.Format("{0}", reader[13]));
                foodPref.Thai = Int32.Parse(String.Format("{0}", reader[14]));
                foodPref.Spanish = Int32.Parse(String.Format("{0}", reader[15]));
                foodPref.Indian = Int32.Parse(String.Format("{0}", reader[16]));
                foodPref.Mediterranean = Int32.Parse(String.Format("{0}", reader[17]));
                foodPref.Canadian = Int32.Parse(String.Format("{0}", reader[18]));
            }
            return foodPref;
        }
        public void AddToBackpack(CouponSchedule _coupon, User _user)
        {
            cmd.CommandText = "INSERT INTO UserBackpack (UserId, CouponId) VALUES ('" + _user.Id + "', '" + _coupon.thisCoupon.id + "')";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            connection.Open();

            int newrows = cmd.ExecuteNonQuery();
            Console.WriteLine($"Inserted {newrows} row(s).");
            connection.Close();
        }
       
        //public async Task<List<Coupon>> GetCouponImages(List<String> places)
        //{
        //    List<Coupon> couponImages = new List<Coupon>();
        //    int i = 0;
        //    // Coupon cpn = new Coupon();
        //    foreach (var place in places)
        //    {
        //        cmd.CommandText = "SELECT Id, CouponId FROM Coupons WHERE Id = '" + place + "'";
        //        cmd.CommandType = CommandType.Text;
        //        cmd.Connection = connection;
        //        connection.Open();
        //        Coupon cpn = new Coupon();
        //        reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            // cpn = null;

        //            cpn.StoreId = String.Format("{0}", reader[0]);
        //            cpn.ImageUrl = String.Format("{0}", reader[1]);

        //            cpn.id = i;

        //            couponImages.Add(cpn);


        //            i++;
        //        }
        //        connection.Close();
        //    }

        //    var coupons = await GetCouponTypes(couponImages);

        //    return coupons;
        //}
        //public async Task<List<Coupon>> GetCouponTypes(List<Coupon> _coupons)
        //{
        //    List<Coupon> finalCoupons = new List<Coupon>();
        //    int i = 0;
        //    // Coupon cpn = new Coupon();
        //    foreach (var c in _coupons)
        //    {
        //        cmd.CommandText = "SELECT LocationType FROM Stores WHERE Id = '" + c.StoreId + "'";
        //        cmd.CommandType = CommandType.Text;
        //        cmd.Connection = connection;
        //        connection.Open();

        //        reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            // cpn = null;

        //            c.CouponType = String.Format("{0}", reader[0]);

        //            finalCoupons.Add(c);
        //            Console.WriteLine(String.Format("coupong type is ---->>> {0}", c.CouponType));

        //            i++;
        //        }
        //        connection.Close();
        //    }



        //    return finalCoupons;
        //}
        private static bool CanRetry(SqlError error)
        {
            // Use this switch if you want to handle only well-known errors,
            // remove it if you want to always retry. A "blacklist" approach may
            // also work: return false when you're sure you can't recover from one
            // error and rely on Class for anything else.
            switch (error.Number)
            {
                case 4060:
                    Debug.WriteLine("cannot open DB");
                    break;
                case 40197:
                    Debug.WriteLine("error processing request");
                    break;
                case 40501:
                    Debug.WriteLine("services busy - retry in 10 seconds");
                    break;
                case 40613:
                    Debug.WriteLine("database  currently unavailable");
                    break;
                case 49918:
                    Debug.WriteLine("cannot process request - not enough resources");
                    break;
                case 49919:
                    Debug.WriteLine("cannot process create or update request - too many operations");
                    break;
                case 49920:
                    Debug.WriteLine("cannot process request - too many operations");
                    break;

                    // Handle well-known error codes, 
            }

            // Handle unknown errors with severity 21 or less. 22 or more
            // indicates a serious error that need to be manually fixed.
            // 24 indicates media errors. They're serious errors (that should
            // be also notified) but we may retry...
            return RetriableClasses.Contains(error.Class); // LINQ...
        }
    }
}
