using System;
using System.Collections.Generic;
using System.Text;

namespace TazzerClean.Util
{
    public class TazzerCleanConstants
    {
        public const string GeneralError = "Something went wrong";
        public const string PasswordChanged = "Password changed successfully.";
        public const string ProfileUpdated = "Profile Updated successfully.";
        public const string SavedSuccess = "Data saved successfully";
        public const string InviteSent = "Invite sent successfully";
        public const string InvalidLink = "Invalid Invitation";
        public const string ServicePageExists = "Page with this service already exists.";
        //public const 
    }

    public class EmailSubjectConstants
    {
        public const string WelcomeEmail = "Welcome to Tazzer Clean";
        public const string ForgotPassword = "Forgot your password?";
        public const string ResetPassword = "Password reset successful.";
        
    }

    public class StripeConstants
    {
        public const string CustomerAdded = "Customer added to stripe";
        public const string SubscriptionAdded = "Subscription added for the customer";
        public const string PaymentInitiated = "Payment initiated";
        public const string CancelSubscription = "Cancel subscription initiated";
        public const string UpdateSubscription = "Update subscription initiated";
        public const string StripeNotFound = "Some issues with teacher's account. Please reach support team";
        public const string OrderFails = "Failed to add order, please try again";
    }
}
