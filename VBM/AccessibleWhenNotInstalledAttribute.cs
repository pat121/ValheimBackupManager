using System;

namespace VBM
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    class AccessibleWhenNotInstalledAttribute : Attribute
    {
    }
}
