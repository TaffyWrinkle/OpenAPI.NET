﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;
using Microsoft.OpenApi.Models;

namespace Microsoft.OpenApi.Services
{
    /// <summary>
    /// Defines behavior for comparing properties of <see cref="OpenApiParameter"/>.
    /// </summary>
    public class OpenApiParameterComparer : OpenApiComparerBase<OpenApiParameter>
    {
        /// <summary>
        /// Executes comparision against source and target <see cref="OpenApiParameter"/>.
        /// </summary>
        /// <param name="sourceParameter">The source.</param>
        /// <param name="targetParameter">The target.</param>
        /// <param name="comparisonContext">Context under which to compare the source and target.</param>
        public override void Compare(
            OpenApiParameter sourceParameter,
            OpenApiParameter targetParameter,
            ComparisonContext comparisonContext)
        {
            if (sourceParameter == null && targetParameter == null)
            {
                return;
            }

            if (sourceParameter == null || targetParameter == null)
            {
                comparisonContext.AddOpenApiDifference(
                    new OpenApiDifference
                    {
                        OpenApiDifferenceOperation = OpenApiDifferenceOperation.Update,
                        SourceValue = sourceParameter,
                        TargetValue = targetParameter,
                        OpenApiComparedElementType = typeof(OpenApiParameter),
                        Pointer = comparisonContext.PathString
                    });

                return;
            }

            WalkAndCompare(
                comparisonContext,
                OpenApiConstants.Content,
                () => comparisonContext
                    .GetComparer<IDictionary<string, OpenApiMediaType>>()
                    .Compare(sourceParameter.Content, targetParameter.Content, comparisonContext));

            WalkAndCompare(comparisonContext, OpenApiConstants.Description,
                () => Compare(sourceParameter.Description, targetParameter.Description, comparisonContext));

            WalkAndCompare(comparisonContext, OpenApiConstants.Required,
                () => Compare(sourceParameter.Required, targetParameter.Required, comparisonContext));

            WalkAndCompare(comparisonContext, OpenApiConstants.Name,
                () => Compare(sourceParameter.Name, targetParameter.Name, comparisonContext));

            WalkAndCompare(comparisonContext, OpenApiConstants.Deprecated,
                () => Compare(sourceParameter.Deprecated, targetParameter.Deprecated, comparisonContext));

            WalkAndCompare(comparisonContext, OpenApiConstants.AllowEmptyValue,
                () => Compare(sourceParameter.AllowEmptyValue, targetParameter.AllowEmptyValue, comparisonContext));

            WalkAndCompare(comparisonContext, OpenApiConstants.Explode,
                () => Compare(sourceParameter.Explode, targetParameter.Explode, comparisonContext));

            WalkAndCompare(comparisonContext, OpenApiConstants.AllowReserved,
                () => Compare(sourceParameter.AllowReserved, targetParameter.AllowReserved, comparisonContext));

            WalkAndCompare(comparisonContext, OpenApiConstants.Style,
                () => Compare<ParameterStyle>(sourceParameter.Style, targetParameter.Style, comparisonContext));

            WalkAndCompare(comparisonContext, OpenApiConstants.In,
                () => Compare<ParameterLocation>(sourceParameter.In, targetParameter.In, comparisonContext));

            WalkAndCompare(
                comparisonContext,
                OpenApiConstants.Schema,
                () => comparisonContext
                    .GetComparer<OpenApiSchema>()
                    .Compare(sourceParameter.Schema, targetParameter.Schema, comparisonContext));

            // To Do Add compare for reference object
            // To Do Compare Examples
            // To Do Compare parameter as IOpenApiExtensible
        }
    }
}