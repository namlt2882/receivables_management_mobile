using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;

namespace RCM.Mobile.Components
{
    public class HideableToolbarItem : Xamarin.Forms.ToolbarItem
    {
        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        public static readonly BindableProperty IsVisibleProperty =
          BindableProperty.Create(nameof(IsVisible),
            typeof(bool),
            typeof(HideableToolbarItem),
            true,
            propertyChanged: OnIsVisibleChanged);

        private string oldText = "";
        private System.Windows.Input.ICommand oldCommand = null;

        private static void OnIsVisibleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var item = bindable as HideableToolbarItem;

            var newValueBool = (bool)newValue;
            var oldValueBool = (bool)oldValue;

            if (!newValueBool && oldValueBool)
            {
                item.oldText = item.Text;
                item.oldCommand = item.Command;
                item.Text = "";
                item.Command = null;
            }

            if (newValueBool && !oldValueBool)
            {
                item.Text = item.oldText;
                item.Command = item.oldCommand;
            }
        }
    }

    //public class HideableToolbarItem : Xamarin.Forms.ToolbarItem
    //{
    //    public HideableToolbarItem() : base()
    //    {
    //        this.InitVisibility();
    //    }

    //    private async void InitVisibility()
    //    {
    //        await Task.Delay(100);
    //        OnIsVisibleChanged(this, false, IsVisible);
    //    }

    //    public ContentPage Parent { set; get; }

    //    public bool IsVisible
    //    {
    //        get { return (bool)GetValue(IsVisibleProperty); }
    //        set { SetValue(IsVisibleProperty, value); }
    //    }

    //    public static BindableProperty IsVisibleProperty =
    //        BindableProperty.Create<HideableToolbarItem, bool>(o => o.IsVisible, false, propertyChanged: OnIsVisibleChanged);

    //    private static void OnIsVisibleChanged(BindableObject bindable, bool oldvalue, bool newvalue)
    //    {
    //        var item = bindable as HideableToolbarItem;

    //        if (item.Parent == null)
    //            return;

    //        var items = item.Parent.ToolbarItems;

    //        if (newvalue && !items.Contains(item))
    //        {
    //            items.Add(item);
    //        }
    //        else if (!newvalue && items.Contains(item))
    //        {
    //            items.Remove(item);
    //        }
    //    }
    //}
}
