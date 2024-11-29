public static class Permissions {
    public static List<string> GeneratePermissions(string module) {
        return new List<string>(){
            $"Permission.{module}.Create",
            $"Permission.{module}.Edit",
            $"Permission.{module}.Read",
            $"Permission.{module}.Delete",
        };
    }
}