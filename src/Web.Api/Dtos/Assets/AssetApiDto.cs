﻿using System;

namespace Web.Api.Dtos.Assets
{
    public abstract class AssetApiDto
    {
        public Guid? ChangePlanId { get; set; }
        public Guid? RackId { get; set; }
        public string Hostname { get; set; }
        public string Comment { get; set; }
        public int RackPosition { get; set; }
        public Guid? OwnerId { get; set; }
        public Guid ModelId { get; set; }
        public int? AssetNumber { get; set; }
        public Guid? ChassisId { get; set; }
        public int? ChassisSlot { get; set; }
        public string CustomDisplayColor { get; set; }
        public string CustomCpu { get; set; }
        public int? CustomMemory { get; set; }
        public string CustomStorage { get; set; }
    }
}