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
    [Activity(Theme = "@android:style/Theme.Dialog")]
    [Obsolete]
    class UserDetailsDialog : DialogFragment
    {
        TextView txtFullName;
        TextView txtUserName;
        TextView txtEmailId;
        TextView txtPhoneNumber;
        Button btnEdit;
        Button btnDelete;
        String parameter = "";
        public EventHandler eventHandler;
        public UserDetailsDialog(String parameterIn)
        {
            parameter = parameterIn;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                base.OnCreateView(inflater, container, savedInstanceState);
                var view = inflater.Inflate(Resource.Layout.dialog_userdetails, container, false);
                txtFullName = view.FindViewById<TextView>(Resource.Id.viewFullName);
                txtUserName = view.FindViewById<TextView>(Resource.Id.viewUserName);
                txtEmailId = view.FindViewById<TextView>(Resource.Id.viewEmailId);
                txtPhoneNumber = view.FindViewById<TextView>(Resource.Id.viewPhoneNumber);
                btnEdit = view.FindViewById<Button>(Resource.Id.edit);
                btnDelete = view.FindViewById<Button>(Resource.Id.delete);
                string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3");
                var db = new SQLiteConnection(dpPath);
                var data = db.Table<UserDetails>();
                var data1 = data.Where(x => x.fullname == parameter).FirstOrDefault();
                txtFullName.Text = parameter;
                txtUserName.Text = data1.username;
                txtEmailId.Text = data1.email;
                txtPhoneNumber.Text = data1.phonenumber;
                btnEdit.Click += (object sender, System.EventArgs e) =>
                {
                    var intent = new Intent(this.Activity, typeof(AddUserActivity));
                    intent.PutExtra("username", data1.username);
                    StartActivityForResult(intent, 2);
                };
                btnDelete.Click += (object sender, System.EventArgs e) =>
                {
                    db.Delete(data1);
                    eventHandler.Invoke(this, e);
                    this.Dismiss();
                    Toast.MakeText(Activity, "Item deleted successfully", ToastLength.Short).Show();
                };
                return view;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 2 && resultCode == Result.Ok)
            {
                string result = data.GetStringExtra("result");
                if (result == "Success")
                {
                    eventHandler.Invoke(this, null);
                    this.Dismiss();
                }
            }
        }
    }
}