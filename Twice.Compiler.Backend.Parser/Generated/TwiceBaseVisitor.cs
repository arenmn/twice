//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from C:/Users/aren_/RiderProjects/Twice/Twice.Compiler.Backend.Parser/Twice.g4 by ANTLR 4.13.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="ITwiceVisitor{Result}"/>,
/// which can be extended to create a visitor which only needs to handle a subset
/// of the available methods.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.Diagnostics.DebuggerNonUserCode]
[System.CLSCompliant(false)]
public partial class TwiceBaseVisitor<Result> : AbstractParseTreeVisitor<Result>, ITwiceVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="TwiceParser.prog"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitProg([NotNull] TwiceParser.ProgContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="TwiceParser.chunk"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitChunk([NotNull] TwiceParser.ChunkContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="TwiceParser.statement"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitStatement([NotNull] TwiceParser.StatementContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>statementIf</c>
	/// labeled alternative in <see cref="TwiceParser.flowStatement"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitStatementIf([NotNull] TwiceParser.StatementIfContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>statementWhile</c>
	/// labeled alternative in <see cref="TwiceParser.flowStatement"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitStatementWhile([NotNull] TwiceParser.StatementWhileContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>statementFor</c>
	/// labeled alternative in <see cref="TwiceParser.flowStatement"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitStatementFor([NotNull] TwiceParser.StatementForContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>statementBlock</c>
	/// labeled alternative in <see cref="TwiceParser.flowStatement"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitStatementBlock([NotNull] TwiceParser.StatementBlockContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>statementFunctionDefinition</c>
	/// labeled alternative in <see cref="TwiceParser.flowStatement"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitStatementFunctionDefinition([NotNull] TwiceParser.StatementFunctionDefinitionContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="TwiceParser.functionType"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitFunctionType([NotNull] TwiceParser.FunctionTypeContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="TwiceParser.functionArgs"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitFunctionArgs([NotNull] TwiceParser.FunctionArgsContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="TwiceParser.forLoop"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitForLoop([NotNull] TwiceParser.ForLoopContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>functionCallStatement</c>
	/// labeled alternative in <see cref="TwiceParser.singleStatement"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitFunctionCallStatement([NotNull] TwiceParser.FunctionCallStatementContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>awaitStatement</c>
	/// labeled alternative in <see cref="TwiceParser.singleStatement"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitAwaitStatement([NotNull] TwiceParser.AwaitStatementContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>externFunctionDefinition</c>
	/// labeled alternative in <see cref="TwiceParser.singleStatement"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitExternFunctionDefinition([NotNull] TwiceParser.ExternFunctionDefinitionContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>channelPushStatement</c>
	/// labeled alternative in <see cref="TwiceParser.singleStatement"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitChannelPushStatement([NotNull] TwiceParser.ChannelPushStatementContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>declarationStatement</c>
	/// labeled alternative in <see cref="TwiceParser.singleStatement"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitDeclarationStatement([NotNull] TwiceParser.DeclarationStatementContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>returnStatement</c>
	/// labeled alternative in <see cref="TwiceParser.singleStatement"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitReturnStatement([NotNull] TwiceParser.ReturnStatementContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>assignmentStatement</c>
	/// labeled alternative in <see cref="TwiceParser.singleStatement"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitAssignmentStatement([NotNull] TwiceParser.AssignmentStatementContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="TwiceParser.functionCall"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitFunctionCall([NotNull] TwiceParser.FunctionCallContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>expressionAdd</c>
	/// labeled alternative in <see cref="TwiceParser.expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitExpressionAdd([NotNull] TwiceParser.ExpressionAddContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>awaitExpression</c>
	/// labeled alternative in <see cref="TwiceParser.expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitAwaitExpression([NotNull] TwiceParser.AwaitExpressionContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>expressionBool</c>
	/// labeled alternative in <see cref="TwiceParser.expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitExpressionBool([NotNull] TwiceParser.ExpressionBoolContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>expressionMultiply</c>
	/// labeled alternative in <see cref="TwiceParser.expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitExpressionMultiply([NotNull] TwiceParser.ExpressionMultiplyContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>expressionFunctionCall</c>
	/// labeled alternative in <see cref="TwiceParser.expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitExpressionFunctionCall([NotNull] TwiceParser.ExpressionFunctionCallContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>expressionModulo</c>
	/// labeled alternative in <see cref="TwiceParser.expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitExpressionModulo([NotNull] TwiceParser.ExpressionModuloContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>expressionNegate</c>
	/// labeled alternative in <see cref="TwiceParser.expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitExpressionNegate([NotNull] TwiceParser.ExpressionNegateContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>expressionParenthesis</c>
	/// labeled alternative in <see cref="TwiceParser.expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitExpressionParenthesis([NotNull] TwiceParser.ExpressionParenthesisContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>expressionLambda</c>
	/// labeled alternative in <see cref="TwiceParser.expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitExpressionLambda([NotNull] TwiceParser.ExpressionLambdaContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>expressionBlock</c>
	/// labeled alternative in <see cref="TwiceParser.expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitExpressionBlock([NotNull] TwiceParser.ExpressionBlockContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>expressionDivide</c>
	/// labeled alternative in <see cref="TwiceParser.expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitExpressionDivide([NotNull] TwiceParser.ExpressionDivideContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>expressionComparison</c>
	/// labeled alternative in <see cref="TwiceParser.expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitExpressionComparison([NotNull] TwiceParser.ExpressionComparisonContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>expressionSubtract</c>
	/// labeled alternative in <see cref="TwiceParser.expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitExpressionSubtract([NotNull] TwiceParser.ExpressionSubtractContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>expressionArrayLiteral</c>
	/// labeled alternative in <see cref="TwiceParser.expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitExpressionArrayLiteral([NotNull] TwiceParser.ExpressionArrayLiteralContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>expressionNumber</c>
	/// labeled alternative in <see cref="TwiceParser.expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitExpressionNumber([NotNull] TwiceParser.ExpressionNumberContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>expressionAnd</c>
	/// labeled alternative in <see cref="TwiceParser.expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitExpressionAnd([NotNull] TwiceParser.ExpressionAndContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>expressionUntil</c>
	/// labeled alternative in <see cref="TwiceParser.expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitExpressionUntil([NotNull] TwiceParser.ExpressionUntilContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>expressionString</c>
	/// labeled alternative in <see cref="TwiceParser.expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitExpressionString([NotNull] TwiceParser.ExpressionStringContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>expressionOr</c>
	/// labeled alternative in <see cref="TwiceParser.expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitExpressionOr([NotNull] TwiceParser.ExpressionOrContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>expressionArrayAccess</c>
	/// labeled alternative in <see cref="TwiceParser.expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitExpressionArrayAccess([NotNull] TwiceParser.ExpressionArrayAccessContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>expressionChannelLoad</c>
	/// labeled alternative in <see cref="TwiceParser.expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitExpressionChannelLoad([NotNull] TwiceParser.ExpressionChannelLoadContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>expressionVariable</c>
	/// labeled alternative in <see cref="TwiceParser.expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitExpressionVariable([NotNull] TwiceParser.ExpressionVariableContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>typeFunction</c>
	/// labeled alternative in <see cref="TwiceParser.type"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitTypeFunction([NotNull] TwiceParser.TypeFunctionContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>typeBasic</c>
	/// labeled alternative in <see cref="TwiceParser.type"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitTypeBasic([NotNull] TwiceParser.TypeBasicContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>typeGeneric</c>
	/// labeled alternative in <see cref="TwiceParser.type"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitTypeGeneric([NotNull] TwiceParser.TypeGenericContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="TwiceParser.comp"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitComp([NotNull] TwiceParser.CompContext context) { return VisitChildren(context); }
}
