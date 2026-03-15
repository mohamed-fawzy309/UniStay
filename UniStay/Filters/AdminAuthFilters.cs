// Filters/AdminAuthFilters.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UniStay.Filters
{
    // ─────────────────────────────────────────
    //  تأكد إن الأدمن مسجل دخوله
    // ─────────────────────────────────────────
    public class AdminAuthAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            if (string.IsNullOrEmpty(session.GetString("AdminId")))
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
                return;
            }
            base.OnActionExecuting(context);
        }
    }

    // ─────────────────────────────────────────
    //  Supervisor فقط
    // ─────────────────────────────────────────
    public class SupervisorOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var role = context.HttpContext.Session.GetString("AdminRole");
            if (role != "Supervisor")
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Auth", null);
                return;
            }
            base.OnActionExecuting(context);
        }
    }

    // ─────────────────────────────────────────
    //  Manager أو Supervisor (مش Staff)
    // ─────────────────────────────────────────
    public class ManagerOrAboveAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var role = context.HttpContext.Session.GetString("AdminRole");
            if (role != "Supervisor" && role != "Manager")
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Auth", null);
                return;
            }
            base.OnActionExecuting(context);
        }
    }

    // ─────────────────────────────────────────
    //  Helper Extension للـ Views
    // ─────────────────────────────────────────
    public static class SessionRoleHelper
    {
        public static bool IsSupervisor(this ISession session)
            => session.GetString("AdminRole") == "Supervisor";

        public static bool IsManagerOrAbove(this ISession session)
            => session.GetString("AdminRole") is "Supervisor" or "Manager";

        public static bool IsStaff(this ISession session)
            => session.GetString("AdminRole") == "Staff";
    }
}
