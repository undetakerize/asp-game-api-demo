namespace GameService.Application.Constant;

public static class ValidationMessage
{
    public static class Field
    {
        public const string FieldRequired = "{0} is required.";
        public const string FieldNotEmpty = "{0} cannot be empty";
        public const string FieldMaxLength = "{0} cannot be longer than {1} characters";
    }
}