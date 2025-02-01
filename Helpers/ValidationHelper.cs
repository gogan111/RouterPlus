namespace RouterPlus.Helpers;

public static class ValidationHelper
{
    public static bool ValidateField(
        string value,
        string fieldName,
        ref string errorMessage,
        ref bool isErrorVisible)
    {
        if (isErrorVisible)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            errorMessage = $"{fieldName} cannot be empty.";
            isErrorVisible = true;
            return false;
        }

        errorMessage = string.Empty;
        isErrorVisible = false;
        return true;
    }
}