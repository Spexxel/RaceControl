﻿using Microsoft.Xaml.Behaviors;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace RaceControl.Core.Behaviors
{
    public class CheckBoxMultiSelectBehavior : Behavior<CheckBox>
    {
        public INotifyCollectionChanged SelectedItems
        {
            get => (INotifyCollectionChanged)GetValue(SelectedItemsProperty);
            set => SetValue(SelectedItemsProperty, value);
        }

        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(nameof(SelectedItems), typeof(INotifyCollectionChanged), typeof(CheckBoxMultiSelectBehavior), new PropertyMetadata(OnSelectedItemsChanged));

        protected override void OnAttached()
        {
            base.OnAttached();
            SubscribeToEvents(true, false);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            UnsubscribeFromEvents(true, false);
        }

        private static void OnSelectedItemsChanged(DependencyObject target, DependencyPropertyChangedEventArgs args)
        {
            var behavior = (CheckBoxMultiSelectBehavior)target;

            if (args.OldValue is INotifyCollectionChanged oldCollection)
            {
                oldCollection.CollectionChanged -= behavior.SelectedItems_CollectionChanged;
            }

            if (args.NewValue is INotifyCollectionChanged newCollection)
            {
                newCollection.CollectionChanged += behavior.SelectedItems_CollectionChanged;
                behavior.SelectedItems_CollectionChanged(behavior, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, (IList)newCollection));
            }
        }

        private void SelectedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UnsubscribeFromEvents(true, false);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems.Contains(AssociatedObject.Tag))
                    {
                        AssociatedObject.IsChecked = true;
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems.Contains(AssociatedObject.Tag))
                    {
                        AssociatedObject.IsChecked = false;
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    AssociatedObject.IsChecked = false;
                    break;
            }

            SubscribeToEvents(true, false);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (e.Source is CheckBox checkBox)
            {
                UnsubscribeFromEvents(false, true);

                var list = (IList)SelectedItems;
                var item = checkBox.Tag;

                if (checkBox.IsChecked == true && !list.Contains(item))
                {
                    list.Add(item);
                }

                if (checkBox.IsChecked == false && list.Contains(item))
                {
                    list.Remove(item);
                }

                SubscribeToEvents(false, true);
            }
        }

        private void SubscribeToEvents(bool checkBox, bool collection)
        {
            if (checkBox)
            {
                AssociatedObject.Checked += CheckBox_Checked;
                AssociatedObject.Unchecked += CheckBox_Checked;
            }

            if (collection)
            {
                SelectedItems.CollectionChanged += SelectedItems_CollectionChanged;
            }
        }

        private void UnsubscribeFromEvents(bool checkBox, bool collection)
        {
            if (checkBox)
            {
                AssociatedObject.Checked -= CheckBox_Checked;
                AssociatedObject.Unchecked -= CheckBox_Checked;
            }

            if (collection)
            {
                SelectedItems.CollectionChanged -= SelectedItems_CollectionChanged;
            }
        }
    }
}