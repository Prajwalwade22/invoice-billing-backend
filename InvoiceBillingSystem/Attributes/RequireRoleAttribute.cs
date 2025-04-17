using Microsoft.AspNetCore.Mvc.Filters;

namespace InvoiceBillingSystem.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RequireRoleAttribute : Attribute, IFilterMetadata
    {
        public string[] AllowedRoles { get; }

        public RequireRoleAttribute(params string[] roles)
        {
            AllowedRoles = roles;
        }
    }
}
