﻿using System;
using System.Collections.Generic;

namespace Web.Api.Dtos.Users
{
    public class GetUserRolesApiDto
    {
        public List<string> Roles { get; set; }
        public string Datacenters { get; set; }
    }
}