using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Graphics;

namespace DerehHaJudo_Android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
        }
        LinearLayout.LayoutParams WrapContParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
        LinearLayout.LayoutParams LP1 = new LinearLayout.LayoutParams(400, 250);
        LinearLayout OAlayout, TitleLayout, NameLayout, IDLayout, AGUDALayout, CB1Layout, CB2Layout, CB3Layout, SpinnerLayout, SendButtonLayout;
        TextView TitleTV, NameTV, IDTV, AGUDATV, CB1TV, CB2TV, CB3TV;
        EditText NameET, IDET, AGUDAET;
        CheckBox CB1, CB2, CB3;
        Button SendButton;
        private void BuildScreen()
        {
            OAlayout = FindViewById<LinearLayout>(Resource.Id.MainPageLayout);
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
                LayoutParameters = LP1,
            };
            TitleTV.SetTextColor(Color.ParseColor("#FF6A00"));
            TitleLayout.AddView(TitleTV);
            //
            NameLayout = new LinearLayout(this)
            {
                LayoutParameters = WrapContParams,
                Orientation = Orientation.Horizontal,
            };
        }
    }
}