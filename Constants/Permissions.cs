public static class Permissions {
    public static List<string> GeneratePermissions(string module) {
        return new List<string>(){
            $"Permission.{module}.Create",
            $"Permission.{module}.Edit",
            $"Permission.{module}.Read",
            $"Permission.{module}.Delete",
        };
    }
    public static List<string> GenerateAllPermissions() {
        var modules = Enum.GetValues(typeof(Modules));
        var allPermissions = new List<string>();
        foreach(var module in modules) {
            allPermissions.AddRange(GeneratePermissions(module.ToString()));
        }

        return allPermissions;
    }
}