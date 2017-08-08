using System;

namespace EdmxConverter.DomainLogic.Service
{
    public static class FlowExtensions
    {
        public static TResult Then<TObject, TResult>(this TObject obj, Func<TObject, TResult> func)
        {
            return func(obj);
        }
    }
}