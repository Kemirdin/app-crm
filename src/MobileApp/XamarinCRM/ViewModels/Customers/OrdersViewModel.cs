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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinCRM.Clients;
using XamarinCRM.Statics;
using XamarinCRM.ViewModels.Base;
using XamarinCRM.Extensions;
using XamarinCRM.AppModels;
using XamarinCRM.Models;

namespace XamarinCRM.ViewModels.Customers
{
    public class OrdersViewModel : BaseViewModel
    {
        public Account Account { get; private set; }

        List<Order> _Orders;

        ObservableCollection<Grouping<Order, string>> _OrderGroups;

        public ObservableCollection<Grouping<Order, string>> OrderGroups
        {
            get { return _OrderGroups; }
            set
            {
                _OrderGroups = value;
                OnPropertyChanged("OrderGroups");
            }
        }

        readonly IDataClient _DataClient;

        public OrdersViewModel(Account account)
        {
            Account = account;

            _Orders = new List<Order>();

            _DataClient = DependencyService.Get<IDataClient>();

            OrderGroups = new ObservableCollection<Grouping<Order, string>>();

            MessagingCenter.Subscribe<Order>(this, MessagingServiceConstants.SAVE_ORDER, order =>
                {
                    var index = _Orders.IndexOf(order);
                    if (index >= 0)
                    {
                        _Orders[index] = order;
                    }
                    else
                    {
                        _Orders.Add(order);
                    }

                    GroupOrders();
                });
        }

        Command _LoadOrdersCommand;

        /// <summary>
        /// Command to load orders
        /// </summary>
        public Command LoadOrdersCommand
        {
            get
            {
                return _LoadOrdersCommand ??
                (_LoadOrdersCommand = new Command(async () =>
                        await ExecuteLoadOrdersCommand()));
            }
        }

        public async Task ExecuteLoadOrdersCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            var orders = new List<Order>();
            orders.AddRange(await _DataClient.GetOpenOrdersForAccountAsync(Account.Id));
            orders.AddRange(await _DataClient.GetClosedOrdersForAccountAsync(Account.Id));

            _Orders.Clear();
            _Orders.AddRange(SortOrders(orders));

            GroupOrders();

            IsBusy = false;
        }

        void GroupOrders()
        {
            OrderGroups.Clear();
            OrderGroups.AddRange(_Orders, "Status"); // The AddRange() method here is a custom extension to ObservableCollection<Grouping<K,T>>. Check out its declaration; it's pretty neat.
        }

        static IEnumerable<Order> SortOrders(IEnumerable<Order> orders)
        {
            return orders.OrderByDescending(x => x.IsOpen).ThenByDescending(x => x.OrderDate).ThenByDescending(x => x.ClosedDate);
        }
    }
}

