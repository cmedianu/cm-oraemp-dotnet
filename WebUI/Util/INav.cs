namespace Hides.WebUI.Util
{
    // ReSharper disable once InconsistentNaming
    public static class INav
    {
        public static string DepartmentList()
        {
            return "department/list";
        }

        public static string DepartmentEdit()
        {
            return "department/edit";
        }

        public static string DepartmentEdit(string departmentId)
        {
            return $"department/edit/{departmentId}";
        }
    }
}