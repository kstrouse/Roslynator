﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Roslynator.CSharp.CodeFixes.Test
{
    internal static class AddTypeArgument
    {
        private class Foo : Base
        {
            private void Bar()
            {
                List list1 = null;
                System.Collections.Generic.List list2 = null;
            }
        }

        private class Base<T>
        {

        }

        private class Base<T, T2>
        {
        }
    }
}
