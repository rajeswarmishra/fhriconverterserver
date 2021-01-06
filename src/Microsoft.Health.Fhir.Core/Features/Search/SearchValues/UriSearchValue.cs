// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using EnsureThat;

namespace Microsoft.Health.Fhir.Core.Features.Search.SearchValues
{
    /// <summary>
    /// Represents an URI search value.
    /// </summary>
    public class UriSearchValue : ISearchValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UriSearchValue"/> class.
        /// </summary>
        /// <param name="uri">The URI value.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1054:Uri parameters should not be strings", Justification = "Value is passed in from user")]
        public UriSearchValue(string uri)
        {
            EnsureArg.IsNotNullOrWhiteSpace(uri, nameof(uri));

            Uri = uri;
        }

        protected UriSearchValue()
        {
        }

        /// <summary>
        /// Gets the URI value.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1056:URI properties should not be strings", Justification = "Value is passed in from user")]
        public string Uri { get; protected set; }

        /// <inheritdoc />
        public bool IsValidAsCompositeComponent => true;

        /// <inheritdoc />
        public virtual void AcceptVisitor(ISearchValueVisitor visitor)
        {
            EnsureArg.IsNotNull(visitor, nameof(visitor));

            visitor.Visit(this);
        }

        public virtual bool Equals([AllowNull] ISearchValue other)
        {
            if (other == null)
            {
                return false;
            }

            var uriSearchValueOther = other as UriSearchValue;

            if (uriSearchValueOther == null)
            {
                return false;
            }

            return Uri == uriSearchValueOther.Uri;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Uri;
        }
    }
}
