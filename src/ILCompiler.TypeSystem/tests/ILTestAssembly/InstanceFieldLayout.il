// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

.class public sequential ansi sealed beforefieldinit IsByRefLike.InvalidStruct
       extends [CoreTestAssembly]System.ValueType
{
  .field private valuetype [CoreTestAssembly]System.ByReference`1<object> ByRef
}

.class public auto ansi beforefieldinit IsByRefLike.InvalidClass1
       extends [CoreTestAssembly]System.Object
{
  .field private valuetype [CoreTestAssembly]System.ByReference`1<object> ByRef
}

.class public auto ansi beforefieldinit IsByRefLike.InvalidClass2
       extends [CoreTestAssembly]System.Object
{
  .custom instance void [CoreTestAssembly]System.Runtime.CompilerServices.IsByRefLikeAttribute::.ctor() = ( 01 00 00 00 ) 
  .field private valuetype [CoreTestAssembly]System.ByReference`1<object> ByRef
}
