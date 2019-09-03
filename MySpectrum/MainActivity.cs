using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.IO;
using SQLite;
using MySpectrum.Models;
using System;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System.Collections.Generic;

namespace MySpectrum
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class MainActivity : AppCompatActivity
    {
        Button btnLogin;
        Button btnSignUp;
        EditText txtUserName;
        EditText txtPassword;
        TextView forgotpassword;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                Xamarin.Essentials.Platform.Init(this, savedInstanceState);

                //* Track the crashes, issues & Event logs  *//
                //AppCenter.Start("copy the value from App Center",typeof(Analytics), typeof(Crashes));


                // Set our view from the "main" layout resource
                SetContentView(Resource.Layout.activity_main);
                CreateDB();
                btnLogin = FindViewById<Button>(Resource.Id.login);
                btnSignUp = FindViewById<Button>(Resource.Id.newuser);
                txtUserName = FindViewById<EditText>(Resource.Id.loginUserName);
                txtPassword = FindViewById<EditText>(Resource.Id.loginPassword);
                forgotpassword = FindViewById<TextView>(Resource.Id.loginForgotPassword);
                btnLogin.Click += BtnLogin_Click;
                btnSignUp.Click += (object sender, System.EventArgs e) =>
                {
                    txtUserName.Text = txtPassword.Text = "";
                    FragmentTransaction transaction = FragmentManager.BeginTransaction();
                    SignUpDialog dialog = new SignUpDialog();
                    dialog.Show(transaction, "sign up frgament");
                };
                forgotpassword.Click += Forgotpassword_Click;
            }
            catch(Exception ex)
            {
               // Crashes.TrackError(ex);  // copy this across the application's try-catch block to track the crashes/issues
            }
        }

        private void Forgotpassword_Click(object sender, EventArgs e)
        {

            // A dummy transaction has been made just for login UI. 
            if (txtUserName.Text == "")
            {
                Toast.MakeText(this, "Please enter the username", ToastLength.Long).Show();
            }
            else
            {
                Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
                Android.App.AlertDialog alert = dialog.Create();
                alert.SetTitle("Forgot Password");
                alert.SetMessage("A link to reset your password has been sent to your registered Email Id");
                alert.SetButton("OK", (c, ev) =>
                {
                    txtUserName.Text = "";
                });
                alert.Show();

                /* copy this event where ever necessary to track the user's live events */
               // Analytics.TrackEvent("Forgotpassword Clicked", new Dictionary<string, string> { { "Test", "value" } });
            }
        }

        private void BtnLogin_Click(object sender, System.EventArgs e)
        {
            try
            {
                string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3"); 
                var db = new SQLiteConnection(dpPath);
                var data = db.Table<UserDetails>();   
                var data1 = data.Where(x => x.username == txtUserName.Text && x.password == txtPassword.Text).FirstOrDefault(); //Linq Query  
                if (data1 != null)
                {
                    txtUserName.Text = txtPassword.Text = "";
                    Toast.MakeText(this, "Login Success", ToastLength.Short).Show();
                    StartActivity(typeof(UserListActivity));
                   // Finish();
                }
                else
                {
                    Toast.MakeText(this, "Username or Password invalid", ToastLength.Short).Show();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public void CreateDB()
        {
            try
            {
                string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3");
                var db = new SQLiteConnection(dpPath);
                db.CreateTable<UserDetails>();
                //UserDetails dummydata = new UserDetails();
                //dummydata.username = "username";
                //dummydata.password = "1234";
                //dummydata.fullname = "Vijay Panneerselvam";
                //dummydata.email = "vijay1.be@yahoo.com";
                //dummydata.phonenumber = "773-225-8681";
                //db.Insert(dummydata);
            }
            catch(Exception)
            {

            }
        }
    }
}