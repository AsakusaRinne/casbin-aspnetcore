﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Casbin.AspNetCore.Authorization.Policy
{
    public class CasbinPolicyCreator : ICasbinPolicyCreator
    {
        private readonly IEnumerable<IAuthorizationRequirement> _casbinAuthorizationRequirements =
            new []{CasbinAuthorizationRequirement.Requirement};

        private AuthorizationPolicy? _emptyPolicy;

        public AuthorizationPolicy Create(ICasbinAuthorizationData authorizationData)
        {
            var authTypesSplit = authorizationData.AuthenticationSchemes?.Split(',');
            if (!(authTypesSplit?.Length > 0))
            {
                _emptyPolicy ??=  new AuthorizationPolicy(_casbinAuthorizationRequirements, Array.Empty<string>());
                return _emptyPolicy;
            }

            var authenticationSchemes = new List<string>();
            foreach (var authType in authTypesSplit)
            {
                if (!string.IsNullOrWhiteSpace(authType))
                {
                    authenticationSchemes.Add(authType.Trim());
                }
            }
            return new AuthorizationPolicy(_casbinAuthorizationRequirements, authenticationSchemes);
        }
    }
}
