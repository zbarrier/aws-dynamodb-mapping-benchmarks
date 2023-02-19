using System.Reflection;

using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace Mapping.Benchmarks.Extensions;
public static class DocumentExtensions
{
    static MethodInfo? fromDocumentMi;
    static MethodInfo? toDocumentMi;

    public static object? FromDocumentDynamic(this DynamoDBContext ctx, Document doc, Type type)
    {
        if (fromDocumentMi is null)
        {
            fromDocumentMi = typeof(DynamoDBContext)
                .GetMethods()
                .First(m => m.Name == "FromDocument" && m.GetParameters().Length == 1);
        }

        var obj = fromDocumentMi
            .MakeGenericMethod(type)
            .Invoke(ctx, new object[] { doc });

        return obj;
    }

    public static Document? ToDocumentDynamic(this DynamoDBContext ctx, object obj, Type objType)
    {
        if (toDocumentMi is null)
        {
            toDocumentMi = typeof(DynamoDBContext)
                .GetMethods()
                .First(m => m.Name == "ToDocument" && m.GetParameters().Count() == 1);
        }

        var doc = toDocumentMi
            .MakeGenericMethod(objType)
            .Invoke(ctx, new[] { obj }) as Document;

        return doc;
    }
}
