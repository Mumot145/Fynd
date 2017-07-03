using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Text;
using Xamarin.Forms;
using System.Collections;

namespace SaveOn.Behaviors
{
    public class InfiniteScroll : Behavior<ListView>
    {
        public static readonly BindableProperty LoadMoreCommandProperty = BindableProperty.Create("LoadMoreCommand", typeof(ICommand), typeof(InfiniteScroll), null);
        public ICommand LoadMoreCommand
        {
            get
            {
                return (ICommand)GetValue(LoadMoreCommandProperty);
            }
            set
            {
                SetValue(LoadMoreCommandProperty, value);
            }
        }
        public ListView AssociatedObject
        {
            get;
            private set;
        }
        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);
            AssociatedObject = bindable;
            bindable.BindingContextChanged += Bindable_BindingContextChanged;
            bindable.ItemAppearing += InfiniteListView_ItemAppearing;
        }
        private void Bindable_BindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
        }
        protected override void OnDetachingFrom(ListView bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.BindingContextChanged -= Bindable_BindingContextChanged;
            bindable.ItemAppearing -= InfiniteListView_ItemAppearing;
        }
        void InfiniteListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var items = AssociatedObject.ItemsSource as IList;
            if (items != null && e.Item == items[items.Count - 1])
            {
                if (LoadMoreCommand != null && LoadMoreCommand.CanExecute(null)) LoadMoreCommand.Execute(null);
            }
        }
    }
}
