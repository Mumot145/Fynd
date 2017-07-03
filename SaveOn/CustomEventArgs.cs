using SaveOn.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaveOn
{
    public class CustomEventArgs
    {
        public class FindEventArgs : EventArgs
        {
            private CouponSchedule m_Data;
            public FindEventArgs(CouponSchedule data)
            {           
                m_Data = data;
            }
            public CouponSchedule Data
            { get { return m_Data; } }
        }
    }
}
