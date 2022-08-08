using NUnit.Framework;
using System;

namespace LogModuleTest
{
    public class InnerExceptionTest
    {


        [Test]
        public void InnerException_Test()
        {
            InnerException.LogInnerException(new InvalidCastException());
        }





    }
}