.assembly extern mscorlib {}
.assembly MyFirstSample {}

/*.method static public void main(class [mscorlib]System.String[]) cil managed
{
    .entrypoint
    .maxstack 1

    ldstr "Hi my name is {0}"
    
    ldarg.0
    ldc.i4.0
    ldelem.ref  // this is a reference .ref because is a string

    call void [mscorlib]System.Console::WriteLine(string, object)
    ret
}*/

/*.method static public void main(class System.String[]) cil managed
{
    .entrypoint
    .maxstack 1
   
    ldarg.0
    ldc.i4.0
    ldelem.ref 

    call int32 [mscorlib]System.Convert::ToInt32(string)
    ldc.i4 10
    mul

    stloc.0
    ldstr "Result is {0}"
    ldloc.0
    box int32

    call void [mscorlib]System.Console::WriteLine(string, object)
    ret
}*/

.method static public void main(class System.String[]) cil managed
{
    .entrypoint
    .maxstack 1
   
    ldarg.0
    ldc.i4.0
    ldelem.ref

    newobj instance void MyFirstSample.Person::.ctor(string)
    call instance string MyFirstSample.Person::get_Name()

    stloc.0
    ldstr "Hello there {0}"
    ldloc.0

    call void [mscorlib]System.Console::WriteLine(string, object)
    ret
}

.class public MyFirstSample.Person {
    .field private string name
    .method public hidebysig specialname instance string get_Name() cil managed{
        ldarg.0
        ldfld string MyFirstSample.Person::name
    }
    .method public hidebysig specialname rtspecialname instance void .ctor(string name) cil managed{
        ldarg.0
        ldarg.1
        stfld string MyFirstSample.Person::name
        ret
    }
    .property instance string Name(){
        .get instance string MyFirstSample.Person::get_Name()
    }
}