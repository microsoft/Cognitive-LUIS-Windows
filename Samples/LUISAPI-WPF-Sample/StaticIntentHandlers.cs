using System;
using System.Windows;
using Microsoft.Cognitive.LUIS;

namespace Sample
{
    class StaticIntentHandlers
    {
        // 0.65 is the confidence score required by this intent in order to be activated
        // Only picks out a single entity value
        [IntentHandler(0.65, Name = "Book a flight")]
        public void BookAFlight(LuisResult result, object context)
        {
            UsingStaticIntentRouter usingStaticIntentRouter = (UsingStaticIntentRouter)context;
            usingStaticIntentRouter.queryTextBlock.Text = result.OriginalQuery;
            usingStaticIntentRouter.topIntentTextBlock.Text = "Book a flight";
        }

        [IntentHandler(0.65, Name = "Book a cab")]
        public void BookACab(LuisResult result, object context)
        {
            UsingStaticIntentRouter usingStaticIntentRouter = (UsingStaticIntentRouter)context;
            usingStaticIntentRouter.queryTextBlock.Text = result.OriginalQuery;
            usingStaticIntentRouter.topIntentTextBlock.Text = "Book a cab";
        }

        [IntentHandler(0.7, Name = "None")]
        public static void None(LuisResult result, object context)
        {
            UsingStaticIntentRouter usingStaticIntentRouter = (UsingStaticIntentRouter)context;
            usingStaticIntentRouter.queryTextBlock.Text = result.OriginalQuery;
            usingStaticIntentRouter.topIntentTextBlock.Text = "None";
        }
    }
}
