using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Graphics;
using System.Collections.Generic;
using ES.DMoral.ToastyLib;
using Android.Telephony;
using System.Threading.Tasks;
using Android;
using Android.Content;
using System;
using Android.Icu.Text;
using Org.Apache.Http.Impl.Client;
using Android.Views;
using System.Linq;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Firebase;
using Firebase.Firestore;

namespace DerehHaJudo_Android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : Activity
    {
        public FirebaseFirestore GetDataBase()
        {
            FirebaseFirestore database;
            var options = new FirebaseOptions.Builder()
                .SetProjectId("mylittleclubproject")
                .SetApplicationId("mylittleclubproject")
                .SetApiKey("AIzaSyDG3jgrxvbvW8pwKZRPXjsm1EHNAkM_k5U")
                .SetDatabaseUrl("https://mylittleclubproject.firebaseio.com")
                .SetStorageBucket("mylittleclubproject.appspot.com")
                .Build();
            var app = FirebaseApp.InitializeApp(this, options);
            database = FirebaseFirestore.GetInstance(app);
            return database;
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            AppCenter.Start("8a54b3ee-bc50-4177-a6ce-57a95d92e026",
                   typeof(Analytics), typeof(Crashes));
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            TryToGetPermissions();
            sp = this.GetSharedPreferences("details", FileCreationMode.Private);
            trainers.Add("בחר מאמן");
            trainers.Add("עידו");
            trainers.Add("נועם");
            trainers.Add("נתי");
            trainers.Add("יוליה");
            trainers.Add("אורי");
            BuildScreen();
        }
        #region RuntimePermissions

        async Task TryToGetPermissions()
        {
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                await GetPermissionsAsync();
                return;
            }

        }
        const int RequestLocationId = 0;

        readonly string[] PermissionsGroupLocation =
            {
                            //TODO add more permissions
                            Manifest.Permission.SendSms,
                            Manifest.Permission.WriteSms,
             };
        async Task GetPermissionsAsync()
        {
            const string permission = Manifest.Permission.AccessFineLocation;

            if (CheckSelfPermission(permission) == (int)Android.Content.PM.Permission.Granted)
            {
                //TODO change the message to show the permissions name
                Toast.MakeText(this, "SMS permissions granted", ToastLength.Short).Show();
                return;
            }
            if (ShouldShowRequestPermissionRationale(permission))
            {
                //set alert for executing the task
                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                alert.SetTitle("Permissions Needed");
                alert.SetMessage("The application need SMS permissions to continue");
                alert.SetPositiveButton("Request Permissions", (senderAlert, args) =>
                {
                    RequestPermissions(PermissionsGroupLocation, RequestLocationId);
                });
                alert.SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show();
                });
                Dialog dialog = alert.Create();
                dialog.Show();
                return;
            }
            RequestPermissions(PermissionsGroupLocation, RequestLocationId);
        }
        public override async void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestLocationId:
                    {
                        if (grantResults[0] == (int)Android.Content.PM.Permission.Granted)
                        {
                            Toast.MakeText(this, "SMS permissions granted", ToastLength.Short).Show();

                        }
                        else
                        {
                            //Permission Denied :(
                            Toast.MakeText(this, "SMS permissions denied", ToastLength.Short).Show();

                        }
                    }
                    break;
            }
            //base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        //https://github.com/egarim/XamarinAndroidSnippets/blob/master/XamarinAndroidRuntimePermissions
        #endregion
        List<string> trainers = new List<string>();
        
        LinearLayout.LayoutParams WrapContParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
        LinearLayout.LayoutParams LP1 = new LinearLayout.LayoutParams(800, 350, 1);
        LinearLayout OAlayout, TitleLayout, NameLayout, IDLayout, AGUDALayout, CBLayout, SpinnerLayout, SendButtonLayout;
        TextView TitleTV, NameTV, IDTV, AGUDATV;
        EditText NameET, IDET, AGUDAET;
        CheckBox CB1, CB2;
        Button SendButton;
        Spinner TrainersSpinner;
        Color mYellow = Color.ParseColor("#f9f39a");
        Color MBlue1 = Color.ParseColor("#2c3c6d");
        Color MBlue2 = Color.ParseColor("#656584");
        Color MBlue3 = Color.ParseColor("#9f989a");
        ISharedPreferences sp; 
        private void BuildScreen()
        {
            OAlayout = FindViewById<LinearLayout>(Resource.Id.MainPageLayout);
            OAlayout.SetBackgroundColor(mYellow);
            //
            TitleLayout = new LinearLayout(this)
            {
                LayoutParameters = WrapContParams,
                Orientation = Orientation.Horizontal,
            };
            //
            TitleTV = new TextView(this)
            {
                Text = "הצהרת בריאות",
                TextSize = 55,
                LayoutParameters = LP1,
            };
            TitleTV.SetTextColor(MBlue1);
            //
            TitleLayout.AddView(TitleTV);
            //======================================================================
            //======================================================================
            NameLayout = new LinearLayout(this)
            {
                LayoutParameters = WrapContParams,
                Orientation = Orientation.Horizontal,
            };
            //
            NameTV = new TextView(this)
            {
                Text = "שם הספורטאי: ",
                LayoutParameters = LP1,
                TextSize = 30,
            };
            NameTV.SetTextColor(MBlue2);
            //
            NameET = new EditText(this)
            {
                LayoutParameters = LP1,
                Hint = "שם + שם משפחה",
                Text = sp.GetString("Name", ""),
                TextSize = 30,
                TextDirection = Android.Views.TextDirection.Rtl,
            };
            NameET.SetTextColor(Color.Black);
            //
            NameLayout.AddView(NameET);
            NameLayout.AddView(NameTV);
            //======================================================================
            //======================================================================
            IDLayout = new LinearLayout(this)
            {
                LayoutParameters = WrapContParams,
                Orientation = Orientation.Horizontal,
            };
            //
            IDTV= new TextView(this)
            {
                Text = "ת.ז הספורטאי: ",
                LayoutParameters = LP1,
                TextSize = 30,
            };
            IDTV.SetTextColor(MBlue2);
            //
            IDET = new EditText(this)
            {
                LayoutParameters = LP1,
                Hint = "ת.ז",
                Text = sp.GetString("ID", ""),
                TextSize = 30,

            };
            IDET.SetTextColor(Color.Black);
            //
            IDLayout.AddView(IDET);
            IDLayout.AddView(IDTV);
            //======================================================================
            //======================================================================
            AGUDALayout = new LinearLayout(this)
            {
                LayoutParameters = WrapContParams,
                Orientation = Orientation.Horizontal,
            };
            //
            AGUDATV = new TextView(this)
            {
                Text = "אגודת הספורטאי: ",
                LayoutParameters = LP1,
                TextSize = 30,
            };
            AGUDATV.SetTextColor(MBlue2);
            //
            AGUDAET = new EditText(this)
            {
                LayoutParameters = LP1,
                Hint = "אגודה",
                Text = sp.GetString("AGUDA", ""),
                TextSize = 30,
                TextDirection = Android.Views.TextDirection.Rtl,
            };
            AGUDAET.SetTextColor(Color.Black);
            //
            AGUDALayout.AddView(AGUDAET);
            AGUDALayout.AddView(AGUDATV);
            //======================================================================
            //======================================================================
            CBLayout = new LinearLayout(this)
            {
                LayoutParameters = WrapContParams,
                Orientation = Orientation.Vertical,
            };
            //
            CB1 = new CheckBox(this)
            {
                Text = "אני מצהיר/ה כי ערכתי היום בדיקה למדידת חום גוף, בה נמצא כי חום גופי אינו עולה על 38 מעלות צלזיוס",
                TextSize = 20,
                TextDirection = Android.Views.TextDirection.Rtl,
            };
            CB1.SetTextColor(MBlue3);
            //
            CB2 = new CheckBox(this)
            {
                Text = "אני מצהיר/ה כי איני משתעל/ת וכן כי אין לי קשיים בנשימה.",
                TextSize = 20,
                TextDirection = Android.Views.TextDirection.Rtl,
            };
            CB2.SetTextColor(MBlue3);
            CBLayout.AddView(CB1);
            CBLayout.AddView(CB2);
            //======================================================================
            //======================================================================
            SpinnerLayout = new LinearLayout(this)
            {
                LayoutParameters = WrapContParams,
                Orientation = Orientation.Horizontal
            };
            SpinnerLayout.SetGravity(GravityFlags.Right);
            //
            var adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleSpinnerItem, trainers);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            TrainersSpinner = new Spinner(this)
            {
                Adapter = adapter,
                LayoutParameters = new Android.Views.ViewGroup.LayoutParams(350, 220),
            };
            TrainersSpinner.SetBackgroundColor(Color.White);
            TrainersSpinner.Adapter = adapter;
            TrainersSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(TrainersSpinner_ItemSelected);
            SpinnerLayout.AddView(TrainersSpinner);
            //======================================================================
            //======================================================================
            SendButtonLayout = new LinearLayout(this)
            {
                LayoutParameters = WrapContParams,
                Orientation = Orientation.Vertical,
            };
            SendButtonLayout.SetGravity(GravityFlags.Right);
            //
            SendButton = new Button(this)
            {
                Text = "שליחה",
                TextSize = 40,
            };
            SendButton.SetBackgroundColor(mYellow);
            SendButton.SetTextColor(MBlue1);
            SendButton.Click += this.SendButton_Click;
            //
            SendButtonLayout.AddView(SendButton);
            //======================================================================
            //======================================================================
            OAlayout.AddView(TitleLayout);
            OAlayout.AddView(NameLayout);
            OAlayout.AddView(IDLayout);
            OAlayout.AddView(AGUDALayout);
            OAlayout.AddView(CBLayout);
            OAlayout.AddView(SpinnerLayout);
            OAlayout.AddView(SendButtonLayout);
        }
        private void TrainersSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spin = (Spinner)sender;
            currTrainer = spin.GetItemAtPosition(e.Position).ToString();
            CurrLoc = spin.GetItemAtPosition(e.Position).ToString();
            switch (currTrainer)
            {
                case "עידו":
                    CurrNumber = "0542077344";
                    Toasty.Info(this, "עידו " + CurrNumber, 3, true).Show();
                    c = true;
                    break;
                case "נועם":
                    CurrNumber = "0546544244";
                    Toasty.Info(this, " נועם" + CurrNumber, 3, true).Show();
                    c = true;
                    break;
                case "נתי":
                    CurrNumber = "0547682373";
                    Toasty.Info(this, "נתי " + CurrNumber, 3, true).Show();
                    c = true;
                    break;
                case "יוליה":
                    CurrNumber = "0545492383";
                    Toasty.Info(this, "יוליה " +  CurrNumber, 3, true).Show();
                    c = true;
                    break;
                case "אורי":
                    CurrNumber = "0542150457";
                    Toasty.Info(this, "אורי " + CurrNumber, 3, true).Show();
                    c = true;
                    break;
                default:
                    c = false;
                    break;
            }
        }
        string currTrainer, CurrNumber, CurrLoc;
        bool c = true;
        private void SendButton_Click(object sender, EventArgs e)
        {
            if (Validinput() && c)
            {
                if (!CB1.Checked || !CB2.Checked)
                {
                    Toasty.Error(this, "אנא סמן ווי בשתי בתיבות", 5, false).Show();
                }
                else
                {
                    List<string> ts = new List<string>();
                    ts.Add("שם: " + NameET.Text);
                    ts.Add("\nת.ז: " + IDET.Text);
                    ts.Add("\nאגודה: " + AGUDAET.Text);
                    ts.Add("\nמצהיר כי ערכתי היום בדיקה למדידת חום גוף, בה נמצא כי חום גופי אינו עולה על 38 מעלות צלזיוס");
                    ts.Add("\nוכי איני משתעל/ת וכן כי אין לי קשיים בנשימה");
                    string toSend = "";
                    for (int i = 0; i<ts.Count; i++)
                    {
                        toSend += ts[i];
                    }
                    var content = toSend;
                    var destinationAdd = CurrNumber;
                    SmsManager sm = SmsManager.Default;
                    if (content.Length >= 150)
                    {
                        List<string> parts = new List<string>();
                        //split the string into chunks of 20 chars.
                        var enumerable = Enumerable.Range(0, content.Length / 20).Select(i => content.Substring(i * 20, 20));
                        parts = enumerable.ToList();
                        sm.SendMultipartTextMessage(destinationAdd, null, parts, null, null);
                    }
                    else
                    {
                        sm.SendTextMessage(destinationAdd, null, content, null, null);
                    }
                    var editor = sp.Edit();
                    editor.PutString("Name", NameET.Text);
                    editor.PutString("ID", IDET.Text);
                    editor.PutString("AGUDA", AGUDAET.Text);
                    editor.Commit();
                    Toasty.Success(this, "הצהרה נשלחה בהצלחה", 5, true).Show();
                }
            }
        }
        public bool Validinput()
        {
           if (NameET.Text == "")
            {
                Toasty.Error(this, "שם ריק", 3, true).Show();
                return false;
            }
           if (IDET.Text == "")
            {
                Toasty.Error(this, "ת.ז ריק", 3, true).Show();
                return false;
            }
           if (AGUDAET.Text == "")
            {
                Toasty.Error(this, "אגודה ריקה", 3, true).Show();
                return false;
            }
            return (IsValidName(NameET.Text) && IsValidID(IDET.Text));

        }
        public bool IsValidName(string name)
        {
            bool Tr = true;
            Tr = name.Length >= 4;
            if (!Tr)
            {
                Toasty.Error(this, "שגיאה בשם", 5, true).Show();
                return false;
            }
            else
                return Tr;
        }
        public bool IsValidID(string id)
        {
            if (id.Length != 9)
            {
                Toasty.Error(this, "ת.ז שגוי", 5, true).Show();
                return false;
            }
            else
                return true;
        }
    }
}