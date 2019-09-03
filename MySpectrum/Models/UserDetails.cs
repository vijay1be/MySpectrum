using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace MySpectrum.Models
{
    public class UserDetails
    {
        [PrimaryKey]
        public string username { get; set; }

        public string password { get; set; }

        public string fullname { get; set; }

        public string email { get; set; }

        public string phonenumber { get; set; }
    }
}