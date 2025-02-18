using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPopulate
{
    /// <summary>
    /// Enum for handling references in recursive objects.
    /// </summary>
    public enum RecursionReferenceBehavior
    {
        NewInstance,
        ExistingReference,
        NullReference
    }
}
