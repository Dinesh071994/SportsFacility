namespace SportsFacility.Domain.Common
{
    public static class AppConstants
    {
        public static class Roles
        {
            public const string Admin = "Admin";
            public const string SuperAdmin = "SuperAdmin";
            public const string Member = "Member";
            public const string Customer = "Customer";
            public const string Trainer = "Trainer";
        }

        public static class Defaults
        {
            public const string DefaultPassword = "Welcome@123";
            public const string TrainerPassword = "Trainer@123";
            public const string SystemCreatedBy = "System";
        }

        public static class CacheKeys
        {
            public const string SubscriptionPlans = "subscription_plans";
            public const string Facilities = "facilities";
        }
    }
}
