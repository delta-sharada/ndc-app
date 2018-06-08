using Fly360.iOS;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("Fly360")]
[assembly: ExportEffect(typeof(EntryEffect), "EntryEffect")]
namespace Fly360.iOS
{
    [Preserve(AllMembers = true)]
    public class EntryEffect : PlatformEffect
    {
        UITextBorderStyle old;

        protected override void OnAttached()
        {
            var editText = Control as UITextField;
            if (editText == null)
                return;

            old = editText.BorderStyle;
            editText.BorderStyle = UITextBorderStyle.None;
        }

        protected override void OnDetached()
        {
            var editText = Control as UITextField;
            if (editText == null)
                return;

            editText.BorderStyle = old;
        }
    }
}