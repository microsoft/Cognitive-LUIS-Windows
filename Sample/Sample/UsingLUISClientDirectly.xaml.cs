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
    public partial class UsingLUISClientDirectly : Page
    {
        private LuisResult prevResult { get; set; }

        public UsingLUISClientDirectly()
        {
            InitializeComponent();
        }

        private void btnPredict_Click(object sender, RoutedEventArgs e)
        {
            Predict();
        }

        private void btnReply_Click(object sender, RoutedEventArgs e)
        {
            if (prevResult == null || (prevResult.DialogResponse != null
                && prevResult.DialogResponse.Status == "Finished"))
            {
                ((MainWindow)Application.Current.MainWindow).Log("There is nothing to reply to.");
                return;
            }
            try
            {
                Reply();
            }
            catch (Exception exception)
            {
                ((MainWindow)Application.Current.MainWindow).Log(exception.Message);
            }
        }

        public async void Predict()
        {
            string appId = txtAppId.Text;
            string subscriptionKey = ((MainWindow)Application.Current.MainWindow).SubscriptionKey;
            bool verbose = true;
            string textToPredict = txtPredict.Text;
            string forceSetParameterName = txtForceSet.Text;
            try
            {
                LuisClient client = new LuisClient(appId, subscriptionKey, verbose);
                LuisResult res = await client.Predict(textToPredict);
                processRes(res);
                ((MainWindow)Application.Current.MainWindow).Log("Predicted successfully.");
            }
            catch (System.Exception exception)
            {
                ((MainWindow)Application.Current.MainWindow).Log(exception.Message);
            }
        }

        public async void Reply()
        {
            string appId = txtAppId.Text;
            string subscriptionKey = ((MainWindow)Application.Current.MainWindow).SubscriptionKey;
            bool preview = true;
            string textToPredict = txtPredict.Text;
            string forceSetParameterName = txtForceSet.Text;
            try
            {
                LuisClient client = new LuisClient(appId, subscriptionKey, preview);
                LuisResult res = await client.Reply(prevResult, textToPredict, forceSetParameterName);
                processRes(res);
                ((MainWindow)Application.Current.MainWindow).Log("Replied successfully.");
            }
            catch (System.Exception exception)
            {
                ((MainWindow)Application.Current.MainWindow).Log(exception.Message);
            }
        }

    private void processRes(LuisResult res)
        {
            txtPredict.Text = "";
            prevResult = res;
            queryTextBlock.Text = res.OriginalQuery;
            topIntentTextBlock.Text = res.TopScoringIntent.Name;
            List<string> entitiesNames = new List<string>();
            var entities = res.GetAllEntities();
            foreach(Entity entity in entities)
            {
                entitiesNames.Add(entity.Name);
            }
            entitiesListBox.ItemsSource = entitiesNames;
            if (res.DialogResponse != null)
            {
                if (res.DialogResponse.Status != "Finished")
                {
                    dialogTextBlock.Text = res.DialogResponse.Prompt;
                }
                else
                {
                    dialogTextBlock.Text = "Finished";
                }
            }
        }
    }
}
