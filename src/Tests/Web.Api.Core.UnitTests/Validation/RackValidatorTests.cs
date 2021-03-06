﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;
using Web.Api.Core.Validation;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories.Interfaces;
using Xunit;

namespace Web.Api.Core.UnitTests.Validation
{
    public class RackValidatorTests
    {
        [Fact]
        public void RackValidator_FailsIfColumnIsInvalid()
        {
            var mockRepo = new Mock<IRackRepository>();
            var sut = new RackValidator(mockRepo.Object);

            sut.ShouldHaveValidationErrorFor(x => x.Column, 0);
            sut.ShouldHaveValidationErrorFor(x => x.Column, -1);
        }
        [Fact]
        public void RackValidator_FailsIfRowIsInvalid()
        {
            var mockRepo = new Mock<IRackRepository>();
            var sut = new RackValidator(mockRepo.Object);

            sut.ShouldHaveValidationErrorFor(x => x.Row, null as string);
            sut.ShouldHaveValidationErrorFor(x => x.Row, "AA");
            sut.ShouldHaveValidationErrorFor(x => x.Row, "3");
        }
        [Fact]
        public async void RackValidator_Delete_FailsIfContainsAssets()
        {
            var mockRepo = new Mock<IRackRepository>();
            var sut = new RackValidator(mockRepo.Object);

            var rack = GetValidRackWithAssets();

            var result = await sut.ValidateAsync(rack, ruleSet: "delete");

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors,
                validationFailure => validationFailure.PropertyName == nameof(rack.Assets));
        }

        [Fact]
        public void RackValidator_Delete_PassesIfNoAssets()
        {
            var mockRepo = new Mock<IRackRepository>();
            var sut = new RackValidator(mockRepo.Object);
            
            sut.ShouldNotHaveValidationErrorFor(x => x.Assets, null as List<Asset>);
        }

        [Fact]
        public async void RackValidator_Create_FailsIfAddressAlreadyExistsInDatacenter()
        {
            var mockRepo = new Mock<IRackRepository>();
            mockRepo.Setup(x => x.AddressExistsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .ReturnsAsync(true);
            var sut = new RackValidator(mockRepo.Object);

            var rack = GetValidRackWithAssets();

            var result = await sut.ValidateAsync(rack, ruleSet: "create");

            Assert.False(result.IsValid);
        }
        [Fact]
        public async void RackValidator_Create_FailsIfSameAddressExistsInDifferentDatacenters()
        {
            var mockRepo = new Mock<IRackRepository>();
            mockRepo.Setup(x => x.AddressExistsAsync(It.IsAny<string>(), It.IsAny<int>(), Guid.NewGuid()))
                .ReturnsAsync(true);
            var sut = new RackValidator(mockRepo.Object);

            var rack = GetValidRackWithAssets();

            var result = await sut.ValidateAsync(rack, ruleSet: "create");

            Assert.True(result.IsValid);
        }

        private static Rack GetValidRackWithAssets()
        {
            var rack = GetValidRackWithoutAssets();
            var model = new Model() { Height = 4};
            var datacenter = new Datacenter() { Id = Guid.NewGuid() };
            var assets = new List<Asset>
            {
                new Asset() {Rack = rack, Model = model, RackPosition = 1},
                new Asset() {Rack = rack, Model = model, RackPosition = 5},
                new Asset() {Rack = rack, Model = model, RackPosition = 9},
                new Asset() {Rack = rack, Model = model, RackPosition = 13}
            };
            model.Assets = assets;
            rack.Assets = assets;
            rack.Datacenter = datacenter;
            return rack;
        }
        private static Rack GetValidRackWithoutAssets()
        {
            var rack = new Rack() {Row = "A", Column = 1};
            return rack;
        }
    }
}
