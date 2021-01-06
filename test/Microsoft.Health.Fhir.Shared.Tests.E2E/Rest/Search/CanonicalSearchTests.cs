// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Linq;
using Hl7.Fhir.Model;
using Microsoft.Health.Fhir.Client;
using Microsoft.Health.Fhir.Tests.Common.FixtureParameters;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace Microsoft.Health.Fhir.Tests.E2E.Rest.Search
{
    [HttpIntegrationFixtureArgumentSets(DataStore.All, Format.Json)]
    public class CanonicalSearchTests : SearchTestsBase<CanonicalSearchTestFixture>
    {
        public CanonicalSearchTests(CanonicalSearchTestFixture fixture)
            : base(fixture)
        {
        }

        [Fact]
        public async Task GivenAnObservationWithProfile_WhenSearchingByCanonicalUriFragmentVersion_Then1ExpectedResultIsFound()
        {
            FhirResponse<Bundle> result = await Fixture.TestFhirClient.SearchAsync($"Observation?_profile={Fixture.ObservationProfileV1}");

            Assert.Collection(result.Resource.Entry, x => Assert.Equal(Fixture.ObservationProfileV1, x.Resource.Meta.Profile.Single()));
        }

        [Fact]
        public async Task GivenAnObservationWithProfile_WhenSearchingByCanonicalUri_Then2ExpectedResultsAreFound()
        {
            FhirResponse<Bundle> result = await Fixture.TestFhirClient.SearchAsync($"Observation?_profile={Fixture.ObservationProfileUri}&_order=_lastModified");

            Assert.Collection(
                result.Resource.Entry,
                x => Assert.Equal(Fixture.ObservationProfileV1, x.Resource.Meta.Profile.Single()),
                x => Assert.Equal(Fixture.ObservationProfileV2, x.Resource.Meta.Profile.Single()));
        }

        [Fact]
        public async Task GivenAnObservationWithProfile_WhenSearchingByCanonicalUriVersion_Then1ExpectedResultIsFound()
        {
            FhirResponse<Bundle> result = await Fixture.TestFhirClient.SearchAsync($"Observation?_profile={Fixture.ObservationProfileUri}|2");

            Assert.Collection(
                result.Resource.Entry,
                x => Assert.Equal(Fixture.ObservationProfileV2, x.Resource.Meta.Profile.Single()));
        }

        [Fact]
        public async Task GivenAnObservationWithProfile_WhenSearchingByCanonicalUriFragment_Then1ExpectedResultIsFound()
        {
            FhirResponse<Bundle> result = await Fixture.TestFhirClient.SearchAsync($"Observation?_profile={Fixture.ObservationProfileUri}{Fixture.ObservationProfileV1Fragment}");

            Assert.Collection(
                result.Resource.Entry,
                x => Assert.Equal(Fixture.ObservationProfileV1, x.Resource.Meta.Profile.Single()));
        }

        [Fact]
        public async Task GivenAnObservationWithProfile_WhenSearchingByCanonicalUriMultipleProfiles_Then1ExpectedResultIsFound()
        {
            FhirResponse<Bundle> result = await Fixture.TestFhirClient.SearchAsync($"Observation?_profile={Fixture.ObservationProfileUriAlternate}");

            Assert.Collection(
                result.Resource.Entry,
                x => Assert.Equal(Fixture.ObservationProfileUriAlternate, x.Resource.Meta.Profile.Single()));
        }
    }
}
