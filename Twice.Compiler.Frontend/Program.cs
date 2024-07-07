// See https://aka.ms/new-console-template for more information

using System;
using System.IO;
using Antlr4.Runtime;
using LLVMSharp.Interop;
using Twice.Compiler.Backend.AST;
using Twice.Compiler.Backend.Codegen;
using System.Linq;
using TypedAST;

if (args.Length is < 1 or > 3)
{
    Console.WriteLine("Please pass one command line argument, specifying the input source file.");
    Console.WriteLine("An additional argument may be passed to specify the output file");
    return;
}

string fileName = args[0];

if (!File.Exists(fileName))
{
    Console.WriteLine("Input source file not found");
    return;
}

string codeToRun = File.ReadAllText(fileName);

CommonTokenStream stream = new CommonTokenStream(new TwiceLexer(new AntlrInputStream(codeToRun)));

TwiceParser parser = new TwiceParser(stream);

TwiceVisitor visitor = new TwiceVisitor();

TwiceChunk chunk = (TwiceChunk)visitor.Visit(parser.prog());

if (parser.NumberOfSyntaxErrors > 0)
    throw new Exception("Could not continue due to parse error");

TypeChecker typeChecker;

try
{
    typeChecker = new TypeChecker(chunk);
}
catch (Exception e)
{
    Console.WriteLine("Type checking failed: " + e.Message);
    return;
}



LLVMModuleRef moduleRef = LLVMModuleRef.CreateWithName("twc");

CodegenVisitor cvisitor = new CodegenVisitor(moduleRef, typeChecker);

chunk.Statements.ForEach(x => cvisitor.Visit(x));

cvisitor.Finish();

Console.WriteLine("--------");
{
    if (!moduleRef.TryVerify(LLVMVerifierFailureAction.LLVMPrintMessageAction, out var error))
    {
        Console.WriteLine(error);
        return;
    }
}

var outName = args.Length > 1 ? args[1] : "out.ll";

moduleRef.WriteBitcodeToFile(outName);
Console.WriteLine($"Bitcode written to {outName}");