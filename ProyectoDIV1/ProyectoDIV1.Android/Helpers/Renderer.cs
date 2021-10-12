using Android.App;
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Material.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.DatePicker), typeof(ProyectoDIV1.Droid.Helpers.DatePickerRenderer), new[]
{ typeof(VisualMarker.MaterialVisual) })]
namespace ProyectoDIV1.Droid.Helpers
{
    class DatePickerRenderer : MaterialDatePickerRenderer

    {

        public DatePickerRenderer(Context context) : base(context) { }



        protected override DatePickerDialog CreateDatePickerDialog(int year, int month, int day)

        {

            Xamarin.Forms.DatePicker view = Element;

            var dialog = new DatePickerDialog(Context, Resource.Style.AppCompatDialogStyle, (o, e) =>

             {

                 view.Date = e.Date;

                 ((IElementController)view).SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);

             }, year, month, day);



            return dialog;

        }

    }

}