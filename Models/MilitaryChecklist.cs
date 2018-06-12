using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QnABot.Models
{
    [Serializable]
    public class MilitaryChecklist
    {
        [Prompt("Have you applied to the Coding Dojo?")]
        public bool ApplicationStep { get; set; }

        [Prompt("Have you done the online admissions orientation?")]
        public bool AdmissionsOrientationStep { get; set; }

        [Prompt("Have you scheduled a phone interview with an admissions specialists?")]
        public bool PhoneInterviewStep { get; set; }

        [Prompt("Have you applied for scholarships? (Yes, you can use them to lessen the burden on your GI Bill)")]
        public bool ScholarshipStep { get; set; }

        [Prompt("Do you have a recent copy of your Certificate of Eligibility?")]
        public bool CertificatedOfEligibilityStep { get; set; }

        [Prompt("Have you submitted an application for benefits through VONAPP?")]
        public bool BenefitsStep { get; set; }

        [Prompt("Have you updated WAVE? (only needed if you're using MGIB. Choose 'yes' if you're using Post 9/11)")]
        public bool WAVE_Step { get; set; }

        [Prompt("Have you scheduled an interview with the community outreach specialist?")]
        public bool CommunityOutreachStep { get; set; }

        [Prompt("Have you taken the skills assessment?")]
        public bool SkillsAssessmentStep { get; set; }

        [Prompt("Have you checked back with the community outreach specialist after submitting your VA paperwork to the dojo?")]
        public bool CheckStatusStep { get; set; }


        public static IForm<MilitaryChecklist> BuildForm()
        {
            var builder = new FormBuilder<MilitaryChecklist>();

            builder
                .Message("We do so love our checklists in the military! Let's get started! Please answer YES or NO to the following questions:")
                .OnCompletion(async (context, order) =>
                {
                    await context.PostAsync("For further information, please inquire about any step you haven't completed.");
                });
            return builder.Build();
        }
    }
}