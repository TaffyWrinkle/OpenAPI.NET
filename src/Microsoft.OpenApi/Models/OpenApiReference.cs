﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Writers;

namespace Microsoft.OpenApi.Models
{
    public enum ReferenceType
    {
        Schema,
        Parameter,
        Header,
        Response,
        RequestBody,
        Example,
        SecurityScheme,
        Callback,
        Link,
        Tags
    }

    public class OpenApiReference : IOpenApiElement
    {
        public ReferenceType ReferenceType { get; set; }
        public string TypeName { get; set; }

        public string ExternalFilePath { get; set; } = String.Empty;

        public OpenApiReference()
        {

        }

        public OpenApiReference(string pointer)
        {
            var pointerbits = pointer.Split('#');
            if (pointerbits.Length == 2)
            {
                ExternalFilePath = pointerbits[0];
                ParseLocalPointer(pointerbits[1]);
            } else
            {
                ParseLocalPointer(pointerbits[0]);
            }
        }

        private void ParseLocalPointer(string pointer)
        {
            var pointerbits = pointer.Split('/');
            if (pointerbits.Length == 0)
            {
                throw new ArgumentException();
            } else if (pointerbits.Length == 1)
            {
                ReferenceType = ReferenceType.Schema;
                TypeName = pointerbits[0];
            }
            else if (pointerbits.Length == 3) {  //tags
                ReferenceType = ParseReferenceType(pointerbits[1]);
                TypeName = pointerbits[2];
            } else if (pointerbits.Length == 4)
            {
                ReferenceType = ParseReferenceType(pointerbits[2]);
                TypeName = pointerbits[3];
            }
        }

        private ReferenceType ParseReferenceType(string referenceTypeName)
        {
            switch (referenceTypeName)
            {
                case "schemas":
                    return ReferenceType.Schema;
                case "parameters":
                    return ReferenceType.Parameter;
                case "responses":
                    return ReferenceType.Response;
                case "headers":
                    return ReferenceType.Header;
                case "tags":
                    return ReferenceType.Tags;
                case "securitySchemes":
                    return ReferenceType.SecurityScheme;
                case "callbacks":
                    return ReferenceType.Callback;
                case "links":
                    return ReferenceType.Link;
                default:
                    throw new ArgumentException();
            }
        }
        private string GetReferenceTypeName(ReferenceType referenceType)
        {
            switch(referenceType)
            {
                case ReferenceType.Schema:
                    return "schemas";
                case ReferenceType.Parameter:
                    return "parameters";
                case ReferenceType.Response:
                    return "responses";
                case ReferenceType.Header:
                    return "headers";
                case ReferenceType.Tags:
                    return "tags";
                case ReferenceType.SecurityScheme:
                    return "securitySchemes";
                case ReferenceType.Callback:
                    return "callbacks";
                case ReferenceType.Link:
                    return "links";
                default:
                    throw new ArgumentException();

            }
        }
        public override string ToString()
        {
            var pointer = string.Empty;
            if (!String.IsNullOrEmpty(ExternalFilePath))
            {
                pointer = ExternalFilePath;
            }
            return pointer + "#/components/" + GetReferenceTypeName(this.ReferenceType) + "/" + TypeName;
        }

        public JsonPointer GetLocalPointer()
        {
            return new JsonPointer("#/components/" + GetReferenceTypeName(this.ReferenceType) + "/" + TypeName);
        }

        /// <summary>
        /// Serialize <see cref="OpenApiReference"/> to Open Api v3.0
        /// </summary>
        public virtual void WriteAsV3(IOpenApiWriter writer)
        {
            // nothing here
        }

        /// <summary>
        /// Serialize <see cref="OpenApiReference"/> to Open Api v2.0
        /// </summary>
        public virtual void WriteAsV2(IOpenApiWriter writer)
        {
            // nothing here
        }
    }
}
