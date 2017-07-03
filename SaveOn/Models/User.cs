using System;
using System.Collections.Generic;
using System.Text;

namespace SaveOn.Models
{
    public class User
    {
        public string Id { get; set; }
        public string name { get; set; }
        public string FacebookId { get; set; }

        public class FoodPreferences
        {
            public int FastFood { get; set; }
            public int Vegan { get; set; }
            public int Vegetarian { get; set; }
            public int GlutenFree { get; set; }
            public int Cafe { get; set; }
            public int Organic { get; set; }
            public int Dessert { get; set; }
            public int Chinese { get; set; }
            public int Mexican { get; set; }
            public int Italian { get; set; }
            public int Japanese { get; set; }
            public int Greek { get; set; }
            public int French { get; set; }
            public int Thai { get; set; }
            public int Spanish { get; set; }
            public int Indian { get; set; }
            public int Mediterranean { get; set; }
            public int Canadian { get; set; }


        }
    }
}
