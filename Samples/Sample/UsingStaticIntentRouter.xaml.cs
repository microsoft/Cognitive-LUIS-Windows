// 
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
// 
// Microsoft Cognitive Services: http://www.microsoft.com/cognitive
// 
// Microsoft Cognitive Services Github:
// https://github.com/Microsoft/Cognitive
// 
// Copyright (c) Microsoft Corporation
// All rights reserved.
// 
// MIT License:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

using System;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Cognitive.LUIS;

namespace Sample
{
    /// <summary>
    /// Interaction logic for UsingLUISClientDirectly.xaml
    /// </summary>
    public partial class UsingStaticIntentRouter : Page
    {
        public UsingStaticIntentRouter()
        {
            InitializeComponent();
        }

        private void btnPredict_Click(object sender, RoutedEventArgs e)
        {
            Predict();
        }

        public async void Predict()
        {
            string _appId = txtAppId.Text;
            string _subscriptionKey = ((MainWindow)Application.Current.MainWindow).SubscriptionKey;
            string _textToPredict = txtPredict.Text;

            try
            {
                // Set up an intent router using the StaticIntentHandlers class to process intents
                using (var router = IntentRouter.Setup<StaticIntentHandlers>(_appId, _subscriptionKey))
                {
                    // Feed it into the IntentRouter to see if it was handled
                    var handled = await router.Route(_textToPredict, this);
                    if (!handled)
                    {
                        queryTextBlock.Text = "";
                        topIntentTextBlock.Text = "";
                        ((MainWindow)Application.Current.MainWindow).Log("Intent was not matched confidently, maybe more training required?");
                    }
                    else
                    {
                        ((MainWindow)Application.Current.MainWindow).Log("Predicted successfully.");
                    }
                }
            }
            catch (Exception exception)
            {
                ((MainWindow)Application.Current.MainWindow).Log(exception.Message);
            }
        }
    }
}
