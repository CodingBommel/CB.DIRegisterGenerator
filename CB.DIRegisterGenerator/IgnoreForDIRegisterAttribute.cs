using System;

namespace CB.DIRegisterGenerator
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class IgnoreForDIRegisterAttribute : Attribute
    {

    }
}
