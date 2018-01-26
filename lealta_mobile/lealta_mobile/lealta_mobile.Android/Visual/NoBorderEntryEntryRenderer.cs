using Android.Graphics.Drawables;
using lealta_mobile;
using lealta_mobile.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(NoBorderEntry), typeof(NoBorderEntryEntryRenderer))]
namespace lealta_mobile.Droid
{
    public class NoBorderEntryEntryRenderer : EntryRenderer
    {
        public NoBorderEntryEntryRenderer(Android.Content.Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Background = new ColorDrawable(Android.Graphics.Color.Transparent);
            }
        }
    }
}