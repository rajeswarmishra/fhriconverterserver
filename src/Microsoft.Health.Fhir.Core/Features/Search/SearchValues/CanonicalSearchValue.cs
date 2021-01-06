// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using EnsureThat;
using Microsoft.Health.Fhir.Core.Models;

namespace Microsoft.Health.Fhir.Core.Features.Search.SearchValues
{
    /// <summary>
    /// Represents an URI search value.
    /// </summary>
    public class CanonicalSearchValue : UriSearchValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CanonicalSearchValue"/> class.
        /// </summary>
        /// <param name="uri">The URI value.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1054:Uri parameters should not be strings", Justification = "Value is passed in from user")]
        public CanonicalSearchValue(string uri)
        {
            EnsureArg.IsNotNullOrWhiteSpace(uri, nameof(uri));

            string[] components = uri.Split("|");
            Uri = components.First();

            if (components.Length > 1)
            {
                Version = components.Last();

                string[] fragments = Version.Split("#");

                if (fragments.Length == 2)
                {
                    Version = fragments.First();
                    Fragment = fragments.Last();
                }
            }
        }

        public string Version { get; set; }

        public string Fragment { get; set; }

        /// <summary>
        /// When true the search value has Canonical components Uri, Version and/or Fragment.
        /// When false the search value contains only Uri.
        /// </summary>
        public bool IsCanonicalValue
        {
            get
            {
                return !string.IsNullOrEmpty(Version) || !string.IsNullOrEmpty(Fragment);
            }
        }

        /// <summary>
        /// Parses the string value to an instance of <see cref="CanonicalSearchValue"/>.
        /// </summary>
        /// <param name="s">The string to be parsed.</param>
        /// <returns>An instance of <see cref="CanonicalSearchValue"/>.</returns>
        public static UriSearchValue Parse(string s)
        {
            EnsureArg.IsNotNullOrWhiteSpace(s, nameof(s));

            if (ModelInfoProvider.Version == FhirSpecification.Stu3)
            {
                return new UriSearchValue(s);
            }

            return new CanonicalSearchValue(s);
        }

        /// <inheritdoc />
        public override void AcceptVisitor(ISearchValueVisitor visitor)
        {
            EnsureArg.IsNotNull(visitor, nameof(visitor));

            visitor.Visit(this);
        }

        public override bool Equals([AllowNull] ISearchValue other)
        {
            if (other == null)
            {
                return false;
            }

            return ToString() == other.ToString();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Version))
            {
                return Uri;
            }

            if (string.IsNullOrEmpty(Fragment))
            {
                return string.Concat(Uri, "|", Version);
            }

            return string.Concat(Uri, "|", Version, "#", Fragment);
        }
    }
}
