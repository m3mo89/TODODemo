using System;
using Android.Content;
using TODODemo.Droid.Renderers;
using TODODemo.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomButton), typeof(NoShadowButtonRenderer))]
namespace TODODemo.Droid.Renderers
{
    public class NoShadowButtonRenderer: ButtonRenderer
    {
        public NoShadowButtonRenderer(Context context) : base(context)
        {
            
        }

        protected override void OnDraw(Android.Graphics.Canvas canvas)
        {
            base.OnDraw(canvas);
        }
    }
}
