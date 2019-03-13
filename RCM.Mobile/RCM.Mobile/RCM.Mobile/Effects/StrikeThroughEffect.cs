using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace RCM.Mobile.Effects
{
    public class StrikeThroughEffect : RoutingEffect
    {
        public static readonly BindableProperty IsStrikeThroughProperty =
            BindableProperty.CreateAttached("IsStrikeThrough", typeof(bool),
                typeof(StrikeThroughEffect), false, propertyChanged: OnStrikeThroughChanged);

        public StrikeThroughEffect()
            : base("RCM.Mobile.StrikeThroughEffect")
        {
        }

        public static bool GetIsStrikeThrough(BindableObject bindable)
        {
            return (bool)bindable.GetValue(IsStrikeThroughProperty);
        }

        public static void SetIsStrikeThrough(BindableObject bindable, bool value)
        {
            bindable.SetValue(IsStrikeThroughProperty, value);
        }

        private static void OnStrikeThroughChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as View;

            if (view != null)
            {
                if ((bool)newValue)
                {
                    var effect = new StrikeThroughEffect();

                    view.Effects.Add(effect);
                }
                else
                {
                    var effect = view.Effects
                        .OfType<StrikeThroughEffect>()
                        .FirstOrDefault();

                    if (effect != null)
                    {
                        view.Effects.Remove(effect);
                    }
                }
            }
        }
    }

}
