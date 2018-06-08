using System.Linq;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Runtime;
using Android.Text;
using Fly360.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("Fly360")]
[assembly: ExportEffect(typeof(EntryEffect), "EntryEffect")]
namespace Fly360.Droid
{
    [Preserve(AllMembers = true)]
    public class EntryEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var shape = new ShapeDrawable(new RectShape());
            shape.Paint.Color = global::Android.Graphics.Color.Transparent;
            shape.Paint.StrokeWidth = 0;
            shape.Paint.SetStyle(Paint.Style.Stroke);
            Control.Background = shape;

            Control.SetPadding(2, 2, 2, 2);

            if(Control is FormsEditText editText)
            {
                editText.InputType = editText.InputType | global::Android.Text.InputTypes.TextFlagNoSuggestions;

                var oldFilters = editText.GetFilters().ToArray();

                editText.SetRawInputType(InputTypes.ClassText | InputTypes.TextFlagCapCharacters);

                var newFilters = oldFilters.ToList();
                newFilters.Add(new InputFilterAllCaps());
                editText.SetFilters(newFilters.ToArray());    
            }
        }

        protected override void OnDetached()
        {
        }
    }
}

