﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Api.Infrastructure.Entities
{
    public class Asset
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid ModelId { get; set; }
        public virtual Model Model { get; set; }

        [MaxLength(63)]
        public string Hostname { get; set; }

        public Guid RackId { get; set; }
        public virtual Rack Rack { get; set; }

        [Range(1, int.MaxValue)]
        public int RackPosition { get; set; }

        public string OwnerId { get; set; }
        public virtual User Owner { get; set; }

        public string Comment { get; set; }
        public DateTime LastUpdatedDate { get; set; }

        public int AssetNumber { get; set; }

        public virtual List<AssetPowerPort> PowerPorts { get; set; }
        public virtual List<AssetNetworkPort> NetworkPorts { get; set; }
        public virtual List<Asset> Blades { get; set; }
        public Guid? ChassisId { get; set; }
        public virtual Asset Chassis { get; set; }
        public int? ChassisSlot { get; set; }
        [MinLength(6)]
        [MaxLength(7)] // '#' is optional before hex color code
        public string CustomDisplayColor { get; set; }
        [MaxLength(50)]
        public string CustomCpu { get; set; }
        public int? CustomMemory { get; set; }
        [MaxLength(50)]
        public string CustomStorage { get; set; }
    }
}