using System;
using CSharpFunctionalExtensions;
using EdmxConv.Schema;
using EdmxConv.Schema.Extensions;

namespace EdmxConv.Behaviours.Modules
{
    public class Base64Module
    {
        public static Result<ByteArray> Base64ToByteArray(ResourceEdmx resourceEdmx)
        {
            try
            {
                var byteArray = Convert.FromBase64String(resourceEdmx.Value).ToByteArray();
                return Result.Ok(byteArray);
            }
            catch (FormatException)
            {
                return Result.Fail<ByteArray>("Cannot convert given EDMX. Format of BASE64 is invalid.");
            }
        }

        public static Result<string> BytesToBase64(ByteArray array)
        {
            try
            {
                var str = Convert.ToBase64String(array.Bytes);
                return Result.Ok(str);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}