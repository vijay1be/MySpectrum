using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MySpectrum.Models;
using SQLite;

namespace MySpectrum
{
    [Activity(Label = "User Details")]
    class UserListActivity : Activity
    {
        string[] items;
        ListView userList;
        Toolbar menu;

        [Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.activity_userlist); 
                menu = FindViewById<Toolbar>(Resource.Id.toolbar);
                userList = (ListView)FindViewById<ListView>(Resource.Id.userlistview);
                SetActionBar(menu);
                UpdateListivew();       
                userList.ItemClick += (s, e) =>
                {

                    FragmentTransaction transaction = FragmentManager.BeginTransaction();
                    UserDetailsDialog dialog = new UserDetailsDialog(items[e.Position].ToString());
                    dialog.Show(transaction, "user details frgament");
                    dialog.eventHandler += (object sender, System.EventArgs ev) =>
                    {
                        UpdateListivew();
                    };
                };
            }
            catch (Exception)
            {

            }
        }
        void UpdateListivew()
        {
            try
            {
                string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3");
                var db = new SQLiteConnection(dpPath);
                var data = db.Table<UserDetails>();
                int i = 0;
                items = new string[data.Count()];
                foreach (var item in data.ToList())
                {
                    items[i] = item.fullname.ToString();
                    i++;
                }
                userList.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, items);
            }
            catch(Exception)
            {

            }
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Drawable.top_menus, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            var intent = new Intent(this, typeof(AddUserActivity));
            intent.PutExtra("username", "");
            StartActivityForResult(intent, 1);
            return base.OnOptionsItemSelected(item);
        }
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            //when regester activity retrun data, it will be execute 

            if (requestCode == 1 && resultCode == Result.Ok)
            {
                string result = data.GetStringExtra("result");
                if (result == "Success")
                {
                    UpdateListivew();
                }
            }
        }
    }
}