
using SaveOn.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace SaveOn.Models
{

    public class CouponList {
        
        //public List<CouponSchedule> Coupons = new List<CouponSchedule>();
      
        //public CouponList(List<CouponSchedule> initCouponList = null)
        //{
        //    Coupons = initCouponList;
        //}
        //public void AddCoupons(List<CouponSchedule> addingCoupons)
        //{
        //    foreach(var ac in addingCoupons)
        //    {
        //        Coupons.Add(ac);
        //    }
        //}
        //public void ResetCoupons(List<CouponSchedule> newCoupons)
        //{
        //    Coupons = newCoupons;
        //}
        //public List<CouponSchedule> getCouponPage(int page)
        //{
        //    return Coupons.Take(10).Skip(page - 1).ToList();
        //}
        private List<CouponSchedule> coupons = new List<CouponSchedule>();
        public String backpackType { get; set; }
        public User currentUser { get; set; }
        public List<CouponSchedule> couponList
        {
            get { return coupons; }
            set { coupons = value; }
        }
    }
}
//{


//}

