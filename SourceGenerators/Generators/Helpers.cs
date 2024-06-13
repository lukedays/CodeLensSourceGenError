using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenerators.Generators;

internal static class Helpers
{
    public static MethodNodeInfo GetMethodInfo(GeneratorAttributeSyntaxContext context)
    {
        // Get class/method info
        var containingClass = context.TargetSymbol.ContainingType;
        var className = containingClass.Name;
        var ns = containingClass.ContainingNamespace?.ToDisplayString(
            SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(
                SymbolDisplayGlobalNamespaceStyle.Omitted
            )
        );
        var method = (IMethodSymbol)context.TargetSymbol;
        var methodName = method.Name;
        var methodSyntax = (MethodDeclarationSyntax)context.TargetNode;
        var classSyntax = (ClassDeclarationSyntax)methodSyntax.Parent!;
        var methodId = $"{ns}.{className}.{methodName}";

        // Find full parameter names, with namespace and default values
        var paramsDefinitions = string.Join(
            ", ",
            method.Parameters.Select(p =>
                p.ToDisplayString(
                    SymbolDisplayFormat
                        .FullyQualifiedFormat.WithGlobalNamespaceStyle(
                            SymbolDisplayGlobalNamespaceStyle.Omitted
                        )
                        .WithParameterOptions(
                            SymbolDisplayParameterOptions.IncludeDefaultValue
                                | SymbolDisplayParameterOptions.IncludeType
                                | SymbolDisplayParameterOptions.IncludeName
                        )
                )
            )
        );

        return new MethodNodeInfo
        {
            ClassName = className,
            ClassModifiers = classSyntax.Modifiers.ToString(),
            ClassTypeParameters = classSyntax.TypeParameterList?.ToString() ?? "",
            Namespace = ns,
            ReturnType = method.ReturnType,
            ParamsDefinitions = paramsDefinitions,
            ParamsCall = string.Join(", ", method.Parameters.Select(p => p.Name)),
            MethodName = methodName,
            MethodAccessibility = method.DeclaredAccessibility.ToString().ToLower(),
            MethodModifiers = methodSyntax.Modifiers.ToString(),
            MethodTypeParameters = methodSyntax.TypeParameterList?.ToString() ?? "",
            Filename = methodId
        };
    }
}
