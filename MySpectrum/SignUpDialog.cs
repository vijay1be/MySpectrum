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
    [Activity(Theme = "@android:style/Theme.Dialog")]
    [Obsolete]
    public class SignUpDialog : DialogFragment
    {
        EditText txtUserName;
        EditText txtPassword;
        EditText txtConfirmPassword;
        EditText txtEmailId;
        EditText txtFullName;
        EditText txtPhoneNumber;
        TextView txtErrorMessage;
        Button btnSignUp;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                base.OnCreateView(inflater, container, savedInstanceState);
                var view = inflater.Inflate(Resource.Layout.dialog_signup, container, false);
                txtUserName = view.FindViewById<EditText>(Resource.Id.signupUserName);
                txtPassword = view.FindViewById<EditText>(Resource.Id.signupPassword);
                txtConfirmPassword = view.FindViewById<EditText>(Resource.Id.signupConfirmPassword);
                txtEmailId = view.FindViewById<EditText>(Resource.Id.signupMailId);
                txtFullName = view.FindViewById<EditText>(Resource.Id.signupFullName);
                txtPhoneNumber = view.FindViewById<EditText>(Resource.Id.signupPhoneNumber);
                txtErrorMessage = view.FindViewById<TextView>(Resource.Id.signupErrorMessage);
                btnSignUp = view.FindViewById<Button>(Resource.Id.submit);
                btnSignUp.Click += (object sender, System.EventArgs e) =>
                {
                    if (Validtion())
                        return;
                    string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "user.db3");
                    var db = new SQLiteConnection(dpPath);
                    var data = db.Table<UserDetails>();

                    UserDetails addData = new UserDetails();
                    addData.username = txtUserName.Text;
                    addData.password = txtPassword.Text;
                    addData.fullname = txtFullName.Text;
                    addData.email = txtEmailId.Text;
                    addData.phonenumber = txtPhoneNumber.Text;
                    db.Insert(addData);
                    StartActivity(new Intent(this.Activity, typeof(UserListActivity)));
                    this.Dismiss();
                    Toast.MakeText(Activity, "Signed up successfully", ToastLength.Short).Show();
                };

                return view;
            }
            catch(Exception)
            {
                return null;
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
                var _userdata = data.Where(x => x.username == txtUserName.Text).FirstOrDefault();
                if (_userdata != null)
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