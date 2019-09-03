using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    [Activity(Label = "Add User")]
    class AddUserActivity : Activity
    {
        Button btnAdd;
        EditText txtUserName;
        EditText txtPassword;
        EditText txtConfirmPassword;
        EditText txtFullName;
        EditText txtEmailId;
        EditText txtPhoneNumber;
        TextView txtErrorMessage;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.activity_adduser); 


                var _username = Intent.GetStringExtra("username");
                btnAdd = FindViewById<Button>(Resource.Id.add);
                txtUserName = FindViewById<EditText>(Resource.Id.adduserUserName);
                txtPassword = FindViewById<EditText>(Resource.Id.adduserPassword);
                txtConfirmPassword = FindViewById<EditText>(Resource.Id.adduserConfirmPassword);
                txtFullName = FindViewById<EditText>(Resource.Id.adduserFullName);
                txtEmailId = FindViewById<EditText>(Resource.Id.adduserEmailId);
                txtPhoneNumber = FindViewById<EditText>(Resource.Id.adduserPhoneNumber);
                txtErrorMessage = FindViewById<TextView>(Resource.Id.adduserErrorMessage);

                string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3");
                var db = new SQLiteConnection(dpPath);
                var data = db.Table<UserDetails>();
                if (_username.ToString() != "")
                {
                    var _userdata = data.Where(x => x.username == _username).FirstOrDefault();
                    if (_userdata != null)
                    {
                        btnAdd.Text = "Edit";
                        txtUserName.SetTextColor(Android.Graphics.Color.LightGray);
                        txtUserName.Text = _userdata.username;
                        txtPassword.Text = txtConfirmPassword.Text = _userdata.password;
                        txtFullName.Text = _userdata.fullname;
                        txtEmailId.Text = _userdata.email;
                        txtPhoneNumber.Text = _userdata.phonenumber;
                        txtUserName.Enabled = false;
                    }
                }
                btnAdd.Click += (object sender, System.EventArgs e) =>
                {
                    if (Validtion())
                        return;

                    var data1 = data.Where(x => x.username == txtUserName.Text).FirstOrDefault();
                    if (btnAdd.Text == "Edit")
                    {
                        UserDetails editData = new UserDetails();
                        editData.username = txtUserName.Text;
                        editData.password = txtPassword.Text;
                        editData.fullname = txtFullName.Text;
                        editData.email = txtEmailId.Text;
                        editData.phonenumber = txtPhoneNumber.Text;
                        db.Update(editData);
                        Toast.MakeText(this, "User edited successfully", ToastLength.Short).Show();
                        //StartActivity(typeof(UserListActivity));
                        //Finish();

                        Intent returnIntent = new Intent();
                        returnIntent.PutExtra("result", "Success");
                        SetResult(Result.Ok, returnIntent);
                        Finish();
                    }
                    else if (btnAdd.Text == "Add")
                    {
                        if (data1 == null)
                        {
                            UserDetails addData = new UserDetails();
                            addData.username = txtUserName.Text;
                            addData.password = txtPassword.Text;
                            addData.fullname = txtFullName.Text;
                            addData.email = txtEmailId.Text;
                            addData.phonenumber = txtPhoneNumber.Text;
                            db.Insert(addData);
                            Toast.MakeText(this, "User added successfully", ToastLength.Short).Show();
                            //StartActivity(typeof(UserListActivity));
                            //Finish();

                            Intent returnIntent = new Intent();
                            returnIntent.PutExtra("result", "Success");
                            SetResult(Result.Ok, returnIntent);
                            Finish();
                        }
                        else
                        {
                            txtFullName.Text = txtUserName.Text = txtEmailId.Text = txtPhoneNumber.Text = "";
                            Toast.MakeText(this, "Username already exit !", ToastLength.Long).Show();
                        }
                    }

                };
            }
            catch(Exception ex)
            {

            }
        }
        public bool Validtion()
        {
            try
            {
                txtErrorMessage.Text = "";
                string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3");
                var db = new SQLiteConnection(dpPath);
                var data = db.Table<UserDetails>();
                var data1 = data.Where(x => x.username == txtUserName.Text).FirstOrDefault();
                if (data1 != null && btnAdd.Text == "Add")
                    txtErrorMessage.Text = "* UserName already exists !";
                if (string.IsNullOrWhiteSpace(txtFullName.Text))
                    txtErrorMessage.Text = "\n* Please enter a valid Name";
                if (txtPassword.Text != txtConfirmPassword.Text)
                    txtErrorMessage.Text += "\n* The Password does not match";
                if (IsValidPassword())
                    txtErrorMessage.Text += "\n* Please enter a valid Password";
                if (!isValidEmail(txtEmailId.Text))
                    txtErrorMessage.Text += "\n* Please enter a valid Email Id";
                if (txtPhoneNumber.Text.Length != 10)
                    txtErrorMessage.Text += "\n* Please enter a valid Phone Number";
                if (txtErrorMessage.Text == "")
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private bool IsValidPassword()
        {
            try
            {
                var char_check = new Regex(@"(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{5,12})$");
                var sequence_check = new Regex(@"^.*(?<grp>[a-z0-9]{2,})(\k<grp>).*$");
                if (!char_check.IsMatch(txtPassword.Text))
                    return true;
                if (sequence_check.IsMatch(txtPassword.Text))
                    return true;
                return false;
            }
            catch
            {
                return true;
            }
        }
        public bool isValidEmail(string email)
        {
            return Android.Util.Patterns.EmailAddress.Matcher(email).Matches();
        }
    }
}