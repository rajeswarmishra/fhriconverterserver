// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using Microsoft.Health.Fhir.Core.Features.Search.Expressions;
using Microsoft.Health.Fhir.SqlServer.Features.Schema.Model;
using Microsoft.Health.SqlServer.Features.Schema.Model;

namespace Microsoft.Health.Fhir.SqlServer.Features.Search.Expressions.Visitors.QueryGenerators
{
    internal class UriSearchParameterQueryGenerator : NormalizedSearchParameterQueryGenerator
    {
        public static readonly UriSearchParameterQueryGenerator Instance = new UriSearchParameterQueryGenerator();

        public override Table Table => VLatest.UriSearchParam;

        public override SearchParameterQueryGeneratorContext VisitString(StringExpression expression, SearchParameterQueryGeneratorContext context)
        {
            return VisitSimpleString(expression, context, VLatest.UriSearchParam.Uri, expression.Value);
        }

        public override SearchParameterQueryGeneratorContext VisitBinary(BinaryExpression expression, SearchParameterQueryGeneratorContext context)
        {
            if (expression.BinaryOperator != BinaryOperator.Equal)
            {
                throw new NotSupportedException();
            }

            switch (expression.FieldName)
            {
                case FieldName.UriFragment:
                    return VisitSimpleBinary(BinaryOperator.Equal, context, VLatest.UriSearchParam.Fragment, null, expression.Value);
                case FieldName.UriVersion:
                    return VisitSimpleBinary(BinaryOperator.Equal, context, VLatest.UriSearchParam.Version, null, expression.Value);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
