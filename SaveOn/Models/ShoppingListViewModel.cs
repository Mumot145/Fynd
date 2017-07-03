using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SaveOn.Models
{
    public class ShoppingListViewModel : INotifyPropertyChanged
    {
        public ICommand SimulateDownloadCommand { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;
        bool canDownload = true;
        string simulatedDownloadResult;
        public User thisUser { get; set; }

        public List<string> News
        {
            get;
            set;
        }
        public ICommand LoadMore
        {
            get;
            set;
        }
        public ShoppingListViewModel()
        {
            var repo = new CouponList();
            //this.News = repo.getNews(1);
            OnpropertyChanged("News");
            //int page = 2;
            this.LoadMore = new Command( async () => await SimulateDownloadAsync(), () => canDownload);
        }
        public string SimulatedDownloadResult
        {
            get { return simulatedDownloadResult; }
            private set
            {
                if (simulatedDownloadResult != value)
                {
                    simulatedDownloadResult = value;
                   // OnPropertyChanged("SimulatedDownloadResult");
                }
            }
        }
        async Task SimulateDownloadAsync()
        {
            CanInitiateNewDownload(false);
            SimulatedDownloadResult = string.Empty;
           // await Task.Run(() => SimulateDownload());
            SimulatedDownloadResult = "Simulated download complete";
            CanInitiateNewDownload(true);
        }

        void CanInitiateNewDownload(bool value)
        {
            canDownload = value;
            ((Command)SimulateDownloadCommand).ChangeCanExecute();
        }
        //void SimulateDownload()
        //{
        //    // Simulate a 5 second pause
        //    var endTime = DateTime.Now.AddSeconds(5);
        //    while (true)
        //    {
        //        if (DateTime.Now >= endTime)
        //        {
        //            break;
        //        }
        //    }
        //}
        void OnpropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
