namespace TaskTracker.Domain
{
    public enum TaskCategory
    {
        General, 
        Development, 
        Design, 
        Marketing,
        Research
    }

    public static class AIConstants
    {
        public const string DefaultModel = "typeform/distilbert-base-uncased-mnli";
        public const string DefaultBaseUrl = "https://api-inference.huggingface.co/models";
        public const string AuthHeaderScheme = "Bearer";
        public const string HypothesisTemplate = "This task is related to {}.";
        public static readonly string[] CandidateLabels =
        {
                "Work", "Personal", "Health", "Fitness", "Shopping", "Travel", "Finance",
                "Chores", "Errands", "Learning", "Family", "Social", "Household",
                "Self Improvement", "Technical", "Meetings", "Appointments", "Other",
                "Computer", "Laptop", "Face"
            };
    }
}
