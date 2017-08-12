using ApprovalTests;

namespace EdmxConverter.DomainLogic.Tests
{
    public static class ApprovalsExtensions
    {
        public static T Verify<T>(this T target)
        {
            Approvals.Verify(target);
            return target;
        }
    }
}
