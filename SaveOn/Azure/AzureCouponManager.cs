//#define OFFLINE_SYNC_ENABLED

using System;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using SaveOn.Models;
using SaveOn.ViewModels;
#if OFFLINE_SYNC_ENABLED
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
#endif

namespace SaveOn.Azure
{
    class AzureCouponManager
    {
        static AzureCouponManager defaultInstance = new AzureCouponManager();
        MobileServiceClient client;


#if OFFLINE_SYNC_ENABLED
        IMobileServiceSyncTable<TodoItem> todoTable;
#else
        IMobileServiceTable<CouponSchedule> couponTable;

#endif
        User _UserInfo;
        const string offlineDbPath = @"localstore.db";

        private AzureCouponManager()
        {

            this.client = new MobileServiceClient(Constants.AzureMobileService);

#if OFFLINE_SYNC_ENABLED
            var store = new MobileServiceSQLiteStore(offlineDbPath);
            store.DefineTable<TodoItem>();

            //Initializes the SyncContext using the default IMobileServiceSyncHandler.
            this.client.SyncContext.InitializeAsync(store);

            this.todoTable = client.GetSyncTable<TodoItem>();
            Console.WriteLine("inside constructor");
#else

            this.couponTable = client.GetTable<CouponSchedule>();
#endif
        }
        public static AzureCouponManager DefaultManager
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }

        public MobileServiceClient CurrentClient
        {
            get { return client; }
        }

        public bool IsOfflineEnabled
        {
            get { return couponTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<CouponSchedule>; }
        }

        public async Task<ObservableCollection<CouponSchedule>> GetTodoItemsAsync(List<CouponSchedule> couponList, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                List<string> placeList = new List<string>();
               foreach(var cl in couponList)
                {
                    placeList.Add(cl.thisCoupon.GoogleData.place_id);
                    Console.WriteLine("before====>" + cl.thisCoupon.GoogleData.name);
                } 
                IEnumerable<CouponSchedule> items = await couponTable
                       //.Take(50)
                       // .ToListAsync();
                       //   .Where(cpnItem => _couponSchedule.Contains(cpnItem.PlaceId))
                       .Where(ct => placeList.Contains(ct.PlaceId))
                    //   .OrderBy(time => time.SendTime)
                    .ToEnumerableAsync();
                int b = 0;
                List<CouponSchedule> fixList = new List<CouponSchedule>();
                CouponSchedule addCoupon;


                foreach(var cl in couponList)
                {
                    addCoupon = new CouponSchedule();
                    var place = cl.thisCoupon.GoogleData.place_id;
                    if(items.Where(i=>i.PlaceId==place).FirstOrDefault() != null)
                    {
                        Console.WriteLine("THIS PLACE ID IS IN OUR SERVER! => "+ place);
                        addCoupon = items.Where(i => i.PlaceId == place).FirstOrDefault();
                        addCoupon.thisCoupon = new Coupon();
                        addCoupon.thisCoupon.GoogleData = cl.thisCoupon.GoogleData;
                        addCoupon.notInSystem = false;
                    } else
                    {
                        Console.WriteLine("XXXXXXXXXXXXXXXXXX NOT IN OUR SERVER! => " + place);
                        addCoupon = cl;
                        Console.WriteLine(cl.thisCoupon.GoogleData.name);
                        addCoupon.notInSystem = true;
                    }
                    fixList.Add(addCoupon);
                }
                //foreach (var i in items)
                //{
                //    addCoupon = i;
                    
                //    addCoupon.Loc.lat = couponList.Where(cl => cl.PlaceId == i.PlaceId).Select(l => l.Loc.lat).FirstOrDefault();
                //    addCoupon.Loc.lng = couponList.Where(cl => cl.PlaceId == i.PlaceId).Select(l => l.Loc.lng).FirstOrDefault();
                //    fixList.Add(addCoupon);
                //}
        
                return new ObservableCollection<CouponSchedule>(fixList);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return new ObservableCollection<CouponSchedule> (couponList);
        }

        public async Task SaveTaskAsync(CouponSchedule item)
        {

            await couponTable.UpdateAsync(item);
            
        }

#if OFFLINE_SYNC_ENABLED
        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                await this.client.SyncContext.PushAsync();

                await this.todoTable.PullAsync(
                    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                    //Use a different query name for each unique query in your program
                    "allTodoItems",
                    this.todoTable.CreateQuery());
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }

            // Simple error/conflict handling. A real application would handle the various errors like network conditions,
            // server conflicts and others via the IMobileServiceSyncHandler.
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        //Update failed, reverting to server's copy.
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                    }

                    Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                }
            }
        }
#endif
    }
}
