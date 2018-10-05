//----------- Program header
.assembly extern mscorlib { auto }
.assembly ConsoleApp1 { }

.namespace ConsoleApp1 {
    .class public auto ansi Even extends[mscorlib]System.Object {
        //----------- Field declaration
        .field public static int32 val
        //----------- Method declaration
        .method public static void check() cil managed
        {
            .entrypoint // uncomment this line and compile with: ilasm Even => will produce Even.exe
            .locals init (int32 Retval)
            AskForNumber:
                ldstr "Enter a number"
                call void [mscorlib]System.Console::WriteLine (string)
				.try { // CHANGE!
					// Guarded block begins
					call string [mscorlib]System.Console::ReadLine ()
					//pop
					//ldnull
					ldstr "%d" // CHANGE! // ldsflda valuetype CharArray8 Format
					ldsflda int32 ConsoleApp1.Even::val
					call vararg int32 sscanf(string,string,...,int32*) // sscanf(string,int8*,...,int32*)
					stloc.0 //stloc Retval
					leave.s DidntBlowUp // CHANGE!
					// Guarded block ends
				} // CHANGE!
				// CHANGE block begins! --->
				catch [mscorlib]System.Exception
				{ // Exception handler begins
					pop
					ldstr "KABOOM!"
					call void [mscorlib]System.Console::WriteLine(string)
					leave.s DidntBlowUp //Return
				} // Exception handler ends
			DidntBlowUp:
				// <--- CHANGE block ends!
                ldloc.0 //ldloc Retval
                brfalse.s Error //brfalse Error
                ldsfld int32 ConsoleApp1.Even::val
                ldc.i4.1 //ldc.i4 1
                and
                brfalse.s ItsEven //brfalse ItsEven
                ldstr bytearray(6F 00 64 00 64 00 21 00 00 00)    //ldstr "odd!" <=> ldstr bytearray(6F 00 64 00 64 00 21 00 00 00)
                br.s PrintAndReturn //br PrintAndReturn
            ItsEven:
                ldstr "even!"
                br.s PrintAndReturn //br PrintAndReturn
            Error:
                ldstr "How rude!"
            PrintAndReturn:
                call void [mscorlib]System.Console::WriteLine (string)
                ldloc.0 //ldloc Retval
                brtrue.s AskForNumber //brtrue AskForNumber
			Return: // CHANGE!
				ret
        } // End of method
    } // End of class
} // End of namespace

//----------- Global items
//.field public static valuetype CharArray8 Format at FormatData
//----------- Data declaration
//.data FormatData = bytearray(25 64 00 00 00 00 00 00) // % d . . . . . .
//----------- Value type as placeholder
//.class public explicit CharArray8
//    extends [mscorlib]System.ValueType { .size 8 }
//----------- Calling unmanaged code
.method public static pinvokeimpl("msvcrt.dll" cdecl)
    vararg int32 sscanf(string,string) cil managed preservesig { }	//sscanf(string,int8*)