﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Composition;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CSharp.CodeFixes;

namespace Roslynator.CSharp.Analyzers.ReturnTaskInsteadOfNull
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ReturnTaskInsteadOfNullCodeFixProvider))]
    [Shared]
    public class ReturnTaskInsteadOfNullCodeFixProvider : BaseCodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(DiagnosticIdentifiers.ReturnTaskInsteadOfNull); }
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            SyntaxNode root = await context.GetSyntaxRootAsync().ConfigureAwait(false);

            if (!TryFindNode(root, context.Span, out ExpressionSyntax expression))
                return;

            SemanticModel semanticModel = await context.GetSemanticModelAsync().ConfigureAwait(false);

            InvocationExpressionSyntax newExpression = ReturnTaskInsteadOfNullRefactoring.CreateNewExpression(expression, semanticModel, context.CancellationToken);

            CodeAction codeAction = CodeAction.Create(
                $"Replace '{expression}' with '{newExpression}'",
                cancellationToken => context.Document.ReplaceNodeAsync(expression, newExpression, cancellationToken),
                GetEquivalenceKey(DiagnosticIdentifiers.ReturnTaskInsteadOfNull));

            context.RegisterCodeFix(codeAction, context.Diagnostics[0]);
        }
    }
}
