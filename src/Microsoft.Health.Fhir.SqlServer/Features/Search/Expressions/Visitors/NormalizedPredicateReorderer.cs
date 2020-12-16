﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Microsoft.Health.Fhir.Core.Features.Search.Expressions;
using Microsoft.Health.Fhir.SqlServer.Features.Search.Expressions.Visitors.QueryGenerators;

namespace Microsoft.Health.Fhir.SqlServer.Features.Search.Expressions.Visitors
{
    /// <summary>
    /// Reorders table expressions by expected selectivity. Most selective are moved to the front.
    /// </summary>
    internal class NormalizedPredicateReorderer : SqlExpressionRewriterWithInitialContext<object>
    {
        public static readonly NormalizedPredicateReorderer Instance = new NormalizedPredicateReorderer();

        public override Expression VisitSqlRoot(SqlRootExpression expression, object context)
        {
            if (expression.TableExpressions.Count == 1)
            {
                return expression;
            }

            List<TableExpression> reorderedExpressions = expression.TableExpressions.OrderByDescending(t =>
            {
                // Sort _id (denormalized) expression to the front
                if (t.NormalizedPredicate == null && t.DenormalizedPredicate != null && t.Kind == TableExpressionKind.All)
                {
                    return 20;
                }

                if (t.NormalizedPredicate is MissingSearchParameterExpression)
                {
                    return -10;
                }

                switch (t.SearchParameterQueryGenerator)
                {
                    case ReferenceSearchParameterQueryGenerator _:
                        return 10;
                    case CompartmentSearchParameterQueryGenerator _:
                        return 10;
                    case IncludeQueryGenerator _:
                        return -20;
                    default:
                        return 0;
                }
            }).ToList();

            return new SqlRootExpression(reorderedExpressions, expression.ResourceExpressions);
        }
    }
}
