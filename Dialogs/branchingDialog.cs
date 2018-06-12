using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using QnABot.Dialogs;
using Microsoft.Bot.Sample.QnABot;
using QnABot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace QnABot.Dialogs
{
    
    
    [Serializable]
    public class branchingDialog : LuisDialog<object>
    {
        [LuisIntent("")]
        [LuisIntent("None")]

        public async Task None(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
            //await context.PostAsync($"Why hello there! I'm afraid I don't understand your question of {result.Query}. Perhaps you can rephrase it?");
        }
        //
        //Internationl Intent -> Route to QnA Maker DONE ********************************
        //
        [LuisIntent("international")]
        public async Task InternationalIntent(IDialogContext context, IAwaitable<IMessageActivity> message, LuisResult result)
        {

            var msgToForward = await message as Activity;
            await context.Forward(new RootDialog(), this.AfterFAQDialog, msgToForward, CancellationToken.None);
        }

        private async Task AfterFAQDialog(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            context.Wait(this.MessageReceived);
        }
        //Internationa DONE *************************************************************************************

        [LuisIntent("military.general")]
        public async Task MilitaryGenIntent(IDialogContext context, LuisResult result)
        {
            var answer = result.Query;
            //await this.ShowLuisResult(context, result);

            if (answer.Contains("MHA") || answer.Contains("BAH") || answer.Contains("housing allowance"))
            {
                await context.PostAsync("The housing allowance for the Seattle campus of the Coding Dojo pays out at the current year's E-5 with dependant rate for zipcode 98004. It only covers the 14 active weeks of school, and does not include career week after graduation.");
            }
            else
            {
                await context.PostAsync("Veterans are very much welcome at the Coding Dojo! We acceps veteran students (we need programming school too!) at the Seattle campus only. You can use your Post 9/11 GI Bill or the MGIB, so long as you have 6+ months of remaining benefits.");
            }

        }
        [LuisIntent("military.checklist")]
        public Task MilitaryChecklistIntent(IDialogContext context, LuisResult result)
        {
            //await this.ShowLuisResult(context, result);
            var checklist = new FormDialog<MilitaryChecklist>(new MilitaryChecklist(), MilitaryChecklist.BuildForm, FormOptions.PromptInStart, null);
            context.Call<MilitaryChecklist>(checklist, After_CompletedChecklist);

            return Task.CompletedTask;
        }

        private async Task After_CompletedChecklist(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Which step do you need assistance with?");
            context.Wait(MessageReceived);
        }

        [LuisIntent("military.standardSteps")]
        public async Task MilitaryStandardIntent(IDialogContext context, LuisResult result)
        {
            var answer = result.Query;
            //await this.ShowLuisResult(context, result);
            System.Diagnostics.Debug.WriteLine(context);

            await context.PostAsync("Applying to the dojo, taking the admissions orientation, a phone interview and a skills assessment as well as applying for scholarships are all standard, non-veteran-specific steps to applying to the Dojo. Your admissions advisor will help you with all of these steps. Keep an eye on your email!");
        }
        [LuisIntent("military.va")]
        public async Task MilitaryVAIntent(IDialogContext context, LuisResult result)
        {
            //await this.ShowLuisResult(context, result);
            var holdme = result.Query;
            if (holdme.Contains("WAVE"))
            {
                await context.PostAsync("If you're using the MGIB, you can update your WAVE at the VA's GI Bill webpage (https://www.gibill.va.gov/wave/index.do).");
            }
            else if (holdme.Contains("benefits") || holdme.Contains("eligibility"))
            {
                await context.PostAsync("Both the certificate of Eligibility and applying for benefits can be accomplished through the VONAPP website (https://www.ebenefits.va.gov/ebenefits/vonapp).");
            }
            else if (holdme.Contains("long"))
            {
                await context.PostAsync("The biggest hiccup is after you've spoken to the Outreach Advisor. It can take up to 8 weeks to get your benefits approved through the Dojo's legal office: keep on top of this!Email your Outreach Advisor if you haven't heard back in a couple of weeks.");
            }
            else
            {
                await context.PostAsync("Both the certificate of Eligibility and applying for benefits can be accomplished through the VONAPP website (https://www.ebenefits.va.gov/ebenefits/vonapp). If you're using the MGIB, you can update your WAVE at the VA's GI Bill webpage (https://www.gibill.va.gov/wave/index.do). The biggest hiccup is after you've spoken to the Outreach Advisor. It can take up to 8 weeks to get your benefits approved through the Dojo's legal office: keep on top of this! Email your Outreach Advisor if you haven't heard back in a couple of weeks.");
            }
            // System.Diagnostics.Debug.WriteLine(holdme.GetType());
        }

        [LuisIntent("greeting")]
        public async Task GreetingIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Why hello to you to! How can I help you?");
        }

        [LuisIntent("goodbye")]
        public async Task GoodbyeIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I hope I've been as helpful as possible! Feel free to come back at any time if you have further questions!");
        }

        [LuisIntent("help")]
        public async Task HelpIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I am here to help! Are you an international student or a veteran?");
        }

        private async Task ShowLuisResult(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"You have reached {result.Intents[0].Intent}. You said: {result.Query}");
            context.Wait(MessageReceived);
        }
    }
}
