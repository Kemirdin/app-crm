﻿//
//  Copyright 2015  Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using Xamarin.Forms;
using XamarinCRM.Cells;
using XamarinCRM.Statics;
using XamarinCRM.Views.Base;

namespace XamarinCRM.Views.Customers
{
    public class CustomerOrderListView : BaseNonPersistentSelectedItemListView
    {
        public CustomerOrderListView()
        {
            HasUnevenRows = true; // Circumvents calculating heights for each cell individually. The rows of this list view will have a static height.
            RowHeight = (int)Sizes.LargeRowHeight; // set the row height for the list view items
            SeparatorVisibility = SeparatorVisibility.None;
            ItemTemplate = new DataTemplate(typeof(OrderListItemCell));
            SeparatorColor = Palette._013;
        }
    }
}

